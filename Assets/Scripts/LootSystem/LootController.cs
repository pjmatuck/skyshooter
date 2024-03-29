using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField] List<LootItem> lootItems;
    [SerializeField] bool normalize;

    Dictionary<float, GameObject> dropTable = new Dictionary<float, GameObject>();

    void Start()
    {
        if (normalize)
        {
            GenerateNormalizedDropTable();
        } else
        {
            GenerateDropTable();
        }
    }

    private void GenerateDropTable()
    {
        float rateSupport = 0;

        foreach (var loot in lootItems)
        {
            float key = loot.DropRate + rateSupport;
            dropTable[key] = loot.Item;
            rateSupport = key;
        }

        dropTable[100] = null;
    }

    private void GenerateNormalizedDropTable()
    {
        float dropRateSum = 0;

        foreach (var loot in lootItems)
        {
            dropRateSum += loot.DropRate;
        }

        float normalization = 100 / dropRateSum;
        float normalizationSupport = 0;

        foreach (var loot in lootItems)
        {
            float key = (loot.DropRate * normalization) + normalizationSupport;
            dropTable[key] = loot.Item;
            normalizationSupport = key;
        }
    }

    public GameObject Drop()
    {
        if (lootItems.Count == 0) return null;

        GameObject drop = null;

        float chance = Random.Range(0f, 100f);

        foreach(var loot in dropTable)
        {
            if(chance <= loot.Key)
            {
                drop = loot.Value;
                break;
            }
        }

        return drop;
    }
}
