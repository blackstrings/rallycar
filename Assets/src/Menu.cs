using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DropD = TMPro.TMP_Dropdown;


public class Menu : MonoBehaviour {
    public GameObject level_dd;


    // Start is called before the first frame update
    void Start() {
        OnValidate();
        loadAndPopulateMenu();
    }

    private void OnValidate() {
        if (level_dd == null) {
            throw new UnityException("invalid setup");
		}
	}

	private void loadAndPopulateMenu() {
        updateLevelDropdown();
    }

    private void updateLevelDropdown() {
        DropD[] dd = level_dd.GetComponents<DropD>();
        if (dd[0]) {
            List<DropD.OptionData> options = new List<DropD.OptionData>();

            // get additional items other than the defaults

            options.Add(new DropD.OptionData("testing"));

            dd[0].AddOptions(options);
        } else {
            Debug.Log("no opptions");
        }
    }

    public void GoToBossFight() {
        SceneManager.LoadScene(1);
    }
}
