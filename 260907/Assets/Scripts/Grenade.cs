using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private int _damage;

    [SerializeField] private float _power;
    private Transform _startTr;

    [SerializeField] private float _boomTime;
    [SerializeField] private GameObject _boomEffect;

    private Rigidbody _rigidbody;

    [SerializeField] private LayerMask _targetDamageMask;

    private void Awake() => CacheComponents();

    private void Update()
    {
        UpdateThrow();
    }
    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void UpdateThrow()
    {
    }

    public void Throw(Transform tr, float power)
    {
        _startTr = tr;
        _power = power;
        _rigidbody.AddForce(_startTr.forward * _power + Vector3.up * _power, ForceMode.Impulse);

        Destroy(gameObject, _boomTime);
    }

    public void OnDestroy()
    {
        Instantiate(_boomEffect, transform.position, Quaternion.identity);

        Collider[] col = Physics.OverlapSphere(transform.position, 3f, _targetDamageMask);

        for(int i = 0; i < col.Length; i++)
        {
            if (col[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
    }
}