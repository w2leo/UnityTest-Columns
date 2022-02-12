using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject ColumnPrefab;
    [SerializeField] Material[] columnMaterials;
    [SerializeField] private float levelTime;
    private List<Column> columns = new List<Column>();
    private Column currentChangedColumn;
    private Coroutine activeCoroutine;

    private void SpawnColumns()
    {
        DestroyAllColumns();
        foreach (var point in spawnPoints)
        {
            Column newColumn = Instantiate(ColumnPrefab, point.position, Quaternion.identity, transform).GetComponent<Column>();
            newColumn.InitializeColumn();
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
        currentChangedColumn.SetMaterial(newMaterial);
    }

    private Column ChooseColumn()
    {
        return RandomBag.RandomChoice(columns);
    }

    public void StartGame()
    {
        SpawnColumns();
        NextLevel();
    }

    public void StopGame()
    {
        DestroyAllColumns();
    }

    public void NextLevel()
    {
        if (currentChangedColumn == null || currentChangedColumn.DefaultState)
        {
            activeCoroutine = StartCoroutine(nextLevel());
        }
    }

    IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(levelTime);
        Material newMaterial = RandomBag.RandomChoice(columnMaterials);
        SetMaterialToRandomColumn(newMaterial);
    }
}
