using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Layer
    { 
        Ground = 3,
        Enemy = 6,
        LockOn_Layer = 7,
        Player = 8,
    }

    public enum CameraType
    {
        Normal,
        LockOn
    }
}
