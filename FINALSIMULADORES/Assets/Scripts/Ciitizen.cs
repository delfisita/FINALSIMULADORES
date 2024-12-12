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

           
            if (state == CitizenState.Desinformado)
            {
                adjustedInfectionChance += 0.3f;
            }
           
            else if (state == CitizenState.Precavido)
            {
                adjustedInfectionChance -= 0.2f; 
            }

            if (chance < adjustedInfectionChance)
            {
                BecomeInfected();
                Debug.Log($"{gameObject.name} se ha infectado.");
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
        //cambiar el sprite renderer por sprite 
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
