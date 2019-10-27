using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public Transform spawnPlace;
    void Start()
    {
        transform.position = spawnPlace.position;
        transform.localPosition = spawnPlace.localPosition;
        transform.rotation = spawnPlace.rotation;
    }

    
}
