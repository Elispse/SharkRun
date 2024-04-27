using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScript : MonoBehaviour
{
    public enum Location { None = 0, TOP, MIDDLE, BOTTOM };
    [SerializeField] public int Weight = 0;
    [SerializeField] public Location Area;
}
