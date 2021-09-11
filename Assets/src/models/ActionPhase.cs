using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// An action phase will containt a list of actions under this phase.
/// We are using newtonsoft json serializer due to nested arrays
/// hint https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
/// actionQueues
/// </summary>
[Serializable]
public class ActionPhase {

	/// <summary>
	/// name of the phase
	/// </summary>
	public string name;

	/// <summary>
	/// The action ids this phase contains
	/// </summary>
	public List<List<int>> actionOrderIds;

	/// <summary>
	/// Returns the actions for this phase.
	/// </summary>
	/// <param name="allActionQueues"></param>
	/// <returns></returns>
	public List<ActionQueue> GetActionQueues(List<ActionQueue> allActionQueues) {

		// turn list into map
		Dictionary<int, ActionQueue> allActionsDict = GetAllActionsAsDictionary(allActionQueues);

		// collect the actions for this phase
		List<ActionQueue> finalActionList = new List<ActionQueue>();

		if (actionOrderIds.Count > 0) {
			//Debug.Log("posible Rows: " + actionOrderIds.Count);
			actionOrderIds.ForEach(idsList => {

				if (idsList.Count != 0) {

					if (idsList.Count > 1) {
						ActionQueue action = pickRandomAction(idsList, allActionsDict);
						if (action != null) {
							finalActionList.Add(action);
						} else {
							Debug.Log("failed to randomize action");
						}

					} else {
						ActionQueue action = allActionsDict[idsList[0]];
						if (action != null) {
							finalActionList.Add(action);
						} else {
							Debug.Log("No action found for id: " + idsList[0]);
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

	private ActionQueue pickRandomAction(List<int> ids, Dictionary<int, ActionQueue> allActions) {
		//Debug.Log("randoming between 0-" + ids.Count);
		int rand = Random.Range(0, ids.Count);
		//Debug.Log("randomed: " + rand);
		return allActions[ids[rand]];
	}

	private Dictionary<int, ActionQueue> GetAllActionsAsDictionary(List<ActionQueue> actions) {
		Dictionary<int, ActionQueue> map = new Dictionary<int, ActionQueue>();
		actions.ForEach(action => {
			map.Add(action.id, (ActionQueue)action.Clone());
		});
		return map;
	}
}
