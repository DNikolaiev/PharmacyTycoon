using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour {
    public Button constructor;
    public Button researcher;
    public Button cancel;
    public Button laboratory;
    public Button engineering;
    public Button quest;
    public List<Button> buttons;
    public EngineeringPanel engineeringPanel;
    public MarketPanel market;
    public HintPanel hint;
    public ConfirmationWindow confirm;
    public MessageBox messageBox;
    public PaymentPanel paymentPanel;
    public DescriptionPanel[] dPanels;
    public TutorialPanel tPanel;
    
    private void Start()
    {
        if(!buttons.Contains(laboratory))
        {
            buttons.Add(laboratory);
        }
        if(!buttons.Contains(engineering))
        {
            buttons.Add(engineering);
        }
        if(!buttons.Contains(cancel))
        {
            buttons.Add(cancel);
        }
        if(!buttons.Contains(constructor))
        {
            buttons.Add(constructor);
        }
        if(!buttons.Contains(researcher))
        {
            buttons.Add(researcher);
        }
        ShowAllButtons();
    }
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
        else if (dPanels[3].gameObject.activeInHierarchy)
            return dPanels[3];
        else return null;
    }
    public void SwitchButtons(Button from, Button to)
    {
        to.gameObject.SetActive(true);
       from.gameObject.SetActive(false);
    }
    public void HideAllButtons()
    {
      
        
        foreach(Button btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
    }
    public void ShowButtons(Button[] btns)
    {

        foreach(Button btn in btns)
        {
            
            btn.gameObject.SetActive(true);
           
        }
    }
    public void HideCancel()
    {
        cancel.gameObject.SetActive(false);
    }
    public void ShowCancel()
    {
        cancel.gameObject.SetActive(true);
    }
    public void ShowAllButtons()
    {

       
        foreach (Button btn in buttons)
        {
            if(btn!=cancel && btn!=null)
            btn.gameObject.SetActive(true);
        }
        cancel.gameObject.SetActive(false);
        if (!GameController.instance.roomOverseer.SceneHasObjectOfType<Laboratory>()) laboratory.gameObject.SetActive(false);
        if (!GameController.instance.roomOverseer.SceneHasObjectOfType<EngineeringRoom>()) engineering.gameObject.SetActive(false);
    }
    public void GetHint(string text)
    {
        
            hint.SetPanel(Input.mousePosition, text);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Start");
    }

   
   
}
