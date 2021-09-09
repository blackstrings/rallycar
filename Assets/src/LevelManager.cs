using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour {
	private Boss boss;

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
	}

	IEnumerator Init() {
		// always wait for 1 second before starting round to give time for everythign to setup
		yield return new WaitForSeconds(3);

		LevelInfo level = GameManager.Instance.savedLevelInfo;
		if (level != null) {
			loadLevel(level);
		} else {
			throw new UnityException("level init failed, level null");
		}
	}

	public void loadLevel(LevelInfo levelInfo) {
		// load boss


		// load all players

		// start the level
		StartRound(); // todo use a timer or something
	}

	void StartRound() {
		if (boss) {
			boss.StartRound(null);
		} else {
			throw new UnityException("StartRound failed, boss null");
		}
	}

}
