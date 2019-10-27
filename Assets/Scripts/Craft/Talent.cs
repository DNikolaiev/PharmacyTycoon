using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName ="NewTalent")]
public class Talent: ScriptableObject  {

    public Description description;
   [FullSerializer.fsIgnore] public Sprite lockedSprite;
    public bool isPrimary;
   [FullSerializer.fsIgnore] public bool isSelected;
    public string cures;
    public List<string> combinations;
    public List<string> diseases;
    public int id;
    public float timeToResearch;
    public bool isUnlocked;
    public bool canBeUnlocked;
    public Characteristics characteristics;
    
    
    
   

}
