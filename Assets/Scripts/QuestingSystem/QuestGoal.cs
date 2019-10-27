using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestObjectiveType
{
    BuildRoom,
    BuildObject,
    Research,
    Kill,
    Cure,
    CraftMedicine,
    CraftPills,
    CraftLiquid,
    CraftRecipe,
    SuriveYears,
    Sell,
    CollectMoney,
    CollectHerbs,
    CollectChems,
    CollectPlastic,
    Overkill,
    ReduceToxicity,
    Heal,
    Upgrade

}
[System.Serializable]
public class QuestGoal {
    private Quest quest;
    private string eventName;
    public string description;
    public QuestObjectiveType objective;
    public int currentAmount;
    public int neededAmount;
    public bool isCompleted;
    
    public void AssignToQuest(Quest quest)
    {
        this.quest = quest;
    }
    public bool Evaluate()
    {
        isCompleted = (currentAmount >= neededAmount) ? true : false;
        if (isCompleted) quest.Complete();
        return isCompleted;
    }
    private void AchieveObjective(object value)
    {
        currentAmount += (int)value;
        Debug.Log(currentAmount + " " + value);
        Evaluate();
    }
    public void StopListeningToObjective()
    {
        if(eventName!=null)
        EventManager.StopListening(eventName, AchieveObjective);
    }
    public void StartListeningToObjective()
    {
        switch (objective)
        {
            case QuestObjectiveType.Upgrade:
                {
                    EventManager.StartListening("OnUpgrade", AchieveObjective);
                    eventName = "OnUpgrade";
                    break;
                }
            case QuestObjectiveType.Heal:
                {
                    EventManager.StartListening("OnHeal", AchieveObjective);
                    eventName = "OnHeal";
                    break;
                }
            case QuestObjectiveType.BuildObject: //implemented
                {
                    EventManager.StartListening("OnBuildObject", AchieveObjective);
                    eventName = "OnBuildObject";
                    break;
                }
            case QuestObjectiveType.BuildRoom://implemented
                {
                    EventManager.StartListening("OnBuildRoom", AchieveObjective);
                    eventName = "OnBuildRoom";
                    break;
                }
            case QuestObjectiveType.CollectChems://implemented
                {
                    EventManager.StartListening("OnCollectChems",AchieveObjective);
                    eventName = "OnCollectChems";
                    break;
                }
            case QuestObjectiveType.CollectHerbs://implemented
                {
                    EventManager.StartListening("OnCollectHerbs", AchieveObjective);
                    eventName = "OnCollectHerbs";
                    break;
                }
            case QuestObjectiveType.CollectPlastic://implemented
                {
                    EventManager.StartListening("OnCollectPlastic", AchieveObjective);
                    eventName = "OnCollectPlastic";
                    break;
                }
            case QuestObjectiveType.CollectMoney://implemented
                {
                    EventManager.StartListening("OnCollectMoney", AchieveObjective);
                    eventName = "OnCollectMoney";
                    break;
                }
            case QuestObjectiveType.CraftMedicine://implemented
                {
                    EventManager.StartListening("OnCraftMedicine", AchieveObjective);
                    eventName = "OnCraftMedicine";
                    break;
                }
            case QuestObjectiveType.CraftRecipe://implemented
                {
                    EventManager.StartListening("OnCraftRecipe", AchieveObjective);
                    eventName = "OnCraftRecipe";
                    break;
                }
            case QuestObjectiveType.Cure://implemented
                {
                    EventManager.StartListening("OnCure", AchieveObjective);
                    eventName = "OnCure";
                    break;
                }
            case QuestObjectiveType.Kill://implemented
                {
                    EventManager.StartListening("OnKill", AchieveObjective);
                    eventName = "OnKill";
                    break;
                }
            case QuestObjectiveType.ReduceToxicity://implemented
                {
                    EventManager.StartListening("OnReduceToxicity", AchieveObjective);
                    eventName = "OnReduceToxicity";
                    break;
                }
            case QuestObjectiveType.Research://implemented
                {
                    EventManager.StartListening("OnResearch", AchieveObjective);
                    eventName = "OnResearch";
                    break;
                }
            case QuestObjectiveType.Sell://implemented
                {
                    EventManager.StartListening("OnSell", AchieveObjective);
                    eventName = "OnSell";
                    break;
                }
            case QuestObjectiveType.SuriveYears://implemented
                {
                    EventManager.StartListening("OnSurviveYears", AchieveObjective);
                    eventName = "OnSurviveYears";
                    break;
                }
            case QuestObjectiveType.Overkill://implemented
                {
                    EventManager.StartListening("OnOverkill", AchieveObjective);
                    eventName = "OnOverkill";
                    break;
                }
            case QuestObjectiveType.CraftLiquid://implemented
                {
                    EventManager.StartListening("OnCraftLiquid", AchieveObjective);
                    eventName = "OnCraftLiquid";
                    break;
                }
            case QuestObjectiveType.CraftPills://implemented
                {
                    EventManager.StartListening("OnCraftPills", AchieveObjective);
                    eventName = "OnCraftPills";
                    break;
                }
        }
    }

}
