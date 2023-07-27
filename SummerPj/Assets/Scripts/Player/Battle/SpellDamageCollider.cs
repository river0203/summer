 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    public GameObject _impactParticles;
    public GameObject _projectileParticles;
    public GameObject _muzzleParticles;

    bool hasCollided = false;

    CharacterStatsManager _spellTarget;
    Rigidbody _rigid;

    Vector3 impactNormal;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _projectileParticles = Instantiate(_projectileParticles, transform.position, transform.rotation);
        _projectileParticles.transform.parent = transform;

        if(_muzzleParticles)
        {
            _muzzleParticles = Instantiate(_muzzleParticles, transform.position, transform.rotation);
            Destroy(_muzzleParticles, 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasCollided)
        {
            _spellTarget = collision.transform.GetComponent<CharacterStatsManager>();

            if(_spellTarget != null)
            {
                _spellTarget.TakeDamage(_currentWeaponDamage);
            }
            hasCollided = true;
            _impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

            Destroy(_projectileParticles);
            Destroy(_impactParticles, 5f);
            Destroy(gameObject, 5f);
        }
    }
}
