using UnityEngine;
using UnityEngine.UIElements;

public class cameraRotation : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newY = transform.eulerAngles.y + (rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(20.4f, newY, 0);
    }
}
