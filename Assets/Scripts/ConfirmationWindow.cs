using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConfirmationWindow : Panel {

    public Button ok;
    public Button cancel;
    public Image panel;
    public Text confirmText;

   public override void Hide()
    {
        cancel.onClick.RemoveAllListeners();
        ok.onClick.RemoveAllListeners();
        Activate(false);
    }
    public void SetPanel(string txt)
    {
        confirmText.text = txt;
    }
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    private void Start()
    {

        // assign to the centre of the screen
    }
    
    public void Activate(bool state)
    {
        gameObject.SetActive(state);
    }
}
