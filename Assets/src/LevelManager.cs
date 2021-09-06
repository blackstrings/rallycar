using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public Boss boss;

	// Start is called before the first frame update
	void Start() {
		init();
	}

	private void init() {
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
