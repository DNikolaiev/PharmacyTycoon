using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SceneSaver 
{
   
    public RoomSaveData roomSaveData;
    public ObjectSaveData objectSaveData;
    public void SaveScene()
    {
        SaveRooms();
        SaveObjects();
    }
    private void SaveRooms()
    {
        RoomManager roomManager = GameController.instance.roomOverseer;
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        List<WorldPos> worldPositions = new List<WorldPos>();

        foreach (Room room in roomManager.rooms)
        {
            if (roomManager.rooms.IndexOf(room) == 0) continue; // skip first one, it's build by default
            positions.Add(room.transform.position);
            rotations.Add(room.transform.rotation);
            worldPositions.Add(room.adjacent.position);
        }
        roomSaveData = new RoomSaveData(positions, rotations, worldPositions);

    }
    private void SaveObjects()
    {
        RoomManager roomManager = GameController.instance.roomOverseer;
        List<SceneObject> allObjects = roomManager.GetAllSceneObjects();
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> scales = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        List<WorldPos> worldPositions = new List<WorldPos>();
        List<string> tags = new List<string>();
        List<ObjectInfo> infos = new List<ObjectInfo>();
        List<int> ids = new List<int>();
        int resourcePerTime = 0;
        int currentStorage = 0;
        float productionTime = 0;

        foreach (SceneObject obj in allObjects)
        {
            positions.Add(obj.transform.position);
            rotations.Add(obj.transform.rotation);
            worldPositions.Add(obj.adjacent.position);
            scales.Add(obj.transform.localScale);
            tags.Add(obj.tag);
            ids.Add(obj.id);
            if (obj.GetComponent<Manufactory>() != null)
            {
                 resourcePerTime = obj.GetComponent<Manufactory>().resourcePerTime;
                 currentStorage = obj.GetComponent<Manufactory>().GetCurrentStorage();
                 productionTime = obj.GetComponent<Manufactory>().productionTime;
            }
            ObjectInfo info = new ObjectInfo(obj.description.buyPrice, obj.lvl, resourcePerTime, currentStorage, productionTime,obj.isMerged);
            infos.Add(info);
        }
        objectSaveData = new ObjectSaveData(positions, rotations, worldPositions, tags, infos, scales, ids);
    }
    
}
[System.Serializable]
public class RoomSaveData
{
    public List<Vector3> positions;
    public List<Quaternion> rotations;
    public List<WorldPos> worldPositions;

    public RoomSaveData(List<Vector3> positions,
    List<Quaternion> rotations,
    List<WorldPos> worldPositions)
    {
        this.positions = positions;
        this.rotations = rotations;
        this.worldPositions = worldPositions;
    }
}
[System.Serializable]

public class ObjectSaveData
{
    public List<Vector3> positions;
    public List<Quaternion> rotations;
    public List<WorldPos> worldPositions;
    public List<Vector3> scales;
    public List<string> tags;
    public List<int> ids;
    public List<ObjectInfo> infos;
    public ObjectSaveData(List<Vector3> positions,
    List<Quaternion> rotations,
    List<WorldPos> worldPositions, List<string> tags, List<ObjectInfo> infos, List<Vector3> scales, List<int> ids)
    {
        this.positions = positions;
        this.rotations = rotations;
        this.worldPositions = worldPositions;
        this.tags = tags;
        this.infos=infos;
        this.scales = scales;
        this.ids = ids;
    }
}
[System.Serializable]
public struct ObjectInfo
{
    public int buyPrice;
    public int level;
    public int resourcePerTime;
    public int currentStorage;
    public float productionTime;
    public bool isMerged;
    public ObjectInfo(int buyPrice, int level, int resourcePerTime=0, int currentStorage=0, float productionTime=0, bool isMerged=false)
    {
        this.buyPrice = buyPrice;
        this.level = level;
        this.resourcePerTime = resourcePerTime;
        this.currentStorage = currentStorage;
        this.productionTime = productionTime;
        this.isMerged = isMerged;
    }
}


