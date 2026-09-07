using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _muzzlePoint;

    [Header("Bullet")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletDestroyDelay;

    private float _currentCooldown;
    private Transform _playerTransfom;
    private bool _isPlayerInTrigger => _playerTransfom != null;
    private bool _isPlayerInSight = false;
    private bool _isReadyToFire { get { return _currentCooldown >= _cooldown; } }
    private SphereCollider _sphereCollider;
    private void Awake() => CacheComponents();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerTransfom = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerTransfom = null;
        }
    }

    private void Update()
    {
        UpdateCurrentCooldown();
        RayShotPlayer();
        Rotate();
        Fire();
    }

    private void CacheComponents()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Fire()
    {
        if (!_isPlayerInSight || !_isPlayerInTrigger) return;

        Vector3 look = new Vector3(_playerTransfom.position.x, _headTransform.position.y, _playerTransfom.position.z);
        _headTransform.LookAt(_playerTransfom.position);

        if (!_isReadyToFire) return;

        // 발사 (Bullet Spawn)
        SpwanBullet();

        _currentCooldown = 0f;
    }

    private void UpdateCurrentCooldown()
    {
        if (_isReadyToFire) return;

        _currentCooldown += Time.deltaTime;
    }

    private void SpwanBullet()
    {
        // 프리팹
        // Instantiate 하면서 position, rotation 설정까지
        BulletController bullet = Instantiate(_bulletPrefab, _muzzlePoint.position, _muzzlePoint.rotation);

        bullet.SetData(_bulletDamage, _bulletSpeed, _bulletDestroyDelay);
    }

    private void Rotate()
    {
        if (!_isPlayerInSight) return;

        _headTransform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }

    private void RayShotPlayer()
    {
        if (!_isPlayerInTrigger) return;

        Vector3 from = new Vector3(transform.position.x, transform.position.y + _muzzlePoint.position.y / 2, transform.position.z);
        Vector3 to = new Vector3(_playerTransfom.position.x, _playerTransfom.position.y + _muzzlePoint.position.y / 2, _playerTransfom.position.z);

        Ray ray = new Ray(from, (to - from).normalized);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _sphereCollider.radius))
        {
            // 플레이어 찾았으면 감지 완료 된거임.
            if(hit.transform.CompareTag("Player"))
            {
                _isPlayerInSight = true;
                Debug.Log("플레이어 감지");
            }
            else
            {
                _isPlayerInSight = false;
            }
        }
        else
        {
            _isPlayerInSight = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (_sphereCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(
                new Vector3(transform.position.x,
                transform.position.y + _muzzlePoint.position.y / 2,
                transform.position.z),
                transform.forward * _sphereCollider.radius);
        }
    }
}