using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Progress bar. tutorial https://www.youtube.com/watch?v=J1ng1zA3-Pk
/// </summary>
public class ProgressBar : MonoBehaviour
{
	public Image image;
	public bool canPlay = false;
	public float status = 0f;
	private float startTime;
	private float currTime;

	// Start is called before the first frame update
    void Start()
    {
        if(!image) {
			throw new UnityException("image null");
		}
	}

	public void restart(float time) {
		startTime = time;
		currTime = time;
		canPlay = true;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if(canPlay && 1f - status > 0) {
			currTime -= Time.deltaTime;
			status = currTime / startTime;
			image.fillAmount = 1f - status;
		}

	}
}
