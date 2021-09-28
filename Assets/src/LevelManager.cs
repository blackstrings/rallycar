using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour {
	private Boss boss;

	public GameObject goToMarker_pf;
	private GameObject goToMarker;

	// Start is called before the first frame update
	void Start() {
		GameObject bossGO = GameObject.FindGameObjectsWithTag("boss")[0];
		if (bossGO) {
			boss = bossGO.GetComponent<Boss>();
		} else {
			Debug.Log("boss gameobject not found");
		}

		if (boss) {
			StartCoroutine(Init());
		} else {
			Debug.Log("boss componen not found");
		}

		goToMarker = GameObject.Instantiate<GameObject>(goToMarker_pf);
		goToMarker.tag = "goToMarker";
	}

	IEnumerator Init() {
		// always wait for 1 second before starting round to give time for everythign to setup
		yield return new WaitForSeconds(3);

		LevelModel level = GameManager.Instance.selectedLevel;
		if (level != null) {
			loadLevel(level);
		} else {
			throw new UnityException("level init failed, level null");
		}
	}

	public void loadLevel(LevelModel levelInfo) {
		// load boss


		// load all players

		// start the level
		StartRound(); // todo use a timer or something
	}

	void StartRound() {
		if (boss) {
			ActionQueueLoader loader = GameManager.Instance.actionLoader;
			if (loader != null) {
				Debug.Log("Boss action queues available, Using boss action from network");
				boss.StartRound(new List<ActionQueue>(loader.actionsQueues));
			} else {
				Debug.LogWarning("boss action failed, passing null to start round");
				boss.StartRound(null);
			}
		} else {
			throw new UnityException("StartRound failed, boss null");
		}
	}

}
