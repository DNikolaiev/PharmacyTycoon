using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EventsLibrary : MonoBehaviour
{
    public void PlaySound(AudioClip clip)
    {

    }
    public void EnableAlarm()
    {
        EventManager.TriggerEvent("OnAlarmEnable");
        Debug.Log("ALAARM TRIGGERED");
    }
    public void DisableInterface()
    {
        GameController.instance.buttons.HideAllButtons();
    }
    public void ChangeBalance(int n)
    {
        GameController.instance.player.resources.ChangeBalance(n);
    }
    public void AddChemistry(int n)
    {
        GameController.instance.player.resources.AddChemistry(n);
    }
    public void AddHerbs(int n)
    {
        GameController.instance.player.resources.AddHealingPlants(n);
    }
    public void AddPlastic(int n)
    {
        GameController.instance.player.resources.AddPlastic(n);
    }
    public void AddResearchPoints(int n)
    {
        GameController.instance.player.resources.AddResearchPoints(n);
    }
    public void IncreaseResearchSpeed(int n)
    {
        GameController.instance.researcher.ResearchSpeed += n;
    }
    public void AddSecondChance()
    {
        GameController.instance.player.resources.AddSecondChance(1);
    }
    public void AddMedCoins(int n)
    {
        GameController.instance.player.resources.medCoins += n;
    }
    public void Inspection(int fineValue)
    {
        List<Recipe> deadlyRecipes = GameController.instance.player.inventory.recipes.Where(x => x.GetDeathRating() >= 5).ToList();
        GameController.instance.player.resources.ChangeBalance(-fineValue * deadlyRecipes.Count);
        foreach(Recipe r in deadlyRecipes)
        {
            GameController.instance.player.inventory.Delete(r.description.Name);
        }
    }
    public void RaiseSalary(int onValue)
    {
        GameController.instance.player.finances.expences.salaryPerWorker += onValue;
    }
    public void DivideRecipePrice(float divider)
    {
        foreach(Recipe r in GameController.instance.player.inventory.recipes)
        {
            r.price.divider = divider;
            r.RecalculatePrice();
        }
    }
    public void DeleteAllMedicine()
    {
        foreach(Recipe r in GameController.instance.player.inventory.recipes)
        {
            GameController.instance.player.inventory.RemoveAllMedicine(r.description.Name);
        }
    }
    public void DecreaseHealth(int divider)
    {
        foreach(Area a in World.instance.areas)
        {
            a.health = a.health/divider;
        }
    }
    public void AddLevel()
    {
        GameController.instance.player.LevelUp();
    }
}
