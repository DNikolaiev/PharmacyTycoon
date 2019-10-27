using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ProductionUpgrade : MonetizedObject {
    [SerializeField] int productionMultiplier;
    public override void OnSell()
    {
        foreach(Manufactory sObject in GameController.instance.roomOverseer.GetAllSceneObjects().Where(x=>x.GetComponent<Manufactory>()!=null))
        {
            sObject.resourcePerTime *= productionMultiplier;
        }
        PlayGameScript.UnlockAchievement(GPGSIds.achievement_man_of_honor);
    }

   
}
