using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject ColumnPrefab;
    [SerializeField] private Material[] columnMaterials;
    [SerializeField] private float nextLevelDelayInSeconds;
    private List<Column> columns = new List<Column>();
    private Coroutine activeCoroutine;
    private Column currentChangedColumn;

    public Column CurrentChangedColumn
    {
        get
        {
            if (currentChangedColumn != null)
            {
                return currentChangedColumn;
            }
            return null;
        }
    }

    public Material[] ColumnMaterials => columnMaterials;

    public void StartGame()
    {
        SpawnColumns();
        NextLevel();
    }

    public void NextLevel()
    {
        if (currentChangedColumn == null || currentChangedColumn.DefaultState)
        {
            currentChangedColumn = null;
            activeCoroutine = StartCoroutine(ConfigureNextLevel());
        }
    }

    public void StopGame()
    {
        DestroyAllColumns();
    }

    private void SpawnColumns()
    {
        DestroyAllColumns();
        foreach (var point in spawnPoints)
        {
            GameObject newGameObject = Instantiate(ColumnPrefab, point.position, Quaternion.identity, transform);
            Column newColumn = newGameObject.GetComponent<Column>();
            newColumn.InitializeColumn(this);
            columns.Add(newColumn);
        }
    }

    private void DestroyAllColumns()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        foreach (var column in columns)
        {
            Destroy(column.gameObject);
        }
        columns.Clear();
        currentChangedColumn = null;
    }

    private void SetMaterialToRandomColumn(Material newMaterial)
    {
        currentChangedColumn = ChooseColumn();
        currentChangedColumn.SetColoredMaterial(newMaterial);
    }

    private Column ChooseColumn()
    {
        return RandomBag.RandomChoice(columns);
    }

    IEnumerator ConfigureNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelayInSeconds);
        Material newMaterial = RandomBag.RandomChoice(columnMaterials);
        SetMaterialToRandomColumn(newMaterial);
    }
}
