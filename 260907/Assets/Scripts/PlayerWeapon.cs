using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Transform _cameraTransform;

    [SerializeField] private KeyCode _firekey = KeyCode.Mouse0;

    [SerializeField] private float _range;
    [SerializeField] private int _damage;

    [SerializeField] private float _cooldown;
    private float _currentCooldown;
    private bool _isReadyFire { get { return _currentCooldown >= _cooldown; } }
    
    private bool _isPressedFire => Input.GetKey(_firekey);

    private void Awake() => CacheComponents();

    private void Update() => UpdateCurrentCooldown();

    private void UpdateCurrentCooldown()
    {
        if (_isReadyFire) return;

        _currentCooldown += Time.deltaTime;
    }

    public void Fire()
    {
        if (!_isPressedFire) return;

        if (!_isReadyFire) return;

        // Raycast -> IDamageable
        IDamageable damageable = GetDamageable();

        if (damageable == null) return;

        damageable.TakeDamage(_damage);
        Debug.Log($"Player : {damageable.GameObject.name}");

        // IDamagealbe.TakeDamage

        _currentCooldown = 0f;
    }

    private IDamageable GetDamageable()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        IDamageable damageable = null;

        if(Physics.Raycast(ray, out hit, _range))
        {
            damageable = hit.transform.GetComponent<IDamageable>();
        }

        return damageable;
    }

    private void CacheComponents()
    {
        _cameraTransform = Camera.main.transform;
    }
}