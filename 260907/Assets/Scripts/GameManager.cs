using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameRunning { get; private set; }
    public static GameManager instance;
    private void Awake() => SetSingleton();

    private void Start() => Run();

    public void Run()
    {
        LockCursor();
        Time.timeScale = 1;
        IsGameRunning = true;
    }

    public void Pause()
    {
        UnLockCursor();
        Time.timeScale = 0;
        IsGameRunning = false;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}