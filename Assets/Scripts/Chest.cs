using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject Pannel;
    internal bool Opened;
    [SerializeField] Animator anim;

    [SerializeField] GameObject[] reward;


    [SerializeField] internal int Price;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Opened)
        {
            Pannel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pannel.SetActive(false);
        }
    }
    // Start is called before the first frame update


    public void ChangeAnimation()
    {
        if (Opened)
        {
            anim.Play("Opening");


        }
        else
        {
            anim.Play("Closed");
        }
    }

    public void GiveReward()
    {
        foreach (GameObject objecti in reward)
        {
            Instantiate(objecti, transform.position, Quaternion.identity);
        }
    }
}
