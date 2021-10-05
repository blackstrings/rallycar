using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour {
	public float power = 1;
	public BoxCollider bc;
	public Vector3 originPosition;
	public Vector3 facingDirection;


	// Start is called before the first frame update
	void Start() {
		facingDirection = new Vector3(transform.rotation.x, 0, 0).normalized;
	}

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyUp(KeyCode.Space)) {
			Debug.Log("Launch");
			action();
		}
	}

	public void action() {
		if (bc.isTrigger) {
			originPosition = transform.position;
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Rigidbody rg = player.GetComponent<Rigidbody>();
			if (rg) {
				//rg.AddForceAtPosition(Vector3.up, player.transform.position);
				rg.AddForce(facingDirection * power, ForceMode.Impulse);

			} else {
				Debug.Log("no rigid");
			}
		} else {
			Debug.Log("not trigger");
		}
	}
}
