using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource bgm;

    private void Start()
    {
        StartCoroutine(CheckGameOver());
    }

    IEnumerator CheckGameOver()
    {
        yield return new WaitUntil(() => GameManager.mainGameLoaded);
        GameManager.mainGameLoaded = false;
        while (true)
        {   
            yield return new WaitUntil(() => PlayerPrefs.GetInt("musicOnOff", 1) == 1);
            if (!bgm.isPlaying)
            {
                bgm.Play();
            }
            yield return new WaitUntil(() => Eatman.gameOver);
            if (bgm.isPlaying)
            { bgm.Stop(); }
            yield return new WaitUntil(() => !Eatman.gameOver);
        }

    }

    private void OnDestroy()
    {
        StopCoroutine((CheckGameOver()));
    }
}
