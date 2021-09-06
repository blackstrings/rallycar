using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/// <summary>
	/// used to identify the player
	/// </summary>
	public int id;

	public bool isAI = true;
	public TextAsset playerActionRespondJson;
	private ActionQueueLoader actionLoader;
	private List<ActionQueue> actionQueues;

	public float aiTurnSmoothTime = .1f;
	private float aiTurnSmoothVelocity = .05f;

	// When a player takes an action, they will uppdate the final position here.
	// this will help the player teleport to the correct position  instantly
	// should the time be altered during debugging
	private Vector3 currPos = Vector3.zero;

	// gets cleared after action is done moving
	private ActionQueue playerAction;

	// should be found at runtime dynamically with tag "boss"
	private GameObject bossGO;
	private ClassType classType;

	void Awake() {
		if(isAI) {
			SetupAI();
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		bossGO = GameObject.FindGameObjectWithTag("boss");
		if(!bossGO)
        {
			throw new UnityException("player " + id + " can't find boss gameobject");
        }
	}

	// define the player stats
	void load(ClassType classType, int id) {
		if(!(classType.ToString().Equals(""))){
			this.classType = classType;
		} else {
			throw new UnityException("load failed, classType not supported");
		}
	}

	public void Reset() {
		
	}

	/// <summary>
	/// Load in the positional actions this player should be taking as boss calls out actions.
	/// A network loader should be doing this.
	/// </summary>
	/// <param name="actions"></param>
	public void LoadActions(List<ActionQueue> actions) {
		this.actionQueues = actions;
	}

	/// <summary>
	/// Only when the player is an AI
	/// </summary>
	private void SetupAI() {
		// listen to boss action alert
		EventManager.onBossUpcomingActionAlert += positionPlayerViaAI;
		// load scripted json from referenced text file
		actionLoader = JsonUtility.FromJson<ActionQueueLoader>(playerActionRespondJson.text);
		// get the actions from the loader class
		ActionQueue[] actions = actionLoader.actionsQueues;
		// convert array to list for easier use
		actionQueues = new List<ActionQueue>(actions);
	}

	private void OnDestroy() {
		EventManager.onBossUpcomingActionAlert -= positionPlayerViaAI;
	}

	public void positionPlayerViaAI(ActionQueue bossAction) {
		// match boss action name as a map to get the action location to take
		playerAction = null;

		// if there is currPos still available, teleport here quickly before
		// moving forward to the next destination
		if(currPos != Vector3.zero)
        {
			transform.position = currPos;
			stopPrevMoveTo();
			//reset back to zero
			currPos = Vector3.zero;
        }

		for(int i=0; i<actionQueues.Count; i++) {
			if(actionQueues[i].id == bossAction.id) {
				playerAction = actionQueues[i];
				break;
			}
		}

		// tele and go to position and face
		if(playerAction != null) {
			currPos = playerAction.getGoToPosition();
			moveTo(currPos);
		} else {
			Debug.LogError("player action not found for player " + id);
		}
	}

	private void faceDirection(Vector3 dir) {
		dir = dir.normalized;
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
	}

	private void stopPrevMoveTo()
    {
		StopCoroutine("MoveOverSeconds");
    }

	private void moveTo(Vector3 newPosition) {
		StartCoroutine(MoveOverSeconds (newPosition, 2f));
	}
  
  	/*private IEnumerator MoveOverSpeed(Vector3 end, float speed) {
		// speed should be 1 unit per second
		while (transform.position != end) {
			transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
	*/

	private IEnumerator MoveOverSeconds(Vector3 end, float seconds) {
		float elapsedTime = 0;
		Vector3 startingPos = transform.position;
		while (elapsedTime < seconds) {
			transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			faceDirectionOfTravel(end-startingPos);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = end;

		// hard set the players facing direction upon reaching location
		string faceDirAuto = playerAction.faceDirectionAuto;
		if(faceDirAuto != null)
        {
			if (faceDirAuto.Equals("boss"))
			{
				Vector3 bossPos = bossGO.transform.position;
				bossPos.y = 0;  // y should be zero
				end.y = 0;
				faceDirection(bossPos - end);
			} else if(faceDirAuto.Equals("away"))
			{
				Vector3 bossPos = bossGO.transform.position;
				bossPos.y = 0;  // y should be zero
				end.y = 0;
				faceDirection(end - bossPos);
			}
        } else
        {
			Debug.LogError("plpayer " + id + " faceDirAuto not set");
        }
	}

	private void faceDirectionOfTravel(Vector3 direction) {
		direction = direction.normalized;
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		// smooth turn
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref aiTurnSmoothVelocity, aiTurnSmoothTime);
		transform.rotation = Quaternion.Euler(0f, angle, 0f);
	}
}
