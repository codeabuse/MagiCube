using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private List<Side> boundSides = new List<Side>();
    [SerializeField]
    private List<ColorPlate> plates = new List<ColorPlate>();
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void BindToSide(Side side)
    {
        boundSides.Add(side);
    }

    public void ResetBindings()
    {
        boundSides.Clear();
    }

    private void OnMouseUp()
    {
        string report = "Bound sides: ";
        foreach (var side in boundSides)
        {
            report += side.SideColor.ToString() + " ";
        }
        Debug.Log(report);
    }
}
