using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private const int MAX_MAGEZINE_CNT = 30;
    private const int MAX_GRENADE_CNT = 3;

    private Transform _cameraTransform;

    [SerializeField] private KeyCode _firekey = KeyCode.Mouse0;

    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    public int MaxMagazine => MAX_MAGEZINE_CNT;
    public int CurrentMagazine => _currentBulletCnt;


    [SerializeField] private KeyCode _grenadeKey = KeyCode.Alpha3;
    private int _curGrenadeCnt;
    private bool _isGrenadeReady;
    [SerializeField] private KeyCode _grenadePowerKey = KeyCode.Space;
    [SerializeField] private Transform _grenadeTr;
    [SerializeField] private GameObject _grenadeObj;
    [SerializeField] private Grenade _grenade;
    private const float MAX_THROW_POWER = 10f;
    private float _throwPower;
    public int MaxGrenade => MAX_GRENADE_CNT;
    public int CurrentGrenade => _curGrenadeCnt;




    private bool _isItemSpeed;
    private const float MAX_COOL_DOWN = 0.5f;
    private float _cooldown;
    private float _currentCooldown;
    private bool _isReadyFire { get { return _currentCooldown >= _cooldown; } }

    private float _itemDuration;
    private float _curItemDuration;
    private float _itemValue;

    private PlayerUIController _playerUI;

    
    private bool _isPressedFire => Input.GetKey(_firekey);

    // 한 탄알집에 30발 들어갈 수 있다고 가정
    // 30발 다 쏘면 총 안쏴짐.
    // 리로드 버튼(R) 눌러야 다시 30발 참
    // 리로드 할 수 있는 탄환은 무제한

    private int _currentBulletCnt;
    private bool _isMagzine { get { return _currentBulletCnt > 0; } }


    [SerializeField] private FlameEffect _flameEffect;
    [SerializeField] private FlameEffect _bulletImpactEffectPrafab;


    [SerializeField] private LayerMask _targetMask;


    private void Awake() => CacheComponents();

    private void Start() => Init();

    private void Update()
    {
        UpdateCurrentCooldown();
        RefillBullet();
        UpdateShottingSpeedUp();
        SummonGrenade();
        ThorwGrenade();
    }

    private void Init()
    {
        _currentBulletCnt = MAX_MAGEZINE_CNT;
        _cooldown = MAX_COOL_DOWN;
        _isItemSpeed = false;
        _curItemDuration = 0;
        _curGrenadeCnt = MAX_GRENADE_CNT;
        _isGrenadeReady = false;
        _throwPower = 0;
        _grenadeObj.SetActive(false);
    }

    public void UICom(PlayerUIController playerUI)
    {
        _playerUI = playerUI;
    }

    private void UpdateCurrentCooldown()
    {
        if (_isReadyFire) return;

        _currentCooldown += Time.deltaTime;
    }

    public void Fire()
    {
        if (!_isPressedFire) return;

        if (!_isReadyFire) return;

        if (!_isMagzine)
        {
            Debug.Log("총알이 없습니다.");
            return;
        }

        PlayerFlameEffect();

        _currentCooldown = 0f;
        _currentBulletCnt--;
        Debug.Log($"총알이 {_currentBulletCnt}개 남았습니다.");
        _playerUI.SetMagazineUI(this);

        // Raycast -> IDamageable
        if (!TryGetDamageable(out IDamageable damageable)) return;

        damageable.TakeDamage(_damage);
        Debug.Log($"Player : {damageable.GameObject.name}");

        // IDamagealbe.TakeDamage
    }

    private void PlayerFlameEffect()
    {
        _flameEffect.gameObject.SetActive(true);
        _flameEffect.Play();
    }

    private void PlayBulletImpactEffect(RaycastHit hit)
    {
        Transform effectTransform = Instantiate(_bulletImpactEffectPrafab).transform;
        effectTransform.position = hit.point;
        effectTransform.forward = hit.normal;
    }

    private void RefillBullet()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_currentBulletCnt == MAX_MAGEZINE_CNT)
                Debug.Log("총알이 꽉 찼습니다.");

            Debug.Log($"총알 {MAX_MAGEZINE_CNT - _currentBulletCnt}개 재장전 완료.");
            _currentBulletCnt = MAX_MAGEZINE_CNT;
            _playerUI.SetMagazineUI(this);
        }
    }

    private bool TryGetDamageable(out IDamageable damageable)
    {
        bool result = false;
        damageable = null;

        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;


        if(Physics.Raycast(ray, out hit, _range, _targetMask)) //, QueryTriggerInteraction.Ignore))
        {
            PlayBulletImpactEffect(hit);
            result = hit.transform.TryGetComponent(out damageable);
        }

        return result;
    }

    private void CacheComponents()
    {
        _cameraTransform = Camera.main.transform;
    }
    public void ShootingSpeedUp(float duration, float value)
    {
        if (!_isItemSpeed)
        {
            _isItemSpeed = true;
            _itemDuration = duration;
            _itemValue = value;
            _cooldown /= _itemValue;
            Debug.Log($"사격 속도 {_itemValue}배 상승!");
        }
    }

    private void UpdateShottingSpeedUp()
    {
        if (_isItemSpeed)
        {
            _curItemDuration += Time.deltaTime;
            if (_curItemDuration > _itemDuration)
            {
                _isItemSpeed = false;
                _cooldown = MAX_COOL_DOWN;
                _curItemDuration = 0;
                Debug.Log($"사격 속도 아이템 효과가 끝났습니다.");
            }
        }
    }

    private void SummonGrenade()
    {
        if (_curGrenadeCnt <= 0) return;

        if (_isGrenadeReady) return;

        if (Input.GetKeyDown(_grenadeKey))
        {
            _grenadeObj.SetActive(true);
            _isGrenadeReady = true;
        }
    }

    private void ThorwGrenade()
    {
        if (!_isGrenadeReady) return;

        if(Input.GetKey(_grenadePowerKey))
        {
            _throwPower += (Time.deltaTime * 3);
            
            if( _throwPower >= MAX_THROW_POWER)
            {
                _throwPower = MAX_THROW_POWER;
            }
        }

        if(Input.GetKeyUp(_grenadePowerKey))
        {
            _isGrenadeReady = false;
            Instantiate(_grenade, _grenadeTr.position, _grenadeTr.rotation).Throw(_grenadeObj.transform, _throwPower);
            _curGrenadeCnt--;
            _playerUI.SetMagazineUI(this);
            _throwPower = 0;
            _grenadeObj.SetActive(false);
        }
    }
}