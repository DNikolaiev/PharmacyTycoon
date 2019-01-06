using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Researcher : MonoBehaviour {
    [SerializeField] private int researchSpeed;

    public float ResearchSpeed
    { get { return researchSpeed; } set { researchSpeed = (int)value; } }
    public float timeToWait;
    public bool isResearching;
    public bool newResearchAvailiable;
    public Talent researchable;
    public GameObject researchScreen;
    public List<Talent> talents;
    public ResearchHolder[] holders;
    public int[] map;

    [SerializeField] private int stage;
    private int lastLevelSpotted;
    private Button activateButton;
    private Button cancel;

   

    public static Researcher instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
    }
    private void Start()
    {
        activateButton = ButtonController.instance.researcher;
        cancel = ButtonController.instance.cancel;
        researchScreen.SetActive(false);
      talents= talents.OrderBy(p => p.id).ToList();
      for(int i=0; i<talents.Count;i++)
        {
            holders[i].Talent = talents[i];
            holders[i].SetPanel();
        }
    }
    public void EnhanceResearchTime(int amount)
    {
        ResearchSpeed += amount;
    }
    public void UpdateResearchScreen()
    {
        if (Player.instance.level%2==0 && lastLevelSpotted != Player.instance.level)
        {
            for (int i=0; i<map[stage];i++)
            {
                if (!talents[i].isUnlocked)
                {
                    holders[i].lockedSprite.SetActive(false);
                }
                else holders[i].picture.sprite = holders[i].Talent.description.sprite;
            }
            if(stage<map.Length)
            stage++;
            lastLevelSpotted = Player.instance.level;
        }
    }
    public IEnumerator UnlockTalent(int id)
    {
         timeToWait = talents[id].timeToResearch;
        researchable = talents[id];
        isResearching = true;
        while(timeToWait>0)
        {
            timeToWait--;
            yield return new WaitForSeconds(1);
        }
        talents[id].isUnlocked = true;
        isResearching = false;
        holders[id].ChangeHolderPicture(true);
        
    }
    public void Research(int id)
    {
        if (!isResearching && Player.instance.resources.ResearchPoints >= talents[id].description.buyPrice)
        {
            Player.instance.resources.AddResearchPoints(-talents[id].description.buyPrice);
            StartCoroutine(UnlockTalent(id));
        }
    }
    public void UnlockTalentWithoutWaiting(int id)
    {
        talents[id].isUnlocked = true;
        holders[id].ChangeHolderPicture(true);
    }

    public void ResearchON()
    {
        UpdateResearchScreen();
        researchScreen.SetActive(true);
        CheckIfNewResearchesAvailiable();
        ButtonController.instance.HideAllButtons();
        ButtonController.instance.cancel.gameObject.SetActive(true);
        GameController.instance.isGameSceneEnabled = false;

    }
    public void ResearchOFF()
    {
        GameController.instance.isGameSceneEnabled = true;
        DescriptionPanel.ReturnToOrigin();
        researchScreen.SetActive(false);

        ButtonController.instance.HideAllButtons();
        ButtonController.instance.ShowAllButtons();
        
        

    }
    public bool CheckIfNewResearchesAvailiable()
    {
        if (lastLevelSpotted < Player.instance.level && Player.instance.level % 2 == 0 && lastLevelSpotted!=0)
            newResearchAvailiable = true;
        if (newResearchAvailiable)
        {
            activateButton.GetComponentInChildren<ParticleSystem>().Play();
            newResearchAvailiable = false;
            return true;
        }
        else { activateButton.GetComponentInChildren<ParticleSystem>().Stop(); return false; }
    }

}
