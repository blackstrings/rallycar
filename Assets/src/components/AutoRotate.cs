using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{

	public float speed = .1f;
	public float x = 0f;
	public float y = 0f;
	public float z = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		gameObject.transform.Rotate(x * speed, y * speed, z * speed);
    }
}
