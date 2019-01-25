using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject camera_GameObject;
    public float zoomSpeed;
    public float dragSpeed;
    public bool isActive = true;
    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;

    
    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.isGameSceneEnabled) return;
        if (Input.touchCount == 0 && isZooming)
        {
            isZooming = false;
        }

        if (Input.touchCount == 1 && isActive)
        {
            
            if (!isZooming)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 NewPosition = GetWorldPosition();
                    Vector2 PositionDifference = NewPosition - StartPosition;
                    camera_GameObject.transform.Translate(-PositionDifference*dragSpeed);
                }
                StartPosition = GetWorldPosition();
            }
        }
        else if (Input.touchCount == 2 && isActive)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                isZooming = true;

                DragNewPosition = GetWorldPositionOfFinger(1);
                Vector2 PositionDifference = DragNewPosition - DragStartPosition;

                if (Vector2.Distance(DragNewPosition, Finger0Position) < DistanceBetweenFingers)
                    camera_GameObject.GetComponent<Camera>().orthographicSize += (PositionDifference.magnitude*zoomSpeed);

                if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers)
                    camera_GameObject.GetComponent<Camera>().orthographicSize -= (PositionDifference.magnitude*zoomSpeed);
                camera_GameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(camera_GameObject.GetComponent<Camera>().orthographicSize, 10, 70);
                DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
            }
            DragStartPosition = GetWorldPositionOfFinger(1);
            Finger0Position = GetWorldPositionOfFinger(0);
        }
    }

    Vector2 GetWorldPosition()
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetWorldPositionOfFinger(int FingerIndex)
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(FingerIndex).position);
    }
}
