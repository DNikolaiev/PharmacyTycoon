using UnityEngine;
 using System.Collections;

public class Builder : MonoBehaviour
{

    public Constructible Build(int id, Constructible[] objectsArray, Cell cell)
    {
        Constructible selectedObject = objectsArray[id];
        return Instantiate(selectedObject, new Vector3(cell.transform.position.x,cell.transform.position.y,cell.transform.position.z-0.5f), cell.transform.rotation);
        
    }
}