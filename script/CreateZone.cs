using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateZone : MonoBehaviour {

    [System.Serializable]
    public struct CreateInterval
    {
        public float min;
        public float max;
    }
    public CreateInterval createInterval;

    private float createIntervalTime;
    private int gameObjectIndex;
    public GameObject [] gameobjects;
    public static int loadedCreateZoneCount = 0;
    public AudioClip levelUpAudio;
    public AudioSource levelUpSource;


    private struct ProbFav
    {
        public float prob;
        public int gameObjectIndex;
    }
    private struct ProbGameOver
    {
        public float prob;
        public int gameObjectIndex;
    }
    private struct ProbNotScore
    {
        public float prob;
        public int gameObjectIndex;
    }
    private struct ProbScore
    {
        public float prob;
        public int []gameObjectIndex;
    }
    ProbFav probFav; ProbGameOver probGameOver; ProbNotScore probNotScore; ProbScore probScore;


    void Start ()
    {
        createIntervalTime = Random.Range(createInterval.min, createInterval.max);
        string favoriteFood = Eatman.currentCharacter.favoriteFood;
        string hateFood = Eatman.currentCharacter.hateFood;
        //Debug.Log("favoriteFood = " + favoriteFood);

        probScore.gameObjectIndex = new int[4];
        int k = 0; 
        for (int i = 0; i<gameobjects.Length; i++)
        {
            if (gameobjects[i].name.Equals(favoriteFood))
            {
                gameobjects[i].tag = "favScoreObject";
                probFav.gameObjectIndex = i;
            }
            else if (gameobjects[i].name.Equals("bomb"))
            {
                gameobjects[i].tag = "gameOverObject";
                //gameobjects[i].tag = "favScoreObject";
                probGameOver.gameObjectIndex = i;

            }
            else if(gameobjects[i].name.Equals(hateFood))
            {
                gameobjects[i].tag = "notScoreObject";
                //gameobjects[i].tag = "favScoreObject";
                probNotScore.gameObjectIndex = i; 
            }
            else
            {
                gameobjects[i].tag = "scoreObject";
                //gameobjects[i].tag = "favScoreObject";
                probScore.gameObjectIndex[k] = i;
                k++;
            }

           
        }
        StartCoroutine(LevelManager());
        loadedCreateZoneCount++;
        Invoke("createScoreObject", createIntervalTime);
    }

    private int ChooseProb (float[] probs)
    {

        float total = 0;

        foreach (float elem in probs) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
   
    void createScoreObject()
    {
        if (Eatman.gameOver) return;

        float[] probs = new float[] {probFav.prob, probGameOver.prob, probNotScore.prob, probScore.prob};
        int choosedProbIndex = ChooseProb(probs);

        switch(choosedProbIndex)
        {
            case 0:
                //Debug.Log("favorite");
                gameObjectIndex = probFav.gameObjectIndex;
                break;
            case 1:
                //Debug.Log("gameover");
                gameObjectIndex = probGameOver.gameObjectIndex;
                break;
            case 2:
                //Debug.Log("notscore");
                gameObjectIndex = probNotScore.gameObjectIndex;
                break;
            case 3:
                //Debug.Log("score");
              if(probScore.gameObjectIndex!=null)
              { 
                    
                    gameObjectIndex = probScore.gameObjectIndex[(int)(Random.value * 3.40f)];
              }
              break;
        }
        Instantiate(gameobjects[gameObjectIndex], transform.position, Quaternion.identity);
        CancelInvoke();
        createIntervalTime = Random.Range(createInterval.min, createInterval.max);
        InvokeRepeating("createScoreObject", createIntervalTime, createIntervalTime);
    }


    float time;


    IEnumerator LevelManager()
    {
        Pattern1();
        yield return new WaitUntil(() => Eatman.score >= 100);
        levelUpSource.Play();
        Pattern2();
        yield return new WaitUntil(() => Eatman.score >= 200);
        levelUpSource.Play();
        Pattern3();
        yield return new WaitUntil(() => Eatman.score >= 350);
        levelUpSource.Play();
        Pattern4();
        yield return new WaitUntil(() => Eatman.score >= 500);
        levelUpSource.Play();
        Pattern5();
        yield return new WaitUntil(() => Eatman.score >= 1000);
        levelUpSource.Play();
        Pattern6();
        yield return new WaitUntil(() => Eatman.score >= 2000);
        levelUpSource.Play();
        Pattern7();
        yield return new WaitUntil(() => Eatman.score >= 3000);
        levelUpSource.Play();
        Pattern8();
        yield return new WaitUntil(() => Eatman.score >= 5000);
        levelUpSource.Play();
        Pattern9();
        yield return new WaitUntil(() => Eatman.score >= 7000);
        levelUpSource.Play();
        Pattern10();
    }

   
    private void SetProbs(float fav, float gameOver, float notScore, float score)
    {
        probFav.prob = fav; probGameOver.prob = gameOver;
        probNotScore.prob = notScore; probScore.prob = score;

    }

    void Pattern1()
    {
        //Debug.Log("pattern1");
        GameManager.level = 1;
        GameManager.levelUp = false;
        GravityInfo.SetGravity(1.8f);
        createInterval.max = 5.1f;
        SetProbs(40, 5, 10, 45);
        Eatman.speed = 10;
    }
    void Pattern2()
    {
        GameManager.level = 2;
        GameManager.levelUp = true;
        //Debug.Log("pattern2");
        GravityInfo.SetGravity(2.5f);
        createInterval.max = 4.5f;
        SetProbs(35, 8, 17, 40);
        Eatman.speed = 11;
    }
    void Pattern3()
    {
        GameManager.level = 3;
        GameManager.levelUp = true;
       
        //Debug.Log("pattern3");
        GravityInfo.SetGravity(3.0f);
        createInterval.max = 4.2f;
        SetProbs(35, 15, 15, 35);
        Eatman.speed = 12;
    }
    void Pattern4()
    {
        GameManager.level = 4;
        GameManager.levelUp = true;
        
        // Debug.Log("pattern4");
        GravityInfo.SetGravity(4.5f);
        createInterval.max = 4.0f;
        SetProbs(30, 18, 15, 37);
        Eatman.speed = 13;
    }
    void Pattern5()
    {
        GameManager.level = 5;
        GameManager.levelUp = true;
       
        // Debug.Log("pattern5");
        createInterval.max = 3.7f;
        GravityInfo.SetGravity(5.5f);
        SetProbs(30, 25, 20, 25);
        Eatman.speed = 14;
    }
    void Pattern6()
    {
        GameManager.level = 6;
        GameManager.levelUp = true;
       
        // Debug.Log("pattern6");
        createInterval.max =3.5f;
        GravityInfo.SetGravity(6.5f);
        SetProbs(25, 30, 20, 25);
        Eatman.speed = 15;
    }
    void Pattern7()
    {
        GameManager.level = 7;
        GameManager.levelUp = true;
      
        // Debug.Log("pattern7");
        createInterval.max = 3.3f;
        GravityInfo.SetGravity(7.5f);
        SetProbs(25, 40, 20, 15);
        Eatman.speed = 16;
    }
    void Pattern8()
    {
        GameManager.level = 8;
        GameManager.levelUp = true;

        // Debug.Log("pattern7");
        createInterval.max = 3.1f;
        GravityInfo.SetGravity(8.5f);
        SetProbs(20, 40, 20, 20);
        Eatman.speed = 17;
    }
    void Pattern9()
    {
        GameManager.level = 9;
        GameManager.levelUp = true;

        // Debug.Log("pattern7");
        createInterval.max = 3.0f;
        GravityInfo.SetGravity(9.5f);
        SetProbs(40, 40, 10, 10);
        Eatman.speed =18f;
    }
    void Pattern10()
    {
        GameManager.level = 10;
        GameManager.levelUp = true;

        // Debug.Log("pattern7");
        createInterval.max = 2.5f;
        GravityInfo.SetGravity(15.5f);
        SetProbs(40, 40,0, 20);
        Eatman.speed =19f;
    }

}
