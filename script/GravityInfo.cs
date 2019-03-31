using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInfo : MonoBehaviour
{
    public static void SetGravity(float gravity)
    {
        Physics2D.gravity = new Vector2(0, -gravity);
    }
}
