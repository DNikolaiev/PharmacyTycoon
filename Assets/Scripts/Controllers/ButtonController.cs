using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour {
    public Button constructor;
    public Button researcher;
    public Button cancel;
    public Button craft;
    public Button laboratory;
    public HintPanel hint;
    public ConfirmationWindow confirm;
    public MessageBox messageBox;
    public DescriptionPanel[] dPanels;
   
    public DescriptionPanel GetDescriptionPanel(ResearchHolder holder)
    {
        if (dPanels[2].gameObject.activeInHierarchy)
            return dPanels[2];
        else return null;
    }
    public DescriptionPanel GetDescriptionPanel(CraftHolder holder)
    {
        if (dPanels[0].gameObject.activeInHierarchy)
            return dPanels[0];
        else return null;
    }
    public DescriptionPanel GetDescriptionPanel(RecipeHolder holder)
    {
        if (dPanels[0].gameObject.activeInHierarchy)
            return dPanels[0];
        else if (dPanels[1].gameObject.activeInHierarchy)
            return dPanels[1];
        else return null;
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
        laboratory.gameObject.SetActive(false);
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
        Button[] btns = { craft, constructor, cancel, researcher, laboratory };
        foreach (Button btn in btns)
        {
            if(btn!=cancel)
            btn.gameObject.SetActive(true);
        }
    }
    public void GetHint(string text)
    {
        if (GameController.instance.player.level < 3)
            hint.SetPanel(Input.mousePosition, text);
    }
}
