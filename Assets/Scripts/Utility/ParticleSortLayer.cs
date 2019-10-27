using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortLayer : MonoBehaviour {
    public string layerName = "Foreground";
    private ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        //Change Foreground to the layer you want it to display on 
        //You could prob. make a public variable for this
        particleSystem.GetComponent<Renderer>().sortingLayerName = layerName;
    }
}
