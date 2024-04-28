using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    [SerializeField] VoidEvent ObstacleHitEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (!collision.gameObject.CompareTag("Player")) return; // Player didn't hit obstacle

        ObstacleHitEvent.RaiseEvent();
    }
}
