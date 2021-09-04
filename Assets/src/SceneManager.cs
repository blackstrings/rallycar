using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        // load boss script from manager

        // load players from manager
        Debug.Log("Starting round", gameObject);
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
