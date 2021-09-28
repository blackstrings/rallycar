using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class DataService : MonoBehaviour {

	private LevelModelLoader levelLoader;
	private ActionQueueLoader actionLoader;
	private StrategyModelLoader strategyLoader;

	// ---- defaults ---- //
	public TextAsset defaultLevelJson;
	public TextAsset defaultStrategyJson;

	public Menu menu;


	// Start is called before the first frame update
	void Start() {
		if (!defaultLevelJson || !defaultStrategyJson) {
			throw new UnityException("DataService failed to load, default json not referenced");
		}

		menu = GetComponent<Menu>();
		if (menu) {
			StartCoroutine(LoadAllGameData());
		} else {
			throw new UnityException("DataService start failed, menu null");
		}
	}

	IEnumerator LoadAllGameData() {
		Coroutine a = StartCoroutine(loadAllLevelData());
		Coroutine b = StartCoroutine(loadStrategyData());
		Coroutine c = StartCoroutine(loadBossE9SData());


		yield return a;
		yield return b;
		yield return c;

		if (levelLoader != null || strategyLoader != null) {
			menu.populateMenu(levelLoader, strategyLoader, actionLoader);
		} else {
			Debug.Log("menu not popluated, levels null");
		}
	}

	IEnumerator loadStrategyData() {
		string url = "http://xailao.com/gamename/strategy";
		//string url = query;

		UnityWebRequest www = getWebRequest(url, null);
		yield return www.SendWebRequest();

		string gameDataJson;
		if (www.result == UnityWebRequest.Result.ConnectionError) {
			Debug.Log(www.error);
			Debug.Log("Load Strategy data failed using default strategy data");
			gameDataJson = defaultStrategyJson.text;

		} else {
			Debug.Log("Load Strategy data success");
			//Debug.Log(www.downloadHandler.text);
			gameDataJson = www.downloadHandler.text;
		}

		DeserializeStrategyData(gameDataJson);
	}

	/// <summary>
	/// If the network is down, it'll fallback to the gamedata build with the version.
	/// </summary>
	IEnumerator loadAllLevelData() {
		Debug.Log("Loading boss level data");
		string url = "http://www.noApiYet.com";
		// string url = "http://www.rainkey.io/simulations/1.json";

		UnityWebRequest www = getWebRequest(url, null);
		yield return www.SendWebRequest();

		string gameDataJson;
		if (www.result == UnityWebRequest.Result.ConnectionError) {
			Debug.Log(www.error);
			Debug.Log("Level data load failed using default level data");
			gameDataJson = defaultLevelJson.text;

		} else {
			Debug.Log("Level data loaded success");
			gameDataJson = www.downloadHandler.text;
		}

		DeserializeLevelData(gameDataJson);
	}

	/// <summary>
	/// If the network is down, it'll fallback to the gamedata build with the version.
	/// </summary>
	IEnumerator loadBossE9SData() {
		Debug.Log("Loading boss level data");
		// string url = "http://www.noApiYet.com";
		string url = "http://www.rainkey.io/simulations/1.json";

		UnityWebRequest www = getWebRequest(url, null);
		yield return www.SendWebRequest();

		string gameDataJson;
		if (www.result == UnityWebRequest.Result.ConnectionError) {
			Debug.Log(www.error);
			Debug.Log("Boss data load failed using default level data");
			gameDataJson = defaultLevelJson.text;

		} else {
			Debug.Log("Boss data loaded success");
			gameDataJson = www.downloadHandler.text;
		}

		DeserializeBossE9SData(gameDataJson);
	}

	private UnityWebRequest getWebRequest(string url, Dictionary<string, string> hash) {
		//json data - works with only UnityWebRequest
		//string jstr = "{\'name\':\'john\'}";

		//form data - works with both UnityWebRequest and WWW
		//WWWForm f = new WWWForm();
		//f.AddField("name", "Kimmy");
		//f.AddField("gameid", "1001001");
		//f.AddField("user", "222-0001");

		// Hash data -works with only UnityWebRequest
		//Dictionary<string, string> hash = new Dictionary<string, string>();
		//hash.Add("name", "john");

		// if not getting correct data, make sure GET POST UPDATE are correct
		UnityWebRequest www = UnityWebRequest.Get(url);
		//UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("Content-Type", "application/json");
		//www.SetRequestHeader("Content-Type", "text/json");

		www.downloadHandler = new DownloadHandlerBuffer();
		return www;
	}

	private void DeserializeBossE9SData(string gameDataJson) {
		if (gameDataJson != null && gameDataJson.Length > 0) {
			actionLoader = JsonConvert.DeserializeObject<ActionQueueLoader>(gameDataJson);
			if (actionLoader == null) {
				throw new UnityException("actionLoader null");
			}
		}
	}

	private void DeserializeLevelData(string gameDataJson) {
		if (gameDataJson != null && gameDataJson.Length > 0) {
			levelLoader = JsonConvert.DeserializeObject<LevelModelLoader>(gameDataJson);
			if (levelLoader == null) {
				throw new UnityException("levelLoader null");
			}
		}
	}

	private void DeserializeStrategyData(string json) {
		if (json != null && json.Length > 0) {
			strategyLoader = JsonConvert.DeserializeObject<StrategyModelLoader>(json);
			if (strategyLoader == null) {
				throw new UnityException("strategyLoader null");
			}
		}
	}

}
