using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPDropdown = TMPro.TMP_Dropdown;
using OptionData = TMPro.TMP_Dropdown.OptionData;

/// <summary>
/// Handles drop downs.
/// </summary>
public class Menu : MonoBehaviour {

	/// <summary> level </summary>
	public GameObject level_dd;
	/// <summary> playstyle </summary>
	public GameObject dropdown2;
	/// <summary> class </summary>
	public GameObject dropdown3;
	/// <summary> checkpoint </summary>
	public GameObject checkpoint_dd;

	// provided on data load
	private List<LevelModel> levels;

	// level to playstyle mapping
	private Dictionary<string, List<LevelModel>> allLevelModelMap = new Dictionary<string, List<LevelModel>>();
	// playstyle to class
	//private Dictionary<string, List<string>> levelPlaystylesMap = new Dictionary<string, List<string>>();

	// Start is called before the first frame update
	void Start() {
		OnValidate();
	}

	private void OnValidate() {
		bool result = true;

		if (level_dd == null) { result = false; }
		if (dropdown2 == null) { result = false; }
		if (dropdown3 == null) { result = false; }
		if (checkpoint_dd == null) { result = false; }

		if (!result) {
			throw new UnityException("invalid setup");
		}
	}

	/// <summary>
	/// Dataservice calls this
	/// </summary>
	/// <param name="levels"></param>
	public void populateMenu(List<LevelModel> levels) {
		updateLevelDropdown(levels);
	}

	private void updateLevelDropdown(List<LevelModel> levels) {
		TMPDropdown[] ddList = level_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd && levels != null && levels.Count > 0) {
			this.levels = levels;
			// get level name as list
			List<string> levelNames = new List<string>();
			levels.ForEach(level => {
				levelNames.Add(level.name);
			});

			addToDropDown(dd, levelNames);

			// update other drop downs on selection
			dd.onValueChanged.AddListener(delegate {
				UpdateCheckpointDropdowns(dd.value);
			});
		} else {
			Debug.Log("updateLevelDropdown failed, dd null");
		}
	}

	/// <summary>
	/// Strategy is also aka playstyle for the level
	/// </summary>
	/// <param name="selectedLevelIndex"></param>
	private void UpdateCheckpointDropdowns(int selectedLevelIndex) {
		TMPDropdown[] ddList = checkpoint_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {

			if(levels != null) {
				// pull the play style based on the level name
				LevelModel selectedLevel = levels[selectedLevelIndex];
				if(selectedLevel != null) {
					List<string> checkpoints = selectedLevel.checkpoints;
					addToDropDown(dd, checkpoints);
				}
			}
		} else {
			Debug.Log("updatePlaystyleDropdown failed, dd null");
		}
	}

	/// <summary>
	/// Add a list of strings values to a specific drop down
	/// </summary>
	/// <param name="dropdown"></param>
	/// <param name="items"></param>
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
