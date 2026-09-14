using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TempPlayer : MonoBehaviour
{
    // InterChange : 반환형이 없고, int 매개변수를 1개 받는 함수를 담아둘 수 있는 타입이다.
    // 아래 Action 으로 변환 가능
    //public delegate void IntChange(int value);
    public UnityEvent TempEvent;
    private int _health;
    public int Health 
    { 
        get => _health;
        private set
        {
            _health = value;
            OnHealthChange?.Invoke(_health);
        }
    }
    public event Action<int> OnHealthChange;

    public ObservableProperty<float> Exp = new(0);

    private void OnEnable()
    {
        TempEvent.AddListener(Foo);
        TempEvent.RemoveListener(Foo);
        TempEvent.RemoveAllListeners();
    }

    private void Foo()
    {

    }

    private void TryLoadData(Action s, Action f)
    {/*
        // 로드 ~~
        if (성공 했다면 ?)
        {
            s.Invoke();
        }
        else
        {
            f.Invoke();
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TakeDamage(5);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Heal(10);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Exp.Value += 20.5f;
        if (Input.GetKeyDown(KeyCode.Alpha4)) TempEvent?.Invoke();
    }



    public void TakeDamage(int damage)
    {
        Debug.Log("데미지 받음");
        Health -= damage;
    }

    public void Heal(int heal)
    {
        Debug.Log("회복했다");
        Health += heal;
    }
}