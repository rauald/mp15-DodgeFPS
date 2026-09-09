using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTest : MonoBehaviour
{
    public LayerMask TargetLayer;
    
    // 레이캐스트의 거리는 인스펙터에서 조절할 수 있도록 한다.
    [SerializeField] private float _rayRange;

    private void Start()
    {
        TargetLayer = TargetLayer.Remove(9);
    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = (1 << other.gameObject.layer);

        //if (ContainsLayer(TargetLayer, other))
        if(TargetLayer.Contains(other))
        {
            Debug.Log("찾음");
        }

        Debug.Log(layer);
        Debug.Log(TargetLayer.value);
    }


    private bool ContainsLayer(LayerMask mask, Collider layer)
    {
        return 0 != (mask.value & (1 << layer.gameObject.layer));
    }
    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(
                new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z),
                transform.forward * _rayRange);
    }
}