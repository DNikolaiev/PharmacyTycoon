using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EventPanel : Panel
{
    CustomEvent eventAvailable;
    
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI choice1Description;
    [SerializeField] TextMeshProUGUI choice2Description;
    [SerializeField] TextMeshProUGUI result1, result2;
    [SerializeField] Button choice1Btn;
    [SerializeField] Button choice2Btn;
    [SerializeField] List<Actor> actors;
    [SerializeField] Image rightActor, leftActor;
    [SerializeField] List<Panel> panelsToClose;
    [SerializeField] AudioClip clip;
    public override void Hide()
    {
        eventAvailable = null;
        GameController.instance.time.UnPause();
        gameObject.SetActive(false);
        GameController.instance.IsGameSceneEnabled = true;
    }
   
    public override void SetPanel()
    {
        gameObject.SetActive(true);
        GameController.instance.researcher.ResearchOFF();
        GameController.instance.crafter.CraftOFF();
        GameController.instance.constructor.ConstructOFF();
        GameController.instance.buttons.engineeringPanel.Hide();
        foreach(Panel p in panelsToClose)
        {
            p.Hide();
        }
        GameController.instance.time.Pause();
        GameController.instance.IsGameSceneEnabled = false;
        description.text = eventAvailable.description;
        Nametxt.text = eventAvailable.Name;
        if (actors.Count>0)
        {
            rightActor.gameObject.SetActive(true);
            leftActor.gameObject.SetActive(true);
            rightActor.sprite = actors[0].sprite;
            leftActor.sprite = actors[1].sprite;
        }
        else
        {
            rightActor.gameObject.SetActive(false);
            leftActor.gameObject.SetActive(false);
        }
        GameController.instance.audio.MakeSound(clip);
    }
   private void ClearFields()
    {
        choice1Description.text = string.Empty;
        choice2Description.text = string.Empty;
        result1.text = string.Empty;
        result2.text = string.Empty;
    }
    public void SetPanel(ChoiceEvent choiceEvent)
    {
        if (choiceEvent == null) { Hide(); return; }
        eventAvailable = choiceEvent;
        ClearFields();
        choice1Btn.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        choice1Description.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        choice1Btn.onClick.RemoveAllListeners();
        choice2Btn.onClick.RemoveAllListeners();
        SetPanel();
        choice2Btn.gameObject.SetActive(true);
        choice1Btn.gameObject.SetActive(true);
        choice2Btn.onClick.AddListener(choiceEvent.FireSecondEvent);
        choice1Btn.onClick.AddListener(choiceEvent.FireEvent);
        choice2Btn.onClick.AddListener(Hide);
        choice1Btn.onClick.AddListener(Hide);
        choice1Description.text = choiceEvent.choice1;
        choice2Description.text = choiceEvent.choice2;
        choice2Description.text = choice2Description.text.Replace("\\n", "\n");
        choice1Description.text = choice1Description.text.Replace("\\n", "\n");
        result1.text = choiceEvent.result1;
        result2.text = choiceEvent.result2;
    }
    public void SetPanel(ClickerEvent clickEvent)
    {
        if (clickEvent == null) { Hide(); return; }
        eventAvailable = clickEvent;
        ClearFields();
        choice1Btn.GetComponent<RectTransform>().offsetMin = new Vector2(-295, 0);
        choice1Description.GetComponent<RectTransform>().offsetMin = new Vector2(-295, 0);
        choice1Btn.onClick.RemoveAllListeners();
        SetPanel();
        choice2Btn.gameObject.SetActive(false);
        choice1Btn.gameObject.SetActive(true);
        choice1Btn.onClick.AddListener(clickEvent.FireEvent);
        choice2Btn.onClick.AddListener(Hide);
        choice1Btn.onClick.AddListener(Hide);
       
        choice1Description.text = choice1Description.text.Replace("\\n", "\n");
        result1.text = clickEvent.result;
    }
}
