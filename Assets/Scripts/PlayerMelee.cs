using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] internal int playerMeleeDamage;
    [SerializeField] GameObject EnemyimpactEffect;
    bool canhit;

    [SerializeField] string hitRockSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (canhit)
        {
            
            CinemachineShake.CameraInstance.ShakeCamera(0.5f, 0.09f, 6f);
            if (collision.gameObject.CompareTag("Enemy"))
            {

                canhit = false;
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(playerMeleeDamage);
                    Instantiate(EnemyimpactEffect, transform.position, transform.rotation);

                }
            }
            if (collision.gameObject.CompareTag("DamageableBlock"))
            {
                AudioManager.instance.Play(hitRockSound);

                //canhit = false;
                collision.gameObject.GetComponent<IDamageable>().TakeDamage(playerMeleeDamage);
            }
            if (collision.gameObject.CompareTag("Crate"))
            {

                canhit = false;

            }
        }

    }
    private void OnEnable()
    {
        canhit = true;
    }
}
