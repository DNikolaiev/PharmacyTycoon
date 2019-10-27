using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsPanel : Panel
{
    [SerializeField] Canvas canvas;
    [SerializeField] QuestGiver questGiver;
    [SerializeField] GameObject questPrefab;
    [SerializeField] GameObject questHolder;
    [SerializeField] GameObject questsView;
    [SerializeField] ParticleSystem onQuestComplete;
    public override void Hide()
    {
        
    }

    public override void SetPanel()
    {
        ListPopulator.PopulateQuestList(questsView.transform, questHolder, questGiver.player.activeQuests, canvas, questGiver, questPrefab);
        
        
    }
    public void PlayParticle()
    {
        onQuestComplete.Play();
    }
    private void OnEnable()
    {
        EventManager.StartListening(TurnController.OnTurnEvent, SetPanel);
        EventManager.StartListening("OnGetReward", PlayParticle);
        Invoke("SetPanel", 0.1f);
    }
    private void OnDisable()
    {
        Hide();
        EventManager.StopListening("OnGetReward", PlayParticle);
        EventManager.StopListening(TurnController.OnTurnEvent, SetPanel);
        
    }
    
}
