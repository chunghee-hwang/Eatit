using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GoogleGameManager : MonoBehaviour
{
    public GameObject LeaderboardWaiter;
    public Text highScoreText;
    public Text debugText;
    public GameObject GPGSWaiter;
    private static bool leaderboardViewed = false;
    public void Awake()
    {
       if(SceneManager.GetActiveScene().name == "Character Select")
        {
            StartCoroutine(checkInternetConnection());
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            StartCoroutine(GameOverWaiter2());
          //  Debug.Log("Game scene에서 게임 오버기다림");
        }

    }
 
    IEnumerator checkInternetConnection()
    {
            WWW www = new WWW("http://google.com");
            yield return www;
            if (www.error != null)
            {
                //false
            }
            else
            {
                //true
                StartCoroutine(GPGSWait());
                StartCoroutine(Init());
            }
        }

        IEnumerator GPGSWait()
    {
        GPGSWaiter.SetActive(true);
        yield return new WaitUntil(()=> signinEnd&& getHighScoreEnd);
        GPGSWaiter.SetActive(false);

    }

    IEnumerator Init()
    {
#if UNITY_ANDROID
        //debugText.text = "초기 동작";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
#endif

        if (SceneManager.GetActiveScene().name == "Character Select")
            yield return StartCoroutine(Signin());
    }

    private bool signinEnd = false;

    IEnumerator Signin()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                // to do ...
                // 로그인 성공 처리
                //debugText.text = "로그인 성공";

                
                if (SceneManager.GetActiveScene().name == "Character Select")
                {
                    StartCoroutine(GetHighScore());
                }
                else if (SceneManager.GetActiveScene().name == "Game")
                {
                    StartCoroutine(ReportScore(PlayerPrefs.GetInt("HighScore", 0))); 
                }
                signinEnd = true;
            }
            else
            {
                // debugText.text = "로그인 실패";
                // to do ...
                // 로그인 실패 처리
                signinEnd = true;
                getHighScoreEnd = true;
            }
        });

        yield return null;

    }
    bool getHighScoreEnd = false;
    IEnumerator GetHighScore()
    {
        PlayGamesPlatform.Instance.LoadScores
            (
            GPGSIds.leaderboard_high_score,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
        (LeaderboardScoreData data) =>
        {
            if (data.Valid)
            {
                PlayerPrefs.SetInt("HighScore", (int)data.PlayerScore.value);
                PlayerPrefs.Save();
            }
            getHighScoreEnd = true;
        });

        yield return null;
    }

    IEnumerator ReportScore(int score)
    {
        //debugText.text += "점수 제출 시도\n";
        PlayGamesPlatform.Instance.ReportScore((long)score, GPGSIds.leaderboard_high_score, (bool success) =>
        {
            if (success)
            {
               // Debug.Log("점수 보고 성공!");
                // Report 성공
                //debugText.text += "점수 제출 성공\n";
                // 그에 따른 처리   
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score,(UIStatus ui)=> { leaderboardViewed = true; });
            }
            else
            {
                //debugText.text += "점수 제출 실패\n";
                // 그에 따른 처리   
               // Debug.Log("점수 보고 실패!");
                leaderboardViewed = true;
            }
        });

        yield return null;
    }

    IEnumerator ShowLeaderboardUI()
    {
      //  Debug.Log("리더보드 접근중...");
        if(SceneManager.GetActiveScene().name == "Game")
        {
            StartCoroutine(Init());
        }
        if(Social.localUser.authenticated == false)
        {
         //   Debug.Log("로그인이 안되어있음...");
            Social.localUser.Authenticate((bool success) =>
            {
                if(success)
                {
               //     Debug.Log("로그인 재시도 성공!");
                    StartCoroutine(ReportScore(PlayerPrefs.GetInt("HighScore", 0)));
                    
                    leaderboardViewed = true;
                }
                else
                {
                    //로그인 실패
                 //   Debug.Log("로그인 재시도 실패!");
                    leaderboardViewed = true;
                }
            });
            
        }
        //Social.ShowLeaderboardUI();
        else
        {
           // Debug.Log("로그인이 되어있었음...");
            StartCoroutine(ReportScore(PlayerPrefs.GetInt("HighScore", 0)));
        }

       
     
        yield return null;
    }

    IEnumerator GameOverWaiter2()
    {
        yield return new WaitUntil(() => Eatman.gameOver);
      
        yield return new WaitForSecondsRealtime(2);
       
            PlayGamesPlatform.Instance.ReportScore(PlayerPrefs.GetInt("HighScore",0), GPGSIds.leaderboard_high_score, (success) =>
            {
                if (success)
                {
                   // Debug.Log("자동 점수 제출 성공 : " + PlayerPrefs.GetInt("HighScore", 0));
                }
                else
                {
                   // Debug.Log("자동 점수 제출 실패");
                }
            });
    
        
        yield return new WaitForSecondsRealtime(1);
     

    }

    public void OnLeaderBoardB()
    {
        try
        { 
            leaderboardViewed = false;
            StartCoroutine(LeaderboardWait());
            StartCoroutine(ShowLeaderboardUI());
        }
        catch(Exception e)
        {
            leaderboardViewed = true;
        }
    }

    IEnumerator LeaderboardWait()
    {
        LeaderboardWaiter.SetActive(true);
        yield return new WaitUntil(()=> leaderboardViewed);
        LeaderboardWaiter.SetActive(false);
    }

}
   



