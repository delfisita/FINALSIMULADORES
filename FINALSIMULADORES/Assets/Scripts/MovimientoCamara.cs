using UnityEngine;

public class Movimientocamara : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la cámara
    public float zoomSpeed = 5f; // Velocidad de zoom de la cámara
    public float minZoom = 5f; // Zoom mínimo (acercamiento)
    public float maxZoom = 10f; // Zoom máximo (no permitir alejar más allá de este valor)

    private Camera cam;
    private Vector3 defaultPosition; // Posición por defecto de la cámara

    void Start()
    {
        cam = Camera.main; // Obtiene la cámara principal
        defaultPosition = transform.position; // Guarda la posición inicial
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    // Mover la cámara libremente con las teclas de dirección o el mouse
    void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(horizontal, vertical, 0)); // Mueve la cámara en el plano X-Y
    }

    // Hacer zoom solo acercando la cámara (no permitir alejar más allá de lo normal)
    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = cam.orthographicSize - scroll;

        // Limitar el zoom solo para acercarse, no alejarse
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    // Restablecer la cámara a la posición por defecto
    public void ResetCamera()
    {
        transform.position = defaultPosition;
        cam.orthographicSize = maxZoom; // Restablecer el zoom a la vista predeterminada
    }
}
