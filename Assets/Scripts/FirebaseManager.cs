using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Para manejar la UI

public class FirebaseManager : MonoBehaviour
{
    // URL base de tu Realtime Database
    private string firebaseURL = "https://finalsimuladores-54bba-default-rtdb.firebaseio.com/";

    // Referencias a elementos UI en Unity para mostrar estad�sticas
    public Text infectadosText;
    public Text velocidadPropagacionText;

    void Start()
    {
        // Iniciar la escucha de datos en Firebase
        StartCoroutine(EscucharDatos());
    }

    private IEnumerator EscucharDatos()
    {
        string url = firebaseURL + "Simulador/Historial/Tiempoactual.json";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Datos recibidos: " + json);
            ProcesarDatos(json);
        }
        else
        {
            Debug.LogError("Error al obtener datos: " + request.error);
        }
    }

    private void ProcesarDatos(string json)
    {
        Debug.Log("JSON recibido: " + json);

        if (string.IsNullOrEmpty(json) || json == "null")
        {
            Debug.LogWarning("No hay datos disponibles en Firebase o el nodo est� vac�o.");
            return;
        }

        try
        {
            // Deserializar el JSON manualmente
            var datos = JsonUtility.FromJson<Estadisticas>(json);
            Debug.Log("Datos deserializados correctamente.");

            // Actualizar la UI
            infectadosText.text = "Infectados: " + datos.Infectados;

            // Convertir VelocidadDePropagacion a float si es necesario
            float velocidadPropagacion = 0f;
            if (float.TryParse(datos.VelocidadDePropagacion.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out velocidadPropagacion))
            {
                velocidadPropagacionText.text = "Velocidad de propagaci�n: " + velocidadPropagacion.ToString("F2");
            }
            else
            {
                Debug.LogWarning("No se pudo convertir 'Velocidad de propagacion' a float.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al procesar datos: " + ex.Message);
        }
    }

    // M�todo para actualizar las estad�sticas en Firebase
    public void ActualizarEstadisticas(int infectados, int poblacionTotal, float velocidadPropagacion, float tiempo)
    {
        // Crear un objeto para las estad�sticas
        Estadisticas estadisticas = new Estadisticas
        {
            Infectados = infectados,
            VelocidadDePropagacion = velocidadPropagacion.ToString("F2")
        };

        // Convertir a JSON y subir a Firebase
        string json = JsonUtility.ToJson(estadisticas);
        StartCoroutine(SubirEstadisticasFirebase(json));
    }

    private IEnumerator SubirEstadisticasFirebase(string json)
    {
        string url = firebaseURL + "Simulador/Historial/Tiempoactual.json";
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Estad�sticas actualizadas correctamente.");
        }
        else
        {
            Debug.LogError("Error al actualizar estad�sticas: " + request.error);
        }
    }

    [System.Serializable]
    public class Estadisticas
    {
        public int Infectados;
        public string VelocidadDePropagacion; // Cambiado a string para manejar el formato inicial
    }
}
