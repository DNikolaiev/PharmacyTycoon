using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestChecker : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(CheckForCompletedQuests());
    }
    private IEnumerator CheckForCompletedQuests()
    {
        int count = 0;
            while (count==0)
            {

                Debug.Log("QUESTS COMPLETED: " + GameController.instance.player.activeQuests.Where(x => !x.isActive).ToList().Count);
                if (GameController.instance.player.activeQuests.Where(x => !x.isActive).ToList().Count > 0 && GameController.instance.IsGameSceneEnabled)
                {
                    Debug.Log("Muss quest erledigen");
                    GetComponentInChildren<ParticleSystem>().Play();
                }
                else GetComponentInChildren<ParticleSystem>().Stop();
            
                yield return new WaitForSeconds(2);
            }
        
        
    }
}
