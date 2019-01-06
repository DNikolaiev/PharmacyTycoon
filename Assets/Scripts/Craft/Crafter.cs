using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class Crafter : MonoBehaviour {

   [SerializeField] private List<Talent> openTalents;
   
    public bool isLiquid = true;
    public bool isPrescripted = false;
    public List<TalentHolder> primaryHolders;
    public List<TalentHolder> secondaryHolders;
    public CraftHolder holderSelected;
    public Recipe recipeSelected;
    public List<Talent> selectedTalents;
    public Inventory inventory;

    // pseudo-public (just need them in inspector)
    #region inspector
    public CraftView view;
    public ResourcePanel rPanel;
    public GameObject elementInList; // prefab for displaying talent as in-list element
    public GameObject recipeInList; // prefab for displaying recipe as in-list element
    public Transform talentsListView; // list of talents
    public Transform recipesListView; //list of recipes
    public DescriptionPanel craftDescriptionPanel;
    public CraftController controller; // controls input in craft screen
    public Image craftPanel; // main panel
    public Text capacity;
    #endregion
    [SerializeField] Characteristics characteristics;


    public static Crafter instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
    }
    private void Start()
    {
        capacity.text = inventory.GetNumberOfElements().ToString() + " / " + inventory.capacity.ToString();
        craftPanel.gameObject.SetActive(false);
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
            rPanel.SetPanel(characteristics); return;
        }
        CalculateResourcesNeeded(talents);
        characteristics.toxicity = CalculateToxicity(talents);
        characteristics.healingRate = CalculateHealingRate(talents);
        rPanel.SetPanel(characteristics);
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
        characteristics *= quantity;
        int researchPoints = CalculateResearchPoints(selectedTalents);
        if (Player.instance.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && Player.instance.resources.currentChemistry >= characteristics.chemistryNeeded
            && Player.instance.resources.currentPlastic >= characteristics.plasticNeeded
            && Player.instance.resources.ResearchPoints >= researchPoints)
        {
            var messageBox = ButtonController.instance.messageBox;
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
            Player.instance.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded, researchPoints);
            characteristics /= quantity;
            PopulateRecipeList(); // refresh view
            capacity.text = inventory.GetNumberOfElements().ToString() + " / " + inventory.capacity.ToString(); // refresh capacity text
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
        var messageBox = ButtonController.instance.messageBox;
        characteristics *= quantity;

        if (Player.instance.resources.currentHealingPlants >= characteristics.healingPlantsNeeded // does player has enough resources?
            && Player.instance.resources.currentChemistry >= characteristics.chemistryNeeded
            && Player.instance.resources.currentPlastic >= characteristics.plasticNeeded)
        {
            Player.instance.resources.SpendResources(characteristics.healingPlantsNeeded, characteristics.chemistryNeeded, characteristics.plasticNeeded);
            characteristics /= quantity;
            inventory.Set(recipeSelected, quantity); // set selected recipe to a new quantity
            Debug.Log(recipeSelected.description.Name + " " + quantity);
            
            messageBox.Show("x" + quantity + " " + recipeSelected.description.Name + " crafted");
            PopulateRecipeList(); // refresh view
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
        if (holderSelected == null)
            return;
        else if (holderSelected.Talent == null)
        {
            selectedTalents.Add(talent);
        }
        else
        {
            ResetTalent(holderSelected.Talent);
            selectedTalents.Add(talent);
        }
        talent.isSelected = true;
        holderSelected.Talent = talent;
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
        GameObject toInstantiate = Instantiate(elementInList, talentsListView);
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
       
        for (int i=0; i <primaryHolders.Count; i++)
        {
           
            primaryHolders[i].Talent = null;
            
            if (i >= primaryTalents.Count )
            {
                continue;
            }  
                primaryHolders[i].Talent = primaryTalents[i];
                primaryTalents[i].isSelected = true;
                selectedTalents.Add(primaryTalents[i]);
                
        }
        for (int i = 0; i < secondaryHolders.Count; i++)
        {
            
            secondaryHolders[i].Talent = null;
            
            if (i >= secondaryTalents.Count)
            {
                continue;
            }
                secondaryHolders[i].Talent = secondaryTalents[i];
            secondaryTalents[i].isSelected = true;
                selectedTalents.Add(secondaryTalents[i]); 
        }
        Recombine(selectedTalents);
        DeleteAllTalents();
        if (holderSelected.tag == "PrimaryHolder")
        {
            PopulateTalentList();
            HighlightHolders();
        }
        else
        {
            PopulateTalentList(false);
            HighlightHolders(false);
        }
        
       
    }
    
    public void HighlightHolders(bool isPrimary=true)
    {
      
        if (isPrimary)
        {
            primaryHolders.ForEach(x =>
            {
                if (x.Talent == null && x.isUnlocked)
                { x.glowImg.gameObject.SetActive(true); }
                else { x.glowImg.gameObject.SetActive(false); }
            });
            secondaryHolders.ForEach(x => x.glowImg.gameObject.SetActive(false));
        }
        else
        {
            secondaryHolders.ForEach(x => {
                if (x.Talent == null && x.isUnlocked)
                { x.glowImg.gameObject.SetActive(true); }
                else { x.glowImg.gameObject.SetActive(false); }
            });
            primaryHolders.ForEach(x => x.glowImg.gameObject.SetActive(false));
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
    
    public void DeleteAllTalents()
    {
            var children = new List<GameObject>();
            foreach (Transform child in talentsListView) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        openTalents.Clear();    
    }
    public void DeleteAllRecipes()
    {
        var children = new List<GameObject>();
        foreach (Transform child in recipesListView) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        recipeSelected = null;
    }
    public void PopulateTalentList(bool primaryTalents=true)
    {
        
        if (!primaryTalents)
            openTalents = Researcher.instance.talents.Where(p => p.isUnlocked && !p.isSelected && !p.isPrimary).ToList(); //populate list
        else openTalents = Researcher.instance.talents.Where(p => p.isUnlocked && !p.isSelected && p.isPrimary).ToList();
        foreach (Talent talent in openTalents)
        {
            GameObject toInstantiate = Instantiate(elementInList, talentsListView);
            toInstantiate.GetComponentInChildren<CraftHolder>().Talent = talent; 
        }
    }
    public void PopulateRecipeList()
    {
        DeleteAllRecipes();
        List<Recipe> openRecipes = inventory.recipes;
        foreach (Recipe recipe in openRecipes)
        {
            
            GameObject toInstantiate = Instantiate(recipeInList, recipesListView);
            toInstantiate.GetComponentInChildren<RecipeHolder>().recipe = recipe;
            toInstantiate.GetComponentInChildren<RecipeHolder>().SetPanel();

        }
    }

    public void UnlockNewHolders(int primaryUnlock, int secondaryUnlock)
    {
        primaryUnlock++;
        secondaryUnlock++;
        int lastIndex = 0;
        for (int i = 0; i<primaryHolders.Count; i++)
        {
            if(primaryHolders[i].isUnlocked)
            lastIndex = i;
        }
        int finalUnlock = Mathf.Clamp(lastIndex + primaryUnlock, 0, primaryHolders.Count) ;
        for(int n=lastIndex+1; n<finalUnlock; n++)
        {
            primaryHolders[n].Unlock(true);
        }
        lastIndex = 0;
        for (int i = 0; i < secondaryHolders.Count; i++)
        {
            if (secondaryHolders[i].isUnlocked)
                lastIndex = i;
        }
        finalUnlock = Mathf.Clamp(lastIndex + secondaryUnlock, 0, secondaryHolders.Count);
        for (int n = lastIndex+1; n < finalUnlock; n++)
        {
            secondaryHolders[n].Unlock(true);
        }

    }
    public void CraftON()
    {
        GameController.instance.isGameSceneEnabled = false;
        craftPanel.gameObject.SetActive(true);
        ButtonController.instance.HideAllButtons();
        ButtonController.instance.cancel.gameObject.SetActive(true);
        characteristics.Reset();
        PopulateTalentList(true);
        DeleteAllTalents();
        PopulateTalentList(true);
    
    }
    public void CraftOFF()
    {
        GameController.instance.isGameSceneEnabled = true;
        selectedTalents.Clear();
        characteristics.Reset();
        foreach (TalentHolder holder in primaryHolders)
        {
            if(holder.Talent!=null)
                 holder.Talent.isSelected = false;
            holder.Talent = null;
        }
        foreach(TalentHolder holder in secondaryHolders)
        {
            if (holder.Talent != null)
                holder.Talent.isSelected = false;
            holder.Talent = null;
        }
        primaryHolders.ForEach(x => {x.glowImg.gameObject.SetActive(false);  });
        secondaryHolders.ForEach(x => { x.glowImg.gameObject.SetActive(false); });
        craftPanel.gameObject.SetActive(false);
        ButtonController.instance.ShowAllButtons();
    }
    private void OnEnable()
    {
        rPanel.SetPanel(characteristics);
    }
}
