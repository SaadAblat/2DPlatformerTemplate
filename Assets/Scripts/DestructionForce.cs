using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionForce : MonoBehaviour
{
    [SerializeField] Vector2 forcedirection;
    [SerializeField] float torque;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

        float randdirectionX = Random.Range(forcedirection.x - 100, forcedirection.x+100);
        float randdirectionY = Random.Range(forcedirection.y - 50, forcedirection.y + 50);
        float randtorque = Random.Range(torque - 40, torque + 40);

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(randdirectionX, randdirectionY));
        rb.AddTorque(randtorque);

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
