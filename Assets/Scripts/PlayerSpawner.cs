using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    private const int MAX_ABILITIES = 2;

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ColumnSpawner columnSpawner;
    [SerializeField] private Button[] buttons;
    private List<Player> players = new List<Player>();

    public void StartGame()
    {
        SpawnPlayers();
        AttacheButtonsToPlayers();
    }

    public void StopGame()
    {
        DestroyAllPlayers();
    }

    private void SpawnPlayers()
    {
        DestroyAllPlayers();
        int maxPlayers = spawnPoints.Length;
        Material[] columnMaterials = columnSpawner.ColumnMaterials;
        PlayerAbilities[] newAbilities = PlayerAbilities.CreateAbilities(columnMaterials, maxPlayers, MAX_ABILITIES);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            CreatePlayerObject(newAbilities[i], spawnPoints[i].position);
        }
    }

    private void CreatePlayerObject(PlayerAbilities abilities, Vector3 position)
    {
        GameObject playerGameObject = Instantiate(PlayerPrefab, position, Quaternion.identity, transform);
        Player newPlayer = playerGameObject.GetComponent<Player>();
        newPlayer.InitializePlayer(abilities, columnSpawner);
        players.Add(newPlayer);
    }

    private void DestroyAllPlayers()
    {
        foreach (var player in players)
        {
            player.StopAllCoroutines();
            Destroy(player.gameObject);
        }
        players.Clear();
    }  

    private void AttacheButtonsToPlayers()
    {
        if (buttons.Length != players.Count)
        {
            throw new System.Exception("Invalid Scene Initialization");

        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].AttacheButtonToPlayer(buttons[i]);
        }
    }
}
