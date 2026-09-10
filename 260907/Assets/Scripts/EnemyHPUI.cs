using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    [SerializeField] private int _maxHp;
    [SerializeField] private int _curHp;

    private Transform _enemyTr;
    private Vector3 _offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Image _imgHp;
    [SerializeField] private TextMeshProUGUI _txtHp;

    private void LateUpdate()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (_enemyTr == null) return;

        transform.position = _enemyTr.position + _offset;
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetUI(Transform Tr, int maxHp, int curHp)
    {
        _enemyTr = Tr;
        _maxHp = maxHp;
        SetHpUI(curHp);
    }

    public void SetHpUI(int curHp)
    {
        _curHp = curHp;
        _imgHp.fillAmount = (float)_curHp / _maxHp;

        _txtHp.text = $"{_curHp} / {_maxHp}";
    }
}