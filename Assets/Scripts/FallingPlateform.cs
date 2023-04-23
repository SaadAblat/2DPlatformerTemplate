using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlateform : MonoBehaviour
{
    bool Fall;
    Vector2 startpos;
    [SerializeField] Rigidbody2D Rb_plateform;
    [SerializeField] GameObject plateform;
    [SerializeField] BoxCollider2D col;
    [SerializeField] float timeBeforeFalling;
    [SerializeField] float timeBeforSpawning;
    private void Awake()
    {
        startpos = plateform.transform.position;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //collision.transform.parent = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            StartCoroutine(reset());
        }
    }

    IEnumerator reset()
    {
        yield return new WaitForSeconds(timeBeforeFalling);
        Rb_plateform.isKinematic = false;
        Fall = true;
        col.enabled = false;
        yield return new WaitForSeconds(timeBeforSpawning);
        col.enabled = true;
        plateform.transform.position = startpos;
        Rb_plateform.isKinematic = true;
        Rb_plateform.velocity = Vector2.zero;
        StopAllCoroutines();
    }
}