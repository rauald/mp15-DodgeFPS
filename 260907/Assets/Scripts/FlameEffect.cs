using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class FlameEffect : MonoBehaviour
{
    [SerializeField] private float _dectivateDelay;
    [SerializeField] private bool _isDestroy;
    private float _elapsedTime;

    [SerializeField] private bool _playInStart;
    private void Start() => gameObject.SetActive(_playInStart);

    private void Update() => UpdateElapsedTime();
    public void Play()
    {
        // 딜레이 초기화
        _elapsedTime = 0;
    }

    private void OnEnable()
    {
        _elapsedTime = 0;
    }

    private void UpdateElapsedTime()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime < _dectivateDelay) return;

        if (_isDestroy) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}