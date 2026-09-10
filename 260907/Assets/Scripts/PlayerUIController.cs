using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    private int _maxHp;
    private int _curHp;

    private int _maxTurretCnt;
    private int _curTurretCnt;
    [SerializeField] private Image _img;
    [SerializeField] private TextMeshProUGUI _txtCurHp;
    [SerializeField] private TextMeshProUGUI _txtMagazine;
    [SerializeField] private TextMeshProUGUI _txtGrenade;
    [SerializeField] private TextMeshProUGUI _txtTurret;


    public void SetUI(int maxHp, int curHp)
    {
        _maxHp = maxHp;
        SetHpUI(curHp);
    }

    public void SetHpUI(int curHp)
    {
        _curHp = curHp;
        _img.fillAmount = (float)_curHp / _maxHp;

        _txtCurHp.text = $"{_curHp} / {_maxHp}";
    }

    public void SetMagazineUI(PlayerWeapon weapon)
    {
        _txtMagazine.text = $"{weapon.CurrentMagazine} / {weapon.MaxMagazine}";
        _txtGrenade.text = $" {weapon.CurrentGrenade} / {weapon.MaxGrenade}";
    }

    public void SetTurretCntUI()
    {
        _txtTurret.text = $"{_curTurretCnt} / {_maxTurretCnt}";
    }
}