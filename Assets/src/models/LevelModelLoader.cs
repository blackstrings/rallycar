using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Use to deserialize a json text file with JsonUtility, then after loaded, you can extract the instantiated
/// actionQueues
/// </summary>
[Serializable]
public class LevelModelLoader {
	public List<LevelModel> levels;
	public Checkpoint[] checkpoints;
}
