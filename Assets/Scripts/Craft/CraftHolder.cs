using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CraftHolder : TalentHolder, IPointerEnterHandler{
    [SerializeField] Button action;
    [SerializeField] DragHandler dragger;
    public override void SetDescription()
    {
        descriptionPanel.SetPanel(Talent,this);
    }
    
    private void Start()
    {
        if(dragger!=null)
        InitializeDrag();
        descriptionPanel = Crafter.instance.craftDescriptionPanel.GetComponent<DescriptionPanel>();
        if (action!=null)
        action.onClick.AddListener(delegate {Crafter.instance.controller.OnAddTalent(this);});
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
       
        //Vector3 beginPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity, action.transform.parent.parent.parent.parent);
        copy.GetComponent<Image>().raycastTarget = false;
        
        dragger.SetDragItem(copy);
    }
    private void OnEndDrag()
    {
        if ((Crafter.instance.holderSelected.tag == "PrimaryHolder" && !Talent.isPrimary)
            || (Crafter.instance.holderSelected.tag == "SecondaryHolder" && Talent.isPrimary))
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
        if (dragger != null)
            return;
        Crafter.instance.controller.OnSelectHolder(this);
        
    }
    
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (dragger != null)
            return;
        Crafter.instance.controller.OnSelectHolder(this);
        if (tag=="PrimaryHolder")
        {
            Crafter.instance.DeleteAllTalents();
            Crafter.instance.PopulateTalentList(true);
            Crafter.instance.DeleteAllTalents();
            Crafter.instance.PopulateTalentList(true);
        }
        else if (tag=="SecondaryHolder")
        {
            
            Crafter.instance.DeleteAllTalents();
            Crafter.instance.PopulateTalentList(false);
            Crafter.instance.DeleteAllTalents();
            Crafter.instance.PopulateTalentList(false);
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
