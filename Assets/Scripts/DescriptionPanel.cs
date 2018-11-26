using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DescriptionPanel :  Panel {
    public Text type;
    public Text cures;
    public Text toxicity;

    public Text cost;
    public Image resourceImg;
    public Image timeImg;
    public Text timeToResearch;

    public Talent talent;
    public Animation slideAnim;
    private static RectTransform rect;
    public bool isActive;
    public override void Hide()
    {
        isActive = false;
        slideAnim.Play("SlideBackwards_DescriptionPanel");
    }
    
    public void SetPanel(Talent talent, ResearchHolder holder)
    {
        if (talent == null) return;
        this.talent = talent;
        SetMainInformation();
        // show resource & time costs, images
        resourceImg.gameObject.SetActive(true);
        timeImg.gameObject.SetActive(true);
        timeToResearch.text = talent.timeToResearch.ToString();
        cost.text = talent.description.buyPrice.ToString();
    }
    public void SetPanel(Talent talent, CraftHolder holder)
    {
        if (talent == null) return;
        this.talent = talent;
        SetMainInformation();
    }


    private void SetMainInformation()
    {
        isActive = true;
        gameObject.SetActive(true);
        slideAnim.Play("Slide_DescriptionPanel");
        toxicity.text = "<color='green'>Toxicity</color>: " + talent.characteristics.toxicity.ToString() + " %";
        cures.text = "<color='red'>Cures </color>: " + talent.cures;
    
        if (talent.isPrimary)
            type.text = "<color='orange'>Primary</color>";
        else type.text = "<color='orange'>Secondary</color>";
        Nametxt.text = talent.description.Name;
    }
    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {

        rect = GetComponent<RectTransform>();
        ReturnToOrigin();
        
	}
    public static void ReturnToOrigin()
    {
        rect.offsetMin = new Vector2(300, 0);
        rect.offsetMax = new Vector2(300, 0);
    }
	
}
