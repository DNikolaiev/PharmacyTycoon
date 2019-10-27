using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestHolderExpanded : QuestHolder {
    public TextMeshProUGUI Descriptiontxt, Statustxt;
    public Button  backBtn;
    public GameObject background;
    public RewardPanel rewardPanel;
    public override void Hide()
    {
        if (gameObject != null) 
        gameObject.SetActive(false);
        background.SetActive(false);
       
        Destroy(background);
    }
    public override void SetPanel()
    {
        gameObject.SetActive(true);
        background.SetActive(true);
        base.SetPanel();
    }
    private void Start()
    {
        Invoke("DrawProgress", 0.01f);
      
    }
    public  void SetPanel(Quest quest, QuestHolder holder)
    {
        this.quest = quest;
        SetMainInfo(quest);
        Durationtxt.text = quest.duration.ToString() + " Days Left";
        rewardPanel.SetPanel(quest.reward);
        Descriptiontxt.text = quest.description;
       
        if (!quest.isActive)
        {
            Statustxt.text = "Quest Completed";
            backBtn.gameObject.SetActive(false);
            onCollectReward.gameObject.SetActive(true);
            onBonusReward.gameObject.SetActive(true);
            onCollectReward.onClick.AddListener(delegate { CollectReward(quest); });
            onCollectReward.onClick.AddListener(delegate { DeleteHolder(holder); });
            onBonusReward.onClick.AddListener(delegate { ShowAds(); });
            onBonusReward.onClick.AddListener(delegate { Hide(); });
        }
        else
        {
            Statustxt.text = "Quest in Progress";
            backBtn.gameObject.SetActive(true);
            onCollectReward.gameObject.SetActive(false);
            onBonusReward.gameObject.SetActive(false);
        }


    }
    private void DeleteHolder(QuestHolder holder)
    {
        Destroy(holder.gameObject);
    }
   
    
   


}
