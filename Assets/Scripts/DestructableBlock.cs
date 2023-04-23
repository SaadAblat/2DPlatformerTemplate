using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBlock : MonoBehaviour, IDamageable
{
    [SerializeField] float HealthAtStart;
    float Health;
    int animationIndex;
    Animator animator;
    Vector2 starPos;
    bool isShaking;
    [SerializeField] float shakeAmount;
    [SerializeField] GameObject RockDestroy;
    [SerializeField] GameObject[] WhatItGivesOnDestroy;
    [SerializeField] int HowManyItGives;
    [SerializeField] string blockDestructionSound;

    [SerializeField] string giveOneCoinSound;
    public void TakeDamage(int damage)
    {
        starPos = transform.position;
        Health -= damage;
        StartCoroutine(TakeHitFeedBack());
    }

    // Start is called before the first frame update
    void Start()
    {
        animationIndex = 1;
        animator = GetComponent<Animator>();
        Health = HealthAtStart;

    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            transform.position = starPos + Random.insideUnitCircle * shakeAmount;
        }
        if (Health > 0)
        {
            SetAnimationByAnimationIndex(Animationindex(Health));
        }
        else
        {
            Destroy();
        }
    }
    bool courotineStarted = false;
    void Destroy()
    {

        if (!courotineStarted)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Give_WhatItGivesOnDestroy());
            courotineStarted = true;
        }
        //gameObject.SetActive(false);
    }
    IEnumerator Give_WhatItGivesOnDestroy()
    {
        Instantiate(RockDestroy, transform.position, Quaternion.identity);
        AudioManager.instance.Play(blockDestructionSound);
        foreach (GameObject @object in WhatItGivesOnDestroy)
        {
            for (int i = 0; i <= HowManyItGives; i++)
            {
                Instantiate(@object, transform.position, Quaternion.identity);
                AudioManager.instance.Play(giveOneCoinSound);
                if (i == HowManyItGives)
                {
                    Destroy(gameObject);
                }
                yield return new WaitForSeconds(0.05f);
            }

        }
    }
    int Animationindex(float health)
    {
        if (health == HealthAtStart)
        {
            animationIndex = 1;
            return animationIndex;

        }
        else
        {
            animationIndex = 2;
            return animationIndex;
        }
    }
    void SetAnimationByAnimationIndex(int animationIndex)
    {
        animator.Play(animationIndex.ToString());
    }
    IEnumerator TakeHitFeedBack()
    {
        isShaking = true;
        yield return new WaitForSeconds(0.2f);
        isShaking = false;
        transform.position = starPos;

    }
}
