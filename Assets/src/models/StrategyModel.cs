using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyModel {
	public string levelName;
	public string strategyName;
	public List<string> classTypes;
	public List<List<int>> waymakers;

	/// <summary>
	/// Converts the raw waymarkers into vectors/positions.
	/// There should usually be 8 way markers 1,2,3,4,a,b,c,d in that order.
	/// </summary>
	/// <returns></returns>
	public static List<Vector3> getAllWayMakers(List<List<int>> allWayMarkers) {
		List<Vector3> markers = new List<Vector3>();
		if (allWayMarkers != null && allWayMarkers.Count > 0) {
			// for each waymarker, there are 3 numbers for xyz
			allWayMarkers.ForEach(marker => {
				markers.Add(new Vector3(marker[0], marker[1], marker[2]));
			});
		} else {
			Debug.LogWarning("waymarkers null or empty");
		}
		return markers;
	}
}
