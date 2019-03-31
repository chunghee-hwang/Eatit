using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationUIText : MonoBehaviour
{
    public string key;

    void Start()
    {
        GetComponent<Text>().text = LocalizationManager.Instance.GetText(key);
    }


}