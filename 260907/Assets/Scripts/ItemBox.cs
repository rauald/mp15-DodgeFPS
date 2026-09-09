using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemBox : MonoBehaviour, IInteractable
{
    public GameObject GameObject { get => gameObject; }
    private Outline _outline;

    [SerializeField] private ItemType itemType;
    [SerializeField] GameObject[] _itemEffect;

    private Item _item;

    private void Awake() => CacheComponents();
    private void Start()
    { 
        itemType = (ItemType)(Random.Range(0, (int)ItemType.Max));
        Init();
    }

    public void Targeting()
    {
        _outline.enabled = true;
    }

    public void Untargeting()
    {
        _outline.enabled = false;
    }
    public void Interact(IInteractor owner)
    {
        if (!(owner is PlayerController)) return;

        PlayerController player = (PlayerController)owner;

        ItemEffect(player);

        Destroy(gameObject);
    }

    private void Init()
    {
        _outline.enabled = false;
        _item = new Item(itemType);
    }

    private void CacheComponents()
    {
        _outline = gameObject.GetComponent<Outline>();
    }

    private void ItemEffect(PlayerController player)
    {
        GameObject effectObj = Instantiate(_itemEffect[(int)itemType], transform.position, Quaternion.identity);

        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.HPHeal:
                player.Heal((int)_item._value);
                break;
            case ItemType.SpeedUp:
                player.SpeedUp(_item._duration, _item._value);
                break;
            case ItemType.ShootingSpeedUp:
                player.ShootingSpeedUp(_item._duration, _item._value);
                break;
            case ItemType.Boom:
                //player.ItemBoxBoom((int)_item._value, boom.transform);

                Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

                foreach(Collider col in colliders)
                {
                    if(col.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage((int)_item._value);
                    }
                }

                break;
            case ItemType.Max:
                break;
        }
    }
}