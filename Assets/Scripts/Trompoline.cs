using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trompoline : MonoBehaviour
{
    Animator animator;
    Player player;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            StartCoroutine(Launch(player));
        }
    }
    
    IEnumerator Launch(Player pl)
    {
        yield return new WaitForSeconds(0.05f);
        pl.Trompoline();
        animator.Play("Launch");
    }
}
