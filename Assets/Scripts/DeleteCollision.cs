using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCollision : MonoBehaviour
{
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Obstacle") && collision.tag != "Player")
        {
            speed.value += scoreMult.value * Time.deltaTime;
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);
            return;
        }
    }
}