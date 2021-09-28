using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public int mainPlayerId;
	public LevelModel selectedLevel = new LevelModel();
	public ActionQueueLoader actionLoader;


    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() {
		actionLoader = null;
	}

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(this.gameObject);
	}



}
