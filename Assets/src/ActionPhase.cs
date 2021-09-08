using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Use to deserialize a json text file with JsonUtility, then after loaded, you can extract the instantiated
/// actionQueues
/// </summary>
[Serializable]
public class ActionPhase {
	public string name;
	public int[] actionOrderIds;

	public List<ActionQueue> GetActionQueues(List<ActionQueue> allActionQueues) {
		List<ActionQueue> phaseActionQueues = new List<ActionQueue>();

		// turn list into map
		Dictionary<int, ActionQueue> actionMap = GetListAsMap(allActionQueues);

		for(int i=0; i<actionOrderIds.Length; i++) {
		//actionOrderIds.ForEach(ids => {
			if(actionOrderIds[i] != null) {
				ActionQueue action = actionMap[actionOrderIds[i]];
				if (action != null) {
					phaseActionQueues.Add(action);
				}
			}
		}
		

		Debug.Log("actions in phase count: " + actionOrderIds.Length);

		return phaseActionQueues;
	}

	private Dictionary<int, ActionQueue> GetListAsMap(List<ActionQueue> actions) {
		Dictionary<int, ActionQueue> map = new Dictionary<int, ActionQueue>();
		actions.ForEach(action => {
			map.Add(action.id, action);
		});
		return map;
	}
}
