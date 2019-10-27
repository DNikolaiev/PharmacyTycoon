using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SceneObject : Constructible, ITouchable
{
    
    protected HelpPanel panel;
    protected UpgradePanel uPanel;
    protected ConfirmationWindow confirm;
    protected AdjacentObject objectToMerge;
    [SerializeField] protected List<AudioClip> onUpgradeSound;
    public bool isCreated = false;
    public bool isMerged = false;
    public int id;
    public Room room;
    public Sprite resource;
    public int lvl = 1;
    public int requiredLvl = 1;
    public bool isBusy = false;
    public bool canWork = true;
    public ParticleSystem onUpgrade;
    
    public Vector2 direction = Vector2.zero;

    private void Awake()
    {
        description.sprite = GetComponent<SpriteRenderer>().sprite;
    }
    protected virtual void Start()
    {

        room = transform.parent.parent.GetComponent<Room>();
        SetSellPrice(2);


    }
    public virtual void StartWorking()
    {
        canWork = true;
    }
    public virtual void TouchObject(HelpPanel panel)
    {
        if (!GameController.instance.IsGameSceneEnabled) return;
        this.panel = panel;
        ResetPanel();
        panel.Nametxt.text = description.Name;
        panel.resourceImage.sprite = resource;
    }
    public virtual void ConfirmUpgrade(UpgradePanel uPanel)
    {

        this.uPanel = uPanel;
        uPanel.Nametxt.text = description.Name;
        uPanel.resourceIcon.sprite = resource;
        MergeAdjacentRooms(this);
    }
    
    public virtual void MergeAdjacentRooms(SceneObject type) //split in 2 methods. Try merge and merge, add resourcepertime from adjacent object;
    {
        Debug.Log("Merging");
        List<AdjacentObject> adjacents = new List<AdjacentObject>();
        if (adjacent.leftObj != null) adjacents.Add(adjacent.leftObj);
        if (adjacent.rightObj != null) adjacents.Add(adjacent.rightObj);
        if (adjacent.upperObj != null) adjacents.Add(adjacent.upperObj);
        if (adjacent.lowerObj != null) adjacents.Add(adjacent.lowerObj);

        foreach (AdjacentObject nearObj in adjacents)
        {
            if (!nearObj.GetComponent<SceneObject>().hasJointedObject && objectToMerge == null
                && type.GetType() == nearObj.GetComponent<SceneObject>().GetType()
                && nearObj.GetComponent<SceneObject>().lvl == lvl && lvl == 2)
            {
                if (nearObj.GetComponent<SceneObject>().lvl > 2 || nearObj.GetComponent<SceneObject>().id!=id) continue;
                if (nearObj == adjacent.leftObj)
                {
                    direction = new Vector2(-1, 0);
                }
                if (nearObj == adjacent.rightObj)
                {
                    direction = new Vector2(1, 0);
                }
                if (nearObj == adjacent.upperObj)
                {
                    direction = new Vector2(0, 1);
                }
                if (nearObj == adjacent.lowerObj)
                {
                    direction = new Vector2(0, -1);
                }
                hasJointedObject = true;
                nearObj.GetComponent<SceneObject>().hasJointedObject = true;
                objectToMerge = nearObj;
                Debug.Log("ID: "+id+" "+ "MERGED ID: "+objectToMerge.GetComponent<SceneObject>().id);
                if (!isMerged)
                {
                    Debug.Log("why here?");
                    id = GetUniqueID();
                    objectToMerge.GetComponent<SceneObject>().id=id;
                    ResizeObject(1.042f);
                    TurnObject(); // dont do it after loading. Dont forget to turn finished production image
                    RecenterObject(0.5f);
                    isMerged = true;
                    objectToMerge.GetComponent<SceneObject>().isMerged = true;
                }
                else
                {
                    Debug.Log("right here");
                    if (direction.y != 0)
                    {
                        RotateViewObjects();

                    }
                    ResizeCollider();
                }
                return;

            }

        }
    }
    private int GetUniqueID()
    {
        int id = 0;
        id = Random.Range(0, 100000);
        if( GameController.instance.roomOverseer.GetAllSceneObjects().Where(x=>x.id==id).Count() >0)
        {
            GetUniqueID();
        }
        return id;
       
    }
    private void RotateViewObjects()
    {
        if (GetComponent<Manufactory>() == null) return;
        GameObject finishedProductionImage = GetComponent<Manufactory>().finishedProductionImage;
        if (finishedProductionImage != null)
            finishedProductionImage.transform.Rotate(Vector3.forward, 90, Space.Self);
    }
    public void TurnObject()
    {
        if (direction.y != 0)
        {
            transform.Rotate(Vector3.forward, -90, Space.Self);
            RecenterObject(0.0125f);
            RotateViewObjects();

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
    private void ResizeCollider()
    {
        objectToMerge.GetComponent<BoxCollider>().enabled = false;

        var boxcol = GetComponent<BoxCollider>();
        boxcol.size = new Vector3(boxcol.size.x * 2, boxcol.size.y, boxcol.size.z);
    }
    private void ResizeObject(float modifier)
    {
        transform.localScale = new Vector3(transform.localScale.x * modifier, transform.localScale.y * modifier);
        ResizeCollider();
    }
    protected void ResetPanel()
    {
        panel.timetxt.gameObject.SetActive(false);
        panel.resourceImage.gameObject.SetActive(false);
        panel.resourcetxt.gameObject.SetActive(false);
    }
    protected void ChangeViewToDouble()
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
        if (objectToMerge != null && objectToMerge.GetComponent<SceneObject>())
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
        if (description.sellPrice == 0)
            description.sellPrice = (int)(description.buyPrice / priceReducer);
        else
        {
            description.sellPrice += (int)((description.buyPrice / priceReducer) / 2);
        }
    }
}

