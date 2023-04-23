using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] Player player;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Fuel"))
    //    {
    //        Debug.Log("CollidedwithFuel");
    //        activateTheSpaceShip.FuelLevel = activateTheSpaceShip.MaxFuelLevel;
    //        Destroy(collision.gameObject);
    //    }
    //}

    [SerializeField] string collectAmmoSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ammo"))
        {
            AudioManager.instance.Play(collectAmmoSound);
            switch (collision.GetComponent<Ammo>().ammoType)
            {
                case Ammo.AmmoType.Pistol:
                    AddPistolAmmo(collision);
                    break;
                case Ammo.AmmoType.Shotgun:
                    AddShotgunAmmo(collision);
                    break;
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Fuel"))
        {
            Destroy(collision.gameObject);
        }


        if (collision.gameObject.CompareTag("Ladder"))
        {
            player.isOnLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            player.isOnLadder = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.CompareTag("Spike"))
        {
            player.Death();
        }
    }

    void AddShotgunAmmo(Collider2D collision)
    {
        if (player.ShotgunAmmoCount < player.MaxShotgunAmmo)
        {
            player.ShotgunAmmoCount += collision.gameObject.GetComponent<Ammo>().howMuchAmmoContains;
            for (int i = 0; i < collision.gameObject.GetComponent<Ammo>().howMuchAmmoContains; i++)
            {
                if (player.ShotgunAmmoCount < player.MaxShotgunAmmo)
                {
                    player.ShotgunAmmoCount += 1;
                }
                else player.ShotgunAmmoCount = player.MaxShotgunAmmo;
            }

        }
        else
        {
            player.ShotgunAmmoCount = player.MaxShotgunAmmo;
        }
    }
    void AddPistolAmmo(Collider2D collision)
    {
        if (player.PistolAmmoCount < player.MaxPistolAmmo)
        {
            player.PistolAmmoCount += collision.gameObject.GetComponent<Ammo>().howMuchAmmoContains;
            for (int i = 0; i < collision.gameObject.GetComponent<Ammo>().howMuchAmmoContains; i++)
            {
                if (player.PistolAmmoCount < player.MaxPistolAmmo)
                {
                    player.PistolAmmoCount += 1;
                }
                else player.PistolAmmoCount = player.MaxPistolAmmo;
            }

        }
        else
        {
            player.PistolAmmoCount = player.MaxPistolAmmo;
        }

    }
}
