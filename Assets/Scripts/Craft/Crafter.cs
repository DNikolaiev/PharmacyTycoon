using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crafter : MonoBehaviour {

   [SerializeField] private List<Talent> openTalents;
   
    public bool isLiquid = true;
    public bool isPrescripted = false;
    public Recipe recipeSelected;
    public List<Talent> selectedTalents;
    public Inventory inventory;

    public CraftView view; // reflects changes in game view
    public CraftController controller; // controls input in craft screen
   
    [SerializeField] Characteristics characteristics;
    
    private void Start()
    {
        view.capacity.text = inventory.GetNumberOfElements().ToString() + " / " + inventory.capacity.ToString();
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
        return result;
    }
    private void Recombine(List<Talent> talents) // recalculate values needed to craft one piece of medicine
    {
        if (talents.Count == 0)
        {
            characteristics.Reset();
            view.rPanel.SetPanel(characteristics); return;
        }
        CalculateResourcesNeeded(talents);
        characteristics.toxicity = CalculateToxicity(talents);
        characteristics.healingRate = CalculateHealingRate(talents);
        view.rPanel.SetPanel(characteristics);
    }
    private void Recombine(Characteristics ch)
    {
        CalculateResourcesNeeded(selectedTalents);
        characteristics.toxicity = ch.toxicity;
        characteristics.healingRate = ch.healingRate;
        view.rPanel.SetPanel(ch);
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
        return matchCounter;
    }
    public Characteristics GetCharacteristics()
    {
        return characteristics;
    }
    public string Craft(string Name, int quantity) // craft new recipe
    {
        Player player = GameController.instance.player;
        characteristics *= quantity;
        int researchPoints = CalculateResearchPoints(selectedTalents);
        if (player.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && player.resources.currentChemistry >= characteristics.chemistryNeeded
            && player.resources.currentPlastic >= characteristics.plasticNeeded
            && player.resources.ResearchPoints >= researchPoints)
        {
            var messageBox = GameController.instance.buttons.messageBox;
            characteristics /= quantity;
            Recipe recipe = new Recipe(Name, selectedTalents, characteristics, isLiquid); // create recipe
            int storageCapacity = inventory.GetNumberOfElements();
            inventory.Set(recipe, quantity); // set recipe to it's quantity via dictionary
            if (storageCapacity == inventory.GetNumberOfElements()) // check for max capacity in inventory
            {
                messageBox.Show("Inventory is full. Delete one recipe");
                return "FULL";
            }
             characteristics *= quantity;
            player.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded, researchPoints);
            characteristics /= quantity;
            PopulateRecipeList(view.recipesListView,view.recipeInList); // refresh view
            view.capacity.text = inventory.GetNumberOfElements().ToString() + " / " + inventory.capacity.ToString(); // refresh capacity text
            messageBox.Show(recipe.description.Name + " was created");
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
        var messageBox = GameController.instance.buttons.messageBox;
        characteristics *= quantity;
        Player player = GameController.instance.player;
        if (player.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && player.resources.currentChemistry >= characteristics.chemistryNeeded
            && player.resources.currentPlastic >= characteristics.plasticNeeded)
        {
            player.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded);
            characteristics /= quantity;
            inventory.Set(recipeSelected, quantity); // set selected recipe to a new quantity
            Debug.Log(recipeSelected.description.Name + " " + quantity);
            
            messageBox.Show("x" + quantity + " " + recipeSelected.description.Name + " crafted");
            PopulateRecipeList(view.recipesListView, view.recipeInList); // refresh view
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
        Recombine(selectedTalents);
        view.ReflectMatch( MatchTalents());
        isPrescripted = false;
        recipeSelected = null;      
    }

    public Recipe RecognizeRecipe()
    {
        int counter = 0;
        foreach (Recipe recipe in inventory.recipes)
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
                Debug.Log(counter + " " + recipe.Talents.Count + " " + selectedTalents.Count);
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
            view.DisableAllParticles();
        else view.ReflectMatch(MatchTalents());
        
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
            var children = new List<GameObject>();
            foreach (Transform child in panel) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        openTalents.Clear();    
    }
    public void DeleteAllRecipes(Transform panel)
    {
        var children = new List<GameObject>();
        foreach (Transform child in panel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        recipeSelected = null;
    }
    public void PopulateTalentList(Transform panel, GameObject talentPrefab, bool primaryTalents = true)
    {
        
        if (!primaryTalents)
            openTalents = GameController.instance.researcher.talents.Where(p => p.isUnlocked && !p.isSelected && !p.isPrimary).ToList(); //populate list
        else openTalents = GameController.instance.researcher.talents.Where(p => p.isUnlocked && !p.isSelected && p.isPrimary).ToList();
        foreach (Talent talent in openTalents)
        {
            GameObject toInstantiate = Instantiate(talentPrefab, panel);
            toInstantiate.GetComponentInChildren<CraftHolder>().Talent = talent; 
        }
    }
    public void PopulateRecipeList(Transform panel, GameObject recipePrefab)
    {
        DeleteAllRecipes(panel);
        List<Recipe> openRecipes = inventory.recipes;
        foreach (Recipe recipe in openRecipes)
        {
            
            GameObject toInstantiate = Instantiate(recipePrefab, panel);
            toInstantiate.GetComponentInChildren<RecipeHolder>().recipe = recipe;
            toInstantiate.GetComponentInChildren<RecipeHolder>().SetPanel();

        }
    }
    public void CraftON()
    {
        view.recipeHolderSelected = null;
        GameController.instance.isGameSceneEnabled = false;
        view.craftPanel.gameObject.SetActive(true);
        GameController.instance.buttons.HideAllButtons();
        GameController.instance.buttons.cancel.gameObject.SetActive(true);
        characteristics.Reset();
        DeleteAllTalents(view.talentsListView);
        PopulateTalentList(view.talentsListView, view.elementInList, true);
        PopulateRecipeList(view.recipesListView, view.recipeInList);
        recipeSelected = null;
    
    }
    public void CraftOFF()
    {
        view.recipeHolderSelected = null;
        GameController.instance.isGameSceneEnabled = true;
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
    }
    private void OnEnable()
    {
        view.rPanel.SetPanel(characteristics);
    }
}
