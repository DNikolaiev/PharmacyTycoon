using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentObject : MonoBehaviour {

  
    public AdjacentObject leftObj;
    public AdjacentObject rightObj;
    public AdjacentObject upperObj;
    public AdjacentObject lowerObj;
    public List<AdjacentObject> list;
    
    public WorldPos position;
    private void Start()
    {
        
    }
    private void AssignPosition(Cell cell)
    {
        position.x = cell.position.x;
        position.y = cell.position.y;
    }
    public void DestroyConnection(AdjacentObject adj)
    {
        if (list.Contains(adj))
            list.Remove(adj);
        if (rightObj == adj)
            rightObj = null;
        if (leftObj == adj)
            leftObj = null;
        if (upperObj == adj)
            upperObj = null;
        if (lowerObj == adj)
            lowerObj = null;
        
    }
    public void UpdateConnections(Cell cell)
    {
        AssignPosition(cell);
        AdjacentObject[] adjObjects = FindObjectsOfType<AdjacentObject>();
        foreach (AdjacentObject obj in adjObjects)
        {
            
            if (obj.position.x + 1 == this.position.x && obj.position.y == this.position.y)
            {
                leftObj = obj;
                leftObj.rightObj = this;
                list.Add(leftObj);
            }
            else if (obj.position.x - 1 == this.position.x && obj.position.y == this.position.y)
            {
                rightObj = obj;
                rightObj.leftObj = this;
                list.Add(rightObj);
            }
            else if (obj.position.y + 1 == this.position.y && obj.position.x == this.position.x)
            {
                lowerObj = obj;
                lowerObj.upperObj = this;
                list.Add(lowerObj);
            }
            else if (obj.position.y - 1 == this.position.y && obj.position.x == this.position.x)
            {
                upperObj = obj;
                upperObj.lowerObj = this;
                list.Add(upperObj);
            }
            
        }
    }
}
