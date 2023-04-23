using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartingPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(WaitForSaveDataToInstantiate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForSaveDataToInstantiate()
    {
        yield return new WaitUntil(() => (SaveData.SaveInstance != null));
        if (!SaveData.SaveInstance.firstCheckPointActivated)
        {
            Debug.Log("Done");
            SaveData.SaveInstance.LastCheckPointPosition = transform.position;
        }
    }
}
