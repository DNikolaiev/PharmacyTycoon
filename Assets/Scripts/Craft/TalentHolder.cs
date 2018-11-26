using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public  class TalentHolder : Panel, IPointerDownHandler, IDescription {
    [SerializeField] private Talent talent;
    public Talent Talent { get { return talent; } set { talent = value; UpdateView(); } }
    public Image picture;
    public Sprite defaultSprite;
    [SerializeField] protected DescriptionPanel descriptionPanel;
    

    protected bool ClickedInsideHolder()
    {
                RectTransform rectTransform = GetComponent<RectTransform>();

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
                return false;
            else return true; 
    }
    protected void UpdateView()
    {
        if (talent == null)
            picture.sprite = defaultSprite;
        else
        picture.sprite = talent.description.sprite;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
            SetDescription();
    }
    public virtual void SetDescription() { }

    public override void SetPanel()
    {
        if (Talent != null)
            picture.sprite = Talent.description.sprite;
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }


}
