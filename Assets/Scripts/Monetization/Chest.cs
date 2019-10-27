using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonetizedObject
{
    public Reward reward;
    public override void OnSell()
    {
        reward.RewardPlayer();
        PlayGameScript.UnlockAchievement(GPGSIds.achievement_man_of_honor);
    }
}
