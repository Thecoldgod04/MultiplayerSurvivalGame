using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTableManager : MonoBehaviour
{
    public static LootTableManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool Possible(float possibility)
    {
        float num = (int)(possibility * 100);
        float random = Random.Range(0, 101);

        //Debug.LogError(random + " | " + num);
        if (random <= num)
        {
            return true;
        }

        return false;
    }


    /** Returns a Vector2Int where:
     *      x: index of the loot in the LootTable
     *      y: amount of that loot
     */
    public List<Vector2Int> GenerateLoot(LootTable lootTable)
    {
        List<Vector2Int> lootResult = new List<Vector2Int>();
        for (int i = 0; i < lootTable.loots.Count; i++)
        {
            Loot loot = lootTable.loots[i];
            Vector2Int tempLoot = new Vector2Int(i, 0);
            for(int j = 0; j < loot.maxAmount; j++)
            {
                if(Possible(loot.possibility))
                {
                    tempLoot.y++;
                }
            }
            lootResult.Add(tempLoot);
        }

        return lootResult;
    }
}
