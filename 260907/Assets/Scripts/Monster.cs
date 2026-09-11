using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public GameObject GameObject { get => gameObject; }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} : 데미지 입었다. - {damage}");
    }

    [SerializeField] private GameObject _enemyHPCanvas;
    [SerializeField] private List<Transform> _summonPointList = new();
    [SerializeField] private TurretController _turret;
    [SerializeField] private ObjectPool _pool;

    private int _maxTurretCnt;
    private int _curTurretCnt;

    private void Start() => SummonTurret();

    public void SummonTurret()
    {
        int rand = Random.Range(1, _summonPointList.Count);
        _maxTurretCnt = rand;

        for (int i = 0; i < rand; i++)
        {
            Instantiate(_turret, transform).SetTurret(_pool, _enemyHPCanvas, _summonPointList[i]);
        }
    }
}