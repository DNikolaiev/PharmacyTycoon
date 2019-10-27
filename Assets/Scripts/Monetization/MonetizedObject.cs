using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonetizedObject : MonoBehaviour {
    public Description description;
    public string afterPurchaseText;
    public float priceInDollars;
    public int priceInCoins;
    public bool canBeWatched;
    public int coolDownInDays;
  
    public string id;
    public abstract void OnSell();
    public void ReduceCooldown()
    {
        coolDownInDays--;
    }
   

}
