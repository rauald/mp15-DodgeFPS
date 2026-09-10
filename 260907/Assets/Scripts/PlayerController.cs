using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInteractor, IDamageable
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _detectionRange;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;

    private const int MAX_HP = 100;
    private int _curHp;

    private CapsuleCollider playerCol;

    private PlayerWeapon _weapon;
    private PlayerMovement _movement;
    private Transform _cameraTransform;

    private IInteractable _targetInteractable;


    private bool _hasDetectInteractable => _targetInteractable != null;
    private bool _isPressedInteractionKey => Input.GetKeyDown(_interactionKey);
    private bool _canInteraction => _hasDetectInteractable && _isPressedInteractionKey;

    public GameObject GameObject { get => gameObject; }


    private void Awake() => CacheComponents();
    private void Start()
    {
        //LockCursor();
        Init();
    }
    private void FixedUpdate() => _movement.Move();
    private void Update()
    {
        _movement.Rotate();
        _weapon.Fire();
        DetectInteractable();
        TryInteract();

    }
    private void LateUpdate()
    {
        SetCameraTransform();
        SetWeaponTransform();
    }

    private void Init()
    {
        _curHp = MAX_HP;
    }

    private void CacheComponents()
    {
        _movement = GetComponent<PlayerMovement>();
        _weapon = GetComponentInChildren<PlayerWeapon>();
        playerCol = GetComponent<CapsuleCollider>();
        _cameraTransform = Camera.main.transform;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetWeaponTransform()
    {
        _weapon.transform.SetPositionAndRotation(_cameraPivot.position, _cameraPivot.rotation);
    }

    private void SetCameraTransform()
    {
        _cameraTransform.SetPositionAndRotation(_cameraPivot.position, _cameraPivot.rotation);

        // ↑ 같음
        // _cameraTransform.position = _cameraPivot.position;
        // _cameraTransform.rotation = _cameraPivot.rotation;
    }

    public void DetectInteractable()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, _detectionRange))
        {

            if (_hasDetectInteractable)
            {
                _targetInteractable.Untargeting();
                _targetInteractable = null;
            }

            return;
        }
        
        // 이미 인터렉트를 주시하고 있을때
        if(_hasDetectInteractable)
        {
            // 계속 같은 애를 쳐다볼 경우
            if(hit.collider.gameObject == _targetInteractable.GameObject)
            {
                return;
            }

        }

        _targetInteractable?.Untargeting();
        _targetInteractable = hit.collider.GetComponent<IInteractable>();

        _targetInteractable?.Targeting();
    }

    public void TryInteract()
    {
        if (!_canInteraction) return;

        _targetInteractable.Interact(this);
        _targetInteractable = null;
    }

    public void Heal(int heal)
    {
        _curHp += heal;

        Debug.Log($"체력이 {heal} 회복되었습니다.");

        if (_curHp > MAX_HP)
        {
            _curHp = MAX_HP;
        }

        Debug.Log($"현재 체력 : {_curHp}");
    }

    public void SpeedUp(float duration, float value)
    {
        _movement.AddSpeed(duration, value);
    }

    public void ShootingSpeedUp(float duration, float value)
    {
        _weapon.ShootingSpeedUp(duration, value);
    }

    public void ItemBoxBoom(int value, Transform boomTrans)
    {
        float distance = Vector3.Distance(transform.position, boomTrans.position);

        if (distance < 3)
        {
            TakeDamage(value);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} : 데미지 입었다. - {damage}");

        _curHp -= damage;
        Debug.Log($"현재 체력 : {_curHp}");

        DieCheck();
    }

    private void DieCheck()
    {
        if (_curHp <= 0)
        {
            //  죽음
            playerCol.enabled = false;
            Debug.Log("죽었습니다.");
        }
    }
}