using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Transform _cameraTransform;

    [SerializeField] private KeyCode _firekey = KeyCode.Mouse0;

    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    
    private bool _isPressedFire => Input.GetKey(_firekey);

    public void Fire()
    {
        if (!_isPressedFire) return;

        // Raycast -> IDamageable
        IDamageable damageable = GetDamageable();

        if (damageable == null) return;

        damageable.TakeDamage(_damage);
        Debug.Log($"Player : {damageable.gameObject.name}");

        // IDamagealbe.TakeDamage
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
}