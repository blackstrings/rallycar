using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lives on the marker. does a distance check when the boss action is performed.
/// if plyer is near marker, then success, otherwise not success.
/// </summary>
public class MarkerChecker : MonoBehaviour {
	private GameObject mainPlayer;

	public GameObject xMarker;

	public ParticleSystem ps;

	// Start is called before the first frame update
	void Start() {
		ps.Stop();
		setXVisible(false);

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
			setXVisible(true);
			StartCoroutine(playXMarker());
		} else {
			Debug.Log("Success was inside marker");
			ps.Play();
		}
	}

	IEnumerator playXMarker() {
		setXVisible(true);
		yield return new WaitForSeconds(2);
		setXVisible(false);
	}

	private void setXVisible(bool value) {
		xMarker.SetActive(value);
	}
}
