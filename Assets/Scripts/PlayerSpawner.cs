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
        PlayerAbilities[] newAbilities = CreateAbilities();
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

    private PlayerAbilities[] CreateAbilities()
    {
        List<PlayerAbilities> newAbilities = new List<PlayerAbilities>();
        AddMaterialsToAbilities(newAbilities);
        return newAbilities.ToArray();
    }

    private void AddMaterialsToAbilities(List<PlayerAbilities> playersAbilities)
    {
        List<Material> materialBag = CreateMaterialBag();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int startIndex = i * MAX_ABILITIES;
            int endIndex = startIndex + MAX_ABILITIES - 1;
            playersAbilities.Add(new PlayerAbilities());
            foreach (var item in RandomBag.SubArray(materialBag.ToArray(), startIndex, endIndex))
            {
                playersAbilities[i].fixMaterials.Add(item);
            }
        }
    }

    private List<Material> CreateMaterialBag()
    {
        List<Material> materialBag = new List<Material>();
        AddBaseMaterials(materialBag);
        AddAdditionalMaterials(materialBag);
        RandomBag.SuffleBagNoSameNear(materialBag);
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
