using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a GO infinitely in one direction
/// </summary>
public class TranslatorReset : MonoBehaviour
{
	public Vector3 startPos = new Vector3(0, 0, 90);
	public bool isEnabled = false;
	public float x;
	public float y;
	public float z;
	public float speed;
	public float resetDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float t = Time.deltaTime;
		transform.Translate(x * t * speed, y * t * speed, z * t * speed);

		if(transform.position.z <= 0) {
			transform.transform.position = startPos;
		}
	}
}
