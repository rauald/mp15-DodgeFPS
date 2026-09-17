using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private Animator _animator;

    public event Action<Vector2> OnMove;
    public event Action OnAttack;

    private Vector2 _prevMovement;

    private void Awake() => CacheComponents();

    private void Update()
    {
        SetMove();

        if (Input.GetKeyDown(KeyCode.W)) MoveStart();
        else if (Input.GetKeyUp(KeyCode.W)) MoveStop();

        if (Input.GetKeyDown(KeyCode.Space)) Attack();
    }

    private void MoveStart()
    {
        //_animator.SetBool("IsMove", true);
        OnMove?.Invoke(_prevMovement);
        Debug.Log("MoveStart");
    }

    private void MoveStop()
    {
        //_animator.SetBool("IsMove", false);
        OnMove?.Invoke(_prevMovement);
        Debug.Log("MoveStop");
    }

    private void Attack()
    {
        //_animator.SetTrigger("Attack");
        OnAttack?.Invoke();
        Debug.Log("Attack");
    }

    private void CacheComponents()
    {
        _animator = GetComponent<Animator>();
    }

    private void SetMove()
    {
        Vector2 _curMoveMent = GetMovement();

        // 이전 프레임의 Movement 와 같으면 return;
        if (_prevMovement == _curMoveMent) return;

        // 다르다면 OnMove
        OnMove?.Invoke(_curMoveMent);
        _prevMovement = _curMoveMent;
    }

    private Vector2 GetMovement()
    {
        // 입력 받아서 Vector2 반환 GetAxisRaw
        // 단위벡터로 만들지 마세요

        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}