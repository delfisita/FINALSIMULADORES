using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public List<GameObject> citizenPrefabs; 
    public int populationSize = 10; 
    public float initialInfectionRate = 0.05f; 
    public float initialMisinformationRate = 0.2f; 
    public float defaultInfectionChance = 0.3f;
    public float defaultSpreadDelay = 1f;
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
           
            GameObject citizenPrefab = citizenPrefabs[0];
            float randomValue = Random.Range(0f, 1f);

           
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
           if (randomValue < initialInfectionRate + initialMisinformationRate)
            {
                citizenPrefab = citizenPrefabs.Find(prefab => prefab.name == "PRECAVIDO");
            }

            //hacer spawners o chequear el rango
            GameObject citizen = Instantiate(citizenPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(6f, -6f), Random.Range(5f, -5f));
    }
}
