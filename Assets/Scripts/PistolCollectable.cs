using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.GetComponent<Player>() != null)
            {
                collision.gameObject.GetComponent<Player>().havePistol = true;
            }
            else if (collision.transform.parent.GetComponent<Player>() != null)
            {
                collision.transform.parent.GetComponent<Player>().havePistol = true;
            }
            Destroy(gameObject);
        }

    }
}
