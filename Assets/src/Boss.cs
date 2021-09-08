using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss player
/// </summary>
public class Boss : MonoBehaviour
{

	// not the best way to load, as you can't change once at runtime
	// but for now it'll do, as we'll want to load using www from dataserver
	/// <summary>
	/// If no boss script is loaded, use the default e9s boss script
	/// </summary>
	public TextAsset defaultBossScript;
	private List<ActionQueue> actionQueues;

	// keep it at 1 for normal speed, increment betwee 2-6 to speed up fight
	public float debugSpeedUpCast = 1;

	// prevent starting more than once
	public bool canPlayNextAction = true;

	// Start is called before the first frame update
	void Start()
    {
	
    }

	/// <summary>
	/// Begins the round. Called by the sceneManager.
	/// </summary>
	public void StartRound(List<ActionQueue> actionQueues) {
		LoadActionQueues(actionQueues);
		if(validate()) {
			StartCoroutine(playActions());
		} else {
			Debug.LogError("Fail to StartRound, validate failed as Boss has no actions loaded");
		}
	}

	private bool validate() {
		if(actionQueues.Count > 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Boss should be fed a boss script data from the network. If not, it will use the default script.
	/// </summary>
	private void LoadActionQueues(List<ActionQueue> actionQueues) {
		if(actionQueues == null) {
			Debug.Log("No script was loaded, loading default boss action");
			LoadDefaultScript();
		} else {
			this.actionQueues = actionQueues;
		}
	}

	private void LoadDefaultScript() {
		if(defaultBossScript) {
			// load json from referenced text file
			ActionQueueLoader actionLoader = JsonUtility.FromJson<ActionQueueLoader>(defaultBossScript.text);
			// get the actions from the loader class
			ActionQueue[] actions = actionLoader.actionsQueues;
			// convert array to list for easier use
			actionQueues = new List<ActionQueue>(actions);
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
	/// Loop to play all boss actions
	/// </summary>
	/// <returns></returns>
	IEnumerator playActions() {
		while(canPlayNextAction) {
			//if the action exist
			if(actionQueues.Count > 0) {
				//yield return new WaitForSeconds(1);

				// get and pop the action out
				ActionQueue action = actionQueues[0];
				Debug.Log("now starting action " + action.name);
				EventManager.alertBossUpcomingAction(action);
				actionQueues.RemoveAt(0);

				// delay time until cast
				//Debug.Log("delay time for action " + action.castDelay);
				yield return new WaitForSeconds(action.castDelay / debugSpeedUpCast);

				// casting time
				faceDirection(action.getFacingDirection());
				EventManager.alertBossActionCasting(action);
				yield return new WaitForSeconds(action.castTime);

				// delay before actionally attacking
				if(action.delayTakeAction > 0) {
					yield return new WaitForSeconds(action.delayTakeAction);
				}

				// give time for attack animation
				Debug.Log("playing boss animation action" + action.name);
				// perform the action todo
				yield return new WaitForSeconds(action.castAnimationTime / debugSpeedUpCast);


			} else {
				Debug.Log("End of boss fight");
				canPlayNextAction = false;
				StopCoroutine(playActions());
			}
		}
	}


}
