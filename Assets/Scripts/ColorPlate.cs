using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlate : MonoBehaviour
{
    [SerializeField]
    private PlateColor color;
    public PlateColor Color { get { return color; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

public enum PlateColor
{
    White,
    Yellow,
    Red,
    Blue,
    Green,
    Orange
}
