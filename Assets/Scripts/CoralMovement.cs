using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] GameObject coral;
    [SerializeField] float speed;
    private MeshRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
       _renderer = GetComponent<MeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}