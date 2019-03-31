using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreText : MonoBehaviour
{
    private Text highScoreText;
    public GameObject leaderboardB;
    private void OnEnable()
    {
        highScoreText = GetComponent<Text>();
        highScoreText.text = LocalizationManager.Instance.GetText("highScoreIs") + PlayerPrefs.GetInt("HighScore");
        StartCoroutine(checkInternetConnection());
    }

    IEnumerator checkInternetConnection()
    {
        Button b = leaderboardB.GetComponent<Button>();
        float originalAlpha = b.image.color.a;
        while (true)
        { 
            WWW www = new WWW("http://google.com");
            yield return www;
            if (www.error != null)
            {
                //false
                
                b.enabled = false;
                b.image.color = new Color(b.image.color.r, b.image.color.g, b.image.color.b, 0.3f);
            }
            else
            {
              
                b.enabled = true;
                b.image.color = new Color(b.image.color.r,b.image.color.g,b.image.color.b, originalAlpha);
                //true

            }

            yield return new WaitForSecondsRealtime(5);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(checkInternetConnection());
    }
    private void OnDestroy()
    {
        StopCoroutine(checkInternetConnection());
    
    }
}
