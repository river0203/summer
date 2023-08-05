using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStart : MonoBehaviour
{
    ParticleSystem _particle;

    void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _particle.Play();
    }
}
