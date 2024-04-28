using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;
    [SerializeField] FloatVariable speed;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2((_x + (speed.value/(_img.transform.localScale.x*3))) * Time.deltaTime, _y), _img.uvRect.size);
    }
}