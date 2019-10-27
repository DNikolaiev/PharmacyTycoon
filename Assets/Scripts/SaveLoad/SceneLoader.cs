using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject room;

    [SerializeField] List<GameObject> objects;

    public void LoadScene(SceneSaver saver)
    {
        LoadRooms(saver.roomSaveData);
        StartCoroutine(LoadObjectsDelay(saver.objectSaveData));

    }
    private void LoadRooms(RoomSaveData saveData)
    {
        RoomManager roomManager = GameController.instance.roomOverseer;
        int numberOfRooms = saveData.positions.Count;
        List<GameObject> cellsObjects = GameObject.FindGameObjectsWithTag("RoomCell").ToList();
        List<Cell> roomCells = new List<Cell>();
        foreach (GameObject cell in cellsObjects) roomCells.Add(cell.GetComponent<Cell>());

        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject instantiated = Instantiate(room, saveData.positions[i], saveData.rotations[i], roomManager.factory.transform);
            instantiated.GetComponent<AdjacentObject>().position = saveData.worldPositions[i];
            foreach (Cell cell in roomCells)
            {
                if (cell.position == saveData.worldPositions[i])
                {

                    cell.AddObject(instantiated.GetComponent<Room>());
                }
            }


        }
    }
    IEnumerator LoadObjectsDelay(ObjectSaveData saveData)
    {
        yield return new WaitForSeconds(.3f);
        LoadObjects(saveData);
    }
    private void LoadObjects(ObjectSaveData saveData)
    {
        RoomManager roomManager = GameController.instance.roomOverseer;
        int numberOfObjects = saveData.positions.Count;
        List<GameObject> cellsObjects = GameObject.FindGameObjectsWithTag("Cell").ToList();
        List<Cell> cells = new List<Cell>();
        Cell selectedCell = null;
        GameObject selectedObject = null;
        foreach (GameObject cell in cellsObjects) cells.Add(cell.GetComponent<Cell>());
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.CompareTag(saveData.tags[i]))
                {
                    selectedObject = obj;
                    break;
                }
            }
            
            selectedCell = cells.Where(x => x.position == saveData.worldPositions[i]).ToList().FirstOrDefault();
            GameObject instantiated = Instantiate(selectedObject, saveData.positions[i], saveData.rotations[i], selectedCell.transform);
            instantiated.transform.localScale = saveData.scales[i];
            SceneObject spawned = instantiated.GetComponent<SceneObject>();
            spawned.adjacent.position = saveData.worldPositions[i];
            spawned.description.buyPrice = saveData.infos[i].buyPrice;
            spawned.isMerged = saveData.infos[i].isMerged;
            spawned.lvl = saveData.infos[i].level;
            spawned.id=saveData.ids[i];
            spawned.isCreated = true;
            selectedCell.AddObject(spawned);

           
            if (spawned.lvl == 2)
            {
                spawned.GetComponent<IUpgradable>().Upgrade(2, false);
            }
           
            

        }

        List<SceneObject> loadedRooms = GameController.instance.roomOverseer.GetAllSceneObjects();

        foreach(SceneObject spawned in loadedRooms)
        {
           
            if (spawned.lvl == 3 && spawned.isMerged)
            {
                foreach (AdjacentObject adj in spawned.adjacent.list)
                {
                    if (!adj.GetComponent<SceneObject>().isMerged && adj.GetComponent<SceneObject>().id!=spawned.id)
                    {
                        
                        adj.DestroyConnection(spawned.GetComponent<AdjacentObject>());

                    }
                    
                    
                }

                spawned.lvl--;
                //spawned.MergeAdjacentRooms(spawned);
                spawned.GetComponent<IUpgradable>().Upgrade(2, false);
            }
           
            GameController.instance.buttons.ShowAllButtons();
        }

        }
}
