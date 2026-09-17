using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnimationHandler : MonoBehaviour
{
    [SerializeField] private string _moveXParm;
    [SerializeField] private string _moveZParm;

    private int _moveX;
    private int _moveY;
    private int _move;
    private int _attack;
    private BotController _controller;
    private Animator _animator;

    private void Awake()
    {
        CacheComponents();
        Init();
    }

    private void OnEnable() => BindBotEvents();

    private void OnDisable() => UnBindBotEvents();

    private void Init()
    {
        _moveX = Animator.StringToHash(_moveXParm);
        _moveY = Animator.StringToHash(_moveZParm);
    }

    private void CacheComponents()
    {
        _controller = GetComponent<BotController>();
        _animator = GetComponent<Animator>();
    }

    private void BindBotEvents()
    {
        _controller.OnMove += SetMoveAnim;
    }

    private void UnBindBotEvents()
    {
        _controller.OnMove -= SetMoveAnim;
    }

    private void SetMoveAnim(Vector2 movement)
    {
        _animator.SetFloat(_moveX, movement.x);
        _animator.SetFloat(_moveY, movement.y);
    }
}