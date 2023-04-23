using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] internal Rigidbody2D rb;
    //[SerializeField] GameObject GroundimpactEffect;
    //[SerializeField] GameObject EnemyimpactEffect;
    [SerializeField] float Lifetime;
    [SerializeField] float torqueForce;
    internal float torqueSign;
    //[SerializeField] float maxExplosionSize;
    //[SerializeField] float minExplosionSize;
    float Timer;
    SpaceShip ss;
    // Use this for initialization
    void Start()
    {
        Timer = 0;
        ss = FindObjectOfType<SpaceShip>();
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Follow = transform;
        rb.AddForce(speed * transform.right);
        rb.AddTorque(torqueForce * torqueSign);
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        //if (Timer > Lifetime)
        //{
        //    ss.player.gameObject.SetActive(true);
        //    ss.player.transform.position = transform.position;
        //    Destroy(gameObject);
        //}

        if (Input.GetMouseButtonDown(0) || Timer > Lifetime)
        {
            ss.player.gameObject.SetActive(true);
            ss.player.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    ss.player.gameObject.SetActive(true);
    //    ss.player.transform.position = transform.position;
    //    Destroy(gameObject);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100);

            }
        }
        if (collision.gameObject.CompareTag("DamageableBlock"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(100);
        }
        if (collision.CompareTag("SpaceShip"))
        {
            ss.player.gameObject.SetActive(true);
            ss.player.transform.position = transform.position;
            Destroy(gameObject);
        }

    }



}
