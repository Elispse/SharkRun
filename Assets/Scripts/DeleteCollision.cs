using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCollision : MonoBehaviour
{
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            speed.value += (scoreMult / 10) * (Time.deltaTime / 16);
            Destroy(gameObject);
            return;
        }
    }
}