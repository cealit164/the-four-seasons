using UnityEngine;
using UnityEngine.UI;

public class cameraRotation : MonoBehaviour
{
    [Header("References")]
    public Transform camOffset;
    public Camera mainCam;
    public Toggle autoRotateToggle;

    [Header("Sensitivities")]
    public float mouseRotateSpeed = 5.0f;
    public float touchRotateSpeed = 0.15f;
    public float mouseZoomSpeed = 2.0f;
    public float touchZoomSpeed = 0.05f;

    [Header("Auto Rotation")]
    public bool useAutoRotation = true;
    public float autoRotationSpeed = 15f;
    public float resumeDelay = 2f;

    [Header("Limits")]
    public float minDistance = -2f;
    public float maxDistance = -20f;
    [Range(-90, 90)] public float minXRotation = -80f;
    [Range(-90, 90)] public float maxXRotation = 80f;

    private float _currentX = 0f;
    private float _currentY = 0f;
    private float _lastInputTime;
    private Plane _plane;

    void Awake()
    {
        if (camOffset == null) camOffset = transform;
        if (mainCam == null) mainCam = Camera.main;

        Vector3 angles = camOffset.eulerAngles;
        _currentX = angles.x;
        _currentY = angles.y;

        // Sync Toggle UI with the initial variable state
        if (autoRotateToggle != null)
            autoRotateToggle.isOn = useAutoRotation;
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    // --- UI FUNCTION CALL ---
    // Link this to your Toggle's "On Value Changed" event in the Inspector
    public void SetAutoRotation(bool state)
    {
        useAutoRotation = state;
    }

    private void HandleRotation()
    {
        bool hasInput = false;

        // Detect Mouse Input
        if (Input.GetMouseButton(1) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            _currentY += Input.GetAxis("Mouse X") * mouseRotateSpeed;
            _currentX -= Input.GetAxis("Mouse Y") * mouseRotateSpeed;
            hasInput = true;
        }

        // Detect Touch Input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                _currentY += touch.deltaPosition.x * touchRotateSpeed;
                _currentX -= touch.deltaPosition.y * touchRotateSpeed;
                hasInput = true;
            }
        }

        if (hasInput)
        {
            _lastInputTime = Time.time;

            // --- TURN OFF AUTO ROTATE ON MANUAL MOVE ---
            if (useAutoRotation)
            {
                useAutoRotation = false;
                if (autoRotateToggle != null)
                    autoRotateToggle.isOn = false; // Updates the UI checkbox visually
            }
        }
        else if (useAutoRotation && Time.time > _lastInputTime + resumeDelay)
        {
            _currentY += autoRotationSpeed * Time.deltaTime;
        }

        _currentX = Mathf.Clamp(_currentX, minXRotation, maxXRotation);
        camOffset.rotation = Quaternion.Euler(_currentX, _currentY, 0);
    }

    private void HandleZoom()
    {
        float zoomDelta = 0;

        if (Input.mouseScrollDelta.y != 0)
        {
            zoomDelta = Input.mouseScrollDelta.y * mouseZoomSpeed;
            _lastInputTime = Time.time;
        }

        if (Input.touchCount == 2)
        {
            _lastInputTime = Time.time;
            _plane.SetNormalAndPosition(Vector3.up, camOffset.position);
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector3 pos1 = GetPlanePosition(t0.position);
            Vector3 pos2 = GetPlanePosition(t1.position);
            Vector3 pos1Prev = GetPlanePosition(t0.position - t0.deltaPosition);
            Vector3 pos2Prev = GetPlanePosition(t1.position - t1.deltaPosition);

            float currentDist = Vector3.Distance(pos1, pos2);
            float prevDist = Vector3.Distance(pos1Prev, pos2Prev);

            if (prevDist > 0)
            {
                float zoomFactor = currentDist / prevDist;
                zoomDelta = (zoomFactor - 1.0f) * 10f * touchZoomSpeed;
            }
        }

        if (Mathf.Abs(zoomDelta) > 0.001f)
        {
            Vector3 camPos = mainCam.transform.localPosition;
            camPos.z += zoomDelta;
            camPos.z = Mathf.Clamp(camPos.z, maxDistance, minDistance);
            mainCam.transform.localPosition = camPos;
        }
    }

    private Vector3 GetPlanePosition(Vector2 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if (_plane.Raycast(ray, out float enter)) return ray.GetPoint(enter);
        return Vector3.zero;
    }
}