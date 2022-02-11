using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{   
    [SerializeField] GameObject ColumnPrefab;
    [SerializeField] private float spawnTime;
    private float timer;
    [SerializeField] Transform[] spawnPoints;
    private List<Column> columns = new List<Column>();
    private Column currentChangedColumn;

    private void Start()
    {
        SpawnColumns();
    }

    private void SetMaterialToRandomColumn(Material newMaterial)
    {
        currentChangedColumn = ChooseColumn();
        currentChangedColumn.SetMaterial(newMaterial);
    }

    private Column ChooseColumn()
    {
        return RandomChoice(columns);
    }

    private void SpawnColumns()
    {
        foreach (var point in spawnPoints)
        {
            Column newColumn = Instantiate(ColumnPrefab, point.position, Quaternion.identity, transform).GetComponent<Column>();
            columns.Add(newColumn);
        }
    }

    private T RandomChoice<T>(List<T> bag)
    {
        return bag[Random.Range(0, bag.Count - 1)];
    }
}
