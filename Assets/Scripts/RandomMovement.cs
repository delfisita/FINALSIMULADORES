using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 10f;
    void Update()
    {
        Vector2 direction = Random.insideUnitCircle.normalized; 
        transform.Translate(direction * speed * Time.deltaTime);
    }

   

}
