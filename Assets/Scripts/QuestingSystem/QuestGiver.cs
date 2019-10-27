using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestGiver : MonoBehaviour {

    public List<Quest> allQuests;
    public Player player;
    [SerializeField] GameObject questHolder;
    [SerializeField] Canvas canvas;
    [SerializeField] ParticleSystem onQuestComplete;
    public int maxQuestsCount;
    private IEnumerator CheckOnCompletedQuests()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            NotifyAboutQuests();
        }
    }
    private void OnEnable()
    {
        
        EventManager.StartListening("OnQuestComplete", CompleteQuest);
        EventManager.StartListening("OnQuestAbort", AbortQuest);
        EventManager.StartListening(TurnController.OnTurnEvent, GiveQuest);
    
    }
    private void OnDisable()
    {

        EventManager.StopListening("OnQuestComplete", CompleteQuest);
        EventManager.StopListening("OnQuestAbort", AbortQuest);
        EventManager.StopListening(TurnController.OnTurnEvent, GiveQuest);
       
    }
    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(CheckOnCompletedQuests());

    }
    private void NotifyAboutQuests()
    {
        if (player.activeQuests.Where(x => !x.isActive).ToList().Count > 0)
        {
            ActivateQuestParticles();
        }
    }
    private void ActivateQuestParticles()
    {
        GameController.instance.buttons.quest.GetComponentInChildren<ParticleSystem>().Play();
    }
    private void AbortQuest(object Quest)
    {
        Debug.Log("AbortQuest");
        Quest quest = (Quest)Quest;
        player.activeQuests.Remove(quest);
        allQuests[allQuests.IndexOf(quest)].isActive = false;
    }
    private void GiveQuest()
    {
        if (player.activeQuests.Count == maxQuestsCount) return;
        int quests = player.activeQuests.Count;
        
        for (int q = 0; q < 1; q++)
        {
          
            if (q < 0) q = 0;
            int randomIndex = Random.Range(0, allQuests.Count);
            if (player.activeQuests.Contains(allQuests[randomIndex]) || allQuests[randomIndex].isActive)
            {
              
                q--;
            }
            else
            {
                player.activeQuests.Add(allQuests[randomIndex]);
                player.activeQuests.LastOrDefault().Initialize(maxQuestsCount);
                MessageBox messageBox = GameController.instance.buttons.messageBox;
                messageBox.Show("New Quest!", Resources.Load<Sprite>("Icons/quest1"));
                ActivateQuestParticles();

            }
            if (allQuests.Where(x=>x.isActive).ToList().
                Count==allQuests.Count|| player.activeQuests.Count!=quests|| 
                player.activeQuests.Count == allQuests.Count
                ) return;
        }

    }
  
    public void CompleteQuest(object quest)
    {
        Quest Quest = (Quest)quest;
        
        MessageBox messageBox = GameController.instance.buttons.messageBox;
        messageBox.Show("Quest Completed!", Resources.Load<Sprite>("Icons/quest1"));
        //allQuests[allQuests.IndexOf(Quest)].isActive = false;
        allQuests.Where(x => x.Name == Quest.Name).FirstOrDefault().isActive = false;
        GameController.instance.buttons.quest.GetComponentInChildren<ParticleSystem>().Play();
    }
    
}
