using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class HigestScore : MonoBehaviour
{    
    Text highScore;
    void OnEnable()
    {
        highScore = GetComponent<Text>();
        highScore.text = "Higest Score: " + PlayerPrefs.GetInt("HighScore").ToString();
    }
}
