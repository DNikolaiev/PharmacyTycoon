using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakdown : ClickObject
{
    [SerializeField] int moneyToFixObject;
    protected override void ClickAction()
    {
        GameController.instance.player.resources.ChangeBalance(-moneyToFixObject);
    }

    protected override void StartAction()
    {
        
    }
    private void OnDestroy()
    {
        if (GetComponentInParent<SceneObject>() != null)
            GetComponentInParent<SceneObject>().StartWorking();
        
    }
    
}
