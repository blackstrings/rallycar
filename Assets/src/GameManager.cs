using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int score;

    // (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

}
