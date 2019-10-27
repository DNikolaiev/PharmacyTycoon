using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Researcher : MonoBehaviour {
    [FullSerializer.fsIgnore] [SerializeField] private int researchSpeed; //save
     
    public float ResearchSpeed 
    { get { return researchSpeed; } set { researchSpeed = (int)value; } }
    [FullSerializer.fsIgnore] public float timeToWait;
    [FullSerializer.fsIgnore] public bool isResearching;
    [FullSerializer.fsIgnore] public Talent researchable;
    

    [FullSerializer.fsIgnore] [SerializeField] GameObject researchScreen;
    [FullSerializer.fsIgnore] [SerializeField] private int stage; // save
    [FullSerializer.fsIgnore] [SerializeField] private int lastLevelSpotted; // save
    [FullSerializer.fsIgnore] public ResearchView view;
    [FullSerializer.fsIgnore] [SerializeField] Tutorial tutorial;
    
    private bool newResearchAvailiable;
    private Button activateButton;
    private Player player;
    private bool isInitialized;
    public SaveResearchData saveData;

    private void OnEnable()
    {
        SaveController.OnSaveEvent += OnSave;
        SaveController.OnLoadEvent += OnLoad;
        EventManager.StartListening(Player.OnLevelEvent, CheckIfNewResearchesAvailiable);
    }
    private void OnDisable()
    {
        EventManager.StopListening(Player.OnLevelEvent, CheckIfNewResearchesAvailiable);
        SaveController.OnLoadEvent -= OnLoad;
    }
    private void Start()
    {
        
        player = GameController.instance.player;
        activateButton = GameController.instance.buttons.researcher;
        researchScreen.SetActive(false);
     GameController.instance.talentTree.talents= GameController.instance.talentTree.talents.OrderBy(p => p.id).ToList();
       
        view.holders = view.holders.OrderBy(p => p.name).ToArray();
      for(int i=0; i< GameController.instance.talentTree.talents.Count;i++)
        {
            
            view.holders[i].Talent = GameController.instance.talentTree.talents[i];
            view.holders[i].SetPanel();
        }
        LockTalents();
        RefreshScreen();
        RefreshScreen();
       
        
    }
    private void LockTalents()
    {
        for (int i=0; i<view.holders.Length;i++)
        {
            if (stage <= 0) stage = 0;
            if (i < GameController.instance.talentTree.map[stage])
            {

                view.holders[i].lockedSprite.SetActive(false);
            }
            else view.holders[i].lockedSprite.SetActive(true);
        }
    }
    public void EnhanceResearchTime(int amount)
    {
        ResearchSpeed += amount;
    }
    public void RefreshScreen()
    {
        if (stage >= GameController.instance.talentTree.map.Length ) return;
        for (int i = 0; i < GameController.instance.talentTree.map[stage]; i++)
        {

            if (i >= GameController.instance.talentTree.talents.Count) return;
            if (!GameController.instance.talentTree.talents[i].isUnlocked)
            {
                view.holders[i].picture.enabled = true;
                view.holders[i].Talent.canBeUnlocked = true;
                view.holders[i].lockedSprite.SetActive(false);
            }
            else view.holders[i].picture.sprite = view.holders[i].Talent.description.sprite;
            view.holders[i].SetPanel();
        }
       
        lastLevelSpotted = player.level;
    }
    public void UpdateResearchScreen()
    {
        if (lastLevelSpotted != player.level)
        {
            lastLevelSpotted = player.level;
            if (view.progressImage != null)
            {

                view.progressImage.fillAmount += view.fillAmount;
            }
            if (player.level % 2 == 0)
            {

                if (stage < GameController.instance.talentTree.map.Length)
                {
                    print(stage);
                    stage++;
                }
                RefreshScreen();
                
            }
        }
    }
    public IEnumerator UnlockTalent(int id)
    {
         timeToWait = GameController.instance.talentTree.talents[id].timeToResearch/(ResearchSpeed/100);
        researchable = GameController.instance.talentTree.talents[id];
        isResearching = true;
        while(timeToWait>0)
        {
            timeToWait--;
            yield return new WaitForSeconds(1);
        }
        GameController.instance.talentTree.talents[id].isUnlocked = true;
        GameController.instance.talentTree.talents[id].isSelected = false;
        isResearching = false;
        view.holders[id].ChangeHolderPicture(true);
        view.holders[id].onResearch.Play();
        view.onResearch.Play();
        GameController.instance.audio.MakeSound(view.onResearchSound);
        EventManager.TriggerEvent("OnResearch", 1);
        player.GainExperience(50*id);
        if (!tutorial.isTutorialCompleted)
        {
            GameController.instance.buttons.ShowCancel();
            tutorial.ContinueTutorial();
        }
        List<int> allIDs = new List<int>();
        foreach(Talent t in GameController.instance.talentTree.talents)
        {
            allIDs.Add(t.id);
        }
        if(id==allIDs.Max())
        {
            PlayGameScript.UnlockAchievement(GPGSIds.achievement_mad_scientist);
        }
    }
    public void Research(int id)
    {
        if (!isResearching && player.resources.ResearchPoints >= GameController.instance.talentTree.talents[id].description.buyPrice)
        {
            player.resources.AddResearchPoints(-GameController.instance.talentTree.talents[id].description.buyPrice);
            StartCoroutine(UnlockTalent(id));
        }
    }
    public void UnlockTalentWithoutWaiting(int id)
    {
        foreach (Talent t in GameController.instance.talentTree.talents)
            if (t.id == id)
            {
                t.isUnlocked = true;
                t.isSelected = false;
                view.holders[id].ChangeHolderPicture(true);
            }
    }

    public void ResearchON()
    {
        UpdateResearchScreen();
        researchScreen.SetActive(true);
        CheckIfNewResearchesAvailiable();
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        GameController.instance.IsGameSceneEnabled = false;
        GameController.instance.time.Pause();
        if(GameController.instance.generalTutorial.isTutorialCompleted)
             StartCoroutine(GameController.instance.cam.ResetCamera());
        researchScreen.GetComponentInChildren<DescriptionPanel>().ReturnToOrigin();
        view.anim.Play("ResearchPanel_Appear");
        view.closeButton.gameObject.SetActive(false);
        if (tutorial.isTutorialCompleted)
        {
            view.Invoke("ShowCloseButton", 0.65f);
            view.tutorialParticles.Stop();
        }
        else
        {
            tutorial.StartTutorial();
            GameController.instance.buttons.HideCancel();
            
           
            view.tutorialParticles.Play();
        }
        

    }
    private void HideCancel()
    {
        GameController.instance.buttons.HideCancel();
    }
    public void ResearchOFF()
    {
        GameController.instance.IsGameSceneEnabled = true;
        researchScreen.SetActive(false);
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.ShowAllButtons();
        GameController.instance.time.UnPause();
        if (!GameController.instance.generalTutorial.isTutorialCompleted)
        {
            GameController.instance.generalTutorial.isBlocked = false;
            GameController.instance.generalTutorial.ContinueTutorial();
            
        }
    }
    public void CheckIfNewResearchesAvailiable()
    {
        Player player = GameController.instance.player;
        if ( player.level % 2 == 0 )
            newResearchAvailiable = true;
        if (newResearchAvailiable)
        {
            activateButton.GetComponentInChildren<ParticleSystem>().Play();
            newResearchAvailiable = false;
            
        }
        else { activateButton.GetComponentInChildren<ParticleSystem>().Stop();  }
        UpdateResearchScreen();
    }

    public void OnSave()
    {
        List<int> ids = new List<int>();
        foreach(Talent t in GameController.instance.talentTree.talents.Where(t=>t.isUnlocked).ToList())
        {
            ids.Add(t.id);
        }
        saveData = new SaveResearchData(researchSpeed, stage, GameController.instance.player.level, ids, view.progressImage.fillAmount);
    }
    public void OnLoad()
    {
        researchSpeed = saveData.researchSpeed;
        Debug.Log(saveData.stage);
        stage = saveData.stage;
        lastLevelSpotted = saveData.lastLevel;
        view.progressImage.fillAmount = saveData.filled1;
        foreach(Talent t in GameController.instance.talentTree.talents)
        {
            if (saveData.ids.Contains(t.id))
            {
                UnlockTalentWithoutWaiting(t.id);
            }
        }
       
        RefreshScreen();
    }
}
[System.Serializable]
public class SaveResearchData
{
    public int researchSpeed;
    public int stage;
    public int lastLevel;
    public List<int> ids = new List<int>();
    public float filled1;
    public float filled2 = 0;
    public SaveResearchData(List<int> list)
    {
        ids.AddRange(list);

    }
    public SaveResearchData()
    {
        researchSpeed = 0;
        stage = 0;
        lastLevel = 0;

    }
    public SaveResearchData(int speed, int stage, int level, List<int> talentsID, float filled)
    {
        researchSpeed = speed;
        this.stage = stage;
        lastLevel = level;
        ids.AddRange(talentsID);
        this.filled1 = filled;
    }
    public SaveResearchData(int speed, int level, float filled)
    {
        researchSpeed = speed;
        
        lastLevel = level;
        filled1 = filled;
    }
    public SaveResearchData(int speed, int level, float filled1, float filled2)
    {
        researchSpeed = speed;
        this.filled1 = filled1;
        this.filled2 = filled2;
        lastLevel = level;
    }
}
