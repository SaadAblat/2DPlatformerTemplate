using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 40;
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] GameObject GroundimpactEffect;
    [SerializeField] GameObject EnemyimpactEffect;
    [SerializeField] float Lifetime;
    [SerializeField] float maxExplosionSize;
    [SerializeField] float minExplosionSize;
    Vector2 currentPos;
    Vector2 startpos;

    [SerializeField] string[] bulletSounds;

    // Use this for initialization
    void Start()
    {
        rb.AddForce(speed * -transform.right);
        StartCoroutine(DestroyBullet());
        startpos = transform.position;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        string bulletSound = bulletSounds[Random.Range(0, bulletSounds.Length)];
        AudioManager.instance.Play(bulletSound);

        if (collision.gameObject.CompareTag("Player"))
        {

            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                ImpactbulletExplosion();
            }
        }
        else
        {
            GroundbulletExplosion();
        }
        Destroy(gameObject);
    }

    private void GroundbulletExplosion()
    {
        GameObject explosion = Instantiate(GroundimpactEffect, transform.position, transform.rotation);
        float scale = Random.Range(minExplosionSize, maxExplosionSize);
        explosion.transform.localScale = new Vector2(scale, scale);

    }
    private void ImpactbulletExplosion()
    {
        Instantiate(EnemyimpactEffect, transform.position, transform.rotation);
    }
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
        GroundbulletExplosion();
    }
    //IEnumerator DestroyBulletOnImpact()
    //   {
    //	yield return new WaitForSeconds(0.03f);
    //	Destroy(gameObject);
    //   }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

