using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPDropdown = TMPro.TMP_Dropdown;
using OptionData = TMPro.TMP_Dropdown.OptionData;

/// <summary>
/// Handles populating the drop downs dynamically.
/// </summary>
public class Menu : MonoBehaviour {

	/// <summary> level </summary>
	public GameObject level_dd;
	/// <summary> playstyle </summary>
	public GameObject strategy_dd;
	/// <summary> class </summary>
	public GameObject classType_dd;
	/// <summary> checkpoint </summary>
	public GameObject checkpoint_dd;

	private LevelModelLoader levelLoader;
	private StrategyModelLoader strategyLoader;

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
		if (strategy_dd == null) { result = false; }
		if (classType_dd == null) { result = false; }
		if (checkpoint_dd == null) { result = false; }

		if (!result) {
			throw new UnityException("invalid setup");
		}
	}

	/// <summary>
	/// Dataservice calls this
	/// </summary>
	/// <param name="levels"></param>
	public void populateMenu(LevelModelLoader levelLoader, StrategyModelLoader strategyLoader) {
		if (levelLoader != null || strategyLoader != null) {
			this.levelLoader = levelLoader;
			this.strategyLoader = strategyLoader;
			updateLevelDropdown(levelLoader.levels);
		}
	}

	private void updateLevelDropdown(List<LevelModel> levels) {
		TMPDropdown[] ddList = level_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd && levels != null && levels.Count > 0) {

			// get level name as list
			List<string> levelNames = new List<string>();
			levels.ForEach(level => {
				levelNames.Add(level.name);
			});

			addToDropDown(dd, levelNames);
			// force selected checkpoint on first pass
			UpdateCheckpointDropdowns(0);
			UpdateStrategyDropdowns(0);
			UpdateClassTypeDropdowns(0);

			// update other drop downs on future selection
			dd.onValueChanged.AddListener(delegate {
				UpdateCheckpointDropdowns(dd.value);
				UpdateStrategyDropdowns(dd.value);
			});
		} else {
			Debug.Log("updateLevelDropdown failed, dd null");
		}
	}

	/// <summary>
	/// Checkpoints available reflects the selected level
	/// </summary>
	/// <param name="selectedIndex"></param>
	private void UpdateCheckpointDropdowns(int selectedIndex) {
		TMPDropdown[] ddList = checkpoint_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {

			if (levelLoader != null) {
				// pull the play style based on the level name
				LevelModel selectedLevel = levelLoader.levels[selectedIndex];
				if (selectedLevel != null) {
					List<string> checkpoints = selectedLevel.checkpoints;
					addToDropDown(dd, checkpoints);
				}
			}
		} else {
			Debug.Log("updatePlaystyleDropdown failed, dd null");
		}
	}

	/// <summary>
	/// Strategy available reflects the selected level
	/// </summary>
	/// <param name="selectedIndex"></param>
	private void UpdateStrategyDropdowns(int selectedIndex) {
		TMPDropdown[] ddList = strategy_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {

			if (levelLoader != null) {
				// pull the play style based on the level name
				LevelModel selectedLevel = levelLoader.levels[selectedIndex];
				if (selectedLevel != null) {
					if (strategyLoader != null) {
						List<string> strategies = strategyLoader.getStrategyNamesByLevel(selectedLevel.name);
						addToDropDown(dd, strategies);

					} else {
						Debug.Log("UpdateStrategyDropdowns failed, strategyLoader null");
					}
				} else {
					Debug.Log("UpdateStrategyDropdowns failed, selectedLevel null");
				}
			}

			// when strategy selected update the class
			dd.onValueChanged.AddListener(delegate {
				UpdateClassTypeDropdowns(dd.value);
			});

		} else {
			Debug.Log("UpdateStrategyDropdowns, dd null");
		}
	}

	/// <summary>
	/// Strategy available reflects the selected level
	/// </summary>
	/// <param name="selectedStrategyIndex"></param>
	private void UpdateClassTypeDropdowns(int selectedStrategyIndex) {
		TMPDropdown[] ddList = classType_dd.GetComponents<TMPDropdown>();
		TMPDropdown dd = ddList[0];
		if (dd != null && dd) {

			if (strategyLoader != null) {
				// pull the play style based on the level name
				StrategyModel selectedStrategy = strategyLoader.strategies[selectedStrategyIndex];
				if (selectedStrategy != null) {
					List<string> classTypes = selectedStrategy.classTypes;
					addToDropDown(dd, classTypes);

				} else {
					Debug.Log("UpdateClassTypeDropdowns failed, selectedLevel null");
				}
			} else {
				Debug.Log("UpdateClassTypeDropdowns failed, strategyLoader null");
			}
		} else {
			Debug.Log("UpdateClassTypeDropdowns, dd null");
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
