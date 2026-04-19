using UnityEngine;
using UnityEngine.UIElements;

public class cameraRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f;

    [Header("Zoom Settings")]
    public Camera cam;
    public float zoomSpeed = 0.01f;
    public float minZoom = 20f;
    public float maxZoom = 100f;

    void Update()
    {
        // --- 1. ROTATION (Single Touch) ---
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;

            // Rotate around World Up to prevent the X/Z drift we discussed
            transform.Rotate(Vector3.up, -touchDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, touchDelta.y * rotationSpeed, Space.Self);
        }

        // --- 2. ZOOM (Pinch/Double Touch) ---
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Calculate magnitude (distance) between touches
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Difference in distance
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Apply Zoom (Field of View for Perspective, OrthographicSize for 2D)
            if (cam.orthographic)
            {
                cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                cam.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
            }
        }
    }
}
