using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
[System.Serializable]
public struct Characteristics
{
    public int healingPlantsNeeded;
    public int chemistryNeeded;
    public int plasticNeeded;
    public int toxicity;
    public int healingRate;

    public void Reset()
    {
        healingPlantsNeeded = 0;
        chemistryNeeded = 0;
        plasticNeeded = 0;
        toxicity = 0;
        healingRate = 0;
    }
}

public class Crafter : MonoBehaviour {

   [SerializeField] private List<Talent> openTalents;
    private bool isLiquid;
    public List<TalentHolder> primaryHolders;
    public List<TalentHolder> secondaryHolders;
    public CraftHolder holderSelected;
    public List<Talent> selectedTalents;
    public Inventory inventory;
    public ResourcePanel rPanel;
    public GameObject elementInList;
    public Transform talentsListView;
    public DescriptionPanel craftDescriptionPanel;
    public Slider slider;
    public CraftController controller;
    public Image craftPanel;
   
    Characteristics characteristics;


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

        craftPanel.gameObject.SetActive(false);
    }
   
    public void Craft(string Name, List<Talent> talents, int amount)
    {
        Recipe recipe = new Recipe(talents, characteristics, isLiquid);
        if (!inventory.CheckIfWarehouseContains(recipe))
        inventory.Set(recipe, amount);
        //create recipe and craft <amount> of medicine, use dictionary
    }
    public void Craft(Recipe recipe, int amount)
    {
        // craft <amount> according to recipe
        inventory.Set(recipe, amount);
    }
    public void Recombine(List<Talent> talents)
    {
        CalculateResourcesNeeded(talents);
       characteristics.toxicity = CalculateToxicity(talents);
        characteristics.healingRate = CalculateHealingRate(talents);
        rPanel.SetPanel(characteristics);
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
            selectedTalents.Remove(holderSelected.Talent);
            selectedTalents.Add(talent);
        }
        holderSelected.Talent = talent;
        Recombine(selectedTalents);
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
    private void CalculateResourcesNeeded(List<Talent> talents)
    {
        characteristics.Reset();
        foreach(Talent tal in talents)
        {
            this.characteristics.healingPlantsNeeded += tal.characteristics.healingPlantsNeeded;
            this.characteristics.chemistryNeeded += tal.characteristics.chemistryNeeded;
            this.characteristics.plasticNeeded += tal.characteristics.plasticNeeded;
        }
    }
    private int CalculateToxicity(List<Talent> talents)
    {
        int toxicity = 0;
       foreach( Talent tal in talents)
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            CraftOFF();
    }
    public void DeleteAllTalents()
    {
     
            var children = new List<GameObject>();
            foreach (Transform child in talentsListView) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        openTalents.Clear();
        
    }
    public void PopulateTalentList(bool primaryTalents=true)
    {
        
        if (!primaryTalents)
            openTalents = Researcher.instance.talents.Where(p => p.isUnlocked && !p.isPrimary).ToList(); //populate list
        else openTalents = Researcher.instance.talents.Where(p => p.isUnlocked && p.isPrimary).ToList();
        foreach (Talent talent in openTalents)
        {
            GameObject toInstantiate = Instantiate(elementInList, talentsListView);
            elementInList.GetComponentInChildren<CraftHolder>().Talent = talent;
            Debug.Log(elementInList.GetComponentInChildren<CraftHolder>().Talent.description.Name);
           
        }
    }
    public void CraftON()
    {
        craftPanel.gameObject.SetActive(true);
        ButtonController.instance.HideAllButtons();
        PopulateTalentList(true);
        DeleteAllTalents();
        PopulateTalentList(true);
    
    }
    public void CraftOFF()
    {
        foreach(TalentHolder holder in primaryHolders)
        {
            holder.Talent = null;
        }
        foreach(TalentHolder holder in secondaryHolders)
        {
            holder.Talent = null;
        }
        craftPanel.gameObject.SetActive(false);
        ButtonController.instance.ShowAllButtons();
    }
    
}
