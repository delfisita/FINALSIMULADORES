using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Random.insideUnitCircle * speed * Time.deltaTime);
    }
}