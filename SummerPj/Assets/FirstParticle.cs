using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstParticle : MonoBehaviour
{
    ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _particle.Play();
    }
}

