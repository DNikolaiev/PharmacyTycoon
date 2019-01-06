using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DescriptionPanel :  Panel {
    public Text type;
    public Text cures;
    public Text toxicity;
    public Text healingRate;
    public Text cost;
    public Image resourceImg;
    public Image timeImg;
    public Text timeToResearch;

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
        ChangeView(false);
        SetMainInformation(talent);
        // show resource & time costs, images
        resourceImg.gameObject.SetActive(true);
        timeImg.gameObject.SetActive(true);
        timeToResearch.text = talent.timeToResearch.ToString();
        cost.text = talent.description.buyPrice.ToString();
    }
    public void SetPanel(Talent talent, CraftHolder holder)
    {
        if (talent == null) return;
        ChangeView(false);
        SetMainInformation(talent);
        
    }
    public void SetPanel(Recipe recipe, RecipeHolder holder)
    {
        if (recipe == null) return;
        ChangeView(true);
        SetMainInformation(recipe);
    }
    private void ChangeView(bool state)
    {
        Transform formPanel = toxicity.transform.parent.parent.parent.GetChild(1);
        Transform mainDescription = toxicity.transform.parent;
        if(state)
        mainDescription.GetComponent<RectTransform>().offsetMin = new Vector2(0, -50);
        else
            mainDescription.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        formPanel.gameObject.SetActive(!state);
    }
    private void SetMainInformation(Recipe recipe)
    {
        isActive = true;
        gameObject.SetActive(true);
        slideAnim.Play("Slide_DescriptionPanel");
        if (recipe.isLiquid)
            type.text = "Liquid";
        else type.text = "Pills";
        cures.text = "<color='red'>Сures </color> ";
        
        foreach (Talent tal in recipe.Talents)
        {
            if (tal == recipe.Talents[recipe.Talents.Count - 1])
                cures.text += tal.cures + ".";
            else
            cures.text += tal.cures + ", ";
        }
        Nametxt.text = recipe.description.Name;
        toxicity.text = "<color='green'>Toxicity: </color>" + recipe.characteristics.toxicity.ToString() + " %" ;
        healingRate.text = "<color='red'>Healing Rate: </color>" + recipe.characteristics.healingRate.ToString() ;
    }
    private void SetMainInformation(Talent talent)
    {
        isActive = true;
        gameObject.SetActive(true);
        slideAnim.Play("Slide_DescriptionPanel");
        toxicity.text = "<color='green'>Toxicity</color>: " + talent.characteristics.toxicity.ToString() + " %";
        cures.text = "<color='red'>Cures </color>: " + talent.cures;
        healingRate.text = "<color='red'>Healing Rate</color>: " + talent.characteristics.healingRate.ToString() + " %";
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
