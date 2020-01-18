using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private Camera cam;

    private void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Missing component Camera reference on : " + gameObject.name);
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 3f, 20f);
    }
}
