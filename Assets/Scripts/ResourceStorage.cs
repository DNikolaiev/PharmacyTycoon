using System.Collections;
using UnityEngine;
[System.Serializable]
//save all
public class ResourceStorage {
   [FullSerializer.fsIgnore] public ResourcePanel rPanel;

    public int currentHealingPlants;
    public int currentChemistry;
    public int currentPlastic;
    public int ResearchPoints;
    public int money;
    public int medCoins;
    [SerializeField] private int secondChances;
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
    public int SecondChances
    { get { return secondChances; } private set { secondChances = value; } }
    public void AddSecondChance(int amount)
    {
        secondChances += amount;
        if(GameController.instance.gameOver.CheckCondition())
        {
            GameController.instance.player.ApplySecondChance();
        }
    }
    public void ChangeBalance(int amount, bool activeIncome=false)
    {
        money += amount;
       rPanel.SetPanel(this);
        if(amount!=0)
             GameController.instance.buttons.paymentPanel.SetPanel(amount);
        if (amount > 0 && activeIncome)
        {
            GameController.instance.player.finances.AddToRevenue(amount);
        }
        if (amount > 0)
        {
            EventManager.TriggerEvent("OnCollectMoney", amount);
            //add to achievement in google play
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_greed_before_need, amount);
        }
        
       

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
    public int AddHealingPlants(int amount)
    {
        if (amount > 0)
            EventManager.TriggerEvent("OnCollectHerbs",amount);
        int previousResource = currentHealingPlants;

       currentHealingPlants = AddResources(amount, previousResource, MaxHealingPlants);
      
        rPanel.SetPanel(this);
        if (currentHealingPlants == previousResource)
            return 0;
        return currentHealingPlants;
    }
    public int AddCoins(int amount)
    {
        medCoins += amount;
        return medCoins;
    }
    public int AddResearchPoints(int amount)
    {
        ResearchPoints = AddResources(amount, ResearchPoints, MaxResearchPoints);
        rPanel.SetPanel(this);
        if (ResearchPoints == MaxResearchPoints)
            return 0;
        return ResearchPoints;
    }
    public int AddChemistry(int amount)
    {
        if (amount > 0)
            EventManager.TriggerEvent("OnCollectChems",amount);
        int previousResource = currentChemistry;
        currentChemistry = AddResources(amount, previousResource, MaxChemistry);
        rPanel.SetPanel(this);
        if (currentChemistry == previousResource)
            return 0;
        return currentChemistry;
    }
    public int AddPlastic(int amount)
    {
        if (amount > 0)
            EventManager.TriggerEvent("OnCollectPlastic",amount);
        int previousResource = currentPlastic;
        currentPlastic = AddResources(amount, previousResource, MaxPlastic);
        rPanel.SetPanel(this);
        if (currentPlastic == previousResource)
            return 0;
        return currentPlastic;
        
    }
   
    public void SpendResources(int plants, int chem, int plastic, int researchPoints=0)
    {
        Debug.Log("SPEND RESOURCES HERBS: "+ currentHealingPlants);
        AddHealingPlants(-plants);
        
        AddChemistry(-chem);
        AddPlastic(-plastic);
        if (researchPoints != 0)
            AddResearchPoints(-researchPoints);
        rPanel.SetPanel(this);
        
    }
    private int AddResources(int amount,  int currentResource, int maxResource)
    {
        if (currentResource + amount < 0)
        {
            Debug.Log("SPEND RESOURCES HERBS: " + currentHealingPlants + " / MUST SPEND: " + amount);
            currentResource = 0; return currentResource;
        }
        if (currentResource+amount<=maxResource)
        {
            currentResource += amount;
        }
        else if (currentResource + amount > maxResource)
        {
            currentResource += maxResource - currentResource;
        }
       
        return currentResource;
    }
    public void OnLoad()
    {
        rPanel.SetPanel(this);
    }
}
