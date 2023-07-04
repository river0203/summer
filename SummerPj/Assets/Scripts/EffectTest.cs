using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _effect;
    public void StartEffect()
    {
        _effect.gameObject.SetActive(true);
        _effect.Play();
    }
}
