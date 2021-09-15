using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lives on the marker. does a distance check when the boss action is performed.
/// if plyer is near marker, then success, otherwise not success.
/// </summary>
public class MarkerChecker : MonoBehaviour {
	private GameObject mainPlayer;

	// Start is called before the first frame update
	void Start() {

		// todo will need this to dynamically find the main player by id
		//List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		//int mainPlayerId = GameManager.Instance.mainPlayerId;
		//if (mainPlayerId == 0) {
		//	throw new UnityException("one player has id of 0");
		//}
		//	players.ForEach(p => {
		//	Player player = p.GetComponent<Player>();
		//	if (player) {
		//		if (player.id == mainPlayerId) {
		//			mainPlayer = p;
		//			return;
		//		}
		//	} else {
		//		throw new UnityException("player null");
		//	}
		//});

		mainPlayer = GameObject.FindWithTag("mainPlayer");

		// listener
		EventManager.onBossActionCastedAlert += checkMainPlayerWitinDistance;
	}

	private void OnDestroy() {
		EventManager.onBossActionCastedAlert -= checkMainPlayerWitinDistance;
	}

	public void checkMainPlayerWitinDistance(ActionQueue action) {
		if (Vector3.Distance(transform.position, mainPlayer.transform.position) > 3) {
			Debug.Log("Failed to be in marker");
		} else {
			Debug.Log("Success was inside marker");
		}
	}
}
