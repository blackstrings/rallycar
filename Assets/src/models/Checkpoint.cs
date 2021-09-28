using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Contains a list of boss actions
/// We are using newtonsoft json serializer due to nested arrays
/// hint https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
/// actionQueues
/// </summary>
[Serializable]
public class Checkpoint {

	/// <summary>
	/// name of the phase
	/// </summary>
	public string name;

	/// <summary>
	/// The action ids this checkpoint contains
	/// </summary>
	public List<List<int>> actionOrderIds;

	/// <summary>
	/// Returns one or more action(s) for this checkpoint.
	/// Actions may be returned in random order.
	/// </summary>
	/// <param name="allActionQueues"></param>
	/// <returns></returns>
	public List<ActionQueue> GetActionQueues(List<ActionQueue> allActionQueues) {

		// turn list into map
		Dictionary<int, ActionQueue> allActionsDict = GetAllActionsAsDictionary(allActionQueues);

		// collect the actions for this checkpoint
		List<ActionQueue> finalActionList = new List<ActionQueue>();

		if (actionOrderIds.Count > 0) {
			//Debug.Log("posible Rows: " + actionOrderIds.Count);
			actionOrderIds.ForEach(idsList => {

			if (idsList.Count != 0) {

				if (idsList.Count > 1) {
					// it's possible a list of random actions are returned
					List<ActionQueue> actions = pickRandomActions(idsList, allActionsDict);
					if (actions.Count > 0) {
						actions.ForEach(action => {
							finalActionList.Add(action);
						});
					} else {
						Debug.Log("failed to add randomized action, count is 0");
					}

				} else {
					if (allActionsDict.ContainsKey(idsList[0])) {
						ActionQueue action = allActionsDict[idsList[0]];
						if (action != null) {
							finalActionList.Add(action);
						} else {
							Debug.Log("No action found for id: " + idsList[0]);
						}
					} else {
						Debug.LogWarning("action id " + idsList[0] + " not exist in dictionary");
						}
					}

				}
			});

		} else {
			Debug.Log("no actions extracted, actionOrderIds null or empty");
		}

		//Debug.Log("total rows: " + finalActionList.Count);
		return finalActionList;
	}

	/// <summary>
	/// When there is an array of actions, only one action can be choosen if 999 isn't present.
	/// if 999 present, we use it as a flag to indicate that all actions should be put in random order and all returned.
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="actionsMap"></param>
	/// <returns></returns>
	private List<ActionQueue> pickRandomActions(List<int> ids, Dictionary<int, ActionQueue> actionsMap) {
		List<ActionQueue> actions = new List<ActionQueue>();

		if (ids.Contains(999)) {
			// return all actions but random the sort order
			ids.RemoveAt(ids.Count - 1); // pop off the 999 element at end of array
			Utils.Shuffle<int>(ids);    // randomize the order
			for(int i=0; i<ids.Count; i++){
				actions.Add(actionsMap[ids[i]]);
			}
		} else {
			// only return one action from the array
			int rand = Random.Range(0, ids.Count);
			if (actionsMap.ContainsKey(ids[rand])) {
				actions.Add(actionsMap[ids[rand]]);
			} else {
				Debug.LogError("map does not contain key " + rand);
			}
		}
		return actions;
	}

	private Dictionary<int, ActionQueue> GetAllActionsAsDictionary(List<ActionQueue> actions) {
		Dictionary<int, ActionQueue> map = new Dictionary<int, ActionQueue>();
		actions.ForEach(action => {
			map.Add(action.id, (ActionQueue)action.Clone());
		});
		return map;
	}
}
