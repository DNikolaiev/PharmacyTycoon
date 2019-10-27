using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public bool floatUp;
    public bool floatDown;
    public float speed = 0.1f;
    private Vector3 tempPos;
    private float tempVal;
    void Start()
    {
        tempVal = GetComponent<RectTransform>().position.y;
    }

    void Update()
    {
        tempPos.y = tempVal + speed * Mathf.Sin(speed * Time.time);
        GetComponent<RectTransform>().position = tempPos;
    }
}
