using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckPoint : MonoBehaviour
{
    [SerializeField] CheckPointPannel checkPointPannel;
    internal bool Activated;


    [SerializeField] internal int price;
    Animator animator;
    public int CheckPointID;

    bool havePistol_checkpointLocalvar;
    bool haveShotgun_checkpointLocalvar;
    int pistolAmmo_checkpointLocalvar;
    int shotgunAmmo_checkpointLocalvar;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Off");
        foreach (int id in SaveData.SaveInstance.CheckPointIDActivated)
            {
                if (CheckPointID == id)
                {
                   Activated = true;
                   ChangeAnimation();
                }
            }
   

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkPointPannel.gameObject.SetActive(true);
            Player player = collision.GetComponentInParent<Player>();
            if (player != null)
            {
                havePistol_checkpointLocalvar = player.havePistol;
                haveShotgun_checkpointLocalvar = player.haveShotgun;
                pistolAmmo_checkpointLocalvar = player.PistolAmmoCount;
                shotgunAmmo_checkpointLocalvar = player.ShotgunAmmoCount;
            }
        



        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkPointPannel.gameObject.SetActive(false);
        }
    }

    internal void SaveCheckPoint()
    {
        SaveData.SaveInstance.HavePistol = havePistol_checkpointLocalvar;
        SaveData.SaveInstance.HaveShotgun = haveShotgun_checkpointLocalvar;
        SaveData.SaveInstance.LastCheckPointPistolAmmo = pistolAmmo_checkpointLocalvar;
        SaveData.SaveInstance.LastCheckPointShotgunAmmo = shotgunAmmo_checkpointLocalvar;
        SaveData.SaveInstance.firstCheckPointActivated = true;
        SaveData.SaveInstance.LastCheckPointCoin = SaveData.SaveInstance.coinCount;
        SaveData.SaveInstance.LastCheckPointPosition = transform.position;
        SaveData.SaveInstance.CheckPointIDActivated.Add(CheckPointID);
        ChangeAnimation();
    }
    internal void ChangeAnimation()
    {
        if (Activated)
        {
            animator.Play("On");
        }
        else
        {
            animator.Play("Off");
        }
    }
    
}

