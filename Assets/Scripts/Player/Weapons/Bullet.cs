using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 40;
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] GameObject GroundimpactEffect;
    [SerializeField] GameObject EnemyimpactEffect;
    [SerializeField] GameObject dirtImpacttEffect;
    [SerializeField] GameObject BlockImpacttEffect;
    [SerializeField] float Lifetime;
    [SerializeField] float maxExplosionSize;
    [SerializeField] float minExplosionSize;


    [SerializeField] string[] bulletSounds;
    Vector2 currentPos;
	Vector2 startpos;

	// Use this for initialization
	void Start()
	{
		rb.AddForce(speed * transform.right);
		StartCoroutine(DestroyBullet());
		startpos = transform.position;
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        string bulletSound = bulletSounds[Random.Range(0, bulletSounds.Length)];
        AudioManager.instance.Play(bulletSound);
        if (collision.gameObject.layer == 3 && !collision.gameObject.CompareTag("Crate") && !collision.gameObject.CompareTag("DamageableBlock"))
        {
            Instantiate(dirtImpacttEffect, transform.position, Quaternion.identity);

        }
        else if (collision.gameObject.CompareTag("DamageableBlock"))
        {
            Instantiate(BlockImpacttEffect, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
              EnemybulletExplosion();
            }
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Crate") && collision.gameObject.layer != 7)
        {
            //StartCoroutine(DestroyBulletOnImpact());
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
    private void EnemybulletExplosion()
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
