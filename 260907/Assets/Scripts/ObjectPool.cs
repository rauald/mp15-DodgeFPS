using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    // 풀 전체 개수
    [field: SerializeField] public int Size { get; private set; }
    // 오브젝트 풀 배열
    private IPoolable[] _pool;
    // 풀에 들어갈 실제 개수
    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    private void Awake() => Init();

    public IPoolable Take()
    {
        if (IsEmpty) return null;

        Count--;
        IPoolable poolable = _pool[Count];
        _pool[Count] = null;

        return poolable;
    }

    public void Return(IPoolable poolable)
    {
        if (Size <= Count) return;

        _pool[Count] = poolable;
        poolable.tr.gameObject.SetActive(false);
        Count++;
    }

    private void Init()
    {
        _pool = new IPoolable[Size];

        for(int i = 0; i < _pool.Length; i++)
        {
            //IPoolable poolable = Instantiate(_prefab).GetComponent<IPoolable>();
            GameObject go = Instantiate(_prefab);
            _pool[i] = go.GetComponent<IPoolable>();
            _pool[i].Pool = this;
            go.SetActive(false);
        }

        Count = Size;
    }
}