using System;
using UnityEngine;

/// <summary>
/// Read from .json text file https://forum.unity.com/threads/how-to-read-json-file.401306/
/// </summary>
[Serializable]
public class ActionQueue
{
	public string name = "action no name";

	// to see the name of the action aoe, tank buster,
	public string actionDesc = "no description";


	public float castDelay = 5f;
	public float castTime = 5f;
	public float castAnimationTime = 5f;

	public float positionSpeed = 5f;
	public float[] position;
	//public float posX = 0f;
	//public float posY = 0f;
	//public float posZ = 0f;

	// face the user in the direciton
	public float[] faceDirection;
	//public float faceX = 0f;
	//public float faceY = 1f;
	//public float faceZ = 0f;

	public Vector3 getPosition() {
		return new Vector3(position[0], position[1], position[2]);
	}

	public Vector3 getFacingDirection() {
		return new Vector3(faceDirection[0], faceDirection[1], faceDirection[2]);
	}

	// https://stackoverflow.com/questions/30056471/how-to-make-the-script-wait-sleep-in-a-simple-way-in-unity
	// dynamically instantiate and the real action class and play it
	public void play() {
		// instantiate the action class

		// play the the aciton class
	}
}
