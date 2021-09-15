using UnityEngine;

public static class EventManager {

	public delegate void OnBossUpcomingActionAlert(ActionQueue actionQueue);
	public static event OnBossUpcomingActionAlert onBossUpcomingActionAlert;

	public delegate void OnBossActionAlertCasting(ActionQueue actionQueue);
	public static event OnBossActionAlertCasting onBossActionCastingAlert;

	public delegate void OnBossActionCasted(ActionQueue actionQueue);
	public static event OnBossActionCasted onBossActionCastedAlert;

	// boss next action alert
	public static void alertBossUpcomingAction(ActionQueue actionQueue) {
		if (actionQueue != null) {
			onBossUpcomingActionAlert(actionQueue);
		} else {
			Debug.Log("event boss action alert failed, action null");
		}
	}

	// boss casting
	public static void alertBossActionCasting(ActionQueue actionQueue) {
		if (actionQueue != null) {
			onBossActionCastingAlert(actionQueue);
		} else {
			Debug.Log("event boss casting failed, action null");
		}
	}

	// boss action performed
	public static void alertBossActionCasted(ActionQueue actionQueue) {
		if (actionQueue != null) {
			onBossActionCastedAlert(actionQueue);
		} else {
			Debug.Log("event boss casted failed, action null");
		}
	}
}
