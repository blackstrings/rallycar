using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Read from .json text file https://forum.unity.com/threads/how-to-read-json-file.401306/
/// </summary>
[Serializable]
public class ActionQueue : ICloneable {
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

	// speed to each position
	public float positionSpeed = 5f;

	// vector xyz so holds 3 floats, every 3 index is a vector so it can be also used to hold array of positions
	public float[] goToPosition;

	// set to 0 for instant teleport and above 0 for slow animation to position
	public float goToSpeed;

	// face the user in the direciton after reaching position
	public float[] faceDirection;

	// set to "boss" or "away" to look at boss or away at position
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
	
	public List<Vector3> getGoToPositions() {
		List<Vector3> positions = new List<Vector3>();

		if (goToPosition.Length > 0) {
			float remainder = goToPosition.Length % 3;
			if (remainder == 0f) {
				for (int i = 0; i < goToPosition.Length; i += 3) {
					float[] pos = goToPosition;
					positions.Add(new Vector3(pos[i], pos[i + 1], pos[i + 2]));
				}
			} else {
				Debug.LogWarning("cannot deserialize into vector, array not sets of 3, remainder is not zero");
			}
		} else {
			Debug.LogWarning("getPositions failed, length is zero");
		}

		return positions;
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
