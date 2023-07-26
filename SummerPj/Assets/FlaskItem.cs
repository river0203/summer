using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskItem : MonoBehaviour
{
    [Header("Flask Type")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public int healthRecoverAmount;
    public int focusPointsRecoverAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;
}
