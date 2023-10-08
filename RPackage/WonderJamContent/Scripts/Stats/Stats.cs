// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using System.Collections.Generic;
using Rezoskour.OdinSerializer;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public enum StatName
    {
        Health,
        MaxHealth,
        Oil,
        MaxOil,
        CollectRange,
        Range,
        Speed,
        GlobalDamageMultiplier,
        GlobalDamageReductor,
        GlobalAttackRateMultiplier
    }

    [CreateAssetMenu(fileName = "Stats", menuName = "Rezoskour/Stats Data", order = 0)]
    public class Stats : SerializedScriptableObject
    {
        public int health;

        public int maxHealth;

        public int oil;

        public int maxOil;

        [Range(1, 4)] public float collectRange;

        public int range;

        public float speed;

        [HideInInspector] public float globalDamageMultiplier = 1;
        [Range(0.5f, 1f)] [HideInInspector] public float globalDamageReductor = 1;
        [HideInInspector] public float globalAttackRateMultiplier = 1;

        /*
         public void SaveStatsToJson()
         {
             var jsonStr = JsonUtility.ToJson(this);
             //string filePath = Application.persistentDataPath+"/stats.json";
             //System.IO.File.WriteAllText(filePath, jsonStr);
             //Debug.Log("Stats saved.");

             PlayerPrefs.SetString("Stats", jsonStr);
             PlayerPrefs.Save();
             Debug.Log("Stats saved.");
         }

         public bool LoadStatsFromJson()
         {
             var jsonString = PlayerPrefs.GetString("Stats", "");
             Debug.Log("ALEX : " + jsonString);
             if (jsonString is "" or "{}" or "null")
             {
                 return false;
             }

             JsonUtility.FromJsonOverwrite(jsonString, this);
             return true;
         }

         public void LoadStats()
         {
             var filePath = Application.persistentDataPath + "/stats.json";
             var jsonString = System.IO.File.ReadAllText(filePath);

             JsonUtility.FromJsonOverwrite(jsonString, this);
         }
         */

        public void BerserkMode()
        {
            collectRange *= 3;
            range *= 3;
            speed *= 3;
        }
    }
}