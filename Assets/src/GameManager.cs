using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public int mainPlayerId;
	public LevelModel selectedLevel = new LevelModel();


    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(this.gameObject);
	}

	// save data for upcoming level on start
	public void initSelectedLevel() {

	}


}
