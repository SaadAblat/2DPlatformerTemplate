using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipActivator : MonoBehaviour
{
    SpaceShip ss;
    [SerializeField] SSCanon canon;
    SpaceShipController ssController;

    public enum Cases
    {
        outside,
        inside
    }
    Cases cases;
    // Start is called before the first frame update
    void Start()
    {
        cases = Cases.outside;
        ss = GetComponent<SpaceShip>();
        if (ssController != null)
        {
            ssController = GetComponent<SpaceShipController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (cases)
        {
            case Cases.outside:
                //if (Input.GetKeyDown(KeyCode.E))
                //{
                //    ss.playerIsInside = true;
                //    ss.player.gameObject.SetActive(false);
                //    ss.cinemachine.m_Follow = gameObject.transform;

                //    //
                //    canon.enabled = true;
                //    ssController.enabled = true;
                //    cases = Cases.inside;

                //}
                ss.playerIsInside = true;
                ss.player.gameObject.SetActive(false);
                ss.cinemachine.m_Follow = gameObject.transform;

                //
                canon.enabled = true;
                if (ssController != null)
                {
                    ssController.enabled = true;
                }
                cases = Cases.inside;
                break;
            case Cases.inside:
                if (Input.GetMouseButtonDown(0))
                {
                    canon.enabled = false;
                    if (ssController != null)
                    {
                        ssController.enabled = false;
                    }
                    canon.Shoot();
                    ss.playerIsInside = false;
                    cases = Cases.outside;
                    enabled = false; 

                }
                break;
        }
    }
}
