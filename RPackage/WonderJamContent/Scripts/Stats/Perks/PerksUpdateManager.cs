// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Rezoskour.Content.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class PerksUpdateManager : MonoBehaviour
    {
        [SerializeField] private List<PerksData> listPerksData = null!;

        [SerializeField] private GameObject PerksUI = null!;
        [SerializeField] private GameObject PerksPrefab = null!;
        [SerializeField] private GameObject PerksParent = null!;

        [SerializeField] private PlayerStats playerStats = null!;

        [SerializeField] private TextMeshProUGUI playerStatsTxt = null!;


        private void Start()
        {
            listPerksData = Resources.LoadAll<PerksData>("Perks/").ToList();
            GameManager.Instance.OnLevelUp += LootPerks;
            playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        }

        private void LootPerks()
        {
            playerStatsTxt.text = GetStatsTxt();

            var copyListPerksData = new List<PerksData>(listPerksData);
            var randomPerks = new List<PerksData>();
            for (var i = 0; i < 3; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, copyListPerksData.Count);
                randomPerks.Add(copyListPerksData[randomIndex]);
                copyListPerksData.RemoveAt(randomIndex);
            }

            foreach (var rndPerks in randomPerks)
            {
                var perks = Instantiate(PerksPrefab, PerksParent.transform);
                perks.GetComponent<PerksUI>().Init(this, rndPerks);
            }

            PerksUI.SetActive(true);
        }

        private string GetStatsTxt()
        {
            var statsTxt = "";
            statsTxt += "Max Health: " + playerStats.CurrentStats.maxHealth + "\n";
            statsTxt += "Max Oil: " + playerStats.CurrentStats.maxOil + "\n";

            statsTxt += "Attack: " + playerStats.CurrentStats.attack + "\n";
            statsTxt += "Attack Range: " + playerStats.CurrentStats.range + "\n";

            statsTxt += "Speed: " + playerStats.CurrentStats.speed + "\n";
            statsTxt += "Collect Range: " + playerStats.CurrentStats.collectRange + "\n";
            return statsTxt;
        }

        public void ApplyPerks(StatName _name, float _addValue, float _multiplyValue)
        {
            foreach (Transform child in PerksParent.transform)
            {
                child.gameObject.GetComponent<Button>().interactable = false;
            }

            switch (_name)
            {
                case StatName.Attack:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.attack += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.attack = (int)(playerStats.CurrentStats.attack * (1 + _multiplyValue));
                    }

                    break;
                case StatName.MaxHealth:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.maxHealth += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.maxHealth =
                            (int)(playerStats.CurrentStats.maxHealth * (1 + _multiplyValue));
                    }

                    break;
                case StatName.MaxOil:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.maxOil += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.maxOil = (int)(playerStats.CurrentStats.maxOil * (1 + _multiplyValue));
                    }

                    break;
                case StatName.Speed:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.speed += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.speed = (int)(playerStats.CurrentStats.speed * (1 + _multiplyValue));
                    }

                    break;
                case StatName.CollectRange:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.collectRange += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.collectRange =
                            (int)(playerStats.CurrentStats.collectRange * (1 + _multiplyValue));
                    }

                    break;
                case StatName.Range:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.range += (int)_addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.range = (int)(playerStats.CurrentStats.range * (1 + _multiplyValue));
                    }

                    break;
            }

            PerksUI.SetActive(false);
            GameManager.Instance.ChangeState(GameStateName.Main);
            //Delete all perks prefab in PerksParent
            foreach (Transform child in PerksParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}