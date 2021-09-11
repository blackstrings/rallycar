using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// custom enum for level, but might not be good to use due to dynamic levels
/// </summary>
public class LevelName {

	public LevelName E9S = new LevelName("E9S");
	public LevelName E10S = new LevelName("E10S");

	public Dictionary<string, LevelName> map = new Dictionary<string, LevelName>();

	private LevelName(string name) {
		map.Add(name, this);
	}
}