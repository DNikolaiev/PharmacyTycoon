using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Seller : MonoBehaviour {
    public Area area;
    [SerializeField] private int epidemiesCured;
    [SerializeField] private AudioClip onSellSound;
    public int transferCost;
    public SellView view;
    public SellController controller;
    public Tutorial tutorial;
    
    public int EpidemiesCured
    {
        get { CountMatches(); return epidemiesCured; }
        private set { }
    }
    private void Start()
    {

        CloseSale();
    }
    public void Sell(Recipe recipe, Invoice invoice)
    {
        if (!GameController.instance.player.inventory.CheckIfWarehouseContains(recipe.description.Name) || invoice.quantity + area.soldItems>area.maxQuotum) return;

        bool soldSuccesfully = GameController.instance.player.inventory.Remove(recipe.description.Name, invoice.quantity);
        if (soldSuccesfully)
        {
            if (!tutorial.isTutorialCompleted) tutorial.ContinueTutorial();
            view.returnBtn.gameObject.SetActive(true);
            var messageBox = GameController.instance.buttons.messageBox;
            messageBox.Show(
                "x" + invoice.quantity.ToString() + " " 
                + RecipeSelector.recipeHolderSelected.recipe.description.Name + " sold",
                RecipeSelector.recipeHolderSelected.recipe.description.sprite
                );
            GameController.instance.player.GainExperience( invoice.quantity * 10*(area.experienceMultiplier+(recipe.Talents.Count)/4));
            GameController.instance.player.resources.ChangeBalance(invoice.Summary,true);
            int healAmount = recipe.characteristics.healingRate * invoice.quantity;
            area.health += healAmount; area.health = Mathf.Clamp(area.health, 0, area.maxHealth);
            area.soldItems += invoice.quantity;
            // refresh dead/alive rate
            int alive = 0;
            int dead = 0;
            Reaper reaper = new Reaper();
            dead= reaper.GetDeadAliveRate(recipe, invoice.quantity, out alive);
            area.dead += dead;
            area.cured += alive;
            area.health -= dead * 10;
            recipe.soldAmount += invoice.quantity;
            recipe.deadAmount += dead;
            recipe.RecalculatePrice();
            view.onSell.Play();
            // achieve quest's objective if it exists
            EventManager.TriggerEvent("OnSell", invoice.quantity);
            EventManager.TriggerEvent("OnKill", dead);
            EventManager.TriggerEvent("OnCure", alive);
            EventManager.TriggerEvent("OnHeal", healAmount);
            // increment achievement
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_life_saver, alive);
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_grim_reaper, dead);
            GameController.instance.audio.MakeSound(onSellSound);
        }
        view.SetViewToArea(area);
        view.RemoveCheckmarks();
        
        ListPopulator.PopulateRecipeList(view.recipeView, view.recipePrefab, GameController.instance.player.inventory.recipes.Where(x=>x.GetDeathRating()<=area.deathRatingAllowed 
        && GameController.instance.player.inventory.GetQuantity(x.description.Name) > 0).ToList());
    }
    public bool GetTutorialState()
    {
        return tutorial.isTutorialCompleted;
    }
    public void CountMatches()
    {

        if (RecipeSelector.recipeHolderSelected == null) return;
        List<Talent> matches = area.activeEpidemies.Intersect(RecipeSelector.recipeHolderSelected.recipe.Talents).ToList();
        view.CureEpidemic(matches);
        epidemiesCured = matches.Count;
    }
    
    public void SaleOn(Area area, int transferCost)
    {
        this.area = area;
        this.transferCost = transferCost;
        view.gameObject.SetActive(true);
        view.SetViewToArea(area);
        EventManager.StartListening("OnRecipeRemove", view.RemoveCheckmarks);
        GameController.instance.time.Pause();
        RecipeSelector.UnSelectRecipe();
        GameController.instance.IsGameSceneEnabled = false;
        ListPopulator.PopulateRecipeList(view.recipeView, view.recipePrefab, GameController.instance.player.inventory.recipes.Where(x => x.GetDeathRating() <= area.deathRatingAllowed
         && GameController.instance.player.inventory.GetQuantity(x.description.Name) > 0).ToList());
        if (!tutorial.isTutorialCompleted)
        {
            tutorial.ContinueTutorial();

        }
        if (!GameController.instance.generalTutorial.isTutorialCompleted)
        {
            view.returnBtn.gameObject.SetActive(false);
        }


    }
    private void CloseSale()
    {
        GameController.instance.IsGameSceneEnabled = true;
        GameController.instance.time.UnPause();
        RecipeSelector.UnSelectRecipe();
        EventManager.StopListening("OnRecipeRemove", view.RemoveCheckmarks);
        view.cancelBtn.gameObject.SetActive(true);
        view.gameObject.SetActive(false);
    }
    public void SaleOff()
    {
        CloseSale();
        
        if (!GameController.instance.generalTutorial.isTutorialCompleted)
        {
            GameController.instance.generalTutorial.isBlocked = false;
            GameController.instance.generalTutorial.ContinueTutorial();
            GameController.instance.generalTutorial.ContinueTutorial();
           
        }
    }
    public void StartTutorial()
    {
        if (!tutorial.isTutorialCompleted)
        {
            view.cancelBtn.gameObject.SetActive(false);
            tutorial.StartTutorial();
        }
    }
}
