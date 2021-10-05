using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public ProgressBar bossCastBar;
	public Text bossActionNameTxt;

	void Start() {
		if(!bossCastBar) {
			throw new UnityException("bossCastBar null");
		}
		EventManager.onBossUpcomingActionAlert += displayBossActionName;
		EventManager.onBossActionCastingAlert += playBossCastBar;
	}

	private void OnDestroy() {
		EventManager.onBossUpcomingActionAlert -= displayBossActionName;
		EventManager.onBossUpcomingActionAlert -= playBossCastBar;
	}

	public void displayBossActionName(ActionQueue action) {
		bossActionNameTxt.text = action.name;
	}

	public void playBossCastBar(ActionQueue action) {
		bossCastBar.restart(action.castTime);
	}

}
