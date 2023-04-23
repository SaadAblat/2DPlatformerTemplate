using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpaceShip : MonoBehaviour
{
    internal bool playerIsInside;
    SpaceShipActivator Activator;
    internal Player player;
    internal CinemachineVirtualCamera cinemachine;

    private void Start()
    {
        Activator = GetComponent<SpaceShipActivator>();
        player = FindObjectOfType<Player>().GetComponent<Player>();
        cinemachine = CinemachineShake.CameraInstance.cinemaMachineVirtualCamera;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activator.enabled = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerIsInside)
            {
                Activator.enabled = false;
            }

        }
    }
}
