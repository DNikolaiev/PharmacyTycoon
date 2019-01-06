using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
public class CraftHolder : TalentHolder, IPointerEnterHandler{
    
    [SerializeField] Button action;
    [SerializeField] DragHandler dragger;
    private int clickCount = 0;
    private float clickdelay = 0.5f;
    private float clickTime;
    public override void SetDescription()
    {
        descriptionPanel.SetPanel(Talent,this);
    }
    
    private void Start()
    {
        if (isUnlocked)
            lockedSprite.SetActive(false);
        if (glowImg != null)
            glowImg.gameObject.SetActive(false);
        if(dragger!=null)
        InitializeDrag();
        descriptionPanel = Crafter.instance.craftDescriptionPanel.GetComponent<DescriptionPanel>();
        if (action!=null)
        action.onClick.AddListener(SetDescription);
        if (Talent != null)
            SetPanel();
    }
    private void InitializeDrag()
    {
        dragger.SetCanvas(Crafter.instance.craftPanel.transform.parent.GetComponent<Canvas>());
        dragger.onBeginMethod += OnBeginDrag;
        dragger.onEndMethod += OnEndDrag;
    }
    private void OnBeginDrag()
    {
        if (dragger.dragItem != null) return;
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity, action.transform.parent.parent.parent.parent);
        copy.GetComponent<Image>().raycastTarget = false;
        
        dragger.SetDragItem(copy);
    }
    private void OnEndDrag()
    {
        if (dragger == null) return;
        if (Crafter.instance.holderSelected == null && dragger.dragItem!=null)
        {
            dragger.ResetDragItem(); return;
        }
        if ((Crafter.instance.holderSelected.tag == "PrimaryHolder" && !Talent.isPrimary)
            || (Crafter.instance.holderSelected.tag == "SecondaryHolder" && Talent.isPrimary) 
            || !Crafter.instance.holderSelected.isUnlocked) 
        {
            dragger.ResetDragItem();
            return;
        }
     
        
        if (Crafter.instance.holderSelected!=null) { 
                Crafter.instance.controller.OnAddTalent(this);
                dragger.ResetDragItem();
            }

            else dragger.ResetDragItem();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragger != null )
            return;
        if(Input.GetMouseButton(0))
        Crafter.instance.controller.OnSelectHolder(this);
        
    }
    
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        SetDescription();
        Crafter.instance.DeleteAllTalents();
        if (tag=="PrimaryHolder")
        {
            Crafter.instance.PopulateTalentList(true);
            Crafter.instance.HighlightHolders();
        }
        else if (tag=="SecondaryHolder")
        {
            Crafter.instance.PopulateTalentList(false);
            Crafter.instance.HighlightHolders(false);
        }
        if (Talent == null) return; // double tap segment. Double tap = reset talent;
        clickCount++;
        if (clickCount == 1) clickTime = Time.time;
        if (clickCount == 2 && Time.time - clickTime <= clickdelay)
        {
            clickTime = 0;
            clickCount = 0;
            picture.sprite = defaultSprite;
            Crafter.instance.ResetTalent(Talent);
            Talent = null;
            Crafter.instance.isPrescripted = false;
            if (tag == "PrimaryHolder")
                Crafter.instance.HighlightHolders();
            else if (tag == "SecondaryHolder")
                Crafter.instance.HighlightHolders(false);
        }
        else if (clickCount > 2 || Time.time - clickTime > clickdelay)
        {
            clickCount = 0;
            clickTime = 0;
        }
    }
    private bool IsMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.GetComponent<CraftHolder>()!=null)
        {
            return true;
        }
        return false;
    }
  

}
