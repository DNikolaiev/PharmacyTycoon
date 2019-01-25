using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct WorldPos { public int x; public int y; };
public class Cell : MonoBehaviour {

    public Color defaultColor;
    public Color highlightedColor;
    public Color occupiedColor;
    public bool isOccupied;
    public bool isCellSelected;
    public MeshRenderer rend;
    public WorldPos position;
    public Room room;
    private Constructor constructor;
    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        constructor = GameController.instance.constructor;
        rend.material.color = defaultColor;
        room=transform.GetComponentInParent<Room>();
    }
    
    private void OnMouseDown()
    {
        if (!constructor.isShopEnabled && !constructor.inConfirmation)
        Select();
    }
    private void Select() 
    {
        if (isOccupied)
            return;
       
        foreach (Cell cell in room.info.cells)
        {
            cell.Unselect();
        }
        isCellSelected = true;
        rend.material.color = highlightedColor;
        constructor.selectedCell = this;
        if (this.gameObject.tag == "RoomCell")
        {
            constructor.SelectObject(0);
        }
        else
        {
            constructor.ToggleShop(true);
        }
       

    }
    private void Unselect()
    {
        isCellSelected = false;
        if (!isOccupied)
        rend.material.color = defaultColor; // здесь сбрасывает цвет 
    }
  
    public void UpdateColor()
    {
        rend.material.color = (isOccupied == true) ? occupiedColor : defaultColor;
    }
    private void Update()
    {
        isOccupied = (transform.childCount > 0) ? true : false;
    }
    public void AddObject(Constructible obj)
    {
        room.info.objectsInRoom.Add(obj);
        if (tag == "RoomCell")
        {
            obj.transform.parent = GameController.instance.roomOverseer.factory.transform;
            obj.adjacent.UpdateConnections(this);
            room.info.cells.Remove(this);

            Destroy(gameObject);
        }
        else
        {
            obj.transform.parent = transform;
            isOccupied = true;
            UpdateColor();
            //check if object has adjacent sscript attached
            if (obj.adjacent != null)
            {
                obj.adjacent.UpdateConnections(this);
            }
        }
    }
}
