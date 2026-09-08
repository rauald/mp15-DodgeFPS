using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private int _damage;

    [SerializeField] private float _power;
    private Transform _startTr;

    private Rigidbody _rigidbody;

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

    }
}