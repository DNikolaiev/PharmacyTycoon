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
    private Player player;
   
    private void Start()
    {
        player = GameController.instance.player;
        activateButton = GameController.instance.buttons.researcher;
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
        if (player.level%2==0 && lastLevelSpotted != player.level)
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
            lastLevelSpotted = player.level;
        }
    }
    public IEnumerator UnlockTalent(int id)
    {
         timeToWait = talents[id].timeToResearch/(ResearchSpeed/100);
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
        if (!isResearching && player.resources.ResearchPoints >= talents[id].description.buyPrice)
        {
            player.resources.AddResearchPoints(-talents[id].description.buyPrice);
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
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        GameController.instance.isGameSceneEnabled = false;

    }
    public void ResearchOFF()
    {
        GameController.instance.isGameSceneEnabled = true;
        
        researchScreen.SetActive(false);

        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.ShowAllButtons();
        
        

    }
    public bool CheckIfNewResearchesAvailiable()
    {
        Player player = GameController.instance.player;
        if (lastLevelSpotted < player.level && player.level % 2 == 0 && lastLevelSpotted!=0)
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
