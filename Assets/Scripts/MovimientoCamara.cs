using UnityEngine;

public class Movimientocamara : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la c�mara
    public float zoomSpeed = 5f; // Velocidad de zoom de la c�mara
    public float minZoom = 5f; // Zoom m�nimo (acercamiento)
    public float maxZoom = 10f; // Zoom m�ximo (no permitir alejar m�s all� de este valor)

    private Camera cam;
    private Vector3 defaultPosition; // Posici�n por defecto de la c�mara

    void Start()
    {
        cam = Camera.main; // Obtiene la c�mara principal
        defaultPosition = transform.position; // Guarda la posici�n inicial
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    // Mover la c�mara libremente con las teclas de direcci�n o el mouse
    void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(horizontal, vertical, 0)); // Mueve la c�mara en el plano X-Y
    }

    // Hacer zoom solo acercando la c�mara (no permitir alejar m�s all� de lo normal)
    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = cam.orthographicSize - scroll;

        // Limitar el zoom solo para acercarse, no alejarse
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    // Restablecer la c�mara a la posici�n por defecto
    public void ResetCamera()
    {
        transform.position = defaultPosition;
        cam.orthographicSize = maxZoom; // Restablecer el zoom a la vista predeterminada
    }
}
