using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScoreObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "scoreObject" || collision.gameObject.tag =="notScoreObject"
            ||collision.gameObject.tag == "gameOverObject" || collision.gameObject.tag == "favScoreObject")
        {
            Destroy(collision.gameObject);
        }
    }
   
}
