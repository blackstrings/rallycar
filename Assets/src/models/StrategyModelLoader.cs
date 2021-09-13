using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Use to deserialize a json text file with JsonUtility, then after loaded, you can extract the instantiated
/// actionQueues
/// </summary>
[Serializable]
public class StrategyModelLoader {
	public List<StrategyModel> strategies;

	//public List<StrategyModel> getStrategyByLevel(string level) {
	//	List<StrategyModel> strategies = new List<StrategyModel>();
	//	strategies.ForEach(strategy => {
	//		if (strategy.levelName.Equals(level)) {
	//			strategies.Add(strategy);
	//		}
	//	});
	//	return strategies;
	//}

	public List<string> getStrategyNamesByLevel(string level) {
		List<string> stratNames = new List<string>();
		if(strategies != null && strategies.Count > 0) {
			strategies.ForEach(strat => {
				if (strat.levelName.Equals(level)) {
					stratNames.Add(strat.strategyName);
				}
			});
		} else {
			Debug.Log("getStrategyNamesByLevel failed, strategies null");
		}
		return stratNames;
	}
}
