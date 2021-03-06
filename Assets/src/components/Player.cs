using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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

	// to be populated at runtime
	private GameObject bossGO;
	private ClassType classType;

	/// <summary>
	/// Reference when the player is a real human player
	/// </summary>
	private GameObject goToMarker;

	void Awake() {
		if (isAI) {
			Debug.Log("is ai");
			SetupAI();
		} else {
			loadDefaultPlayerAction();
		}
	}

	// Start is called before the first frame update
	void Start() {
		bossGO = GameObject.FindGameObjectWithTag("boss");
		if (!bossGO) {
			throw new UnityException("player " + id + " can't find boss gameobject");
		}

		if (!isAI) {
			goToMarker = GameObject.FindGameObjectWithTag("goToMarker");
			if (!goToMarker) {
				Debug.Log("goToMarker not found");
				// need to load dynamically player actions
			}
			EventManager.onBossUpcomingActionAlert += placePlayerGroundMarker;
		}
	}

	private void placePlayerGroundMarker(ActionQueue bossAction) {
		for (int i = 0; i < actionQueues.Count; i++) {
			if (actionQueues[i].id == bossAction.id) {
				Debug.Log("player action found for marker");
				playerAction = actionQueues[i];
				break;
			}
		}
		if (playerAction != null) {
			if (goToMarker != null) {
				Debug.Log("going to marker");
				goToMarker.transform.position = playerAction.getGoToPosition();
			} else {
				Debug.Log("goToMarker null");
				goToMarker = GameObject.FindGameObjectWithTag("goToMarker");
				goToMarker.transform.position = playerAction.getGoToPosition();
			}
		} else {
			Debug.Log("playerAction null");
		}
	}

	// define the player stats
	void load(ClassType classType, int id) {
		if (!(classType.ToString().Equals(""))) {
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
		loadDefaultPlayerAction();
	}

	private void loadDefaultPlayerAction() {
		// load scripted json from referenced text file
		actionLoader = JsonUtility.FromJson<ActionQueueLoader>(playerActionRespondJson.text);
		// get the actions from the loader class
		ActionQueue[] actions = actionLoader.actionsQueues;
		// convert array to list for easier use
		actionQueues = new List<ActionQueue>(actions);
	}

	private void OnDestroy() {
		EventManager.onBossUpcomingActionAlert -= positionPlayerViaAI;
		EventManager.onBossUpcomingActionAlert -= placePlayerGroundMarker;
	}

	public void positionPlayerViaAI(ActionQueue bossAction) {
		// match boss action name as a map to get the action location to take
		playerAction = null;
		if (bossAction != null) {
			// if there is currPos still available, teleport here quickly before
			// moving forward to the next destination
			if (currPos != Vector3.zero) {
				transform.position = currPos;
				stopPrevMoveTo();
				//reset back to zero
				currPos = Vector3.zero;
			}

			for (int i = 0; i < actionQueues.Count; i++) {
				if (actionQueues[i].id == bossAction.id) {
					playerAction = actionQueues[i];
					break;
				}
			}

			// tele and go to position and face
			if (playerAction != null) {
				currPos = playerAction.getGoToPosition();
				moveTo(currPos, playerAction.faceDirectionAuto);
			} else {
				Debug.LogError("player action not found for player " + id);
			}
		} else {
			Debug.LogError("player " + id + " failed to move, bossAction null");
		}
	}

	private void faceDirection(Vector3 dir) {
		dir = dir.normalized;
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
	}

	private void stopPrevMoveTo() {
		StopCoroutine("MoveOverSeconds");
	}

	private void moveTo(Vector3 newPosition, string faceDirectionAuto) {
		StartCoroutine(MoveOverSeconds(newPosition, 2f, faceDirectionAuto));
	}

	/*private IEnumerator MoveOverSpeed(Vector3 end, float speed) {
		// speed should be 1 unit per second
		while (transform.position != end) {
			transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
	*/

	private IEnumerator MoveOverSeconds(Vector3 end, float seconds, string faceDirectionAuto) {
		float elapsedTime = 0;
		Vector3 startingPos = transform.position;
		while (elapsedTime < seconds) {
			transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			faceDirectionOfTravel(end - startingPos);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = end;

		// hard set the players facing direction upon reaching location
		if (faceDirectionAuto != null) {
			if (faceDirectionAuto.Equals("boss")) {
				Vector3 bossPos = bossGO.transform.position;
				bossPos.y = 0;  // y should be zero
				end.y = 0;
				faceDirection(bossPos - end);
			} else if (faceDirectionAuto.Equals("away")) {
				Vector3 bossPos = bossGO.transform.position;
				bossPos.y = 0;  // y should be zero
				end.y = 0;
				faceDirection(end - bossPos);
			}
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
