using System.Collections;
using System.Collections.Generic;
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
    public float infectionChance; // Probabilidad de infectarse al entrar en contacto
    public float spreadDelay = 1f; // Tiempo entre contagios
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
            if (chance < infectionChance)
            {
                BecomeInfected();
            }
        }
    }

    private void SpreadInfection()
    {
        Collider2D[] nearbyCitizens = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var col in nearbyCitizens)
        {
            Citizen other = col.GetComponent<Citizen>();
            if (other != null && other.state == CitizenState.Healthy)
            {
                other.TryToInfect();
            }
        }
    }

    private void BecomeInfected()
    {
        state = CitizenState.Infected;
        GetComponent<SpriteRenderer>().color = Color.red; // Cambia el color a rojo
    }
}
