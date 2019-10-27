using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour {

    
    public GameObject[] tabs;
    public List<GameObject> exceptions;
    public int defaultTab;
    public void SwitchToTab(int tabNumber)
    {
        foreach (GameObject tab in tabs)
        {
            for (int i = 0; i < tab.transform.childCount; i++)
            {
                if(! exceptions.Contains(tab.transform.GetChild(i).gameObject))
                tab.transform.GetChild(i).gameObject.SetActive(false);
            }
            tab.gameObject.SetActive(false);
        }

        tabs[tabNumber].gameObject.SetActive(true);
        for (int i = 0; i < tabs[tabNumber].transform.childCount; i++)
        {
            if (!exceptions.Contains(tabs[tabNumber].transform.GetChild(i).gameObject))
                tabs[tabNumber].transform.GetChild(i).gameObject.SetActive(true);
        }
     
    }

    
    private void Start()
    {
        SwitchToTab(defaultTab);
    }
}
