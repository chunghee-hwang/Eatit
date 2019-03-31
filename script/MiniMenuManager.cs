using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MiniMenuManager : MonoBehaviour
{
    public GameObject leftB;
    public GameObject rightB;
    public Sprite touch;
    public Sprite drag;
    public Sprite tilt;
    public Sprite musicOn;
    public Sprite musicOff;

    private int musicOnOff;
    GameObject scorePlusGo;
    private void OnEnable()
    {
        StartCoroutine(CheckGameOver());
        Time.timeScale = 0f;

        scorePlusGo = GameObject.FindGameObjectWithTag("scorePlusText");
        if(scorePlusGo != null)
        {
            Text t = scorePlusGo.GetComponent<Text>();
           
            if(t!=null)
            {
               // Debug.Log(t.text);
                t.enabled = false;
            }
        }

        musicOnOff = PlayerPrefs.GetInt("musicOnOff", 1);
        Image img = GameObject.Find("musicToggleB").GetComponent<Image>();

        if (musicOnOff == 1)
        {
            img.sprite = musicOn;
        }
        else
        {
            img.sprite = musicOff;
        }

        
        SetControl();


        if (Eatman.controlIndex == 0)
        {
            leftB.SetActive(false);
        }
        else
        {
            leftB.SetActive(true);
        }
        if (Eatman.controlIndex == 2)
        {
            rightB.SetActive(false);
        }
        else
        {
            rightB.SetActive(true);
        }

    }

    IEnumerator CheckGameOver()
    {
        yield return new WaitUntil(() => Eatman.gameOver);
        gameObject.SetActive(false);
    }

   

    private void OnDisable()
    {
        
        scorePlusGo = GameObject.FindGameObjectWithTag("scorePlusText");
        if (scorePlusGo != null)
        {
            Text t = scorePlusGo.GetComponent<Text>();

            if (t != null)
            {
             //   Debug.Log(t.text);
                t.enabled = false;
            }
        }
    }

    public void OnMiniMenuB()
    {
        gameObject.SetActive(true);
        //게임 일시 정지
    }


    public void OnMusicOnOff()
    {
        Image img = GameObject.Find("musicToggleB").GetComponent<Image>();

        if (musicOnOff == 1)
        {
            musicOnOff = 0;
            img.sprite = musicOff;

            GameObject.Find("backgroundMusic").GetComponent<AudioSource>().Stop();
        }
        else
        {
            musicOnOff = 1;
            img.sprite = musicOn;
            GameObject.Find("backgroundMusic").GetComponent<AudioSource>().Play();
        }
        PlayerPrefs.SetInt("musicOnOff", musicOnOff);
        PlayerPrefs.Save();
    }

    public void OnLeftB()
    {
        if(Eatman.controlIndex!=0)
        {
            Eatman.controlIndex--;
            SetControl();

            if (Eatman.controlIndex == 0)
            {
                leftB.SetActive(false);
            }
            else
            {
                leftB.SetActive(true);
            }
            if (Eatman.controlIndex == 2)
            {
                rightB.SetActive(false);
            }
            else
            {
                rightB.SetActive(true);
            }

            PlayerPrefs.SetInt("Control", Eatman.controlIndex);
            PlayerPrefs.Save();
        }
    }
    public void OnRightB()
    {
        if (Eatman.controlIndex != 2)
        {
            Eatman.controlIndex++;
            SetControl();
            
            if (Eatman.controlIndex == 0)
            {
                leftB.SetActive(false);
            }
            else
            {
                leftB.SetActive(true);
            }
            if (Eatman.controlIndex == 2)
            {
                rightB.SetActive(false);
            }
            else
            {
                rightB.SetActive(true);
            }
        }
        PlayerPrefs.SetInt("Control", Eatman.controlIndex);
        PlayerPrefs.Save();

    }

    private void SetControl()
    {
        //Debug.Log("SetControl: " + Eatman.controlIndex);
        GameObject controlImage = GameObject.Find("controlImage");
        Image img = controlImage.GetComponent<Image>();
        if (Eatman.controlIndex == 0)
        {
            img.sprite = touch;

        }
        if (Eatman.controlIndex == 1)
        {
            img.sprite = tilt;
        }
        else if (Eatman.controlIndex == 2)
        {
            img.sprite = drag;
        }
    }
    public void OnToMainMenuB()
    {
        PlayerPrefs.SetInt("Control", Eatman.controlIndex);
        PlayerPrefs.Save();
        //Debug.Log("ControlIndex saved : " + Eatman.controlIndex);
        //GameObject.Find("backgroundMusic").GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("Menu");
    }

    public void OnExitMiniMenuB()
    {
        PlayerPrefs.SetInt("Control", Eatman.controlIndex);
        PlayerPrefs.Save();
       // Debug.Log("ControlIndex saved : " + Eatman.controlIndex);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }


}
