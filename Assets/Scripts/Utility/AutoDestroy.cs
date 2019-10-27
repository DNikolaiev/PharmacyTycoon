using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {
    public float waitTime;
    private void Start()
    {

        Invoke("DestroyObject", waitTime);
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
