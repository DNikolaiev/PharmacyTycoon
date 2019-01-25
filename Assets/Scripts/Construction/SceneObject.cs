using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneObject : Constructible, ITouchable{

    
    public Room room;
    public int lvl = 1;
    

    protected HelpPanel panel;
    protected UpgradePanel uPanel;
    protected ConfirmationWindow confirm;
    protected AdjacentObject objectToMerge;
    public bool isBusy = false;

    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        description.sprite = GetComponent<SpriteRenderer>().sprite;
    }
    protected virtual void Start()
    {
        
        room = transform.parent.parent.GetComponent<Room>();
        SetSellPrice(2);
      
      
    }
    public virtual void TouchObject(HelpPanel panel)
    {
        this.panel = panel;
        ResetPanel();
        panel.Nametxt.text = description.Name;
    }
    public virtual void ConfirmUpgrade(UpgradePanel uPanel)
    {
        Debug.Log("kk");
        this.uPanel = uPanel;
        uPanel.Nametxt.text = description.Name;
        MergeAdjacentRooms(this);
        
    }
    
    protected virtual void MergeAdjacentRooms(SceneObject type) //split in 2 methods. Try merge and merge, add resourcepertime from adjacent object;
    {
        List<AdjacentObject> adjacents = new List<AdjacentObject>();
        if (adjacent.leftObj != null) adjacents.Add(adjacent.leftObj);
        if (adjacent.rightObj != null) adjacents.Add(adjacent.rightObj);
        if (adjacent.upperObj != null) adjacents.Add(adjacent.upperObj);
        if (adjacent.lowerObj != null) adjacents.Add(adjacent.lowerObj);

        foreach (AdjacentObject nearObj in adjacents)
        {
            if ( !nearObj.GetComponent<SceneObject>().hasJointedObject && objectToMerge==null
                && type.GetType()==nearObj.GetComponent<SceneObject>().GetType()
                && nearObj.GetComponent<SceneObject>().lvl==lvl && lvl==2)
            {
                if(nearObj==adjacent.leftObj)
                {
                    direction = new Vector2(-1, 0);
                }
                   if (nearObj==adjacent.rightObj)
                    {
                        direction = new Vector2(1, 0);
                    }
                    if (nearObj==adjacent.upperObj)
                    {
                        direction = new Vector2(0, 1);
                    }
                     if (nearObj==adjacent.lowerObj)
                    {
                        direction = new Vector2(0, -1);
                    }
                hasJointedObject = true;
                nearObj.GetComponent<SceneObject>().hasJointedObject = true;
                objectToMerge = nearObj;
                
                
                ResizeObject(1.042f);
                TurnObject();
                RecenterObject(0.5f);
                return;
                
            }
           
        }
    }
    public void TurnObject()
    {
        if (direction.y != 0)
        {
            transform.Rotate(Vector3.forward, -90, Space.Self);
            RecenterObject(0.0125f);

        }
        else
        {
            RecenterObject(0.02f);
            return;
        }
    }
    private void RecenterObject(float modifier)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + (direction.x * modifier), transform.localPosition.y + (direction.y * modifier), transform.localPosition.z);
    }
    private void ResizeObject(float modifier)
    {
        transform.localScale = new Vector3(transform.localScale.x * modifier, transform.localScale.y * modifier);
        objectToMerge.GetComponent<BoxCollider>().enabled = false;
        var boxcol = GetComponent<BoxCollider>();     
            boxcol.size = new Vector3(boxcol.size.x*2, boxcol.size.y, boxcol.size.z);
   
    }
    private void ResetPanel()
    {
        panel.timetxt.gameObject.SetActive(false);
        panel.resourceImage.gameObject.SetActive(false);
        panel.resourcetxt.gameObject.SetActive(false);
    }
    protected void ChangeView()
    {
        objectToMerge.GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<SpriteRenderer>().sprite = description.mergedSprite;
    }
    public void ConfirmSale()
    {
        confirm = GameController.instance.buttons.confirm;
        confirm.SetPanel("Sell " + description.Name + " for " + description.sellPrice + "$ ?");
        confirm.Activate(true);
        confirm.ok.onClick.AddListener(SellObject);
        confirm.cancel.onClick.AddListener(Abort);
    }
    protected virtual void SellObject()
    {
        panel.Hide();
        room.info.objectsInRoom.Remove(GetComponent<Constructible>());
        if(objectToMerge!=null && objectToMerge.GetComponent<SceneObject>())
        {
            room.info.objectsInRoom.Remove(objectToMerge.GetComponent<Constructible>());
            Destroy(objectToMerge.gameObject);
        }
        GameController.instance.player.resources.ChangeBalance(description.sellPrice);
        
        Destroy(gameObject);
        
        Abort();

    }
    private void Abort()
    {
        confirm.Hide();
        
    }
    public void SetSellPrice(float priceReducer)
    {
        if(description.sellPrice==0)
       description.sellPrice = int.Parse((description.buyPrice / priceReducer).ToString());
        else
        {
            description.sellPrice += int.Parse(((description.buyPrice / priceReducer)/2).ToString());
        }
    }
}
