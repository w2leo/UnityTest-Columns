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
        AttacheButtons();
    }
    public void StopGame()
    {
        DestroyAllPlayers();
    }

    private void SpawnPlayers()
    {
        DestroyAllPlayers();
        Material[] newAbilities = ChooseAbilities();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject playerGameObject = Instantiate(PlayerPrefab, spawnPoints[i].position, Quaternion.identity, transform);
            Player newPlayer = playerGameObject.GetComponent<Player>();
            int startIndex = i * MAX_ABILITIES;
            int endIndex = startIndex + MAX_ABILITIES - 1;
            newPlayer.InitializePlayer(RandomBag.SubArray(newAbilities, startIndex, endIndex), columnSpawner);
            players.Add(newPlayer);
        }
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

    private Material[] ChooseAbilities()
    {
        List<Material> newAbilities = new List<Material>();
        newAbilities = CreateMaterialBag();
        return newAbilities.ToArray();
    }

    private List<Material> CreateMaterialBag()
    {
        List<Material> materialBag = new List<Material>();

        foreach (var material in columnSpawner.ColumnMaterials)
        {
            materialBag.Add(material);
        }
        while (materialBag.Count < spawnPoints.Length * MAX_ABILITIES)
        {
            materialBag.Add(RandomBag.RandomChoice(columnSpawner.ColumnMaterials));
        }
        int maxOperationCounter = 20;
        do
        {
            if (maxOperationCounter == 0)
            {
                throw new System.StackOverflowException();
            }
            maxOperationCounter--;
            RandomBag.ShuffleBag(ref materialBag);
        }
        while (CheckFoSameInRow(materialBag));
        return materialBag;
    }

    private bool CheckFoSameInRow(List<Material> bag)
    {
        for (int i = 0; i < bag.Count - 1; i += 2)
        {
            if (bag[i] == bag[i + 1])
            {
                return true;
            }
        }
        return false;
    }

    private void AttacheButtons()
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
