using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterManager : MonoBehaviour
{
    public Button leftB;
    public Button rightB;
    public Sprite fish;
    public Sprite poo;
    public Sprite carrot;
    public Sprite bone;
    public Sprite meat;
    
    private List<GameObject> Characters;
    private int characterIndex = 0;
    
    private GameObject favoriteGameImage;
    private GameObject dislikeGameImage;
    public InputField userCharacterName;
    private void Start()
    {
         Characters = new List<GameObject>();
         foreach (Transform child in transform)
         {
             Characters.Add(child.gameObject);
         }
        string defaultCharacter = PlayerPrefs.GetString("Character", "kitty");

        foreach(GameObject ch in Characters)
        {
            ch.SetActive(false);
        }
        if (defaultCharacter == "kitty")
        {
            Characters[0].SetActive(true);
            characterIndex = 0;
        }
        else if (defaultCharacter == "piggy")
        {
            Characters[1].SetActive(true);
            characterIndex = 1;
        }
        else if (defaultCharacter == "puppy")
        {
            Characters[2].SetActive(true);
            characterIndex = 2;
        }
        else if (defaultCharacter == "rabbit")
        {
            Characters[3].SetActive(true);
            characterIndex = 3;
        }

        GameObject Panel = GameObject.Find("Panel");

        foreach (Transform child in Panel.transform)
        {
            GameObject go = child.gameObject;

            if (go.name == "favoriteImage")
            {
                favoriteGameImage = go;
            }
            else if (go.name == "dislikeImage")
            {
                dislikeGameImage = go;
            }
        }
        SetCharacter(defaultCharacter);

        StartCoroutine(LeftArrowActiveCheck());
        StartCoroutine(RightArrowActiveCheck());
    }
    private void SetCharacter(string character)
    {
        Image favImage = favoriteGameImage.GetComponent<Image>();
        Image dislikeImage = dislikeGameImage.GetComponent<Image>();

        switch (character)
        {
            case "kitty":
                favImage.sprite = fish;
                dislikeImage.sprite = poo;
                userCharacterName.text = PlayerPrefs.GetString("userKittyName", LocalizationManager.Instance.GetText("kitty"));
                break;

            case "piggy":
                favImage.sprite = poo;
                dislikeImage.sprite = meat;
                userCharacterName.text = PlayerPrefs.GetString("userPiggyName", LocalizationManager.Instance.GetText("piggy"));
                break;

            case "puppy":
                favImage.sprite = bone;
                dislikeImage.sprite = poo;
                userCharacterName.text = PlayerPrefs.GetString("userPuppyName", LocalizationManager.Instance.GetText("puppy"));
                break;

            case "rabbit":
                favImage.sprite = carrot;
                dislikeImage.sprite = poo;
                userCharacterName.text = PlayerPrefs.GetString("userRabbitName", LocalizationManager.Instance.GetText("rabbit"));
                break;

        }
    }

    IEnumerator LeftArrowActiveCheck()
    {
        while(true)
        {
            leftB.gameObject.SetActive(true);
            yield return new WaitUntil(()=>characterIndex == 0);
            leftB.gameObject.SetActive(false);
            yield return new WaitUntil(() => characterIndex != 0);
        }
    }
    IEnumerator RightArrowActiveCheck()
    {
        while (true)
        {
            rightB.gameObject.SetActive(true);
            yield return new WaitUntil(() => characterIndex == 3);
            rightB.gameObject.SetActive(false);
            yield return new WaitUntil(() => characterIndex != 3);
        }
    }

    public void OnLeftB()
    {
        SaveCharacterName();
        foreach (GameObject ch in Characters)
        {
            ch.SetActive(false);
        }
        if (characterIndex == 0)
        {
            return;
        }
        else
        {
            characterIndex--;
            Characters[characterIndex].SetActive(true);
            SetCharacter(Characters[characterIndex].name);
        };

     

    }
    public void OnRightB()
    {
        SaveCharacterName();
        foreach (GameObject ch in Characters)
        {
            ch.SetActive(false);
        }
        if (characterIndex == 3)
        {
            return;
        }
        else
        {
            characterIndex++;
            Characters[characterIndex].SetActive(true);
            SetCharacter(Characters[characterIndex].name);
        }

      
       
        
    }
    private void SaveCharacterName()
    {
        if (characterIndex == 0)
        {
            PlayerPrefs.SetString("userKittyName", userCharacterName.text);
        }
        else if (characterIndex == 1)
        {
            PlayerPrefs.SetString("userPiggyName", userCharacterName.text);
        }
        else if (characterIndex == 2)
        {
            PlayerPrefs.SetString("userPuppyName", userCharacterName.text);
        }
        else if (characterIndex == 3)
        {
            PlayerPrefs.SetString("userRabbitName", userCharacterName.text);
        }

        PlayerPrefs.SetString("Character", Characters[characterIndex].name);
        PlayerPrefs.Save();
    }

    public void OnSelectB()
    {
        SaveCharacterName();
        Eatman.currentCharacter = new Eatman.Character(Characters[characterIndex].name);

        Eatman.controlIndex = PlayerPrefs.GetInt("Control", 0);
       // Debug.Log("loadedControlIndex : " + PlayerPrefs.GetInt("Control", 0));
      //  Debug.Log("loadedControlIndex : " +Eatman.controlIndex);
        SceneManager.LoadScene("Game");
    }

    public void OnToMenuB()
    {
        SaveCharacterName();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }



}
