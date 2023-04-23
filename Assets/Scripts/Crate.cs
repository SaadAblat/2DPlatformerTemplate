using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] GameObject boxDestroy;
    [SerializeField] GameObject DustParticleExploison;
    [SerializeField] GameObject ShotgunAmmo;
    [SerializeField] GameObject PistolAmmo;
    [SerializeField] GameObject Fuel;
    [SerializeField] GameObject Coin;

    /// <summary>
    /// between 0 and 11
    /// 0 the randomnumber will be never smaller so 0 probability
    /// 11 the randomnumber will be always bigger  
    /// </summary>
    [SerializeField] int Pistol_Ammo_Probability_From_0_To_11; 
    [SerializeField] int ShotgunAmmo_Probability_From_0_To_11; 
    [SerializeField] int Fuel_Probability_From_0_To_11; 
    [SerializeField] int Coin_Probability_From_0_To_11;


    [SerializeField] string crateDestructionSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerWeapon"))
        {
            Instantiate(boxDestroy, transform.position, Quaternion.identity);
            Instantiate(DustParticleExploison, transform.position, Quaternion.identity);

            RandomlyGenerateAmmo(ShotgunAmmo, ShotgunAmmo_Probability_From_0_To_11);
            RandomlyGenerateAmmo(PistolAmmo, Pistol_Ammo_Probability_From_0_To_11);
            RandomlyGenerateAmmo(Fuel, Fuel_Probability_From_0_To_11);
            RandomlyGenerateAmmo(Coin, Coin_Probability_From_0_To_11);
            AudioManager.instance.Play(crateDestructionSound);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            Instantiate(boxDestroy, transform.position, Quaternion.identity);
            Instantiate(DustParticleExploison, transform.position, Quaternion.identity);

            RandomlyGenerateAmmo(ShotgunAmmo, ShotgunAmmo_Probability_From_0_To_11);
            RandomlyGenerateAmmo(PistolAmmo, Pistol_Ammo_Probability_From_0_To_11);
            RandomlyGenerateAmmo(Fuel, Fuel_Probability_From_0_To_11);
            RandomlyGenerateAmmo(Coin, Coin_Probability_From_0_To_11);
            AudioManager.instance.Play(crateDestructionSound);

            Destroy(gameObject);
        }
    }
    void RandomlyGenerateAmmo(GameObject AmmoType, int AmmoProbability)
    {
        int randomNumberBetween0nd10 = (int)Random.Range(0f, 10f);
        if (randomNumberBetween0nd10 < AmmoProbability)
        {
            int AmountOfAmmo = (int)Random.Range(1f, 5f);
            for (int i = 0; i < AmountOfAmmo; i++)
            {
                Instantiate(AmmoType, transform.position, Quaternion.identity);
                i++;
            }
        }
    }

}
