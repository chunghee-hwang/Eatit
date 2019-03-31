using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Eatman : MonoBehaviour
{
    public Text scoreText;
    //public Text debugText;
    public GameObject kittyTear, piggyTear, puppyTear, rabbitTear, GameOverPage, minimenuB,minimenu,highScoreText, gameOverPage;
    public static float speed;

    public static bool gameOver;
    public static int score=0;
    private Rigidbody2D rigidBody;

    private bool eating;
    private bool crying;
    private Animator animator;
    public static bool eatmanLoaded = false;
    public static Character currentCharacter = new Character("kitty");

    private List<GameObject> Characters;
    
    public static int controlIndex;
    public AudioSource appleSource, pooSource, bombSource, boneSource, carrotSource, meatSource, fishSource, notScoreSource, favObjectSource, twentyComboSource;
    private AudioSource[] audioSources;

    public Font arcade;
    public class Character
    {
        public string name { get; set; }

        public string favoriteFood
        {
            get;set;
        }

        public string hateFood
        {
            get;set;
        }

        public Character(string name)
        {
            this.name = name;
            switch(name)
            {
                case "kitty":
                    favoriteFood = "fish";
                    hateFood = "poo";
                    break;
                case "piggy":
                    favoriteFood = "poo";
                    hateFood = "meat";
                    break;
                case "puppy":
                    favoriteFood = "bone";
                    hateFood = "poo";
                    break;
                case "rabbit":
                    favoriteFood = "carrot";
                    hateFood = "poo";
                    break;
            }
        }
    }
    
    

    void Start ()
    {
        StartCoroutine(Init());

    }
    IEnumerator Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        GameObject Chs = GameObject.Find("Characters");
        Characters = new List<GameObject>();
        foreach (Transform child in Chs.transform)
        {
            Characters.Add(child.gameObject);
            // Debug.Log("child.gameObject:" + child.gameObject);
        }
        foreach (GameObject ch in Characters)
        {
            ch.SetActive(false);

        }
        if (currentCharacter.name == "kitty")
        {
            Characters[0].SetActive(true);
        }
        else if (currentCharacter.name == "piggy")
        {
            Characters[1].SetActive(true);
        }
        else if (currentCharacter.name == "puppy")
        {
            Characters[2].SetActive(true);
        }
        else if (currentCharacter.name == "rabbit")
        {
            Characters[3].SetActive(true);
        }

        score = 0;
        speed = 10;
        gameOver = false;


        eating = false;
        crying = false;
        animator = GetComponent<Animator>();
        controlIndex = PlayerPrefs.GetInt("Control", 0);
        audioSources = GameObject.Find("sounds").GetComponentsInChildren<AudioSource>();

        appleSource = audioSources[0]; pooSource = audioSources[1]; bombSource = audioSources[2];
        boneSource = audioSources[3]; carrotSource = audioSources[4]; meatSource = audioSources[5]; fishSource = audioSources[6];

     

        eatmanLoaded = true;
        yield return new WaitForSecondsRealtime(2);
    }
 

    private void OnMouseDrag()
    {
        if (controlIndex == 2 && !gameOver && !minimenu.activeSelf)
        {

            Vector3 point = Camera.main.ScreenToWorldPoint(
                      new Vector3(
                          Input.mousePosition.x ,
                          (transform.position.y - Camera.main.transform.position.y),
                          (transform.position.z - Camera.main.transform.position.z)));

            point.y = transform.position.y;
            point.z = transform.position.z;
            transform.position = point;

        }
        else return;

        
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!gameOverPage.activeSelf)
        {
            Time.timeScale = 0;
            minimenu.SetActive(true);
        }
     
    }


    void Update ()
    {

        // 화면 밖으로 못나가게 막음
        if (transform.localPosition.x < -1.96f)
        {
            transform.localPosition = new Vector3(-1.960f, transform.localPosition.y);
        }
        else if (transform.localPosition.x > 3.3f)
        {
            transform.localPosition = new Vector3(3.3f, transform.localPosition.y);
        }
   
        

        if (gameOver) { rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);  return; }

        //debugText.text=  rigidBody.velocity.ToString();
        if(controlIndex == 0)
        { 
            
                if(Input.touchCount==1)
                { 
                        if (Input.GetTouch(0).position.x < Screen.width / 2)
                        {
                            //transform.position += -transform.right * speed/2f * Time.deltaTime;
                            rigidBody.velocity = new Vector2(-speed/2, rigidBody.velocity.y);
                        }
                        else if (Input.GetTouch(0).position.x > Screen.width / 2)
                        {
                            //transform.position += transform.right * speed/2f * Time.deltaTime;
                            rigidBody.velocity = new Vector2(speed/2, rigidBody.velocity.y);
                        }
                    
                }
                else
                {
                    rigidBody.velocity = new Vector2(0, 0);
                }
        }
        else if(controlIndex == 1)
        { 
            float x = Input.acceleration.x;
            rigidBody.velocity = new Vector2(x * speed*1.1f, 0);
        }

        if(rectTrans!= null)
        {
            if (deltaScore > 0)
            {
                rectTrans.position = new Vector2(rectTrans.position.x, rectTrans.position.y + 1);
            }
            else
            {
                rectTrans.position = new Vector2(rectTrans.position.x, rectTrans.position.y - 1);
            }
            Invoke("disableScorePlusText", 1f);
        }
    }
    private bool disableOnce = false;
    private void disableScorePlusText()
    {
        if (disableOnce) { return; }
        if(scorePlusGo!=null)
        {
           Text t =  scorePlusGo.GetComponent<Text>();
            if (t != null)
            {
                t.enabled = false;
                disableOnce = true;
                Invoke("disableOnceFalse", 0.1f);
            }
        }
    }
    private void disableOnceFalse()
    {
        disableOnce = false;
    }
    private void enableScorePlusText()
    {
       // Debug.Log("enableComboText");
        if (scorePlusGo != null)
        {
            Text[] t = scorePlusGo.GetComponents<Text>();
            foreach (Text text in t)
            {
                text.enabled = true;
              //  Debug.Log(text);
            }
        }
        
    }



    private GameObject lastGameObject = null;
    private int combo;

    GameObject scorePlusGo;
    Text scorePlusText;
    RectTransform rectTrans;
    int deltaScore = 0;
   

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameOver) return;

        PlayEatingSound(col.gameObject);
        
        if (lastGameObject != null)
        {
            if (lastGameObject == col.gameObject)
                return;
        }
        if (col.gameObject.tag != "favScoreObject")
        {
            if (scorePlusGo != null)
            {
                Text[] texts = scorePlusGo.GetComponents<Text>();
                foreach (Text t in texts)
                {
                    t.text = "";
                }
            }
        }

        if (col.gameObject.tag == "scoreObject")
        {
            combo = 0;
            eating = true;
            crying = false;
            InvisibleTear();
            animator.SetBool("Eating", eating);
            animator.SetBool("Crying", crying);
            Invoke("ToIdle", 0.1f);
         
            
            Destroy(col.gameObject, 0.2f);
            score++;
            scoreText.text = score.ToString();


        }
        else if (col.gameObject.tag == "favScoreObject")
        {
           
            
            eating = true;
            crying = false;
            InvisibleTear();
            animator.SetBool("Eating", eating);
            animator.SetBool("Crying", crying);
            Invoke("ToIdle", 0.1f);

            combo++;
            Destroy(col.gameObject, 0.2f);
      
            score += 10 +(combo-1);
            if(combo%10 == 0)
            {
                score += 100;
                
                if (twentyComboSource != null)
                {
                    if (favObjectSource.isPlaying) { favObjectSource.Stop(); }
                    if (!twentyComboSource.isPlaying)
                        twentyComboSource.Play();
                }
            }
            scoreText.text = score.ToString();

        }
        else if (col.gameObject.tag == "notScoreObject")
        {
            combo = 0;
            eating = false;
            crying = true;
            VisibleTear();
            Invoke("InvisibleTear", 1f);
            animator.SetBool("Eating", eating);
            animator.SetBool("Crying", crying);
            Invoke("ToIdle", 0.1f);
           
            Destroy(col.gameObject, 0.2f);
            if (score > 20)
            {
                score -= 20;
                scoreText.text = score.ToString();
            }

        }
        else if (col.gameObject.tag == "gameOverObject")
        {
            if (PlayerPrefs.GetInt("HighScore", 0) < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
                PlayerPrefs.Save();
                // highScoreText.GetComponent<Text>().text = LocalizationManager.Instance.GetText("highScoreIs") + score;
            }

            combo = 0;
            //터지는 애니메이션 구현
            Invoke("VisibleTear", 1.5f);
            gameOver = true;
            animator.SetBool("Eating", false);
            animator.SetBool("Crying", false);
            animator.SetBool("GameOver", gameOver);
            Destroy(col.gameObject, 0.2f);
            disableScorePlusText();
            if (scorePlusGo != null)
            {
                Destroy(scorePlusGo);
            }
           
        }
        CancelInvoke("disableScorePlusText");
    
        enableScorePlusText();
        showScorePlusText(col);
        lastGameObject = col.gameObject;
    }
    private void PlayEatingSound(GameObject go)
    {
        string name = go.name;
       // Debug.Log("name : " + go);
        if (name == "apple(Clone)")
        {
            if(appleSource!=null)
            appleSource.Play();
        }
        else if (name == "poo(Clone)")
        {
            if (pooSource != null)
                pooSource.Play();
        }
        else if (name == "bomb(Clone)")
        {
            if (bombSource != null && !bombSource.isPlaying)
                bombSource.Play();
        }
        else if (name == "bone(Clone)")
        {
            if (boneSource != null)
                boneSource.Play();
        }
        else if (name == "carrot(Clone)")
        {
            if (carrotSource != null)
                carrotSource.Play();
        }
        else if (name == "meat(Clone)")
        {
            if (meatSource != null)
                meatSource.Play();
        }
        else if (name == "carrot(Clone)")
        {
            if (carrotSource != null)
                carrotSource.Play();
        }
        else if(name == "fish(Clone)")
        {
            if (fishSource != null)
                fishSource.Play();
        }

        if(go.tag == "favScoreObject")
        {
            if (favObjectSource != null)
            {
                if(!favObjectSource.isPlaying)
                    favObjectSource.Play();
            }
        }
        else if(go.tag == "notScoreObject")
        {
            if(notScoreSource != null)
            {
                if (!notScoreSource.isPlaying)
                    notScoreSource.Play();
            }
        }
    }

    private void showScorePlusText(Collider2D col)
    {
        if (combo >= 2)
        {
            if (combo % 10 == 0)
            {
                deltaScore += 10 + (combo - 1) + 100;
            }
            else
            {
                deltaScore = 10 + (combo-1);
            }
        }
        else
        {
            if(col.gameObject.tag == "favScoreObject")
            {
                deltaScore = 10;
            }
            else if(col.gameObject.tag == "notScoreObject")
            {
                if (score > 20)
                {
                    deltaScore = -20;
                }
                else
                {
                    deltaScore = 0;
                }
            }
            else if(col.gameObject.tag == "scoreObject")
            {
                deltaScore = 1;
            }
            }
            if(scorePlusGo == null)
        {
            scorePlusGo = new GameObject("scorePlusText");
            scorePlusGo.tag = "scorePlusText";
            scorePlusGo.transform.SetParent(GameObject.Find("Canvas").transform);
            rectTrans = scorePlusGo.AddComponent<RectTransform>();
            scorePlusGo.AddComponent<ContentSizeFitter>();
        }
            Vector2 pos = col.gameObject.transform.position;
            Vector2 posWorld = Camera.main.WorldToScreenPoint(pos);
            rectTrans.anchorMin = posWorld;
            rectTrans.anchorMax = posWorld;
            rectTrans.sizeDelta = new Vector2(200f * (Screen.dpi / 70f), 200f * (Screen.dpi / 70f));
            //  rectTrans.position = viewportPoint;
            if (scorePlusText == null)
            {
                scorePlusText = scorePlusGo.AddComponent<Text>();
            
            }
            if(combo <2)
            {
                if (deltaScore != 0)
                {
                    
                    scorePlusText.text = deltaScore > 0 ? "+" + deltaScore : "-" + deltaScore;
                    scorePlusText.color = deltaScore > 0 ? Color.black : Color.red;
                    scorePlusText.fontSize = 23* (int)(Screen.dpi/70f);
                }
            }
            else if(combo>=2)
            { 
                scorePlusText.text = combo + "Combo !\n+" + deltaScore;
                scorePlusText.color = Color.blue;
                if (combo % 10 == 0)
                {
                    scorePlusText.fontSize = 23*(int)(Screen.dpi /70f);
                    scorePlusText.color = new Color(75, 0, 130);
                }
                else { scorePlusText.fontSize = 23 * (int)(Screen.dpi / 70f); }

            }
            scorePlusText.font = arcade;
            scorePlusText.fontStyle = FontStyle.Bold;
            
            scorePlusText.alignment = TextAnchor.MiddleCenter;


            rectTrans.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        }
        
    

    private void InvisibleTear()
    {
        
        if (currentCharacter.name == "kitty")
        {
            kittyTear.SetActive(false);
        }
        else if (currentCharacter.name == "puppy")
        {
            puppyTear.SetActive(false);
        }
        else if (currentCharacter.name == "rabbit")
        {
            rabbitTear.SetActive(false);
        }
        else if (currentCharacter.name == "piggy")
        {
            piggyTear.SetActive(false);
        }
    }
    private void VisibleTear()
    {
        if (currentCharacter.name == "kitty")
        {
            kittyTear.SetActive(true);
        }
        else if (currentCharacter.name == "puppy")
        {
            puppyTear.SetActive(true);
        }
        else if (currentCharacter.name == "rabbit")
        {
            rabbitTear.SetActive(true);
        }
        else if (currentCharacter.name == "piggy")
        {
            piggyTear.SetActive(true);
        }
    }

    private void ToIdle()
    {
        eating = false;
        crying = false;
        animator.SetBool("Eating", eating);
        animator.SetBool("Crying", crying);
    }

    //private void MyLog(string text)
    //{
    //    debugText.text = text;
    //}

    public static void RestartGame()
    {
        score = 0;
        eatmanLoaded = false;
        SceneManager.LoadScene("Game");
    }

}
