using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public EventManager eventManager;

	public int mainPlayerId;
	private LevelInfo savedLevelInfo;

    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

	// Start is called before the first frame update
	void Start()
    {
		if(!eventManager) { throw new UnityException("eventManager null"); }
	}

	// save data for upcoming level on start
	public void saveLevelInfo() {

	}


}
