using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateAppPanel : Panel
{
    public int visitedTimes;
    public int firstShowInterval;
    public int repeatInterval;
    public string storeLink;
    public GameObject panel;
    public override void Hide()
    {
        panel.SetActive(false);
    }

    public override void SetPanel()
    {
        panel.SetActive(true);
    }
    public void OpenLink()
    {
        Application.OpenURL(storeLink);
    }
    private void IncreaseVisitedTimes()
    {
        visitedTimes++;
        if(visitedTimes==firstShowInterval)
        {
            SetPanel();
        }
        if (visitedTimes == repeatInterval)
        {
            SetPanel();
        }
        else if (visitedTimes%repeatInterval==0)
        {
            SetPanel();
        }
        PlayerPrefs.SetInt("visitedTimes", visitedTimes);
    }
    private void Start()
    {
        visitedTimes = PlayerPrefs.GetInt("visitedTimes");
       // PlayerPrefs.SetInt("visitedTimes", 0);
        
        IncreaseVisitedTimes();
       
    }
    
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("visitedTimes", visitedTimes);
    }
    
}
