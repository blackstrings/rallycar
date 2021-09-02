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

	// gets cleared after action is done moving
	private ActionQueue playerAction;


	// Start is called before the first frame update
	void Start()
    {
		if(isAI) {
			setupPlayerAI();
		}
	}

	private void setupPlayerAI() {
		EventManager.onBossUpcomingActionAlert += handleBossUpcomingAction;
		// load json from referenced text file
		actionLoader = JsonUtility.FromJson<ActionQueueLoader>(playerActionRespondJson.text);
		// get the actions from the loader class
		ActionQueue[] actions = actionLoader.actionsQueues;
		// convert array to list for easier use
		actionQueues = new List<ActionQueue>(actions);
	}

	private void OnDestroy() {
		EventManager.onBossUpcomingActionAlert -= handleBossUpcomingAction;
	}

	// Update is called once per frame
	void Update() {
	}


	public void Reset() {
	}

	public void handleBossUpcomingAction(ActionQueue action) {
		// match boss action name as a map to get the action location to take
		playerAction = null;

		for(int i=0; i<actionQueues.Count; i++) {
			if(actionQueues[i].name.Equals(action.name)) {
				playerAction = actionQueues[i];
				break;
			}
		}

		// tele and go to position and face
		if(playerAction != null) {
			//transform.position = playerAction.getPosition();
			//faceDirection(playerAction.getFacingDirection());
			moveTo(action.getPosition());
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
  
  	private IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed) {
		// speed should be 1 unit per second
		while (objectToMove.transform.position != end) {
			objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator MoveOverSeconds(Vector3 end, float seconds) {
		float elapsedTime = 0;
		Vector3 startingPos = transform.position;
		while (elapsedTime < seconds) {
			transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = end;
		faceDirection(playerAction.getFacingDirection());
	}
}
