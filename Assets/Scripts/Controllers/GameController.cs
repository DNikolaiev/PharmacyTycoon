using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public bool isGameSceneEnabled = true;

    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance);
    }
    public void _StartCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
