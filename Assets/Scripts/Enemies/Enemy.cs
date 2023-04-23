using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int Health;
    [SerializeField] protected GameObject BloodExplosionPrefab;
    [SerializeField] GiveStuffOnDestroy givestuff;
    [SerializeField] string takeDamageSound;
    [SerializeField] string DeathSound;

    protected virtual void Death()
    {
        givestuff.GenerateStuff();
        Instantiate(BloodExplosionPrefab, transform.position, Quaternion.identity);
        AudioManager.instance.Play(DeathSound);

        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        AudioManager.instance.Play(takeDamageSound);

    }

}
