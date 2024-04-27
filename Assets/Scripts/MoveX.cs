using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveX : MonoBehaviour
{
    [SerializeField] float Step = 0f;
    [SerializeField] FloatVariable speed;
    // Update is called once per frame
    void Update()
    {
        Transform trans = gameObject.GetComponent<Transform>();
        Vector3 newVect = trans.position + new Vector3((Step - speed.value) * Time.deltaTime, 0f, 0f);
        trans.position = newVect;
    }
}