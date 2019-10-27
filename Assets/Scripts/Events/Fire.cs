using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : ClickObject
{
    [SerializeField] private int lossPerSecondMoney;
    [SerializeField] private int lossPerClickChemicals;
    [SerializeField] private AudioClip flameSound;

    protected override void ClickAction()
    {
        GameController.instance.player.resources.AddChemistry(-lossPerClickChemicals);
    }

    protected override void StartAction()
    {
        GameController.instance.audio.player.StopAmbience();
        GameController.instance.audio.player.MakeAmbience(flameSound);
        StartCoroutine(LooseMoney());
    }
   
    private IEnumerator LooseMoney()
    {
        while (true)
        {
            GameController.instance.player.resources.ChangeBalance(-lossPerSecondMoney);
            yield return new WaitForSeconds(1);
        }
    }
    private void OnDestroy()
    {
        if(GetComponentInParent<SceneObject>()!=null)
            GetComponentInParent<SceneObject>().StartWorking();
        if(FindObjectsOfType<Fire>().Length ==0 )
        {
            GameController.instance.buttons.ShowAllButtons();
            EventManager.TriggerEvent("OnAlarmDisable");
            GameController.instance.audio.player.StopAmbience();
            GameController.instance.time.UnPause();
        }
    }

}
