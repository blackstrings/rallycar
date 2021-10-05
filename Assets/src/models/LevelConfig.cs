using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use to record the selected level config as user brose menu
/// </summary>
public class LevelConfig
{
    public string levelName;
    public string startingCheckpointName;
    public string strategyName;
    public string playerClassTypeName;
    public int playerId;

    public StrategyModel strategy;
}
