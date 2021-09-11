using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPDropdown = TMPro.TMP_Dropdown;
using OptionData = TMPro.TMP_Dropdown.OptionData;

/// <summary>
/// Handles drop downs.
/// </summary>
public class Menu : MonoBehaviour {

	/// <summary> level </summary>
	public GameObject dropdown1;
	/// <summary> playstyle </summary>
	public GameObject dropdown2;
	/// <summary> class </summary>
	public GameObject dropdown3;
	/// <summary> checkpoint </summary>
	public GameObject dropdown4;

	// level to playstyle mapping
	private Dictionary<string, List<string>> levelPlaystylesMap = new Dictionary<string, List<string>>();
	// playstyle to class
	//private Dictionary<string, List<string>> levelPlaystylesMap = new Dictionary<string, List<string>>();


	// Start is called before the first frame update
	void Start() {
		OnValidate();
		loadAndPopulateMenu();
	}

	private void OnValidate() {
		bool result = true;

		if (dropdown1 == null) { result = false; }
		if (dropdown2 == null) { result = false; }
		if (dropdown3 == null) { result = false; }
		if (dropdown4 == null) { result = false; }

		if (!result) {
			throw new UnityException("invalid setup");
		}
	}

	private void loadAndPopulateMenu() {
		// make web calls to get the level data, playstyle, etc

		//mock level datas todo load the json files
		string[] _levelNames = { "E9S", "E10S", "E11S", "E12S" };
		List<string> levelNames = new List<string>(_levelNames);

		// mock playstyle todo create an object for playstyle
		string[] _playStyleNames = { "JP1", "Happy", "Test1" };
		List<string> playStyleNames = new List<string>(_playStyleNames);

		updateLevelDropdown(levelNames);

		// setup playstyle mapping

	}

	public void loadLevelData() {

	}


	private void updateLevelDropdown(List<string> levelNames) {
		TMPDropdown[] ddList = dropdown1.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {
			addToDropDown(dd, levelNames);
			// add event listener on select
			dd.onValueChanged.AddListener(delegate {
				UpdatePlaystyleDropdown(dd.value);
			});
		} else {
			Debug.Log("updateLevelDropdown failed, dd null");
		}
	}

	public void UpdatePlaystyleDropdown(int levelNameIndex) {
		TMPDropdown[] ddList = dropdown2.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {
			// pull the play style based on the level name
			string[] playstyleNames = { "JP1", "Mr Happy", "test" };
			addToDropDown(dd, new List<string>(playstyleNames));
		} else {
			Debug.Log("updatePlaystyleDropdown failed, dd null");
		}
	}

	private void addToDropDown(TMPDropdown dropdown, List<string> items) {
		if (items != null && items.Count > 0) {
			if (dropdown != null) {
				dropdown.ClearOptions();
				List<TMPDropdown.OptionData> options = new List<TMPDropdown.OptionData>();
				items.ForEach(item => {
					options.Add(new OptionData(item));
				});
				dropdown.AddOptions(options);
			} else {
				throw new UnityException("dropdown null");
			}
		} else {
			throw new UnityException("items null or empty");
		}
	}

	// buttone click event to go to level fight
	public void GoToBossFight() {
		SceneManager.LoadScene(1);
	}
}
