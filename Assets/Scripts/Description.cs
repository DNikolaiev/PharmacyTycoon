using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Description {

    public string Name;
    public string description;
    public int buyPrice;
    public int sellPrice;
   [FullSerializer.fsIgnore] public Sprite sprite;
   [FullSerializer.fsIgnore] public Sprite upgradedSprite;
   [FullSerializer.fsIgnore] public Sprite mergedSprite;
}
