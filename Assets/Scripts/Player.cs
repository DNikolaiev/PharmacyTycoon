using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Player : MonoBehaviour { //save all
    public static readonly string OnLevelEvent = "OnLevelUpEvent";
    public string Name;
    public List<Quest> activeQuests;
    public Finances finances;
    public Inventory inventory;
    public int level = 1;
    public int experience;
    public int experienceNeeded;
    public bool hasAutoClicker;
    public bool premiumState;
    public bool canPlay = true;
    public Skills skills;
    public ResourceStorage resources;
    public PlayerData playerData;
    [SerializeField] private PlayerPanel panel;
    [SerializeField] private LevelUpPanel levelPanel;
    
   
    public void GainExperience(int value)
    {
        if (experience + value < experienceNeeded)
            experience += value;
        else
        {
            experience = value - (experienceNeeded - experience);
            LevelUp();

        }
        panel.SetPanel();
    }
   
    
    private void Start()
    {
        Initialize();
        if(finances.revenue.Count==0 || finances.staticExpences.Count==0)
        {
            finances.Initialize();
        }
       
    }
   
    private void OnDisable()
    {
        EventManager.StopListening(TurnController.OnTurnEvent, finances.expences.ChargeMonthlyExpences);
 
        EventManager.StopListening(TurnController.OnYearEvent, finances.expences.ChargeTaxes);
    }
    private void OnEnable()
    {
        EventManager.StartListening(TurnController.OnYearEvent, finances.expences.ChargeTaxes);
        EventManager.StartListening(TurnController.OnTurnEvent, finances.expences.ChargeMonthlyExpences);
        SaveController.OnLoadEvent += OnLoad;
        SaveController.OnSaveEvent += OnSave;

        
    }
    public void Initialize()
    {
        skills.player = this;
        resources.rPanel.SetPanel(resources);
       experienceNeeded= CalculateExperienceNeeded();
        panel.SetPanel();
        
    }
    public void ApplySecondChance()
    {
        GameController.instance.player.resources.ChangeBalance(10000);
        foreach (Area a in World.instance.areas)
        {
            a.health += a.maxHealth;
        }
        resources.AddSecondChance(-1);
        GameController.instance.time.UnPause();
        GameController.instance.EnableGameOver(false);

    }
    public void LevelUp()
    {
        Researcher researcher = GameController.instance.researcher;
        level += 1;
        experienceNeeded = CalculateExperienceNeeded();
        EventManager.TriggerEvent(OnLevelEvent);
        GainExperience(1);
        panel.SetPanel();
        levelPanel.SetPanel();
        // unlock new inventory slots
        if (level == 4) { PlayGameScript.UnlockAchievement(GPGSIds.achievement_student); }
        else if (level==10) { PlayGameScript.UnlockAchievement(GPGSIds.achievement_scientific_worker); }
        else if (level == 16) { PlayGameScript.UnlockAchievement(GPGSIds.achievement_scholar); }
        if(level % 8==0)
        {
            var messabox = GameController.instance.buttons.messageBox;
            messabox.Show("Inventory capacity is increased");
            inventory.capacity += 2;
        }

    }
    private int CalculateExperienceNeeded()
    {
        
        return Mathf.FloorToInt((0.09f*(Mathf.Pow(level,3)))+0.9f*(level*level)+2*level)*55 + 75;
    }
   
    private void OnSave()
    {
        playerData = new PlayerData(level, experience, finances, resources, skills, activeQuests, hasAutoClicker,premiumState);
    }
    private void OnLoad()
    {
        this.level = playerData.level;
        this.experience = playerData.experience;
        this.finances = playerData.finances;
        this.skills = playerData.skills;
        this.activeQuests = playerData.quests;
        this.hasAutoClicker = playerData.hasAutoClicker;
        this.premiumState = playerData.premiumState;
        List<Quest> allQuests = FindObjectOfType<QuestGiver>().allQuests;
        foreach(Quest q in allQuests)
        {
            foreach (Quest a in activeQuests) {
                a.goal.StopListeningToObjective();
                if (q.Name == a.Name)
                {
                    q.isActive = a.isActive;
                    q.goal.currentAmount = a.goal.currentAmount;
                    q.duration = a.duration;
                    StartCoroutine(q.Load());
                }
                

            }
        }
        List<string> questNames = new List<string>();
        foreach(Quest q in activeQuests)
        {
            questNames.Add(q.Name);
        }
        foreach(Quest q in allQuests)
        {
            if(questNames.Contains(q.Name))
            {
                activeQuests.Remove(activeQuests.Where(x => x.Name == q.Name).LastOrDefault());
                activeQuests.Add(q);
            }
        }

        ResourcePanel r = resources.rPanel;
        this.resources = playerData.resources;
        this.resources.rPanel = r;
        panel.SetPanel();
    }
}
[System.Serializable]
public class PlayerData
{
    public int level;
    public int experience;
    public Finances finances;
    public ResourceStorage resources;
    public bool hasAutoClicker;
    public bool premiumState;
    public Skills skills;
    public List<Quest> quests;

    public PlayerData(int level, int experience, Finances finances, ResourceStorage resources, Skills skills, List<Quest> quests, bool hasAutoClicker, bool premiumState=false)
    {
        this.level = level;
        this.experience = experience;
        this.finances = finances;
        this.resources = resources;
        this.skills = skills;
        this.quests = new List<Quest>();
        this.quests.AddRange(quests);
        this.hasAutoClicker = hasAutoClicker;
        this.premiumState = premiumState;
    }
}
