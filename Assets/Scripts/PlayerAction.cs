using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] Transform whereToGo;
    private PlayerAbilities[] abilities;
    private Transform spawnPoint;
    Coroutine activeCoroutine;

    private bool FixColumn()
    {
        throw new NotImplementedException();
    }

    public void GoToPoint(Transform destination)
    {
        activeCoroutine = StartCoroutine(MovePlayer(destination));
    }

    IEnumerator MovePlayer(Transform destination)
    {
        while (Vector3.Distance(transform.position, destination.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Column"))
        {
            StopCoroutine(activeCoroutine);
            Column column = other.GetComponent<Column>();
        }
    }
}
