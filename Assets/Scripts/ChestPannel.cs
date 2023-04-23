using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestPannel : MonoBehaviour
{
    [SerializeField] Chest chest;
    [SerializeField] Button ActivateButton;

    private void Start()
    {
        ActivateButton = transform.GetComponentInChildren<Button>();
        if (chest.Opened)
        {
            ActivateButton.GetComponentInChildren<TMP_Text>().text = "";
        }
        else
        {
            ActivateButton.GetComponentInChildren<TMP_Text>().text = "Activate CheckPoint for " + chest.Price;
        }
    }

    private void Update()
    {
        if (SaveData.SaveInstance.coinCount >= chest.Price && !chest.Opened)
        {

            ActivateButton.interactable = true;
        }
        else
        {
            ActivateButton.interactable = false;
        }
    }
    public void OpenChest()
    {
        SaveData.SaveInstance.coinCount -= chest.Price;
        chest.Opened = true;
        chest.ChangeAnimation();
        ActivateButton.interactable = false;

    }
}
