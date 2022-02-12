using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Column : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    private ColumnProperty columsProperty;
    private MeshRenderer meshRenderer;

    public bool DefaultState => columsProperty.material == defaultMaterial;

    public void InitializeColumn()
    {
        columsProperty = new ColumnProperty(defaultMaterial);
        meshRenderer = GetComponent<MeshRenderer>();
        SetMaterial(columsProperty.material);
    }

    public void SetMaterial(Material newMaterial)
    {
        columsProperty.material = newMaterial;
        meshRenderer.material = columsProperty.material;
    }

    public Material GetMaterial() => columsProperty.material;
}
