using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject MiniMenuB, minimenu;
    public GameObject gameOverPage;
    public GameObject scoreText;
    public GameObject levelUpPage;
    public GameObject LoadWaiter;
    public static bool levelUp = false;
    public static int level = 1;
    public AudioClip gameOverSound;
    public AudioSource gameOverSource;
    private AdManager admanager;
    public static bool mainGameLoaded = false;
    private void Start()
    {
        StartCoroutine(LoadWait());
        admanager = gameObject.GetComponent<AdManager>();
        StartCoroutine(admanager.RequestInterstitialAd());
        StartCoroutine(GameOverWaiter());
        StartCoroutine(LevelUpWaiter());
        
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
            yield return new WaitUntil(() => admanager != null);
           // Debug.Log("admanager created");
            yield return new WaitUntil(() => (admanager.interstitialAd.IsLoaded() || admanager.adLoadFail));
        }
    }

    IEnumerator LoadWait()
    {
        Time.timeScale = 0;
        LoadWaiter.SetActive(true);
       
        yield return StartCoroutine(checkInternetConnection((isConnected) => {}));
        //Debug.Log("internet connection complete and ad loaded");
        yield return new WaitUntil(() => Eatman.eatmanLoaded);
       // Debug.Log("Eatman loaded");
        yield return new WaitUntil(() => CreateZone.loadedCreateZoneCount == 6);
       // Debug.Log("CreateZones loaded");
        CreateZone.loadedCreateZoneCount = 0;
        LoadWaiter.SetActive(false);
        mainGameLoaded = true;
        Time.timeScale = 1;
     
    }

    IEnumerator GameOverWaiter()
    {
        yield return new WaitUntil(()=>Eatman.gameOver);
        MiniMenuB.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(OnGameOverPage());
        yield return new WaitForSecondsRealtime(1);
        admanager.ShowInterstitialAd();
        
    }


    IEnumerator LevelUpWaiter()
    {
        while(true)
        {
            yield return new WaitUntil(()=>levelUp);
            levelUp = false;
            levelUpPage.SetActive(true);
            yield return new WaitForSeconds(2);
            OffLevelPage();
        }
    }

    private void OffLevelPage()
    {
        levelUpPage.SetActive(false);
    }
 
    IEnumerator OnGameOverPage()
    {

        GameObject fuckingObject = GameObject.FindGameObjectWithTag("scorePlusText");
        if(fuckingObject!=null)
        {
            fuckingObject.SetActive(false);
        }
        gameOverPage.SetActive(true);
        scoreText.SetActive(false);
        gameOverSource.PlayOneShot(gameOverSound);

        yield return null;
    }

   

    //처음부터 시작
    public void Replay()
    {
       // Debug.Log("OnReplayButtonClicked");
        gameOverPage.SetActive(false);
        
        Eatman.RestartGame();
        scoreText.SetActive(true);
        MiniMenuB.SetActive(true);
        level = 1;
    }

    //메인 메뉴로 빠져나감
    public void GoToMenu()
    {
       // Debug.Log("GoToMenu");
        SceneManager.LoadScene("Menu");
    }
}
