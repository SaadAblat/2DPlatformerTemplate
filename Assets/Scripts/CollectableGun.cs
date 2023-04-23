using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGun : MonoBehaviour
{
    float timer;
    BoxCollider2D coll;

    enum weaponType
    {
        pistol,
        shotgun
    }
    [SerializeField] weaponType type;

    // Start is called before the first frame update

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        StartCoroutine(canCollideAfterTime());
    }

    IEnumerator canCollideAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        //canCollide = true;
        coll.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case weaponType.pistol:
                    collision.gameObject.GetComponent<Player>().havePistol = true;
                    Destroy(gameObject);
                    break;
                case weaponType.shotgun:
                    collision.gameObject.GetComponent<Player>().haveShotgun = true;
                    Destroy(gameObject);

                    break;
                default:
                    break;
            }
        }
    }

}
