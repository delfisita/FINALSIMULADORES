using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public FirebaseManager firebaseManager;

    public GameObject infectadoPrefab;
    public GameObject desinformadoPrefab;
    public GameObject precavidoPrefab;
    public GameObject sanoPrefab;

    public int populationSize = 100;
    public float initialInfectionRate = 0.05f;
    public float initialMisinformationRate = 0.2f;
    public float defaultInfectionChance = 0.3f;
    public float defaultSpreadDelay = 1f;

    public Text infectadosText;
    public Text velocidadPropagacionText;

    public GameObject gameOverPanel; 

    private int numInfectados = 0;
    private int numSanos = 0;
    private int numDesinformados = 0;
    private int numPrecavidos = 0;

    private float lastInfectionTime = 0f;

    void Start()
    {
        numInfectados = 0;
        numSanos = populationSize;
        numDesinformados = 0;
        numPrecavidos = 0;

        for (int i = 0; i < populationSize; i++)
        {
            GameObject citizenPrefab = sanoPrefab;
            float randomValue = Random.Range(0f, 1f);

            if (randomValue < initialInfectionRate)
            {
                citizenPrefab = infectadoPrefab;
                numInfectados++;
                numSanos--;
            }
            else if (randomValue < initialInfectionRate + initialMisinformationRate)
            {
                citizenPrefab = desinformadoPrefab;
                numDesinformados++;
                numSanos--;
            }
            else if (randomValue < initialInfectionRate + initialMisinformationRate + 0.1f)
            {
                citizenPrefab = precavidoPrefab;
                numPrecavidos++;
                numSanos--;
            }

            Instantiate(citizenPrefab, GetRandomPositionWithinBounds(), Quaternion.identity);
        }

        if (firebaseManager != null)
        {
            firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, 0f, 0);
        }
        else
        {
            Debug.LogWarning("FirebaseManager no está asignado en SimulationManager.");
        }

        UpdateUI();
    }

    Vector2 GetRandomPositionWithinBounds()
    {
        float x = Random.Range(-25f, 30f);
        float y = Random.Range(0f, -20f);
        return new Vector2(x, y);
    }

    void Update()
    {
        if (Time.time - lastInfectionTime >= defaultSpreadDelay)
        {
            PropagarInfeccion();
            lastInfectionTime = Time.time;
            UpdateUI();

            if (firebaseManager != null)
            {
                firebaseManager.ActualizarEstadisticas(numInfectados, populationSize, CalculateVelocidadPropagacion(), Time.time);
            }

        }
    }

    void PropagarInfeccion()
    {
        if (numSanos > 0)
        {
            numInfectados++;
            numSanos--;
        }
    }

    void UpdateUI()
    {
        infectadosText.text = "Infectados: " + numInfectados;
        float velocidadPropagacion = CalculateVelocidadPropagacion();
        velocidadPropagacionText.text = "Velocidad de propagación: " + velocidadPropagacion.ToString("F2");
    }

    float CalculateVelocidadPropagacion()
    {
        return numInfectados / Mathf.Max(Time.time, 1f);
    }

    
}
