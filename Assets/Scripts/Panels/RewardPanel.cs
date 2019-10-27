using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RewardPanel : Panel {

    public TextMeshProUGUI  HerbsRewardtxt, ChemsRewardTxt, PlasticRewardTxt, ExpRewardTxt, ResearchRewardTxt, Moneytxt;
    public Image herbsImage, chemsImage, plasticImage, expImage, researchImage, moneyImage;
    public Image coinImage;
    public TextMeshProUGUI coinReward;

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPanel()
    {
        
    }
    public void SetPanel(Reward reward)
    {
        if (coinImage != null)
            coinImage.gameObject.SetActive(IsNotZero(reward.coins, coinReward));
        herbsImage.gameObject.SetActive(IsNotZero(reward.herbs, HerbsRewardtxt));
        chemsImage.gameObject.SetActive(IsNotZero(reward.chems, ChemsRewardTxt));
        plasticImage.gameObject.SetActive(IsNotZero(reward.plastic, PlasticRewardTxt));
        expImage.gameObject.SetActive(IsNotZero(reward.experiencePercentage, ExpRewardTxt));
        ExpRewardTxt.text = reward.experiencePercentage + " %";
        researchImage.gameObject.SetActive(IsNotZero(reward.researchPoints, ResearchRewardTxt));
        moneyImage.gameObject.SetActive(IsNotZero(reward.money, Moneytxt));
    }
    private bool IsNotZero(float value, TextMeshProUGUI text)
    {
        text.text = (value != 0) ? value.ToString() : string.Empty;
        return value != 0;
    }
}
