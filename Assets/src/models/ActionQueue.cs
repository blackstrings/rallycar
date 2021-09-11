using System;
using UnityEngine;

/// <summary>
/// Read from .json text file https://forum.unity.com/threads/how-to-read-json-file.401306/
/// </summary>
[Serializable]
public class ActionQueue : ICloneable
{
	public int id;
	public string name = "action no name";
	public string actionDesc = "no description";

	// ---------------------------------
	// delay till cast
	public float castDelay = 2f;
	// casting delay
	public float castTime = 2f;
	// delay action after cast time is up
	public float delayTakeAction = 1f;
	// action animation delay
	public float castAnimationTime = 2f;

	// ---------------------------------
	public float positionSpeed = 5f;
	// vector xyz so holds 3 floats
	public float[] goToPosition;
	// 0 for instant teleport and above 0 for animating to position
	public float goToSpeed;

	// face the user in the direciton
	public float[] faceDirection;
	public string faceDirectionAuto;

	public Vector3 getGoToPosition() {
		return new Vector3(goToPosition[0], goToPosition[1], goToPosition[2]);
	}

	/// <summary>
    /// Direction to face once at position
    /// </summary>
    /// <returns></returns>
	public Vector3 getFacingDirection() {
		return new Vector3(faceDirection[0], faceDirection[1], faceDirection[2]);
	}

	// https://stackoverflow.com/questions/30056471/how-to-make-the-script-wait-sleep-in-a-simple-way-in-unity
	// dynamically instantiate and the real action class and play it
	public void play() {
		// instantiate the action class

		// play the the aciton class
	}

	public object Clone() {
		return this.MemberwiseClone();
	}
}
