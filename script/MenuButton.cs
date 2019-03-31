using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public GameObject rateB;

    private void Start()
    {
        StartCoroutine(checkInternetConnection());
    }

    public void OnGameStartB()
    {
        SceneManager.LoadScene("Character Select", LoadSceneMode.Single);

    }
    public void OnRateB()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + "com.goodperson.eatit3");
        
#endif
    }
    public void OnGameEndB()
    {
        StopAllCoroutines();
        Application.Quit();
    }


    IEnumerator checkInternetConnection()
    {
        Button b = rateB.GetComponent<Button>();
        float originalAlpha = b.image.color.a;
        while (true)
        {
            WWW www = new WWW("http://google.com");
            yield return www;
            if (www.error != null)
            {
                //false

                b.enabled = false;
                b.image.color = new Color(b.image.color.r, b.image.color.g, b.image.color.b, 0.5f);
            }
            else
            {

                b.enabled = true;
                b.image.color = new Color(b.image.color.r, b.image.color.g, b.image.color.b, originalAlpha);
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
