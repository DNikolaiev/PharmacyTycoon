using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour {
    public Button constructor;
    public Button researcher;
    public Button cancel;
    public Button craft;
    public HintPanel hint;
    
    public static ButtonController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
    }
    public void SwitchButtons(Button from, Button to)
    {
        to.gameObject.SetActive(true);
       from.gameObject.SetActive(false);
    }
    public void HideAllButtons()
    {
        constructor.gameObject.SetActive(false);
        researcher.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        craft.gameObject.SetActive(false);
    }
    public void ShowButtons(Button[] btns)
    {
        foreach(Button btn in btns)
        {
            btn.gameObject.SetActive(true);
        }
    }
    public void ShowAllButtons()
    {
        Button[] btns = { craft, constructor, cancel, researcher };
        foreach (Button btn in btns)
        {
            if(btn!=cancel)
            btn.gameObject.SetActive(true);
        }
    }
}
