using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamageable
{
    [SerializeField] private const int MAX_HP = 200;
    [SerializeField] private int _curHp;
    private bool _isDie { get { return _curHp <= 0; } }
    [SerializeField] private GameObject _dieEff;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _muzzlePoint;

    [Header("Bullet")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletReturnDelay;

    private float _currentCooldown;
    private Transform _playerTransfom;
    private bool _isPlayerInTrigger => _playerTransfom != null;
    private bool _isPlayerInMask => _playerTransfom != null;
    [SerializeField] private bool _isPlayerInSight = false;
    private bool _isReadyToFire { get { return _currentCooldown >= _cooldown; } }

    public GameObject GameObject { get => gameObject; }

    private SphereCollider _sphereCollider;

    [SerializeField] private LayerMask _TargetMask;


    [SerializeField] private GameObject _hpUIParents;
    [SerializeField] private GameObject _enemyHPUIObj;
    private EnemyHPUI _enemyHPUI;

    [SerializeField] private ObjectPool _pool;


    private void Awake() => CacheComponents();

    private void Start()
    {
        _dieEff.SetActive(false);
        _curHp = MAX_HP;
        _enemyHPUI = Instantiate(_enemyHPUIObj, _hpUIParents.transform).GetComponent<EnemyHPUI>();
        _enemyHPUI.SetUI(GameObject.transform, MAX_HP, _curHp);
    }

    public void SetTurret(ObjectPool pool, GameObject parents, Transform tr)
    {
        _pool = pool;
        _hpUIParents = parents;
        transform.SetPositionAndRotation(tr.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.CompareTag("Player"))
        {
            _playerTransfom = other.transform;
        }*/

        if(_TargetMask.Contains(other))
        {
            _playerTransfom = other.transform;
            _isPlayerInSight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if(other.CompareTag("Player"))
        {
            _playerTransfom = null;
        }*/

        if (_TargetMask.Contains(other))
        {
            _isPlayerInSight = false;
        }
    }

    private void Update()
    {
        if (_isDie)
        {
            UpdateDieAniamtion();
        }
        else
        {
            UpdateCurrentCooldown();
            //RayShotPlayer();
            Rotate();
            Fire();
        }
    }

    private void CacheComponents()
    {
        _sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    private void Fire()
    {
        //if (!_isPlayerInSight || !_isPlayerInTrigger) return;
        
        if (!_isPlayerInSight || !_isPlayerInMask) return;

        Vector3 look = new Vector3(_playerTransfom.position.x, _headTransform.position.y, _playerTransfom.position.z);
        _headTransform.LookAt(look);

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
        // 1. 얻어오기
        IPoolable bullet = _pool.Take();

        // 2. Transform.Position, rotation 설정
        bullet.tr.position = _muzzlePoint.position;
        bullet.tr.rotation = _muzzlePoint.rotation;

        // 3. 활성화
        bullet.tr.gameObject.SetActive(true);

        (bullet as BulletController).SetData(_bulletDamage, _bulletSpeed, _bulletReturnDelay);


        // 프리팹
        // Instantiate 하면서 position, rotation 설정까지
        //BulletController bullet = Instantiate(_bulletPrefab, _muzzlePoint.position, _muzzlePoint.rotation);

        //bullet.SetData(_bulletDamage, _bulletSpeed, _bulletDestroyDelay);
    }

    private void Rotate()
    {
        if (_isPlayerInSight) return;

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

    public void TakeDamage(int damage)
    {
        if (_isDie) return;

        Debug.Log($"{gameObject.name} : 데미지 입었다. - {damage}");

        _curHp -= damage;
        Debug.Log($"현재 체력 : {_curHp}");

        if(_curHp <= 0)
            _curHp = 0;

        _enemyHPUI.SetHpUI(_curHp);

        DieCheck();
    }

    private void DieCheck()
    {
        if (_isDie)
        {
            _dieEff.SetActive(true);
        }
    }

    private void UpdateDieAniamtion()
    {
        if (_headTransform.eulerAngles.x < 28f)
        {
            _headTransform.Rotate(Vector3.right, _rotateSpeed * Time.deltaTime);
        }
        else if (_headTransform.eulerAngles.x > 32f)
        {
            _headTransform.Rotate(Vector3.left, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            _headTransform.localEulerAngles = new Vector3(30f, _headTransform.localEulerAngles.y, _headTransform.localEulerAngles.z);
        }
    }
}