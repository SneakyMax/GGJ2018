using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub : MonoBehaviour
{
    public PlayerCam Cam;

    /// <summary>
    /// 0-indexed
    /// </summary>
    public int Player;

    public string Input
    {
        get { return "P" + (Player + 1); }
    }
}
