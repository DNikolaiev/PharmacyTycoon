using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Camera Settings")]
public class CameraSettings : ScriptableObject {

    public int minZoomRange;
    public int maxZoomRange;
    public float zoomSpeed;
    public float dragSpeed;
    public float focusSpeed;
    public int xOffset, yOffset;
}
