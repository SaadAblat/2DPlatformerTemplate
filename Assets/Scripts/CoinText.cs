using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinText : MonoBehaviour
{
    TMP_Text Coin_Count_Text;
    SaveData saveData;
    
    // Start is called before the first frame update
    void Start()
    {
        Coin_Count_Text = GetComponent<TMP_Text>();
        saveData = SaveData.SaveInstance;
    }
    // Update is called once per frame
    void Update()
    {
        Coin_Count_Text.text = "X" + saveData.coinCount;
    }
}
