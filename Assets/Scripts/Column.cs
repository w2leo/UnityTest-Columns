using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    private ColumnProperties property;
 
    public void SetMaterial(Material newMaterial)
    {
        property.material = newMaterial;
    }

    public Material GetMaterial() => property.material;
}
