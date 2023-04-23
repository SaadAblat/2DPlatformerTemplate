using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [Header("accuracy from 0 to 90")]
    [SerializeField] float accuracy;
    [SerializeField] float drawBack;
    [SerializeField] float timeBeforeFireAgain;
    float timeElapsedAfterLastBullet;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] EnemyAim aim;
    internal bool ShootRequested;
    [SerializeField] Animator animator;
    [SerializeField] string pistolShootSound;

    public void HandleShooting()
    {
        timeElapsedAfterLastBullet += Time.deltaTime;
        if (timeElapsedAfterLastBullet > timeBeforeFireAgain)
        {
            AudioManager.instance.Play(pistolShootSound);
            Shoot();
            timeElapsedAfterLastBullet = 0;

        }
    }
    void Shoot()
    {
        animator.Play("Shoot", -1, 0f);
        GameObject tmp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        rb.AddForce(-aim.direction * drawBack, ForceMode2D.Impulse);
        tmp.transform.Rotate(0, 0f, Random.Range(-90 + accuracy, 90 - accuracy));


    }
}
