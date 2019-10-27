using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : Constructible {

    public RoomInfo info;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject upperWall;
    public GameObject bottomWall;

    void Start()
    {
        bool constains = GameController.instance.roomOverseer.rooms.Contains(this);
        if (!constains)
             GameController.instance.roomOverseer.AddRoom(this);
        Transform[] children = GetTopLevelChildren(transform);
        info.cells = GetCells(children);
        DestroyAttachedWalls();
        WorldPos startPos = GetLastCordinates();
        AssignCellsToWorldCoordinates(startPos);
    }
    private void AssignCellsToWorldCoordinates(WorldPos startposition)
    {
        if (this == GameController.instance.roomOverseer.factory) return;
        int xcounter = startposition.x+10; int ycounter = startposition.y+10;
        for (int n=0; n <info.cells.Count; n++)
        {
            if (n%2==0)
            {
                info.cells[n].position.x = xcounter;
                info.cells[n].position.y = ycounter;
            }
            else
            {
                info.cells[n].position.x = xcounter + 1;
                info.cells[n].position.y = ycounter;
                ycounter--;
            }
        }
    }
    private WorldPos GetLastCordinates()
    {
        if (GameController.instance.roomOverseer.rooms.Count <2)
            return new WorldPos { x = 0, y = 0};

        return GameController.instance.roomOverseer.rooms.ElementAt(GameController.instance.roomOverseer.rooms.Count-2).info.cells[0].position;
    }
   private void DestroyAttachedWalls()
    {
        if (adjacent == null|| hasJointedObject || adjacent.list.Count==0)
        {
            
            return;
        }
        hasJointedObject = true;
        foreach(AdjacentObject obj in adjacent.list)
        {
 
            if (obj.leftObj==this.adjacent)
            {
      
                Destroy(rightWall);
                Destroy(obj.GetComponent<Room>().leftWall);
            }
            else if (obj.rightObj == this.adjacent)
            {
        
                Destroy(leftWall);
                Destroy(obj.GetComponent<Room>().rightWall);
            }
            else if (obj.upperObj == this.adjacent)
            {
                Destroy(bottomWall);
                Destroy(obj.GetComponent<Room>().upperWall);
            }
            else if (obj.lowerObj == this.adjacent)
            {
                Destroy(upperWall);
                Destroy(obj.GetComponent<Room>().bottomWall);
            }

        }
    }
    private Transform[] GetTopLevelChildren(Transform Parent)
    {
        Transform[] Children = new Transform[Parent.childCount];
        for (int ID = 0; ID < Parent.childCount; ID++)
        {
            Children[ID] = Parent.GetChild(ID);
        }
        return Children;
    }
    public List<Cell> GetCells(Transform[] objects)
    {
        List<Cell> cellList = new List<Cell>();
        foreach(Transform t in objects)
        {
            if (t.GetComponent<Cell>())
                cellList.Add(t.GetComponent<Cell>()); 
        }
        return cellList;
    }





}
