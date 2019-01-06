using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour {

    public GameObject tab1, tab2;

    public void SwitchToTab1()
    {
        tab1.gameObject.SetActive(true);
        for (int i=0; i<tab1.transform.childCount;i++)
        {
            tab1.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < tab2.transform.childCount; i++)
        {
            tab2.transform.GetChild(i).gameObject.SetActive(false);
        }
        tab2.gameObject.SetActive(false);
    }
    public void SwitchToTab2()
    {
        tab2.gameObject.SetActive(true);
        for (int i = 0; i < tab1.transform.childCount; i++)
        {
            tab1.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < tab2.transform.childCount; i++)
        {
            tab2.transform.GetChild(i).gameObject.SetActive(true);
        }
        tab1.gameObject.SetActive(false);
    }
    private void Start()
    {
        SwitchToTab1();
    }
}
