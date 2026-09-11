using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    private int _damage;
    private float _speed;

    public LayerMask _targetLayer;

    public ObjectPool Pool { get; set; }

    public Transform tr { get => transform; }

    private float _elapsedTime;

    private float _returnDelay;

    // 어딘가에 부딪히면
    private void OnTriggerEnter(Collider other)
    {
        int layer = (1 << other.gameObject.layer);

        //if (ContainsLayer(TargetLayer, other))
        if (_targetLayer.Contains(other))
        {
            if(other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                Debug.Log("플레이어 맞음");
            }
            //Destroy(gameObject);
            Pool.Return(this);
        }
    }

    // 플레이어인 경우 데미지 주기
    // 벽인 경우 파괴

    private void Update()
    {
        UpdateElapsedTime();
        MoveForward();
        ReturnToPool();
    }

    // 앞으로 전진
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    
    // 터렛으로부터 데이터 전달 받기
    public void SetData(int damage, float speed, float destroyDelay)
    {
        _damage = damage;
        _speed = speed;
        _elapsedTime = 0f;
        _returnDelay = destroyDelay;
        // 스폰 기준으로 제한시간 이후 파괴
        //Destroy(gameObject, destroyDelay);
    }

    public void ReturnToPool()
    {
        // 제한시간이 경과할 것.
        if(_elapsedTime >= _returnDelay)
        {
            // 자신이 속한 풀에 대한 참조
            // 풀 내부적으로 다시 오브젝트를 넣어놓는 기능
            _elapsedTime = 0f;
            Pool.Return(this);
        }
    }

    private void UpdateElapsedTime()
    {
        _elapsedTime += Time.deltaTime;
    }
}