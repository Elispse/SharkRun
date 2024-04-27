using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    [SerializeField] FloatEvent addScoreEvent = default;
    [SerializeField] float addedScore = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        addScoreEvent.RaiseEvent(addedScore);
        Destroy(gameObject);
    }
}
