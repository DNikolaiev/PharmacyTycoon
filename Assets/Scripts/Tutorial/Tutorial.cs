using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    
    [SerializeField] private Actor actor;
    public TutorialPanel tPanel;
    public Dialogue dialogue;
    public bool isTutorialCompleted;
    public List<GameObject> highlightedObjects;
    [SerializeField] List<GameObject> objectsToClose;
    [SerializeField] List<GameObject> objectsToMove;
    [SerializeField] List<RectTransform> placesToMove;
    private int highlightIndex = -1;
    private int moveIndex = -1;
    public bool isBlocked = false;
    public bool activateCameraOnComplition = true;
    public bool startTimeOnComplition = false;
    public Transform GetPlaceToMove()
    {
        return placesToMove[moveIndex];
    }
    public GameObject GetObjectToMove()
    {
        if (moveIndex < objectsToMove.Count)
            moveIndex++;
        if (moveIndex >= objectsToMove.Count) moveIndex--;
        return objectsToMove[moveIndex];
    }
    public GameObject HighlightObject()
    {
        if (highlightIndex < highlightedObjects.Count)
            highlightIndex++;
        
        return highlightedObjects[highlightIndex];
    }
    public void ReturnHighLights()
    {
        highlightIndex--;
    }
    public void StartTutorial()
    {
        isTutorialCompleted = false;
        highlightIndex = -1;
        dialogue.Initialize();
        tPanel.SetPanel(this, actor);
        
    }
    public void ContinueTutorial()
    {
        if (isTutorialCompleted) return;
        isTutorialCompleted = false;
        tPanel.SetPanel(this, actor);
        GameController.instance.IsGameSceneEnabled = false;
    }
    public void CompleteTutorial()
    {
    
        isTutorialCompleted = true;
        
        if(dialogue.dialogueName=="General")
        {
            Debug.Log("UNLOCK ACHIEV");
            PlayGameScript.UnlockAchievement(GPGSIds.achievement_youve_lasted_so_far_heh);
        }
        if(tPanel.gameObject.activeInHierarchy)
            tPanel.Hide();
        if(objectsToClose.Count>0)
        {
            foreach(GameObject g in objectsToClose)
            {
                g.SetActive(false);
            }
        }

    }

}
