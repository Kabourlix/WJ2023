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
    [CreateAssetMenu(fileName = "Stats", menuName = "Rézoskour/Stats Data", order = 0)]
    public class Stats : SerializedScriptableObject
    {
        [SerializeField] private int health;
        public int Health => health;

        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;

        [SerializeField] private int oil;
        public int Oil => oil;

        [SerializeField] private int maxOil;
        public int MaxOil => maxOil;

        [SerializeField] private int attack;
        public int Attack => attack;

        [SerializeField] private int collectRange;
        public int CollectRange => collectRange;

        [SerializeField] private int range;
        public int Range => range;

        [SerializeField] private int speed;
        public int Speed => speed;

        public void SaveStatsToJson()
        {
            var jsonStr = JsonUtility.ToJson(this);
            /*string filePath = Application.persistentDataPath+"/stats.json";
            System.IO.File.WriteAllText(filePath, jsonStr);
            Debug.Log("Stats saved.");*/

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

        public void BerserkMode()
        {
            attack *= 3;
            collectRange *= 3;
            range *= 3;
            speed *= 3;
        }
    }
}