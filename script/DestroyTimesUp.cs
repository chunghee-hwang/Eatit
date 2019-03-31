using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimesUp : MonoBehaviour
{
    private float startTime;
    private void Start()
    {
        startTime = Time.time;
        StartCoroutine(DestroyIfTimesUp());
    }
    IEnumerator DestroyIfTimesUp()
    {
        yield return new WaitUntil(() => Time.time - startTime > 10);
        if (gameObject.name.Contains("Clone"))
        {
            Destroy(gameObject);
        }
    }
}
