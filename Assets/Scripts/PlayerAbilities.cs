using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAbilities
{
    public List<Material> fixMaterials;

    public PlayerAbilities()
    {
        fixMaterials = new List<Material>();
    }

    public static PlayerAbilities[] CreateAbilities(Material[] baseMaterials, int spawnPoints, int maxAbilities)
    {
        List<PlayerAbilities> newAbilities = new List<PlayerAbilities>();
        AddMaterialsToAbilities(newAbilities, baseMaterials, spawnPoints, maxAbilities);
        return newAbilities.ToArray();
    }

    private static void AddMaterialsToAbilities(List<PlayerAbilities> playersAbilities, Material[] baseMaterials, int spawnPoints, int maxAbilities)
    {
        List<Material> materialBag = CreateMaterialBag(baseMaterials, spawnPoints * maxAbilities);
        for (int i = 0; i < spawnPoints; i++) 
        {
            int startIndex = i * maxAbilities;
            int endIndex = startIndex + maxAbilities - 1;
            playersAbilities.Add(new PlayerAbilities());
            foreach (var item in RandomBag.SubArray(materialBag.ToArray(), startIndex, endIndex))
            {
                playersAbilities[i].fixMaterials.Add(item);
            }
        }
    }

    private static List<Material> CreateMaterialBag(Material[] baseMaterials, int maxLength)
    {
        List<Material> materialBag = new List<Material>();
        CopyBaseMaterials(materialBag, baseMaterials);
        AddAdditionalMaterials(materialBag, maxLength, baseMaterials);
        RandomBag.SuffleBagNoSameNear(materialBag);
        return materialBag;
    }

    private static void CopyBaseMaterials(List<Material> bagTo, Material[] bagFrom)
    {
        foreach (var material in bagFrom) 
        {
            bagTo.Add(material);
        }
    }

    private static void AddAdditionalMaterials(List<Material> materialBag, int maxLength, Material[] bagFrom)
    {
        while (materialBag.Count < maxLength) 
        {
            materialBag.Add(RandomBag.RandomChoice(bagFrom)); 
        }
    }
}
