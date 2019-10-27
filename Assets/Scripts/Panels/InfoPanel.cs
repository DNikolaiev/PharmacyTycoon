using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public class InfoPanel: Panel, IPointerDownHandler {
    
    public Text descriptiontxt;
    public Text pricetxt;
    public Image image;
    public SceneObject objectInfo;
    [SerializeField] GameObject lockedObject;
    [SerializeField] Color lockedColor;
    
    private Vector3 originalScale;
    private RectTransform rect;
    [SerializeField] private Color originalColor;
    [SerializeField] private Animation anim;
    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    private void Start()
    {
       
        rect = GetComponent<RectTransform>();
        SetPanel();
    }
    private void UnlockPanel()
    {
        
        if(objectInfo.requiredLvl > GameController.instance.player.level)
        {
            lockedObject.SetActive(true);
            GetComponent<Image>().color = lockedColor;
            GetComponent<Button>().interactable = false;
            anim.enabled = false;
            pricetxt.text = "Unlock at lvl " + objectInfo.requiredLvl;
            pricetxt.fontSize = 24;
        }
        else
        {
            lockedObject.SetActive(false);
            GetComponent<Image>().color = originalColor;
            GetComponent<Button>().interactable = true;
            anim.enabled = true;
            pricetxt.resizeTextForBestFit = false;
            pricetxt.text = objectInfo.description.buyPrice.ToString() + " $";

        }
    }
    public override void SetPanel()
    {
        Nametxt.text = objectInfo.description.Name;
        descriptiontxt.text = objectInfo.description.description;
        pricetxt.text = objectInfo.description.buyPrice.ToString() + " $";
        image.sprite = objectInfo.description.sprite;
        UnlockPanel();
    }

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!anim.isPlaying)
             anim.Play("ConstructPanel_Click");
    }
    

 
}
