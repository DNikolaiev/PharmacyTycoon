using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoDisabler : MonoBehaviour {

    private void OnEnable()
    {
        EventManager.StartListening("OnGameEnable", Activate);
        EventManager.StartListening("OnGameDisable", Deactivate);
    }
    private void OnDisable()
    {
        EventManager.StopListening("OnGameEnable", Activate);
        EventManager.StopListening("OnGameDisable", Deactivate);
    }
    private void Activate()
    {
        GetComponent<Button>().enabled = true;
    }
    private void Deactivate()
    {
        GetComponent<Button>().enabled = false;
    }
}
