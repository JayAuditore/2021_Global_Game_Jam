using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerGate : MonoBehaviour
{
    [SerializeField] private int gateId;
    private BoxCollider2D collider2d;
    private bool isAbleToTransmit = false;
    private void Start() 
    {
        collider2d = GetComponent<BoxCollider2D>();
        collider2d.isTrigger = true;
        InputManager.Instance.StartListen(true);
        CenterEvent.Instance.AddListener(KeyTypeManager.EKeyDown, Transmit);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            isAbleToTransmit = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            isAbleToTransmit = false;
    }

    private void Transmit()
    {
        if (!isAbleToTransmit)
            return;
        //开始传送
        
    }
}
