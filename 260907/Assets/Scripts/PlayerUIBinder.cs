using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUIBinder : MonoBehaviour
{
    private TempPlayer _player;
    [SerializeField] private HealthGauge _healthGauge;
    [SerializeField] private TempPlayerUI _playerUI;
    [SerializeField] private ExpGauge _expGauge;


    private void Awake() => CacheComponents();
    private void OnEnable() => BindPlayerStatChangeEvents();
    private void OnDisable() => UnbindPlayerStatChangeEvents();
    private void OnDestroy() => _player.Exp.RemoveAllListeners();


    private void BindPlayerStatChangeEvents()
    {
        _player.OnHealthChange += _playerUI.RefreshHealthUI;
        _player.OnHealthChange += _healthGauge.RefreshGauge;
        _player.Exp.AddListener(_expGauge.RefreshGauge);
    }

    private void UnbindPlayerStatChangeEvents()
    {
        _player.OnHealthChange -= _playerUI.RefreshHealthUI;
        _player.OnHealthChange -= _healthGauge.RefreshGauge;
        _player.Exp.RemoveListener(_expGauge.RefreshGauge);
    }

    private void CacheComponents()
    {
        _player = GetComponent<TempPlayer>();
    }
}