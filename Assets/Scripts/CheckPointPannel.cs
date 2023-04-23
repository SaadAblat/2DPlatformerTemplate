using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CheckPointPannel : MonoBehaviour
{
    [SerializeField] CheckPoint checkpoint;
    [SerializeField]  Button ActivateButton;
    // Start is called before the first frame update
    void Start()
    {
        ActivateButton = transform.GetComponentInChildren<Button>();
        if (checkpoint.Activated)
        {
            ActivateButton.GetComponentInChildren<TMP_Text>().text = "Activated";
        }
        else
        {
            ActivateButton.GetComponentInChildren<TMP_Text>().text = "Activate CheckPoint for " + checkpoint.price;
        }
    }

    private void Update()
    {
        if (SaveData.SaveInstance.coinCount >= checkpoint.price && !checkpoint.Activated)
        {
            
            ActivateButton.interactable = true;
        }
        else
        {
            ActivateButton.interactable = false;
        }
    }

    public void ActivateCheckPoint()
    {
        SaveData.SaveInstance.coinCount -= checkpoint.price;
        checkpoint.Activated = true;
        checkpoint.SaveCheckPoint();


        ActivateButton.GetComponentInChildren<TMP_Text>().text = "Activated";
        ActivateButton.interactable = false;
    }
}
