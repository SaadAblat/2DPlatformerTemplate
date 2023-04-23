using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //RememberActivatedCheckPoints();
        Remember_Coin_Count_At_The_Last_Checkpoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string curScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(curScene);
        }
    }
    //void RememberActivatedCheckPoints()
    //{
    //    foreach (CheckPoint checkPoint in FindObjectsOfType<CheckPoint>())
    //    {
    //        foreach (int id in SaveData.SaveInstance.CheckPointIDActivated)
    //        {
    //            if (checkPoint.CheckPointID == id)
    //            {
    //                checkPoint.Activated = true;
    //                checkPoint.ChangeAnimation();
    //            }
    //        }
    //    }
    //}
    void Remember_Coin_Count_At_The_Last_Checkpoint()
    {
        SaveData.SaveInstance.coinCount = SaveData.SaveInstance.LastCheckPointCoin;
    }
}
