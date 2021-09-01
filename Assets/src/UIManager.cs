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
	}

	public void displayBossActionName(string name) {
		bossActionNameTxt.text = name;
	}

	public void handleBossAction(ActionQueue action) {
		bossCastBar.restart(action.castTime);
	}

}
