using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClicker : MonetizedObject
{
    public override void OnSell()
    {
        GameController.instance.player.hasAutoClicker = true;
        PlayGameScript.UnlockAchievement(GPGSIds.achievement_man_of_honor);
    }
}
