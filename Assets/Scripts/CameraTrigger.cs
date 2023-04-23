using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera CinCamera;
    [SerializeField] int Priority_On_Trigger_Enter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CinCamera.Priority = Priority_On_Trigger_Enter;
    }
}
