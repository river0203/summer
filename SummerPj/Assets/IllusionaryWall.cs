using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionaryWall : MonoBehaviour
{
    public bool wallHasBeenHit;
    public Material illusionaryWallMaterial;
    public float alpha;
    public float fadeTimer = 2.5f;
    public BoxCollider wallCollider;

    public AudioSource _audioSource;
    public AudioClip _illusionaryWallSound;

    private void Update()
    {
        if(wallHasBeenHit)
        {
            FadeIllusionaryWall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            wallHasBeenHit = true;
        }
    }

    public void FadeIllusionaryWall()
    {
        alpha = illusionaryWallMaterial.color.a;
        alpha = alpha - Time.deltaTime / fadeTimer; 
        Color fadewallColor = new Color(1,1,1,alpha);
        illusionaryWallMaterial.color = fadewallColor;

        if(wallCollider.enabled)
        {
            wallCollider.enabled = false;
            _audioSource.PlayOneShot(_illusionaryWallSound);
        }

        if(alpha <= 0)
        {
            Destroy(this);
        }
    }
}
