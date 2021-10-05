using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Boss player
/// </summary>
public class Boss : MonoBehaviour {

	public float aiTurnSmoothTime = .1f;
	private float aiTurnSmoothVelocity = .05f;

	// not the best way to load, as you can't change once at runtime
	// but for now it'll do, as we'll want to load using www from dataserver
	/// <summary>
	/// If no boss script is loaded, use the default e9s boss script
	/// </summary>
	public TextAsset defaultBossScript;

	// for referencing all the action
	private List<ActionQueue> allActions = new List<ActionQueue>();

	// keep it at 1 for normal speed, increment betwee 2-6 to speed up fight
	public float debugSpeedUpCast = 1;

	// prevent starting more than once
	public bool canPlayNextAction = true;

	// Start is called before the first frame update
	void Start() {

	}

	/// <summary>
	/// Begins the round. Called by the LevelManager.
	/// </summary>
	public void StartRound(ActionQueueLoader loader) {
		// for debug to load from local files instead of network
		//Debug.LogWarning("Overriding and loading boss Local data, setting boss loader to null");
		//loader = null;

		LoadActionQueues(loader);
		if (validate()) {
			StartCoroutine(playActions());
		} else {
			Debug.LogError("Fail to StartRound, validate failed as Boss has no actions loaded");
		}
	}

	private bool validate() {
		if (allActions.Count > 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Boss should be fed a boss script data from the network. If not, it will use the default script.
	/// </summary>
	private void LoadActionQueues(ActionQueueLoader loader) {
		ActionQueueLoader actionLoader;
		if (loader == null) {
			// load json from referenced text file
			//ActionQueueLoader loader = JsonUtility.FromJson<ActionQueueLoader>(defaultBossScript.text);
			Debug.Log("No script was loaded, loading default boss action");
			actionLoader = JsonConvert.DeserializeObject<ActionQueueLoader>(defaultBossScript.text);
		} else {
			actionLoader = loader;
		}
		LoadBossAllActionsForRound(actionLoader);
	}

	/// <summary>
	/// // todo move to dataService
	/// If network fails, it will only load the default boss scripts provided with the build.
	/// </summary>
	private void LoadBossAllActionsForRound(ActionQueueLoader actionLoader) {
		if (defaultBossScript) {

			// get the actions from the loader class
			ActionQueue[] bossAllActionMasterList = actionLoader.actionsQueues;

			// convert array to list for easier use
			List<ActionQueue> allActionsTemp = new List<ActionQueue>(bossAllActionMasterList);

			List<Checkpoint> allCheckpoints = new List<Checkpoint>(actionLoader.checkpoints);
			//Debug.Log("all phases count: " + allPhases.Length);

			Debug.Log("boss checkpoints count: " + actionLoader.checkpoints.Length);

			// pull all actions from each checkpoint into one major list of actions
			allCheckpoints.ForEach((Checkpoint cp) => {
				List<ActionQueue> actions = cp.GetActionQueues(allActionsTemp);
				if (actions.Count > 0) {
					actions.ForEach((ActionQueue action) => {
						ActionQueue a = (ActionQueue)action.Clone();
						allActions.Add(a);
					});
				} else {
					Debug.LogWarning("actions for checkpoint is null or zero");
				}
			});

			// remove this is hardcode just to one checkpoint
			//allActions = allCheckpoints[0].GetActionQueues(allActionsTemp);

		} else {
			throw new UnityException("defaultBossScript null and required");
		}
	}

	/// <summary>
	/// force boss facing direciton
	/// </summary>
	/// <param name="dir">Dir.</param>
	void faceDirection(Vector3 dir) {
		dir = dir.normalized;
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
	}

	/// <summary>
	/// Loop to play all boss actions until the end
	/// </summary>
	/// <returns></returns>
	IEnumerator playActions() {
		while (canPlayNextAction) {
			//if the action exist
			if (allActions.Count > 0) {
				//yield return new WaitForSeconds(1);

				// get and pop the action out
				ActionQueue action = allActions[0];
				Debug.Log("now starting action " + action.name);
				EventManager.alertBossUpcomingAction(action);
				allActions.RemoveAt(0);

				// move or animate the boss to position
				float moveSpeed = action.goToSpeed;
				Vector3 goToPos = action.getGoToPosition();
				if (moveSpeed > 0) {
				float seconds = moveSpeed / debugSpeedUpCast;
					float elapsedTime = 0;
					Vector3 startingPos = transform.position;
					while (elapsedTime < seconds) {
						transform.position = Vector3.Lerp(startingPos, goToPos, (elapsedTime / seconds));
						faceDirectionOfTravel(goToPos - startingPos);
						elapsedTime += Time.deltaTime;
						yield return new WaitForEndOfFrame();
					}
				}
				transform.position = goToPos;
				faceDirection(action.getFacingDirection());
				// end of move to position

				// delay time until cast
				//Debug.Log("delay time for action " + action.castDelay);
				yield return new WaitForSeconds(action.castDelay / debugSpeedUpCast);

				// casting time
				EventManager.alertBossActionCasting(action);
				yield return new WaitForSeconds(action.castTime);

				// delay before actionally attacking
				if (action.delayTakeAction > 0) {
					yield return new WaitForSeconds(action.delayTakeAction);
				}

				// give time for attack animation
				Debug.Log("playing boss animation action" + action.name);
				// perform the action todo
				EventManager.alertBossActionCasted(action);
				yield return new WaitForSeconds(action.castAnimationTime / debugSpeedUpCast);


			} else {
				Debug.Log("End of boss fight");
				canPlayNextAction = false;
				StopCoroutine(playActions());
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
