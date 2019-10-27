using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CameraController : MonoBehaviour {

    public GameObject camera_GameObject;
    public bool isActive = true;
    [SerializeField] Camera[] cameras;
    [SerializeField] Canvas secondaryCanvas;
    [SerializeField] List<Canvas> canvases;
    [SerializeField] List<CameraSettings> settings;
    [SerializeField] List<Boundary> boundaries;
    private float focusSpeed;
    private int minZoomRange;
    private int maxZoomRange;
    private float zoomSpeed;
    private float dragSpeed;
    private int xOffset, yOffset;
    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;
    float timeToFocus = 1.2f;

  [SerializeField]  private Boundary activeBoundary;
    private Vector3 firstTouchPrev;
    private Vector3 secondTouchPrev;
    private float prevDifference;
    private float currentDifference;
    private Vector3 start;
    private void Start()
    {
        ToggleCameraWithDelay(0);
        ToggleCameraWithDelay(0);
        ToggleCameraWithDelay(0);
        camera_GameObject.GetComponent<Camera>().orthographicSize = 70;
        start = transform.position;

    }
    public void ResetSize()
    {
        StartCoroutine(ResetCamera());
    }
    public IEnumerator ResetCamera()
    {
        while (camera_GameObject.GetComponent<Camera>().orthographicSize < maxZoomRange-2)
        {
            camera_GameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(camera_GameObject.GetComponent<Camera>().orthographicSize, maxZoomRange, Time.deltaTime * focusSpeed);
            yield return null;
        }
    }
    public void ToggleCameraNoDelay(int n)
    {
        ApplySettings(settings[n]);
        SwitchCameras(n);
    }
    private void SwitchCameras(int n)
    {
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
            cam.gameObject.tag = "Untagged";
        }

        canvases.ForEach(x => x.gameObject.SetActive(false));
        canvases[n].gameObject.SetActive(true);
        cameras[n].enabled = true;
        camera_GameObject = cameras[n].gameObject;
        camera_GameObject.tag = "MainCamera";
        camera_GameObject.SetActive(true);

        secondaryCanvas.worldCamera = cameras[n];
        activeBoundary = boundaries[n];
    }
    private IEnumerator ToggleCamera(int n, bool resetSize = true)
    {
        if(resetSize)
            ResetSize();
        yield return new WaitForSeconds(0.3f);
        SwitchCameras(n);
        yield return null;
    }
    public void ToggleCameraWithDelay(int n)
    {
        ApplySettings(settings[n]);
        StartCoroutine(ToggleCamera(n));

    }
    public void ToggleCameraWithDelay(int n, bool returnSize)
    {
        ApplySettings(settings[n]);
        StartCoroutine(ToggleCamera(n,returnSize));

    }

    private IEnumerator CountDownFocusTime(float n)
    {
        yield return new WaitForSecondsRealtime(n);
        timeToFocus = 0;
        yield break;
    }
    public IEnumerator FocusCamera(Vector2 pos)
    {
        StartCoroutine(CountDownFocusTime(timeToFocus));
        GameController.instance.IsGameSceneEnabled = false;
        while (timeToFocus!=0)
        {
            camera_GameObject.transform.position = Vector3.Lerp(camera_GameObject.transform.position, pos, Time.deltaTime*focusSpeed);
            camera_GameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(camera_GameObject.GetComponent<Camera>().orthographicSize, minZoomRange, Time.deltaTime*focusSpeed);
            yield return null;
        }
        Debug.Log("finished");
        timeToFocus = 1.2f;
        GameController.instance.IsGameSceneEnabled = true;
        yield break;
    }
    private void ApplySettings(CameraSettings settings)
    {
        minZoomRange = settings.minZoomRange;
        maxZoomRange = settings.maxZoomRange;
        zoomSpeed = settings.zoomSpeed;
        dragSpeed = settings.dragSpeed;
        xOffset = settings.xOffset;
        yOffset = settings.yOffset;
        focusSpeed = settings.focusSpeed;
    }
    //TODO Add steering force if player off boundaries
    private void CheckForBoundaries()
    {
        float clampedX = Mathf.Clamp(camera_GameObject.transform.position.x, activeBoundary.leftWall.transform.position.x, activeBoundary.rightWall.transform.position.x);
        float clampedY = Mathf.Clamp(camera_GameObject.transform.position.y, activeBoundary.lowerWall.transform.position.y, activeBoundary.upperWall.transform.position.y);
        camera_GameObject.transform.position = new Vector2(clampedX, clampedY);
    }
    // Update is called once per frame
    void Update()
    {
       
        if (!GameController.instance.IsGameSceneEnabled) return;

        if(Time.time>0.5f && Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.WindowsPlayer || Application.platform==RuntimePlatform.WebGLPlayer)
        {
            PcControle();
        }

        if(Application.isMobilePlatform && Time.time>0.5f)
        {
            Debug.Log("mobile");
            MobileControle();
        }
        
    }
    private void PcControle()
    {
        float zoom;
        zoom = camera_GameObject.GetComponent<Camera>().orthographicSize;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            zoom -= zoomSpeed * Time.deltaTime*1000;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) 
        {
            zoom += zoomSpeed * Time.deltaTime*1000;
        }
       zoom= Mathf.Clamp(zoom, minZoomRange, maxZoomRange);
        camera_GameObject.GetComponent<Camera>().orthographicSize = zoom;
        
        if (Input.GetMouseButton(0))
        {
            Vector2 NewPosition = GetWorldPosition();
            Vector2 PositionDifference = NewPosition - StartPosition;
            camera_GameObject.transform.Translate(-PositionDifference * dragSpeed);
            CheckForBoundaries();
        }
        StartPosition = GetWorldPosition();
    }
    private void MobileControle()
    {
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
                    camera_GameObject.transform.Translate(-PositionDifference * dragSpeed);
                    CheckForBoundaries();
                }
                StartPosition = GetWorldPosition();
            }
        }
        else if (Input.touchCount == 2 && isActive && Input.GetTouch(1).phase==TouchPhase.Moved)
        {
            isZooming = true;
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);
            firstTouchPrev = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrev = secondTouch.position - secondTouch.deltaPosition;

            prevDifference = (firstTouchPrev - secondTouchPrev).magnitude;
            currentDifference = (firstTouch.position - secondTouch.position).magnitude;
            float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;
            if (prevDifference > currentDifference)
            {
                camera_GameObject.GetComponent<Camera>().orthographicSize += zoomModifier;
            }
            if (prevDifference <= currentDifference)
            {
                camera_GameObject.GetComponent<Camera>().orthographicSize -= zoomModifier;
            }
            camera_GameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(camera_GameObject.GetComponent<Camera>().orthographicSize, minZoomRange, maxZoomRange);
            
           
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
