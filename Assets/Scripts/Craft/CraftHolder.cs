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
    private float clickdelay = 1f;
    private float clickTime;
    private Crafter crafter;
    public override void SetDescription()
    {
        descriptionPanel.SetPanel(Talent,this);
    }
    
    private void Start()
    {
        crafter = GameController.instance.crafter;
        if (isUnlocked)
            lockedSprite.SetActive(false);
        if (glowImg != null)
            glowImg.gameObject.SetActive(false);
        if(dragger!=null)
        InitializeDrag();
        descriptionPanel = GameController.instance.buttons.GetDescriptionPanel(this);
        if (action!=null)
        action.onClick.AddListener(SetDescription);
        if (Talent != null)
            SetPanel();
    }
    private void InitializeDrag()
    {
        dragger.SetCanvas(crafter.view.craftPanel.transform.parent.GetComponent<Canvas>());
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
        if (crafter.view.holderSelected == null && dragger.dragItem!=null)
        {
            dragger.ResetDragItem(); return;
        }
        if ((crafter.view.holderSelected.tag == "PrimaryHolder" && !Talent.isPrimary)
            || (crafter.view.holderSelected.tag == "SecondaryHolder" && Talent.isPrimary) 
            || !crafter.view.holderSelected.isUnlocked) 
        {
            dragger.ResetDragItem();
            return;
        }
     
        
        if (crafter.view.holderSelected !=null) {
            crafter.controller.OnAddTalent(this);
            
                dragger.ResetDragItem();
            }

            else dragger.ResetDragItem();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragger != null )
            return;
        if(Input.GetMouseButton(0))
            crafter.controller.OnSelectHolder(this);
        
    }
    
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        SetDescription();
        crafter.DeleteAllTalents(crafter.view.talentsListView);
        if (tag=="PrimaryHolder")
        {
            crafter.PopulateTalentList(crafter.view.talentsListView, crafter.view.elementInList, true);
            crafter.view.HighlightHolders();
        }
        else if (tag=="SecondaryHolder")
        {
            crafter.PopulateTalentList(crafter.view.talentsListView, crafter.view.elementInList, false);
            crafter.view.HighlightHolders(false);
            
        }
        if (Talent == null) return; // double tap segment. Double tap = reset talent;
        clickCount++;
        if (clickCount == 1) clickTime = Time.time;
        if (clickCount == 2 && Time.time - clickTime <= clickdelay)
        {
            clickTime = 0;
            clickCount = 0;
            picture.sprite = defaultSprite;
            crafter.ResetTalent(Talent);
            Talent = null;
            crafter.isPrescripted = false;
            if (tag == "PrimaryHolder")
                crafter.view.HighlightHolders();
            else if (tag == "SecondaryHolder")
                crafter.view.HighlightHolders(false);
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
