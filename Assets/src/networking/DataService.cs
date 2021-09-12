using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class DataService : MonoBehaviour {

	private LevelModelLoader levelLoader;
	private List<LevelModel> levels;
	private List<StrategyModel> strategies;

	// ---- defaults ---- //
	public TextAsset defaultLevelJson;

	public Menu menu;


	// Start is called before the first frame update
	void Start() {
		if (!defaultLevelJson) {
			throw new UnityException("DataService failed to load, default json not referenced");
		}

		menu = GetComponent<Menu>();
		if (menu) {
			StartCoroutine(LoadGameData());
		} else {
			throw new UnityException("DataService start failed, menu null");
		}
	}

	IEnumerator LoadGameData() {
		Coroutine a = StartCoroutine(getGameData());
		//Coroutine b = StartCoroutine(getStrategyData());

		yield return a;
		//yield return b;

		if (levels != null) {
			menu.populateMenu(levels);
		} else {
			Debug.Log("menu not popluated, levels null");
		}
	}

	//IEnumerator getStrategyData() {

	//}

	/// <summary>
	/// If the network is down, it'll fallback to the gamedata build with the version.
	/// </summary>
	IEnumerator getGameData() {
		string url = "http://xailao.com/games/poplopoly/retreive.php?query=top10";
		//string url = query;

		//json data - works with only UnityWebRequest
		//string jstr = "{\'name\':\'john\'}";

		//form data - works with both UnityWebRequest and WWW
		//WWWForm f = new WWWForm();
		//f.AddField("name", "Kimmy");
		//f.AddField("gameid", "1001001");
		//f.AddField("user", "222-0001");

		// Hash data -works with only UnityWebRequest
		Dictionary<string, string> hash = new Dictionary<string, string>();
		hash.Add("name", "john");

		UnityWebRequest www = UnityWebRequest.Post(url, hash);
		//UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("Content-Type", "application/json");
		//www.SetRequestHeader("Content-Type", "text/json");

		www.downloadHandler = new DownloadHandlerBuffer();

		yield return www.SendWebRequest();

		string gameDataJson;
		if (www.result == UnityWebRequest.Result.ConnectionError) {
			Debug.Log(www.error);
			Debug.Log("Game network data failed using default game data");
			gameDataJson = getDefaultGameData();

		} else {
			Debug.Log("Game network data success");
			Debug.Log(www.downloadHandler.text);
			gameDataJson = www.downloadHandler.text;
		}

		levels = DeserializeLevelData(gameDataJson);

		// wait on all data before populating the menu

	}

	private string getDefaultGameData() {
		return defaultLevelJson.text;
	}

	private List<LevelModel> DeserializeLevelData(string gameDataJson) {
		if (gameDataJson != null && gameDataJson.Length > 0) {
			levelLoader = JsonConvert.DeserializeObject<LevelModelLoader>(gameDataJson);
			if (levelLoader != null) {
				return levelLoader.levels;
			} else {
				throw new UnityException("levelLoader null");
			}
		}
		Debug.Log("deserializeLevelData failed, gameDataJson null");
		return null;
	}

	public List<LevelModel> getLevels() {
		if (levels != null && levels.Count > 0) {
			return levels;
		} else {
			Debug.LogWarning("getLevels failed, levels null");
			return null;
		}
	}
}
