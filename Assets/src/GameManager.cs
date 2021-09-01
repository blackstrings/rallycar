using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

	public UIManager uiManager;
	public int mainPlayerId;

	public List<Player> players = new List<Player>();

    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

    // Start is called before the first frame update
    void Start()
    {
       
    }

	public void Reset() {
		players.Clear();
		players.ForEach(p => {
			p.Reset();
		});

	}

	public void UIDisplayBossUpcomingAction(string name) {
		uiManager.displayBossActionName(name);
	}

	public void displayBossActionUI(ActionQueue action) {
		uiManager.handleBossAction(action);
	}

}
