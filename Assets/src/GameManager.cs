using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

	public UIManager uiManager;
	public EventManager eventManager;

	public int mainPlayerId;

	public List<Player> players = new List<Player>();

    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

	// Start is called before the first frame update
	void Start()
    {
		if(!uiManager) { throw new UnityException("uIManager null"); }
		if(!eventManager) { throw new UnityException("eventManager null"); }
	}

	public void Reset() {
		players.Clear();
		players.ForEach(p => {
			p.Reset();
		});

	}


}
