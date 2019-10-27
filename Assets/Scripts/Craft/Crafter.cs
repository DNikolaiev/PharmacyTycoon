using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crafter : MonoBehaviour {

   [SerializeField] private List<Talent> openTalents;
    public SavedUnlockmentsData savedData;
    public bool isLiquid = true;
    public int formMultiplier;
    public bool isPrescripted = false;
    public Recipe recipeSelected;
    public List<Talent> selectedTalents;
    public Tutorial tutorial;
    public CraftView view; // reflects changes in game view
    public CraftController controller; // controls input in craft screen
    public List<AudioClip> ambienceSound;
    [SerializeField] Characteristics characteristics;
    private Player player;
    private void Start()
    {
        player = GameController.instance.player.GetComponent<Player>();
        player.inventory.Set(null, 0);
        
        
        view.craftPanel.gameObject.SetActive(false);
        
    }
   
    private void CalculateResourcesNeeded(List<Talent> talents)
    {
        characteristics.Reset();
        foreach (Talent tal in talents)
        {
            
                this.characteristics.healingPlantsNeeded += tal.characteristics.healingPlantsNeeded;
                this.characteristics.chemistryNeeded += tal.characteristics.chemistryNeeded;
                this.characteristics.plasticNeeded += tal.characteristics.plasticNeeded;
        }
    }
    private int CalculateToxicity(List<Talent> talents)
    {
        int toxicity = 0;
        foreach (Talent tal in talents)
        {
            toxicity += tal.characteristics.toxicity;
        }
        return toxicity;
    }
    private int CalculateHealingRate(List<Talent> talents)
    {
        int healingRate = 0;
        foreach (Talent tal in talents)
        {
            healingRate += tal.characteristics.healingRate;
        }
        return healingRate;
    }
    public int CalculateResearchPoints(List<Talent> talents)
    {
        int result = 0;
        foreach (Talent tal in talents)
        {
            result += tal.description.buyPrice;
        }
        return (int)result/2;
    }
    public void Recombine(List<Talent> talents) // recalculate values needed to craft one piece of medicine
    {
        if (talents.Count == 0)
        {
            characteristics.Reset();
            view.rPanel.SetPanel(characteristics); return;
        }
        CalculateResourcesNeeded(talents);
        characteristics.toxicity = CalculateToxicity(talents);
        characteristics.healingRate = CalculateHealingRate(talents);
        if(!isLiquid)
        {
            characteristics *= 2;
            characteristics.plasticNeeded=0;
        }
        view.rPanel.SetPanel(characteristics);
    }
    public Characteristics Recombine(Characteristics ch)
    {
        CalculateResourcesNeeded(selectedTalents);
        characteristics.toxicity = ch.toxicity;
        characteristics.healingRate = ch.healingRate;
        if (!isLiquid)
        {
            characteristics *= 2;
            characteristics.plasticNeeded = 0;
        }
        view.rPanel.SetPanel(ch);
        return ch;
    }
    private bool SelectedTalentsContain(Talent talent)
    {
        int counter = 0;
        foreach (Talent t in selectedTalents)
        {
            if (t == talent)
                counter++;
        }

        if (counter > 0)
            return true;

        else return false;
    }
    private int MatchTalents()
    {
        int matchCounter = 0;
        List<Talent> primaryTalents = selectedTalents.Where(x => x.isPrimary).ToList();
        List<Talent> secondaryTalents = selectedTalents.Where(x => !x.isPrimary).ToList();
        foreach (Talent p in primaryTalents)
        {
            foreach (string pCombination in p.combinations)
            {
                foreach (Talent s in secondaryTalents)
                {
                        if (pCombination == s.description.Name)
                            matchCounter++;
                }
                
            }
         
        }
        Debug.Log("MATCHES: " + matchCounter);
        CharacteristicsScaling(matchCounter);
        return matchCounter;
    }
    private void CharacteristicsScaling(int matchCounter)
    {
        if (matchCounter <= 1) return;
        int healingMultiplier = matchCounter *5;
        int toxicityReducer = matchCounter * 2;
        characteristics.healingRate += healingMultiplier;
        characteristics.toxicity -= toxicityReducer;
        Recombine(characteristics);
        
    }
    public Characteristics GetCharacteristics()
    {
        
        return characteristics;
    }
    public string Craft(string Name, int quantity, Sprite avatar) // craft new recipe
    {
        Player player = GameController.instance.player;
        characteristics *= quantity;
        int researchPoints = CalculateResearchPoints(selectedTalents);
        if (player.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && player.resources.currentChemistry >= characteristics.chemistryNeeded
            && player.resources.currentPlastic >= characteristics.plasticNeeded
            && player.resources.ResearchPoints >= researchPoints)
        {
            
            characteristics /= quantity;
            Recipe recipe = new Recipe(Name, selectedTalents, characteristics, isLiquid, avatar); // create recipe
            int storageCapacity = player.inventory.GetNumberOfElements();
            player.inventory.Set(recipe, quantity); // set recipe to it's quantity via dictionary
            if (storageCapacity == player.inventory.GetNumberOfElements()) // check for max capacity in inventory
            {
                
                return "FULL";
            }
             characteristics *= quantity;
            player.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded, researchPoints);
            characteristics /= quantity;
            PopulateRecipeList(view.recipesListView,view.recipeInList); // refresh view
            view.capacity.text = player.inventory.GetNumberOfElements().ToString() + " / " + player.inventory.capacity.ToString(); // refresh capacity text
            recipeSelected = recipe;
            EventManager.TriggerEvent("OnCraftMedicine", quantity);
            
            EventManager.TriggerEvent("OnCraftRecipe", 1);
            player.GainExperience(100*(recipeSelected.PTalents.Count+recipeSelected.STalents.Count));
            player.finances.AddToProducedItems(quantity);
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_chemical_engineering, quantity);
            return "TRUE";
           
        }
        else
        {
            characteristics /= quantity;
            return "FALSE";
        }
        
    }
    public string Craft( int quantity) // craft quantity of medicine according to selected recipe (field)
    {
       
        characteristics *= quantity;
        Player player = GameController.instance.player;
        if (player.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && player.resources.currentChemistry >= characteristics.chemistryNeeded
            && player.resources.currentPlastic >= characteristics.plasticNeeded)
        {
            player.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded);
            characteristics /= quantity;
            player.inventory.Set(recipeSelected, quantity); // set selected recipe to a new quantity
            Debug.Log(recipeSelected.description.Name + " " + quantity);
            Recipe r = recipeSelected;
           
            PopulateRecipeList(view.recipesListView, view.recipeInList); // refresh view
            recipeSelected = r;
            player.finances.AddToProducedItems(quantity);
            EventManager.TriggerEvent("OnCraftMedicine", quantity);
            
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_chemical_engineering, quantity);
            player.GainExperience(100 * (recipeSelected.PTalents.Count + recipeSelected.STalents.Count));
            view.PlayOnCraftParticles();
            return "TRUE";
        }
        else
        {
            characteristics /= quantity;
            return "FALSE";
        }
       
    }
    
    public void AssignTalent(Talent talent)
    {
        if (view.holderSelected == null)
            return;
        else if (view.holderSelected.Talent == null)
        {
            selectedTalents.Add(talent);
        }
        else
        {
            ResetTalent(view.holderSelected.Talent);
            selectedTalents.Add(talent);
        }
        talent.isSelected = true;
        view.holderSelected.Talent = talent;
        isLiquid = true;
        view.liquidSlider.value = view.liquidSlider.minValue;
        Recombine(selectedTalents);
        if(selectedTalents.Where(x=>x.isPrimary).ToList().Count>0)
            view.ReflectMatch( MatchTalents());
        isPrescripted = false;
        recipeSelected = null;
       
    }

    public Recipe RecognizeRecipe()
    {
        int counter = 0;
        foreach (Recipe recipe in player.inventory.recipes.Where(x=>!x.isDeleted).ToList())
        {
            foreach(Talent talent in recipe.Talents)
            {
                if(SelectedTalentsContain(talent))
                {
                    counter++;
                }
            }
            if (counter == recipe.Talents.Count && recipe.Talents.Count == selectedTalents.Count)
            {
                isLiquid = recipe.isLiquid;
                if (isLiquid)
                    view.liquidSlider.value = view.liquidSlider.minValue;
                else view.liquidSlider.value = view.liquidSlider.maxValue;
                Recombine(selectedTalents);
                return recipe;
            }
            counter = 0;
        }
        return null;
    }
  
    public void ResetTalent(Talent talent)
    {
        selectedTalents.Remove(talent);
        talent.isSelected = false;
        GameObject toInstantiate = Instantiate(view.elementInList, view.talentsListView);
        toInstantiate.GetComponentInChildren<CraftHolder>().Talent = talent;
        Recombine(selectedTalents);
        if (selectedTalents.Count == 0)
        {
            view.DisableAllParticles();
          view.ResetCraftButton();
        }
        else if (selectedTalents.Where(x => x.isPrimary).ToList().Count > 0) view.ReflectMatch(MatchTalents());
        else { view.DisableAllParticles(); view.ResetCraftButton(); }
        
    }
    public void AssignRecipe(Recipe recipe)
    {
        recipeSelected = recipe;
        isPrescripted = true;
        foreach (Talent tal in selectedTalents)
            tal.isSelected = false;
        selectedTalents.Clear();
        List<Talent> primaryTalents = recipe.PTalents;
        List<Talent> secondaryTalents = recipe.STalents;
       
        for (int i=0; i < view.primaryHolders.Count; i++)
        {

            view.primaryHolders[i].Talent = null;
            
            if (i >= primaryTalents.Count )
            {
                continue;
            }
            view.primaryHolders[i].Talent = primaryTalents[i];
                primaryTalents[i].isSelected = true;
                selectedTalents.Add(primaryTalents[i]);
                
        }
        
        for (int i = 0; i < view.secondaryHolders.Count; i++)
        {

            view.secondaryHolders[i].Talent = null;
            
            if (i >= secondaryTalents.Count)
            {
                continue;
            }
            view.secondaryHolders[i].Talent = secondaryTalents[i];
            secondaryTalents[i].isSelected = true;
                selectedTalents.Add(secondaryTalents[i]); 
        }
        isLiquid = recipe.isLiquid;
        Recombine(recipe.characteristics);
        
        DeleteAllTalents(view.talentsListView);
        if (view.holderSelected.tag == "PrimaryHolder")
        {
            PopulateTalentList(view.talentsListView, view.elementInList);
            view.HighlightHolders();
        }
        else
        {
            PopulateTalentList(view.talentsListView, view.elementInList, false);
            view.HighlightHolders(false);
        } 
    }
    public List<Talent> MergeTalents(List<Talent> list1, List<Talent> list2)
    {
        List<Talent> newList = list2.ConvertAll(x => (Talent)x);
       newList.AddRange(list1);
        return newList;
    }
    public List<TalentHolder> MergeHolders(List<TalentHolder> list1, List<TalentHolder> list2)
    {
        List<TalentHolder> newList = list2.ConvertAll(x => (TalentHolder)x);
        newList.AddRange(list1);
        return newList;
    }
    public void DeleteAllTalents(Transform panel)
    {
        ListPopulator.DeleteAllTalents(panel);
        openTalents.Clear();    
    }
    public void DeleteAllRecipes(Transform panel)
    {
        ListPopulator.DeleteAllRecipes(panel);
        recipeSelected = null;
    }
    public void PopulateTalentList(Transform panel, GameObject talentPrefab, bool primaryTalents = true)
    {

        ListPopulator.PopulateTalentList(panel, talentPrefab, GameController.instance.talentTree.talents, primaryTalents);
    }
    public void PopulateRecipeList(Transform panel, GameObject recipePrefab)
    {

        ListPopulator.PopulateRecipeList(panel, recipePrefab, GameController.instance.player.inventory.recipes.Where(x=>!x.isDeleted).ToList());
    }
    public void CraftON()
    {
        view.recipeHolderSelected = null;
        GameController.instance.IsGameSceneEnabled = false;
        view.craftPanel.gameObject.SetActive(true);
        view.capacity.text = player.inventory.GetNumberOfElements().ToString() + " / " + player.inventory.capacity.ToString();
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        StartCoroutine(GameController.instance.cam.ResetCamera());
        characteristics.Reset();
        DeleteAllTalents(view.talentsListView);
        PopulateTalentList(view.talentsListView, view.elementInList, true);
        PopulateRecipeList(view.recipesListView, view.recipeInList);
        GameController.instance.time.Pause();
        GameController.instance.audio.player.MakeAmbience(ambienceSound[Random.Range(0,ambienceSound.Count)]);
        recipeSelected = null;
        //tutor section
        if (!tutorial.isTutorialCompleted) { GameController.instance.buttons.HideCancel();  tutorial.StartTutorial(); }

        
    }
    public void CraftOFF()
    {
        view.recipeHolderSelected = null;
        GameController.instance.IsGameSceneEnabled = true;
        selectedTalents.Clear();
        characteristics.Reset();
        recipeSelected = null;
        foreach (TalentHolder holder in view.primaryHolders)
        {
            if(holder.Talent!=null)
                 holder.Talent.isSelected = false;
            holder.Talent = null;
        }
        foreach(TalentHolder holder in view.secondaryHolders)
        {
            if (holder.Talent != null)
                holder.Talent.isSelected = false;
            holder.Talent = null;
        }
        
        view.primaryHolders.ForEach(x => {x.glowImg.gameObject.SetActive(false);  });
        view.secondaryHolders.ForEach(x => { x.glowImg.gameObject.SetActive(false); });
        view.craftPanel.gameObject.SetActive(false);
        GameController.instance.buttons.ShowAllButtons();
        GameController.instance.time.UnPause();
        GameController.instance.audio.player.StopAmbience();
        if (!GameController.instance.generalTutorial.isTutorialCompleted)
        {
            GameController.instance.generalTutorial.isBlocked = false;
            GameController.instance.generalTutorial.ContinueTutorial();
            
        }
    }
    private void OnEnable()
    {
        view.rPanel.SetPanel(characteristics);
    }
   
}
[System.Serializable]
public class SavedUnlockmentsData
{
   public List<int> data1 = new List<int>();
   public List<int> data2 = new List<int>();

    public SavedUnlockmentsData(List<int> data1, List<int> data2)
    {
        this.data1 = data1;
        this.data2 = data2;
    }
}
