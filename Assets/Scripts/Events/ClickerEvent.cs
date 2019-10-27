using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClickerEvent : CustomEvent
{
    
    [SerializeField] GameObject clickable;
    [SerializeField] float spread;
    public string result;
    public List<UnityEvent> actionOne;
    public override void FireEvent()
    {
        GameController.instance.cam.ToggleCameraNoDelay(0);
       StartCoroutine( GameController.instance.cam.FocusCamera(Vector2.zero));
        foreach(SceneObject sceneObject in GetObjects(spread))
        {
            //stop any work on object
            sceneObject.canWork = false;
            sceneObject.StopAllCoroutines();
            // spawn clickable effects
            Instantiate(clickable, sceneObject.transform.position,Quaternion.identity,sceneObject.transform);
            if (sceneObject.GetComponent<Manufactory>() != null)
            {
                if (sceneObject.GetComponent<Manufactory>().finishedProductionImage != null)
                    sceneObject.GetComponent<Manufactory>().finishedProductionImage.SetActive(false);
            }
        }
        // fire unity events
        if (actionOne.Count > 0)
        {
            foreach (UnityEvent e in actionOne)
                e.Invoke();
        }
    }
    private List<SceneObject> GetObjects(float spread) // spread is percentage value, 0.1, 0.4, 0.6.... Part of objects that will be affected
    {

        List<SceneObject> resultedList = new List<SceneObject>();
        List<SceneObject> allObjects = GameController.instance.roomOverseer.GetAllSceneObjects();
        if (allObjects.Count == 0) return resultedList ;
        while ((float)((float)resultedList.Count / (float)allObjects.Count) < spread) {
            Debug.Log(resultedList.Count / allObjects.Count);
            SceneObject objectToAdd = allObjects[Random.Range(0, allObjects.Count)];
            if (!resultedList.Contains(objectToAdd) && objectToAdd.GetComponent<BoxCollider>().enabled)
                resultedList.Add(objectToAdd);
        }
        return resultedList;
    }
}
