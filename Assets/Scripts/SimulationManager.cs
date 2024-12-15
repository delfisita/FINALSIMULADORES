using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public FirebaseManager firebaseManager; // Referencia al FirebaseManager

    public GameObject infectadoPrefab;
    public GameObject desinformadoPrefab;
    public GameObject precavidoPrefab;
    public GameObject sanoPrefab;

    public int populationSize = 100;  // Total de la poblaci�n
    public float initialInfectionRate = 0.05f;  // Tasa inicial de infecci�n
    public float initialMisinformationRate = 0.2f;
    public float defaultInfectionChance = 0.3f;
    public float defaultSpreadDelay = 1f;

    public Text infectadosText;
    public Text velocidadPropagacionText;

    // Variables para estad�sticas
    private int numInfectados = 0;
    private int numSanos = 0;

    private float lastInfectionTime = 0f;

    void Start()
    {
        // Inicializar contadores
        numInfectados = 0;
        numSanos = populationSize;

        // Crear la poblaci�n
        for (int i = 0; i < populationSize; i++)
        {
            GameObject citizenPrefab = sanoPrefab; // Default to "SANO"
            float randomValue = Random.Range(0f, 1f);

            // Determinar el tipo de ciudadano basado en el valor aleatorio
            if (randomValue < initialInfectionRate)
            {
                citizenPrefab = infectadoPrefab;
                numInfectados++;
                numSanos--;
            }

            // Instanciar al ciudadano dentro del �rea de generaci�n
            Instantiate(citizenPrefab, GetRandomPositionWithinBounds(), Quaternion.identity);
        }

        // Enviar estad�sticas iniciales a Firebase
        if (firebaseManager != null)
        {
            firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, 0f, 0);
        }
        else
        {
            Debug.LogWarning("FirebaseManager no est� asignado en SimulationManager.");
        }

        // Actualizar la UI por primera vez
        UpdateUI();
    }

    Vector2 GetRandomPositionWithinBounds()
    {
        // Asegurar que la posici�n est� dentro de los l�mites definidos
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector2(x, y);
    }

    void Update()
    {
        // Simular la propagaci�n de la infecci�n con el tiempo
        if (Time.time - lastInfectionTime >= defaultSpreadDelay)
        {
            // L�gica para propagar la infecci�n entre los ciudadanos
            PropagarInfeccion();

            // Actualizar el tiempo de la �ltima infecci�n
            lastInfectionTime = Time.time;

            // Actualizar la UI con los nuevos valores
            UpdateUI();

            // Actualizar estad�sticas en Firebase cada segundo (aproximadamente)
            if (firebaseManager != null)
            {
                firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, CalculateVelocidadPropagacion(), Time.time);
            }
        }
    }

    void PropagarInfeccion()
    {
        // Propagar la infecci�n de manera gradual
        if (numSanos > 0)
        {
            // L�gica b�sica para aumentar la infecci�n
            numInfectados++;
            numSanos--;

            // Proceso de propagaci�n (esto se puede hacer m�s complejo dependiendo del modelo)
        }
    }

    void UpdateUI()
    {
        // Actualizar el texto de infectados
        infectadosText.text = "Infectados: " + numInfectados;

        // Actualizar el texto de velocidad de propagaci�n
        float velocidadPropagacion = CalculateVelocidadPropagacion();
        velocidadPropagacionText.text = "Velocidad de propagaci�n: " + velocidadPropagacion.ToString("F2");
    }

    float CalculateVelocidadPropagacion()
    {
        // Calcular la velocidad de propagaci�n basada en los infectados y el tiempo
        return numInfectados / Mathf.Max(Time.time, 1f); // Para evitar divisi�n por cero
    }
}
