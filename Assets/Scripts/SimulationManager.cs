using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject infectadoPrefab;
    public GameObject desinformadoPrefab;
    public GameObject precavidoPrefab;
    public GameObject sanoPrefab;

    public int populationSize = 10;
    public float initialInfectionRate = 0.05f;
    public float initialMisinformationRate = 0.2f;
    public float defaultInfectionChance = 0.3f;
    public float defaultSpreadDelay = 1f;

    // Define bounds for the spawn area
    public Vector2 spawnAreaMin = new Vector2(-5f, -5f);
    public Vector2 spawnAreaMax = new Vector2(5f, 5f);

    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject citizenPrefab = sanoPrefab; // Default to "SANO"
            float randomValue = Random.Range(0f, 1f);

            // Determine citizen type based on random value
            if (randomValue < initialInfectionRate)
            {
                citizenPrefab = infectadoPrefab;
            }
            else if (randomValue < initialInfectionRate + initialMisinformationRate)
            {
                citizenPrefab = desinformadoPrefab;
            }
            else if (randomValue < initialInfectionRate + initialMisinformationRate + 0.2f) // Adjust rate for PRECAVIDO
            {
                citizenPrefab = precavidoPrefab;
            }

            // Instantiate the citizen within the spawn area
            Instantiate(citizenPrefab, GetRandomPositionWithinBounds(), Quaternion.identity);
        }
    }

    Vector2 GetRandomPositionWithinBounds()
    {
        // Ensure the position is within the defined bounds
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }
}
