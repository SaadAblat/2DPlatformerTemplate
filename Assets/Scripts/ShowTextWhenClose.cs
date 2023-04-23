using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextWhenClose : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float proximityThresholdX;
    [SerializeField] float proximityThresholdXY;
    [SerializeField] Transform[] textsToShow;


    void Update()
    {
        if (player != null)
        {
            foreach (Transform text in textsToShow)
            {
                if (IsCLoseEnough(text))
                {
                    text.gameObject.SetActive(true);
                }
            }
        }



    }


    bool IsCLoseEnough(Transform textTrans)
    {
        float distanceX = Mathf.Abs(player.position.x - textTrans.position.x);
        float distanceY = Mathf.Abs(player.position.y - textTrans.position.y);
        if (distanceX < proximityThresholdX && distanceY < proximityThresholdXY)
        {
            return true;
        }
        return false;
    }

}
