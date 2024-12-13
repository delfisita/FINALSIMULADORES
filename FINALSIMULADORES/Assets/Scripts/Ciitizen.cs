using UnityEngine;
public enum CitizenState
{
    Healthy,
    Infected,
    Precavido,
    Desinformado
}
public class Citizen : MonoBehaviour
{
    public CitizenState state;
    public float infectionChance; 
    public float spreadDelay = 1f;
    public float detectionRadius = 1f; 
    private float spreadTimer;

    void Update()
    {
        if (state == CitizenState.Infected)
        {
            spreadTimer -= Time.deltaTime;
            if (spreadTimer <= 0f)
            {
                SpreadInfection();
                spreadTimer = spreadDelay;
            }
        }
    }

    public void TryToInfect()
    {
        if (state == CitizenState.Healthy)
        {
            float chance = Random.Range(0f, 1f);
            float adjustedInfectionChance = infectionChance;

            // Ajusta la probabilidad según el estado del ciudadano
            if (state == CitizenState.Desinformado)
            {
                adjustedInfectionChance += 0.3f; // Mayor chance si está desinformado
            }
            else if (state == CitizenState.Precavido)
            {
                adjustedInfectionChance = Mathf.Max(0f, adjustedInfectionChance - 0.2f); // Reduce, pero nunca negativo
            }

            if (chance < adjustedInfectionChance)
            {
                BecomeInfected();
                Debug.Log($"{gameObject.name} se ha infectado con probabilidad {adjustedInfectionChance}.");
            }
        }
    }


    private void SpreadInfection()
    {
        Collider2D[] nearbyCitizens = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var col in nearbyCitizens)
        {
            Citizen other = col.GetComponent<Citizen>();
            if (other != null && other.state == CitizenState.Healthy)
            {
                Debug.DrawLine(transform.position, other.transform.position, Color.red, 1f);
                other.TryToInfect();
            }
        }
    }


    private void BecomeInfected()
    {
        state = CitizenState.Infected;
       
        GetComponent<SpriteRenderer>().color = Color.red; 
    }

    
    private void OnDrawGizmos()
    {
        if (state == CitizenState.Infected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

}
