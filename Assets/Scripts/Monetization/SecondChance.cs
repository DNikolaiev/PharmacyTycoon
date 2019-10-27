using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChance : MonetizedObject
{
    
    public override void OnSell()
    {
        GameController.instance.player.resources.AddSecondChance(1);
        PlayGameScript.UnlockAchievement(GPGSIds.achievement_man_of_honor);
    }
}
