using System.Collections;
using UnityEngine;
[System.Serializable]
public class ResourceStorage {
    public ResourcePanel rPanel;

    public int currentHealingPlants;
    public int currentChemistry;
    public int currentPlastic;
    public int ResearchPoints;
    public int money;
    [SerializeField] private int _maxHealingPlants;
    [SerializeField] private int _maxChemistry;
    [SerializeField] private int _maxPlastic;
    [SerializeField] private int _maxResearch;

    public int MaxHealingPlants
    { get { return _maxHealingPlants; } private set { _maxHealingPlants = value; } }
    public int MaxChemistry
    { get { return _maxChemistry; } private set { _maxChemistry = value; } }
    public int MaxPlastic
    { get { return _maxPlastic; } private set { _maxPlastic = value; } }
    public int MaxResearchPoints
    { get { return _maxResearch; } private set { _maxResearch = value; } }


    public void ChangeBalance(int amount)
    {
        money += amount;
       rPanel.SetPanel(this);
    }
    public void ExpandPlantsStorage(int amount)
    {
        MaxHealingPlants += amount;
        rPanel.SetPanel(this);
    }
    public void ExpandChemistryStorage(int amount)
    {
        MaxChemistry += amount;
        rPanel.SetPanel(this);
    }
    public void ExpandPlasticStorage(int amount)
    {
        MaxPlastic += amount;
        rPanel.SetPanel(this);
    }
    public void AddHealingPlants(int amount)
    {
       currentHealingPlants= AddResources(amount, currentHealingPlants, MaxHealingPlants);
        rPanel.SetPanel(this);
    }
    public void AddResearchPoints(int amount)
    {
        ResearchPoints = AddResources(amount, ResearchPoints, MaxResearchPoints);
        rPanel.SetPanel(this);
    }
    public void AddChemistry(int amount)
    {
       currentChemistry= AddResources(amount, currentChemistry, MaxChemistry);
        rPanel.SetPanel(this);
    }
    public void AddPlastic(int amount)
    {
        currentPlastic= AddResources(amount, currentPlastic, MaxPlastic);
        rPanel.SetPanel(this);
    }
   
    public void SpendResources(int plants, int chem, int plastic, int researchPoints=0)
    {
        AddHealingPlants(-plants);
        AddChemistry(-chem);
        AddPlastic(-plastic);
        if (researchPoints != 0)
            AddResearchPoints(-researchPoints);
        rPanel.SetPanel(this);
        
    }
    private int AddResources(int amount,  int currentResource, int maxResource)
    {
        if (currentResource+amount<=maxResource)
        {
            currentResource += amount;
        }
        else if (currentResource + amount > MaxHealingPlants)
        {
            currentResource += maxResource - currentResource;
        }
        return currentResource;
    }

}
