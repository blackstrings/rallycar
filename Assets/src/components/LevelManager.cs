using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour {
	private Boss boss;

	public GameObject goToMarker_pf;
	private GameObject goToMarker;

	/// <summary>
	/// Must drag and link up the instances in the editor before runtime
	/// </summary>
	public List<GameObject> wayMarkers_pf = new List<GameObject>();

	// Start is called before the first frame update
	void Start() {
		GameObject bossGO = GameObject.FindGameObjectsWithTag("boss")[0];
		if (bossGO) {
			boss = bossGO.GetComponent<Boss>();
		} else {
			Debug.Log("boss gameobject not found");
		}

		if (boss) {
			StartCoroutine(Init());
		} else {
			Debug.Log("boss componen not found");
		}

		goToMarker = GameObject.Instantiate<GameObject>(goToMarker_pf);
		goToMarker.tag = "goToMarker";
	}

	IEnumerator Init() {
		// always wait for 1 second before starting round to give time for everythign to setup
		yield return new WaitForSeconds(3);

		LevelConfig config = GameManager.Instance.levelConfig;
		loadLevel(config);
	}

	public void loadLevel(LevelConfig config) {
		// load way markers
		loadWayMarkers(config);


		// load boss

		// load all players

		// start the level
		StartRound(); // todo use a timer or something
	}

	private void loadWayMarkers(LevelConfig config) {
		if (config != null) {
			if (config.strategy != null) {
				List<Vector3> wm = StrategyModel.getAllWayMakers(config.strategy.waymakers);
				if (wayMarkers_pf != null && wayMarkers_pf.Count == 8) {

					// position the instances to their data if pos values are not all 3 equal to zero
					// zero means don't  use the wak marker

					// 1
					Vector3 wm1 = wm[0];
					if (!(wm1.x == 0 && wm1.y == 0 && wm1.z == 0)) {
						wayMarkers_pf[0].transform.position = wm1;
					}

					// 2
					Vector3 wm2 = wm[1];
					if (!(wm2.x == 0 && wm2.y == 0 && wm2.z == 0)) {
						wayMarkers_pf[1].transform.position = wm2;
					}

					// 3
					Vector3 wm3 = wm[2];
					if (!(wm3.x == 0 && wm3.y == 0 && wm3.z == 0)) {
						wayMarkers_pf[2].transform.position = wm3;
					}

					// 4
					Vector3 wm4 = wm[3];
					if (!(wm4.x == 0 && wm4.y == 0 && wm4.z == 0)) {
						wayMarkers_pf[3].transform.position = wm4;
					}

					// A
					Vector3 wmA = wm[4];
					if (!(wmA.x == 0 && wmA.y == 0 && wmA.z == 0)) {
						wayMarkers_pf[4].transform.position = wmA;
					}

					// B
					Vector3 wmB = wm[5];
					if (!(wmB.x == 0 && wmB.y == 0 && wmB.z == 0)) {
						wayMarkers_pf[5].transform.position = wmB;
					}

					// C
					Vector3 wmC = wm[6];
					if (!(wmC.x == 0 && wmC.y == 0 && wmC.z == 0)) {
						wayMarkers_pf[6].transform.position = wmC;
					}

					// D
					Vector3 wmD = wm[7];
					if (!(wmD.x == 0 && wmD.y == 0 && wmD.z == 0)) {
						wayMarkers_pf[7].transform.position = wmD;
					}

				}
			} else {
				Debug.LogWarning("waymarkers failed, strategy waymarkers null");
			}
		} else {
			Debug.LogWarning("waymarkers failed, config null");
		}
	}

	void StartRound() {
		if (boss) {
			ActionQueueLoader loader = GameManager.Instance.actionLoader;
			if (loader != null) {
				Debug.Log("Boss action queues available, Using boss action from network");
				// note: change back to null to use boss data from network
				boss.StartRound(loader);
			} else {
				Debug.LogWarning("boss action failed, passing null to start round");
				boss.StartRound(null);
			}
		} else {
			throw new UnityException("StartRound failed, boss null");
		}
	}

}
