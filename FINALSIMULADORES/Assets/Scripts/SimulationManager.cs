using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public List<GameObject> citizenPrefabs; // Lista de prefabs para diferentes tipos de ciudadanos
    public int populationSize = 100; // Número total de ciudadanos
    public float initialInfectionRate = 0.05f; // Porcentaje inicial de infectados
    public float initialMisinformationRate = 0.2f; // Porcentaje inicial de desinformados

    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            // Selecciona un prefab base (el primero en la lista por defecto)
            GameObject citizenPrefab = citizenPrefabs[0];
            float randomValue = Random.Range(0f, 1f);

            // Decide el tipo de ciudadano basado en probabilidades
            if (randomValue < initialInfectionRate)
            {
                citizenPrefab = citizenPrefabs.Find(prefab => prefab.name == "INFECTADO");
            }
            else if (randomValue < initialInfectionRate + initialMisinformationRate)
            {
                citizenPrefab = citizenPrefabs.Find(prefab => prefab.name == "DESINFORMADO");
            }
            else
            {
                citizenPrefab = citizenPrefabs.Find(prefab => prefab.name == "SANO");
            }
            

            // Crea el ciudadano en una posición aleatoria
            GameObject citizen = Instantiate(citizenPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
    }
}
