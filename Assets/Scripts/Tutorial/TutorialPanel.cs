using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialPanel : Panel
{
    private Tutorial tutorial;
    public TextMeshProUGUI tutorText;
    public TextMeshProUGUI actorName;
    public Image actorImg;
    public ParticleSystem glowPrefab;
    private ParticleSystem activeGlow;

    private GameObject highlightedObject;
    [SerializeField] GameObject backShaded;
    
    private Dialogue dialogue;
    private Actor actor;
    private void Start()
    {
        
       //Hide();
    }
    public override void Hide()
    {
        actor = null;
        StopAllCoroutines();
        tutorText.text = string.Empty;
        actorName.text = string.Empty;
        backShaded.SetActive(false);
        if(tutorial.activateCameraOnComplition)
            GameController.instance.IsGameSceneEnabled=true;
        if (tutorial.startTimeOnComplition)
            GameController.instance.time.UnPause();
       
    }

    public override void SetPanel()
    {
        gameObject.SetActive(true);
        backShaded.SetActive(true);
        GameController.instance.IsGameSceneEnabled = false;
    }
    public void SetPanel(Tutorial tutorial, Actor actor)
    {
        SetPanel();
        this.dialogue = tutorial.dialogue;
        this.actor = actor;
        this.tutorial = tutorial;
        actorName.text = actor.Name;
        actorImg.enabled = true;
        actorImg.sprite = actor.sprite;
        GameController.instance.IsGameSceneEnabled = false;
        GameController.instance.time.Pause();
        ProceedTutorial();
        
    }
    public void ProceedTutorial() // on btn click proceed tutorial
    {
        GameController.instance.IsGameSceneEnabled = false;
        if (tutorial.isBlocked) return;
        if (activeGlow != null)
        {
            activeGlow.Stop(); Destroy(activeGlow);
        }
        string phrase = dialogue.GetNextPhrase();
        if (highlightedObject!=null)
            Destroy(highlightedObject.gameObject);
        StopAllCoroutines();
        tutorText.text = string.Empty;
        
        if (phrase.Equals("Final")) { tutorial.CompleteTutorial(); Hide(); return; }
        else if (phrase == string.Empty) { Hide(); return; }
        if (phrase.Contains("highlighted")) { phrase = phrase.Replace("highlighted", " "); HighLightObject(); }
        
        tutorial.isBlocked = (phrase.Contains("block")) ? true : false;
        if (phrase.Contains("block")) { phrase = phrase.Replace("block", " "); }

        if(phrase.Contains("dragg"))
        {
            phrase = phrase.Replace("dragg", "");
            StartCoroutine(BlockTemp(3));
            MoveObject();
        }

        StartCoroutine(ShowTextWithDelay(phrase, tutorText, 0.0005f));
        
    }
    private IEnumerator BlockTemp(float time)
    {
        tutorial.isBlocked = true;
        yield return new WaitForSeconds(time);
        tutorial.isBlocked = false;
        yield break;
    }
    private void MoveObject()
    {
        if (!gameObject.activeInHierarchy) return;
        int timeBetweenLoops = 4;
        GameObject moveable = Instantiate(tutorial.GetObjectToMove(), transform);
       
        moveable.AddComponent<Transporter>().MoveToUI(tutorial.GetPlaceToMove().localPosition, 2);
        foreach(Button child in moveable.transform.GetComponentsInChildren<Button>())
        {
            child.onClick.AddListener(ProceedTutorial);
        }
        
        moveable.AddComponent<AutoDestroy>().waitTime = timeBetweenLoops;
        Invoke("MoveObject", timeBetweenLoops);
    
    }
    private void HighLightObject()
    {
        GameController.instance.IsGameSceneEnabled = false;
       highlightedObject = Instantiate(tutorial.HighlightObject(), transform);
        highlightedObject.transform.SetAsFirstSibling();
        activeGlow = Instantiate(glowPrefab, highlightedObject.transform.position, Quaternion.identity);
        activeGlow.Play();
       /* if (highlightedObject.GetComponent<Button>()) highlightedObject.GetComponent<Button>().onClick.AddListener(ProceedTutorial);
        else
        {
            highlightedObject.AddComponent<Button>(); 
        highlightedObject.GetComponent<Button>().onClick.AddListener(ProceedTutorial);
        } */
    }
    private IEnumerator ShowTextWithDelay(string s, TextMeshProUGUI text, float delayTime)
    {
        text.text = string.Empty;
        if (s == string.Empty) yield break;
        foreach(char c in s)
        {
            text.text += c;
            yield return new WaitForSeconds(delayTime);
        }
        yield break;
    }
    
}
