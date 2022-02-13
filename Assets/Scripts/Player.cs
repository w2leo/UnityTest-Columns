using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform AbilityPrefab;
    [SerializeField] private Transform initialAbilityPosition;
    [SerializeField] private PlayerAbilities abilities;
    private ColumnSpawner columnSpawner;
    private List<Transform> abilityVisuals = new List<Transform>();
    private Vector3 spawnPoint;
    Coroutine activeCoroutine;
    private const float abilityXOffset = 0.6f;
    private bool isGoing;

    private void Start()
    {
        spawnPoint = transform.position;
        StopMove();
    }

    public void InitializePlayer(PlayerAbilities abilities, ColumnSpawner columnSpawner)
    {
        this.columnSpawner = columnSpawner;
        SetAbilityies(abilities);
        AddVisualAbilities();
    }

    private void SetAbilityies(PlayerAbilities abilities)
    {
        this.abilities = abilities;
    }

    private void AddVisualAbilities()
    {
        abilityVisuals.Clear();
        int abilitiesQuantity = abilities.fixMaterials.Count;
        Vector3 position = Vector3.zero;
        position.x = CountXOffset(abilitiesQuantity);
        for (int i = 0; i < abilitiesQuantity; i++)
        {
            Transform newVisualAbility = Instantiate(AbilityPrefab, initialAbilityPosition);
            newVisualAbility.localPosition = position;
            abilityVisuals.Add(newVisualAbility);
            newVisualAbility.GetComponent<MeshRenderer>().material = abilities.fixMaterials[i];
            position.x += abilityXOffset;
        }
    }

    private float CountXOffset(int abilitiesQuantity)
    {
        float xOffset = 0;
        if (abilitiesQuantity % 2 == 0)
            xOffset += 0.5f;

        xOffset += (int)abilitiesQuantity / 2;
        xOffset--;
        Vector3 firstPosition = initialAbilityPosition.localPosition;
        return -xOffset * abilityXOffset;
    }

    private void GoAndFixColumn()
    {
        if (columnSpawner.CurrentChangedColumn == null || isGoing)
        {
            return;
        }
        MoveToPoint(columnSpawner.CurrentChangedColumn.transform.position);
    }

    private bool TryFixColumn(Column column)
    {
        if (!abilities.fixMaterials.Contains(column.GetMaterial()))
        {
            return false;
        }
        column.FixMaterial();
        return true;
    }

    private void MoveToPoint(Vector3 destination)
    {
        if (!isGoing)
        {
            isGoing = true;
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(MovePlayer(destination));
        }
    }

    IEnumerator MovePlayer(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isGoing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Column"))
        {
            StopMove();
            Column column = other.GetComponent<Column>();
            TryFixColumn(column);
            MoveToPoint(spawnPoint);
        }
    }

    private void StopMove()
    {
        isGoing = false;
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
    }

    public void AttacheButtonToPlayer(Button button)
    {
        button.onClick.AddListener(() => GoAndFixColumn());
    }
}
