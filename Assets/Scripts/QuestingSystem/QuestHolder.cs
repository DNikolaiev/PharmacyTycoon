using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;
public class QuestHolder : Panel, IUnityAdsListener {
    public Quest quest;
    public TextMeshProUGUI  Goaltxt, ProgressText, Durationtxt;
    public BarFiller progressFiller;
    public Button onCollectReward, onBonusReward;
    [SerializeField] AudioClip onQuestComplete;
   GameObject questPrefab;
   QuestGiver questGiver;
   Canvas canvas;
    public override void Hide()
    {
        Destroy(gameObject);
    }

    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
    public void SetEnvironment(Canvas canvas, GameObject questPrefab, QuestGiver questGiver)
    {
        this.canvas = canvas;
        this.questPrefab = questPrefab;
        this.questGiver = questGiver;
    }

    private void Start()
    {
        Invoke("DrawProgress", 0.01f);
    }
    public  void SetPanel(Quest quest)
    {
         onCollectReward.onClick.RemoveListener(ShowQuest) ;
        onBonusReward.onClick.RemoveAllListeners();
        this.quest = quest;
        SetMainInfo(quest);  
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ShowQuest);
        if (!quest.isActive)
        {
            onBonusReward.gameObject.SetActive(true);
            onCollectReward.gameObject.SetActive(true);
            onCollectReward.onClick.AddListener(delegate { CollectReward(quest); });
            onBonusReward.onClick.AddListener(delegate { ShowAds(); });
        }
        else
        {
            onCollectReward.gameObject.SetActive(false);
            onBonusReward.gameObject.SetActive(false);
        }
        

    }
    
    protected void SetMainInfo(Quest quest)
    {
        Nametxt.text = quest.Name;
        Goaltxt.text = quest.goal.description;
        ProgressText.text = quest.goal.currentAmount + " / " + quest.goal.neededAmount;
        Debug.Log(quest.goal.currentAmount + " " + quest.goal.neededAmount);
        DrawProgress();

    }
    protected void DrawProgress()
    {
        progressFiller.SetValueToBarScalar(quest.goal.currentAmount, progressFiller.healingBar, quest.goal.neededAmount);
    }
    public void ShowQuest()
    {
       
        GameObject toInst = Instantiate(questPrefab, canvas.transform);
        toInst.transform.SetAsLastSibling();
        toInst.transform.GetChild(1).GetComponent<QuestHolderExpanded>().SetPanel(quest, this);
        progressFiller.SetValueToBarScalar(quest.goal.currentAmount, progressFiller.healingBar, quest.goal.neededAmount);
    }
    protected void CollectReward(Quest quest, int rewardMultiplier=1)
    {
        Player player = GameController.instance.player;
        quest.reward.RewardPlayer(rewardMultiplier);
        player.activeQuests.Remove(quest);
        PlayParticles();
        Hide();
        GameController.instance.audio.MakeSound(onQuestComplete);
    }
    protected void ShowAds()
    {
        ShowOptions options = new ShowOptions
        {
            resultCallback = HandleShowResult
        };
        Advertisement.Show("rewardedVideo", options);
        CollectReward(quest, 2);
       
    }
    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("ADS FINISHED");
            
            
        }
        else if (result == ShowResult.Skipped)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show("Advertisement was skipped. No bonus reward, mate", Resources.Load<Sprite>("Icons/tv_icon"));
        }
        else if (result == ShowResult.Failed)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show("Something went wrong.", Resources.Load<Sprite>("Icons/tv_icon"));
        }
    }
    protected void PlayParticles()
    {
        EventManager.TriggerEvent("OnGetReward");
    }

    public void OnUnityAdsReady(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        CollectReward(quest, 2);
        Destroy(gameObject);
    }
}
