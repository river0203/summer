using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    AudioSource _audioSource;

    [SerializeField] AudioClip _bossWhooshSound;
    [SerializeField] AudioClip _bossWalkSound;
    [SerializeField] AudioClip _bossHitSound;
    [SerializeField] AudioClip _bossDeadSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayBossWhoosh()
    {
        _audioSource.PlayOneShot(_bossWhooshSound);
    }

    public void PlayBossHitSound()
    {
        _audioSource.PlayOneShot(_bossHitSound);
    }

    public void PlayBossDeadSound()
    {
        _audioSource.PlayOneShot(_bossDeadSound);
    }

    public void BossWalkingOnAnimationEvent()
    {
        _audioSource.PlayOneShot(_bossWalkSound);
    }
}
