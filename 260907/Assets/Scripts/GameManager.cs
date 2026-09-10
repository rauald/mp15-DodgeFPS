using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Monster _monster;

    private void Awake() => _monster.SummonTurret();
}