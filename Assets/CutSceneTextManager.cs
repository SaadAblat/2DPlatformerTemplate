using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTextManager : MonoBehaviour
{
    int index;
    [SerializeField] GameObject[] Texts;
    int numberOfTexts;

    void Start()
    {
        index = 1;
        numberOfTexts = Texts.Length;
    }


    private void Update()
    {
        foreach(GameObject text in Texts)
        {
            
            if (index == text.GetComponent<TextId>().Id)
            {
                text.SetActive(true);
            }
            else
            {
                text.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    public void SkipButton()
    {
        if (index < numberOfTexts)
        {
            index += 1;
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
