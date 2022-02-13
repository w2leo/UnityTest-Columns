using System.Collections;
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
        Material[] newAbilities = CreateAbilityBag();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int startIndex = i * MAX_ABILITIES;
            int endIndex = startIndex + MAX_ABILITIES - 1;
            CreatePlayerObject(RandomBag.SubArray(newAbilities, startIndex, endIndex), spawnPoints[i].position);
        }
    }

    private void CreatePlayerObject(Material [] abilities, Vector3 position)
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

    private Material[] CreateAbilityBag()
    {
        List<Material> newAbilities = new List<Material>();
        newAbilities = CreateMaterialBag();
        return newAbilities.ToArray();
    }

    private List<Material> CreateMaterialBag()
    {
        List<Material> materialBag = new List<Material>();

        AddBaseMaterials(materialBag);
        AddAdditionalMaterials(materialBag);
        RandomBag.SuffleBageNoSameNear(materialBag);
        return materialBag;
    }

    private void AddBaseMaterials(List<Material> materialBag)
    {
        foreach (var material in columnSpawner.ColumnMaterials)
        {
            materialBag.Add(material);
        }
    }

    private void AddAdditionalMaterials(List<Material> materialBag)
    {
        while (materialBag.Count < spawnPoints.Length * MAX_ABILITIES)
        {
            materialBag.Add(RandomBag.RandomChoice(columnSpawner.ColumnMaterials));
        }
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
