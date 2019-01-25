using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryPanel : Panel {
    [SerializeField] Transform recipeListView;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] BarFiller bar;
    [SerializeField] Text cost;
    [SerializeField] RecipeHolder holder;
    public override void Hide()
    {
        if (GameController.instance.crafter.view.recipeHolderSelected!=null)
        {
            
            GameController.instance.crafter.view.recipeHolderSelected.recipe = null;
                GameController.instance.crafter.view.recipeHolderSelected = null;
           
        }
        if (GameController.instance.crafter.recipeSelected != null)
            GameController.instance.crafter.recipeSelected = null;
        gameObject.SetActive(false);
    }

    public override void SetPanel()
    {
        
        gameObject.SetActive(true);
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        GameController.instance.crafter.PopulateRecipeList(recipeListView, recipePrefab);
        holder.picture.sprite = holder.defaultSprite;
        SetValueToBar();
       
    }
    public void SetValueToBar() // display toxicity and healing in bars
    {
        cost.text = CalculateCost().ToString();

        var crafter = GameController.instance.crafter;
        if (bar != null && crafter.recipeSelected!= null)
        {
            Image greenBar = bar.toxicityBar.transform.parent.Find("GreenImage").GetComponent<Image>();
            bar.SetValueToBarPercent(crafter.view.recipeHolderSelected.recipe.characteristics.toxicity - GameController.instance.player.skills.toxicityReducer, greenBar); // toxicity after reducing
            if(!bar.coroutineRunning)
                bar.SetValueToBarPercent(crafter.view.recipeHolderSelected.recipe.characteristics.toxicity, bar.toxicityBar); // true toxicity
            bar.SetValueToBarScalar(crafter.view.recipeHolderSelected.recipe.characteristics.healingRate, bar.healingBar); // healing bar
        }
        else if (crafter.recipeSelected==null) return;
    }
    public void ReduceToxicity()
    {
        if (GameController.instance.crafter.recipeSelected == null) return;
        GameController.instance.player.skills.ReduceToxicity(GameController.instance.crafter.recipeSelected, CalculateCost()); // logic
       StartCoroutine( bar.FillWithDelay(bar.toxicityBar, (float)GameController.instance.crafter.view.recipeHolderSelected.recipe.characteristics.toxicity/100));
        Debug.Log((float)GameController.instance.crafter.view.recipeHolderSelected.recipe.characteristics.toxicity / 100);
        SetValueToBar();
    }
    private int CalculateCost()
    {
        if (GameController.instance.crafter.recipeSelected == null)
            return 0;
        Debug.Log(GameController.instance.crafter.recipeSelected == null?true:false);
        
            return GameController.instance.crafter.recipeSelected.Talents.Count * 250;
       
    }
    
    private void Start()
    {
        Hide();
    }
   
}
