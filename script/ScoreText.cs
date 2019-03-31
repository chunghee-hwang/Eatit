using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreText : MonoBehaviour {

    private Text scoreText;
    public void OnEnable()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = LocalizationManager.Instance.GetText("scoreIs") + Eatman.score.ToString();
    }
}
