using System.Collections.Generic;
using System;

/// <summary>
/// Use to deserialize a json text file with JsonUtility, then after loaded, you can extract the instantiated
/// actionQueues
/// </summary>
[Serializable]
public class ActionPhase {
	public string name;
	public List<int> actionOrderIds;
}
