// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Rezoskour.Content.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class PerksUpdateManager : MonoBehaviour
    {
        [SerializeField] private List<PerksData> listPerksData = null!;

        [SerializeField] private GameObject PerksUI = null!;
        [SerializeField] private GameObject PerksPrefab = null!;
        [SerializeField] private GameObject PerksParent = null!;

        private PlayerStats playerStats = null!;
        private AttackManager attackManager = null!;
        private CollectItem collectItem = null!;
        private HealthManager healthManager = null!;
        private OilComponent oilComponent = null!;
        private EventSystem eventSystem = null!;

        [SerializeField] private TextMeshProUGUI playerStatsTxt = null!;


        private void Start()
        {
            listPerksData = Resources.LoadAll<PerksData>("Perks/").ToList();
            GameManager.Instance.OnLevelUp += LootPerks;
            GameObject? player = GameObject.FindWithTag("Player");
            playerStats = player.GetComponent<PlayerStats>();
            attackManager = player.GetComponent<AttackManager>();
            collectItem = player.GetComponentInChildren<CollectItem>();
            healthManager = player.GetComponent<HealthManager>();
            oilComponent = player.GetComponent<OilComponent>();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            GameManager.Instance.OnLevelUp -= LootPerks;
        }

        private void LootPerks()
        {
            playerStatsTxt.text = GetStatsTxt();

            List<PerksData> copyListPerksData = new List<PerksData>(listPerksData);
            List<PerksData> randomPerks = new List<PerksData>();
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, copyListPerksData.Count);
                randomPerks.Add(copyListPerksData[randomIndex]);
                copyListPerksData.RemoveAt(randomIndex);
            }

            PerksUI.SetActive(true);
            foreach (PerksData? rndPerks in randomPerks)
            {
                GameObject? perks = Instantiate(PerksPrefab, PerksParent.transform);
                perks.GetComponent<PerksUI>().perksData = rndPerks;
                perks.GetComponent<PerksUI>().Init(this);
            }

            eventSystem.SetSelectedGameObject(PerksParent.transform.GetChild(0).gameObject);
        }

        private string GetStatsTxt()
        {
            string statsTxt = "";
            statsTxt += "Max Health: " + playerStats.CurrentStats.maxHealth + "\n";
            statsTxt += "Max Oil: " + playerStats.CurrentStats.maxOil + "\n";

            statsTxt += "Attack: " + playerStats.CurrentStats.globalDamageMultiplier + "\n";
            statsTxt += "Attack Range: " + playerStats.CurrentStats.range + "\n";

            statsTxt += "Speed: " + playerStats.CurrentStats.speed + "\n";
            statsTxt += "Damage Reduction: " + playerStats.CurrentStats.globalDamageReductor + "\n";
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
                case StatName.MaxHealth:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.maxHealth += (int) _addValue;
                        healthManager.Heal((int) _addValue);
                    }
                    else
                    {
                        playerStats.CurrentStats.maxHealth =
                            (int) (playerStats.CurrentStats.maxHealth * (1 + _multiplyValue));
                        healthManager.Heal((int) (playerStats.CurrentStats.maxHealth * _multiplyValue));
                    }

                    break;
                case StatName.MaxOil:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.maxOil += (int) _addValue;
                        oilComponent.RefillOil((int) _addValue);
                    }
                    else
                    {
                        playerStats.CurrentStats.maxOil =
                            (int) (playerStats.CurrentStats.maxOil * (1 + _multiplyValue));
                        oilComponent.RefillOil((int) (playerStats.CurrentStats.maxOil * _multiplyValue));
                    }

                    break;
                case StatName.Speed:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.speed += (int) _addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.speed = (int) (playerStats.CurrentStats.speed * (1 + _multiplyValue));
                    }

                    break;
                case StatName.CollectRange:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.collectRange += (int) _addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.collectRange =
                            (int) (playerStats.CurrentStats.collectRange * (1 + _multiplyValue));
                    }

                    //safe check CollectRange max value
                    if (playerStats.CurrentStats.collectRange > 4f)
                    {
                        playerStats.CurrentStats.collectRange = 4f;
                    }

                    collectItem.UpdateCollectRange();
                    break;
                case StatName.Range:
                    if (_addValue > 0)
                    {
                        playerStats.CurrentStats.range += (int) _addValue;
                    }
                    else
                    {
                        playerStats.CurrentStats.range = (int) (playerStats.CurrentStats.range * (1 + _multiplyValue));
                    }

                    break;
                case StatName.GlobalAttackRateMultiplier:
                    playerStats.CurrentStats.globalAttackRateMultiplier *= 1 + _multiplyValue;
                    attackManager.UpdateAttackStats();
                    break;
                case StatName.GlobalDamageMultiplier:
                    playerStats.CurrentStats.globalDamageMultiplier *= 1 + _multiplyValue;
                    attackManager.UpdateAttackStats();
                    break;
                case StatName.GlobalDamageReductor:
                    playerStats.CurrentStats.globalDamageReductor *= 1 + _multiplyValue;
                    //safe check
                    if (playerStats.CurrentStats.globalDamageReductor < 0.5f)
                    {
                        playerStats.CurrentStats.globalDamageReductor = 0.5f;
                    }

                    break;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(GameStateName.Main);
            }

            //Delete all perks prefab in PerksParent
            foreach (Transform child in PerksParent.transform)
            {
                Destroy(child.gameObject);
            }

            PerksUI.SetActive(false);
        }
    }
}