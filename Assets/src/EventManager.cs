public static class EventManager
{

	public delegate void OnBossUpcomingActionAlert(ActionQueue actionQueue);
	public static event OnBossUpcomingActionAlert onBossUpcomingActionAlert;

	public delegate void OnBossActionAlertCasting(ActionQueue actionQueue);
	public static event OnBossActionAlertCasting onBossActionCastingAlert;

	// boss next action alert
	public static void alertBossUpcomingAction(ActionQueue actionQueue) {
		onBossUpcomingActionAlert(actionQueue);
	}

	// boss casting
	public static void alertBossActionCasting(ActionQueue actionQueue) {
		if(actionQueue != null) {
			onBossActionCastingAlert(actionQueue);
		} else {
			
		}
	}
}
