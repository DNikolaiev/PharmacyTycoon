using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="newQuest",menuName ="Quest")]
public class Quest : ScriptableObject {

    public int duration;
    public string Name;
    public string description;
    public QuestGoal goal;
    public Reward reward;
    public bool isActive;
    private void DecreaseDuration(object date)
    {
        duration--;
        if (duration <= 1)
            Abort();
    }
    public void Initialize(int maxQuestsCount, bool isActive = true)
    {
        
        goal.isCompleted = false;
        goal.currentAmount = 0;
        goal.StartListeningToObjective();
        goal.AssignToQuest(this);
        this.isActive = isActive;
        if (isActive)
            DetermineDuration(maxQuestsCount);
        
    }
    
    public IEnumerator Load()
    {
        yield return new WaitForSecondsRealtime(1);
        goal.StopListeningToObjective();
        goal.StartListeningToObjective();
        goal.AssignToQuest(this);
        yield return null;
        
    }
    private void DetermineDuration(int maxQuestCount)
    {
        duration = maxQuestCount * 30;
        EventManager.StartListening(TurnController.OnDayEvent, DecreaseDuration);
    }
    private void Abort()
    {
        FinishQuest();
        EventManager.TriggerEvent("OnQuestAbort", this);
    }
    private void FinishQuest()
    {
        isActive = false;
        goal.isCompleted = false;
        goal.StopListeningToObjective();
        EventManager.StopListening(TurnController.OnDayEvent, DecreaseDuration);
    }
    public void Complete()
    {
        FinishQuest();
        EventManager.TriggerEvent("OnQuestComplete", this);
    }
}
