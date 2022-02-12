using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform AbilityPrefab;
    [SerializeField] private Transform abilityPosition;
    private ColumnSpawner columnSpawner;
    private List<Transform> abilityQuads = new List<Transform>();
    private Vector3 startPoint;
    [SerializeField] private PlayerAbilities abilities;
    Coroutine activeCoroutine;
    private const float quadInterval = 0.6f;
    private bool isGoing;

    private void Start()
    {
        startPoint = transform.position;
        StopMove();
    }

    public void InitializePlayer(Material[] materialAbilities, ColumnSpawner columnSpawner)
    {
        this.columnSpawner = columnSpawner;
        SetAbilityies(materialAbilities);
        AddVisualAbilities();
    }

    private void SetAbilityies(Material[] materialAbilities)
    {
        abilities.fixMaterials.Clear();
        foreach (var material in materialAbilities)
        {
            abilities.fixMaterials.Add(material);
        }
    }

    private void AddVisualAbilities()
    {
        abilityQuads.Clear();
        int abilitiesQuantity = abilities.fixMaterials.Count;
        float xOffset = 0;

        if (abilitiesQuantity % 2 == 0)
            xOffset += 0.5f;

        xOffset += (int)abilitiesQuantity / 2;
        xOffset--;

        Vector3 firstPosition = abilityPosition.localPosition;
        firstPosition.x = -xOffset * quadInterval;
        firstPosition.y = 0;
        firstPosition.z = 0;

        for (int i = 0; i < abilitiesQuantity; i++)
        {
            Transform newVisualAbility = Instantiate(AbilityPrefab, abilityPosition);
            newVisualAbility.localPosition = firstPosition;
            abilityQuads.Add(newVisualAbility);
            newVisualAbility.GetComponent<MeshRenderer>().material = abilities.fixMaterials[i];
            firstPosition.x += quadInterval;
        }

    }

    private void GoAndFixColumn()
    {
        if (columnSpawner.CurrentChangedColumn == null || isGoing)
        {
            return;
        }
        GoToPoint(columnSpawner.CurrentChangedColumn.transform.position);
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

    private void GoToPoint(Vector3 destination)
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
            GoToPoint(startPoint);
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
