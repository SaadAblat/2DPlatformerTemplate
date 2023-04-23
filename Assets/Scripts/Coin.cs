using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] string collectCoinSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.Play(collectCoinSound);
            SaveData.SaveInstance.coinCount++;
            Destroy(gameObject);
        }
    }
}
