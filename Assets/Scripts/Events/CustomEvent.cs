using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomEvent : MonoBehaviour {

    public string Name;
    public string description;
    public abstract void FireEvent();
    

}
