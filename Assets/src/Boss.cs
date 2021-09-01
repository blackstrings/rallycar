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
	public TextAsset bossActionJson;
	private ActionQueueLoader actionLoader;
	private List<ActionQueue> actionQueues;

	// prevent starting more than once
	private bool canInitiate = false;
	public bool canPlayAction = true;

    // Start is called before the first frame update
    void Start()
    {
		// load json from referenced text file
		actionLoader = JsonUtility.FromJson<ActionQueueLoader>(bossActionJson.text);
		// get the actions from the loader class
		ActionQueue[] actions = actionLoader.actionsQueues;
		// convert array to list for easier use
		actionQueues = new List<ActionQueue>(actions);
		canInitiate = true;
    }

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyUp(KeyCode.Space)) {
			if (canInitiate) {
				canInitiate = false;
				beginFight();
			}
		}
	}

	/// <summary>
	/// hard face direciton
	/// </summary>
	/// <param name="dir">Dir.</param>
	void faceDirection(Vector3 dir) {
		dir = dir.normalized;
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
	}

	/// <summary>
	/// Begins the round.
	/// </summary>
	private void beginFight() {
		StartCoroutine(playActions());
	}

	IEnumerator playActions() {
		while(canPlayAction) {
			//if the action exist
			if(actionQueues.Count > 0) {

				// get and pop the action out
				ActionQueue action = actionQueues[0];
				Debug.Log("now starting action " + action.name);
				GameManager.Instance.UIDisplayBossUpcomingAction(action.name);
				actionQueues.RemoveAt(0);

				// delay the action
				Debug.Log("delay time for action " + action.castDelay);
				yield return new WaitForSeconds(action.castDelay);

				// start casting
				Debug.Log("starting casting action");
				faceDirection(action.getFacingDirection());
				GameManager.Instance.displayBossActionUI(action);
				yield return new WaitForSeconds(action.castTime);

				// give time for the animation
				Debug.Log("now activating action " + action.name);
				yield return new WaitForSeconds(action.castAnimationTime);

				// perform the action todo
				Debug.Log("now playing action " + action.name);


			} else {
				Debug.Log("End of boss Actions");
				canPlayAction = false;
				StopCoroutine(playActions());
			}
		}
	}


}
