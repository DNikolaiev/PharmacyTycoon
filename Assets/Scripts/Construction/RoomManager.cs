using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    public Room factory;
    public List<Room> rooms;
    
    
    private void Start()
    {
        rooms = factory.GetComponentsInChildren<Room>().ToList();
        rooms.RemoveAt(0);
    }
    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }
    public List<SceneObject> GetAllSceneObjects()
    {
        List<SceneObject> sceneObjects = new List<SceneObject>();
        foreach(Room room in rooms)
        {
            if (room == factory) continue;
            foreach(SceneObject obj in room.info.objectsInRoom)
            {
                sceneObjects.Add(obj);
            }
        }
        return sceneObjects;
    }
    public bool  SceneHasObjectOfType<T>()
    {
        foreach (SceneObject item in GetAllSceneObjects())
        {
            if(item.GetType() == typeof(T))
            {
                return true;
            }
        }
        return false;
    }
}
