using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpText : MonoBehaviour {
    public Text levelUpText;
    private void OnEnable()
    {
        levelUpText = GetComponent<Text>();
        if (GameManager.level != 10)
        {
            levelUpText.text = LocalizationManager.Instance.GetText("speedUp") + GameManager.level;
        }
        else if(GameManager.level == 10)
        {
            levelUpText.text = LocalizationManager.Instance.GetText("speedUp") + "Max";
        }
      
        InvokeRepeating("blinkling", 0.25f,0.25f);
        
    }

    private bool onOff = false;
    private void blinkling()
    {
        onOff = !onOff;
        if(onOff)
        {
            levelUpText.enabled = true;
        }
        else
        {
            levelUpText.enabled = false;
        }
    }

    private void OnDisable()
    {
        CancelInvoke("blinkling");
    }

}
