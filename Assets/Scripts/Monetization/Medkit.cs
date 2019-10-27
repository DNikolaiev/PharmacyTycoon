using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonetizedObject
{
    public int healingAmount;
    public override void OnSell()
    {
        List<Talent> empty = new List<Talent>();
        Characteristics characteristics = new Characteristics(0, 0, 0, 0, healingAmount);
        Recipe medkit = new Recipe("Medkit", empty, characteristics, true, Resources.Load<Sprite>("Icons/Recipe/medkit"));
        GameController.instance.player.inventory.Set(medkit, 1,true);
        PlayGameScript.UnlockAchievement(GPGSIds.achievement_man_of_honor);
    }
}
