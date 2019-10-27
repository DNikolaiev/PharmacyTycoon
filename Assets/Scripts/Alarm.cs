using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour {

    // Use this for initialization
    [SerializeField] GameObject alarmScreen;
	void OnEnable () {
        EventManager.StartListening("OnAlarmEnable", EnableAlarm);
        EventManager.StartListening("OnAlarmDisable", DisableAlarm);
        
	}
    private void Start()
    {
        
        DisableAlarm();
    }
    private void OnDisable()
    {
        EventManager.StopListening("OnAlarmEnable", EnableAlarm);
        EventManager.StopListening("OnAlarmDisable", DisableAlarm);
    }
    private void EnableAlarm()
    {
        Debug.Log("ALAAARM!!!!!!!");
        gameObject.SetActive(true);
        alarmScreen.SetActive(true);
        alarmScreen.GetComponent<Animator>().Play("Alarm_Idle");
    }
    private void DisableAlarm()
    {
        alarmScreen.SetActive(false);
    }
    

}
