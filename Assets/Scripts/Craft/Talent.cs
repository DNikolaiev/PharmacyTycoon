using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Talent: MonoBehaviour  {

    public Description description;
    public Sprite lockedSprite;
    public bool isPrimary;
   [HideInInspector] public bool isSelected;
    public string cures;
    public List<string> combinations;
    public int id;
    public float timeToResearch;
    public bool isUnlocked;
    public Characteristics characteristics;
    
    
    
   

}
