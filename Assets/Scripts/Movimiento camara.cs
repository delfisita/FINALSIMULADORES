using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Transform> citizens; 
    public float followSpeed = 2f; 
    public float zoomSpeed = 2f;
    public float minZoom = 3f;
    public float maxZoom = 10f; 
    public float zoomPadding = 2f; 

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Obtén la cámara principal
    }

    void LateUpdate()
    {
        if (citizens.Count == 0) return;

        // Calcular el punto medio de todos los ciudadanos
        Vector3 centerPoint = GetCenterPoint();
        centerPoint.z = transform.position.z; // Mantén la posición Z de la cámara

        // Mover la cámara suavemente hacia el centro
        transform.position = Vector3.Lerp(transform.position, centerPoint, followSpeed * Time.deltaTime);

        // Ajustar el zoom según la distancia entre los ciudadanos
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomPadding);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Clamp(newZoom, minZoom, maxZoom), zoomSpeed * Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (citizens.Count == 1)
        {
            return citizens[0].position;
        }

        Bounds bounds = new Bounds(citizens[0].position, Vector3.zero);
        for (int i = 1; i < citizens.Count; i++)
        {
            bounds.Encapsulate(citizens[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(citizens[0].position, Vector3.zero);
        for (int i = 1; i < citizens.Count; i++)
        {
            bounds.Encapsulate(citizens[i].position);
        }

        return bounds.size.x > bounds.size.y ? bounds.size.x : bounds.size.y;
    }
}
