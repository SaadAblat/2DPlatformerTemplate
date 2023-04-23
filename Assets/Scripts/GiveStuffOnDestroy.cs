using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveStuffOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject ShotgunAmmo;
    [SerializeField] GameObject PistolAmmo;
    [SerializeField] GameObject Fuel;
    [SerializeField] GameObject Coin;


    [SerializeField] int Pistol_Ammo_Probability_From_0_To_11;
    [SerializeField] int ShotgunAmmo_Probability_From_0_To_11;
    [SerializeField] int Fuel_Probability_From_0_To_11;
    [SerializeField] int Coin_Probability_From_0_To_11;
    // Start is called before the first frame update


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

    public void GenerateStuff()
    {
        RandomlyGenerateAmmo(ShotgunAmmo, ShotgunAmmo_Probability_From_0_To_11);
        RandomlyGenerateAmmo(PistolAmmo, Pistol_Ammo_Probability_From_0_To_11);
        RandomlyGenerateAmmo(Fuel, Fuel_Probability_From_0_To_11);
        RandomlyGenerateAmmo(Coin, Coin_Probability_From_0_To_11);
    }
}
