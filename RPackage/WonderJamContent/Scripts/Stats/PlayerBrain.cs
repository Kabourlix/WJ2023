// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using UnityEngine;

namespace Rezoskour.Content
{
    [RequireComponent(typeof(HealthManager))]
    [RequireComponent(typeof(OilComponent))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Xp))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerBrain : MonoBehaviour
    {
        [SerializeField] private int oilLossSpeedNormal = 1;
        [SerializeField] private int oilLossSpeedBerserk = 5;

        private bool isBerserk;

        private int CurrentOilLossSpeed => isBerserk ? oilLossSpeedBerserk : oilLossSpeedNormal;

        private OilComponent oilComponent = null!;
        private HealthManager healthManager = null!;
        private Xp xp = null!;
        private PlayerMovement playerMovement = null!;
        private PlayerStats playerStats = null!;

        private void Awake()
        {
            oilComponent = GetComponent<OilComponent>();
            healthManager = GetComponent<HealthManager>();
            xp = GetComponent<Xp>();
            playerMovement = GetComponent<PlayerMovement>();
            playerStats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            GameManager.Instance.OnBerserkModeChange += SwitchMode;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError($"GameManager.Instance is null OnDestroy:{nameof(PlayerBrain)}!");
                return;
            }

            GameManager.Instance.OnBerserkModeChange -= SwitchMode;
        }

        private void Update()
        {
            oilComponent.LooseOil(CurrentOilLossSpeed * Time.deltaTime);
        }

        private void SwitchMode(bool _isBerserkOn)
        {
            Debug.Log("Update Berserk mode in PlayerBrain.");
            isBerserk = _isBerserkOn;
        }
    }
}