using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResearchPanel : Panel
{
    public Text researchPoints;
    public Image researchImg;
    public Image timeImg;
    public Text timeToResearch;
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public  void SetPanel(Talent talent)
    {
        gameObject.SetActive(true);
        researchImg.gameObject.SetActive(true);
        timeImg.gameObject.SetActive(true);
        timeToResearch.text = talent.timeToResearch.ToString();
        researchPoints.text = talent.description.buyPrice.ToString();
    }

    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
}
