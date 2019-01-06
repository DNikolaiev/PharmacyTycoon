using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Characteristics
{
    public int healingPlantsNeeded;
    public int chemistryNeeded;
    public int plasticNeeded;
    public int toxicity;
    public int healingRate;

    public Characteristics(int herbs, int chems, int plastic, int toxicity, int healing)
    {
        healingPlantsNeeded = herbs;
        chemistryNeeded = chems;
        this.toxicity = toxicity;
        healingRate = healing;
        plasticNeeded = plastic;
    }
    public void Reset()
    {
        healingPlantsNeeded = 0;
        chemistryNeeded = 0;
        plasticNeeded = 0;
        toxicity = 0;
        healingRate = 0;
    }
    public static Characteristics operator *(Characteristics ch, int amount)
    {
        ch.plasticNeeded *= amount;
        ch.chemistryNeeded *= amount;
        ch.healingPlantsNeeded *= amount;
        return ch;
    }
    public static Characteristics operator /(Characteristics ch, int amount)
    {
        ch.plasticNeeded /= amount;
        ch.chemistryNeeded /= amount;
        ch.healingPlantsNeeded /= amount;
        return ch;
    }
}
