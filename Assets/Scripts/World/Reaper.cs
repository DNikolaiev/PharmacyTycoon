using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper  {

    //isolate this in separate class
    public int GetDeadAliveRate(Recipe recipe, int quantity, out int alive)
    {
        float startRate = 8;
        startRate -= 2 * recipe.PTalents.Count;
        Debug.Log("ST " + startRate);
        Debug.Log("PT " + recipe.PTalents.Count);
        float deathRate = (float)(1f / startRate);
        Debug.Log("DR " + deathRate);
        int toxicated = quantity * recipe.characteristics.toxicity/100;
        Debug.Log("TOX " + toxicated);
        int dead = (int)(toxicated * deathRate);
        alive = quantity - dead;
        return dead;
    }
}
