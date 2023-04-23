using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpeedParamForAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }
}
