using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [HideInInspector] public int healthLevel = 10;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    [HideInInspector] public int staminaLevel = 10;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float currentStamina;

    [HideInInspector] public int focusLevel = 10;
    [HideInInspector] public float maxFocusPoints;
    [HideInInspector] public float currentFocusPoints;

    [HideInInspector] public bool isDead;
}
