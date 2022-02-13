using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Column : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    private ColumnProperty columsProperty;
    private MeshRenderer meshRenderer;
    private ColumnSpawner columnSpawner;

    public bool DefaultState => columsProperty.material == defaultMaterial;

    public Material GetMaterial() => columsProperty.material;

    public void InitializeColumn(ColumnSpawner columnSpawner)
    {
        columsProperty = new ColumnProperty(defaultMaterial);
        meshRenderer = GetComponent<MeshRenderer>();
        SetMaterial(defaultMaterial);
        this.columnSpawner = columnSpawner;
    }

    public void SetColoredMaterial(Material material)
    {
        if (material != defaultMaterial)
            SetMaterial(material);
    }

    public void FixMaterial()
    {
        if (!DefaultState)
        {
            SetMaterial(defaultMaterial);
            columnSpawner.NextLevel();
        }
    }

    private void SetMaterial(Material newMaterial)
    {
        columsProperty.material = newMaterial;
        meshRenderer.material = columsProperty.material;
    }
}
