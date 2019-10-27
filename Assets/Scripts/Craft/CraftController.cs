using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour {
    public CreationPanel cPanel;
    private Crafter crafter;
    private bool tutorialVar;
    private void Start()
    {
        crafter = GameController.instance.crafter;
    }
    public void DisplaySettings()
    {
       int counter = 0;
       foreach(Talent tal in crafter.selectedTalents)
        {
            if (tal.isPrimary)
                counter++;
        }
        if (crafter.selectedTalents.Count >= 1 && counter>0)
        {
            crafter.RecognizeRecipe();
            
            cPanel.SetPanel(crafter.GetCharacteristics());
            GameController.instance.buttons.cancel.gameObject.SetActive(false);
        }

        else return;
    }
    public void OnSelectHolder(CraftHolder holder) // on click at main talent holders
    {
        crafter.view.holderSelected = holder;
    }
    public void OnSelectHolder(RecipeHolder holder) // on click at main talent holders
    {
        crafter.view.recipeHolderSelected = holder;
    }

    public void OnAddTalent(TalentHolder talentHolder) // on click at talent's list
    {
        crafter.view.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.view.craftDescriptionPanel.SetPanel(talentHolder.Talent, (CraftHolder)talentHolder);
        crafter.AssignTalent(talentHolder.Talent);
        if (!tutorialVar)
        {
            tutorialVar = true;
            crafter.tutorial.ContinueTutorial();
        }
        Destroy(talentHolder.transform.parent.parent.gameObject);
    }
    public void OnAddRecipe(RecipeHolder recipeHolder)
    {
        crafter.view.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.view.craftDescriptionPanel.SetPanel(recipeHolder.recipe, recipeHolder);
        crafter.AssignRecipe(recipeHolder.recipe);
   
    }

    public void OnSliderChange(float value)
    {
        if (crafter.isPrescripted) return;
        if (value == 0)
        {
            
            crafter.isLiquid = true;
            crafter.Recombine(crafter.selectedTalents);
        }
        else
        {
            crafter.isLiquid = false;
            crafter.Recombine(crafter.selectedTalents);
        }
    }
}
