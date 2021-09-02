using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

	public delegate void OnBossActionAlert(ActionQueue actionQueue);
	public static event OnBossActionAlert onBossUpcomingActionAlert;

	public delegate void OnBossActionAlertCasting(ActionQueue actionQueue);
	public static event OnBossActionAlertCasting onBossActionCastingAlert;

	// boss next action alert
	public void alertBossUpcomingAction(ActionQueue actionQueue) {
		onBossUpcomingActionAlert(actionQueue);
	}

	// boss casting
	public void alertBossActionCasting(ActionQueue actionQueue) {
		if(actionQueue != null) {
			onBossActionCastingAlert(actionQueue);
		} else {
			throw new UnityException("actionQueue null");
		}
	}
}
