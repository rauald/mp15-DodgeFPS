using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType _itemType;
    public float _duration;
    public float _value;

    public Item(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.HPHeal:
                _duration = 0;
                _value = 10;
                break;
            case ItemType.SpeedUp:
                _duration = 20f;
                _value = 1.5f;
                break;
            case ItemType.ShootingSpeedUp:
                _duration = 20f;
                _value = 2f;
                break;
            case ItemType.Boom:
                _duration = 0;
                _value = 20f;
                break;
            case ItemType.Max:
                break;
        }
    }
}