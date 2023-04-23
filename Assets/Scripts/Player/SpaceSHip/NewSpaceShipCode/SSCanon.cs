using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSCanon : MonoBehaviour
{
    Vector3 mouseposition;
    Vector3 gunPosition;
    Camera maincamera;
    internal Vector2 direction;
    float gunAngle;

    [SerializeField] SpaceShip ss;

    [SerializeField] GameObject PlayerBall;
    [SerializeField] Transform firePoint;


    [SerializeField] float Camera_shake_intensity;
    [SerializeField] float Camera_shake_time;
    [SerializeField] float Camera_shake_frequency;
    float torquesign;

    internal bool ShootRequested;
    [SerializeField] string launchSound;
    internal void Shoot()
    {
        GameObject playerBall = Instantiate(PlayerBall, firePoint.position,firePoint.rotation);
        playerBall.GetComponent<PlayerBall>().torqueSign = torquesign;
        AudioManager.instance.Play(launchSound);
    }
    private void Update()
    {
        Aiming();
    }

    private void OnEnable()
    {
        maincamera = Camera.main;
    }
    void Aiming()
    {
        mouseposition = Input.mousePosition;
        gunPosition = Camera.main.WorldToScreenPoint(transform.position);
        mouseposition.x -= gunPosition.x;
        mouseposition.y -= gunPosition.y;

        gunAngle = Mathf.Atan2(mouseposition.y, mouseposition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));
        direction = new Vector2(Mathf.Cos(gunAngle * Mathf.PI / 180), Mathf.Sin(gunAngle * Mathf.PI / 180) / 2);


        //RaycastHit2D hit2D = Physics2D.Raycast(player.position, direction);
        //Debug.Log(hit2D.transform.name);


        if (maincamera.ScreenToWorldPoint(Input.mousePosition).x < ss.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));
            torquesign = 1;

        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0f, gunAngle));
            torquesign = -1;



        }
    }
}
