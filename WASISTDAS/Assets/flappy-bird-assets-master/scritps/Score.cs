using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    Text score;
    private void OnEnable()
    {
        score = GetComponent<Text>();
        Debug.Log(GameManager.Instance);
        score.text = "Score: " + GameManager.Instance.Score.ToString();
    }
}
