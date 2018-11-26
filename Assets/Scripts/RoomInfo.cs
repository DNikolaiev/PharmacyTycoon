using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class RoomInfo: IEnumerable {

    
    public List<Constructible> objectsInRoom;
    public List<Cell> cells;

    public RoomInfo(List<Constructible> objects)
    {
    
        objectsInRoom = objects;
 
    }

    public IEnumerator GetEnumerator()
    {
        foreach (SceneObject obj in objectsInRoom)
        {
            yield return obj;
        }
    }

}
