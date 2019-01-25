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
    private Vector3 originalScale;
    private RectTransform rect;
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
    public override void SetPanel()
    {
        Nametxt.text = objectInfo.description.Name;
        descriptiontxt.text = objectInfo.description.description;
        pricetxt.text = objectInfo.description.buyPrice.ToString() + " $";
        image.sprite = objectInfo.description.sprite;
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
