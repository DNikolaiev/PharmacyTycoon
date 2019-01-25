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

}
