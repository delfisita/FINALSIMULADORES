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

    public int populationSize = 100;  // Total de la población
    public float initialInfectionRate = 0.05f;  // Tasa inicial de infección
    public float initialMisinformationRate = 0.2f;
    public float defaultInfectionChance = 0.3f;
    public float defaultSpreadDelay = 1f;

    public Text infectadosText;
    public Text velocidadPropagacionText;

    // Variables para estadísticas
    private int numInfectados = 0;
    private int numSanos = 0;

    private float lastInfectionTime = 0f;

    void Start()
    {
        // Inicializar contadores
        numInfectados = 0;
        numSanos = populationSize;

        // Crear la población
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

            // Instanciar al ciudadano dentro del área de generación
            Instantiate(citizenPrefab, GetRandomPositionWithinBounds(), Quaternion.identity);
        }

        // Enviar estadísticas iniciales a Firebase
        if (firebaseManager != null)
        {
            firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, 0f, 0);
        }
        else
        {
            Debug.LogWarning("FirebaseManager no está asignado en SimulationManager.");
        }

        // Actualizar la UI por primera vez
        UpdateUI();
    }

    Vector2 GetRandomPositionWithinBounds()
    {
        // Asegurar que la posición esté dentro de los límites definidos
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector2(x, y);
    }

    void Update()
    {
        // Simular la propagación de la infección con el tiempo
        if (Time.time - lastInfectionTime >= defaultSpreadDelay)
        {
            // Lógica para propagar la infección entre los ciudadanos
            PropagarInfeccion();

            // Actualizar el tiempo de la última infección
            lastInfectionTime = Time.time;

            // Actualizar la UI con los nuevos valores
            UpdateUI();

            // Actualizar estadísticas en Firebase cada segundo (aproximadamente)
            if (firebaseManager != null)
            {
                firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, CalculateVelocidadPropagacion(), Time.time);
            }
        }
    }

    void PropagarInfeccion()
    {
        // Propagar la infección de manera gradual
        if (numSanos > 0)
        {
            // Lógica básica para aumentar la infección
            numInfectados++;
            numSanos--;

            // Proceso de propagación (esto se puede hacer más complejo dependiendo del modelo)
        }
    }

    void UpdateUI()
    {
        // Actualizar el texto de infectados
        infectadosText.text = "Infectados: " + numInfectados;

        // Actualizar el texto de velocidad de propagación
        float velocidadPropagacion = CalculateVelocidadPropagacion();
        velocidadPropagacionText.text = "Velocidad de propagación: " + velocidadPropagacion.ToString("F2");
    }

    float CalculateVelocidadPropagacion()
    {
        // Calcular la velocidad de propagación basada en los infectados y el tiempo
        return numInfectados / Mathf.Max(Time.time, 1f); // Para evitar división por cero
    }
}
