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

	// gets cleared after action is done moving
	private ActionQueue playerAction;

	void Awake() {
		if(isAI) {
			SetupAI();
		}
	}

	// Start is called before the first frame update
	void Start()
    {
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

		for(int i=0; i<actionQueues.Count; i++) {
			if(actionQueues[i].id == bossAction.id) {
				playerAction = actionQueues[i];
				break;
			}
		}

		// tele and go to position and face
		if(playerAction != null) {
			moveTo(playerAction.getPosition());
		} else {
			throw new UnityException("player action not found for player " + id);
		}
	}

	private void faceDirection(Vector3 dir) {
		dir = dir.normalized;
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
	}

	private void moveTo(Vector3 newPosition) {
		StartCoroutine(MoveOverSeconds (newPosition, 2f));
	}
  
  	private IEnumerator MoveOverSpeed(Vector3 end, float speed) {
		// speed should be 1 unit per second
		while (transform.position != end) {
			transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

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
		faceDirection(playerAction.getFacingDirection());
	}

	private void faceDirectionOfTravel(Vector3 direction) {
		direction = direction.normalized;
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		// smooth turn
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref aiTurnSmoothVelocity, aiTurnSmoothTime);
		transform.rotation = Quaternion.Euler(0f, angle, 0f);

	}
}
