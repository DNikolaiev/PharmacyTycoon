using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holder : Panel {

    public Image picture;
    public Sprite defaultSprite;
    public GameObject lockedSprite;
    [SerializeField] protected DescriptionPanel descriptionPanel;
    public bool isUnlocked = false;



    public void Unlock(bool state)
    {
        isUnlocked = state;
        lockedSprite.SetActive(!state);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void SetPanel()
    {
        throw new System.NotImplementedException();
    }
}
