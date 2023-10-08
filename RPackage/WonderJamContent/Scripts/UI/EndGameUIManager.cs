// Copyrighted by team Rézoskour
// Created by alexandre buzon on 08

#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rezoskour.Content.UI
{
    public class EndGameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject endCanvas = null!;
        [SerializeField] private GameObject hud = null!;
        [SerializeField] private GameObject victoryMessage = null!;
        [SerializeField] private GameObject defeatMessage = null!;
        [SerializeField] private GameObject menuButton = null!;

        private EventSystem eventSystem = null!;

        private GameManager? Manager => GameManager.Instance;

        private void Start()
        {
            if (Manager == null)
            {
                Debug.LogError("Manager is null !");
                return;
            }

            Manager.OnDefeat += DefeatHandler;
            Manager.OnVictory += VictoryHandler;

            eventSystem = EventSystem.current;
        }

        private void OnDestroy()
        {
            if (Manager == null)
            {
                Debug.LogError("Manager is null !");
                return;
            }

            Manager.OnDefeat -= DefeatHandler;
            Manager.OnVictory -= VictoryHandler;
        }

        private void VictoryHandler()
        {
            EndGames(true);
        }

        private void DefeatHandler()
        {
            EndGames(false);
        }

        private void EndGames(bool _isVictory)
        {
            Debug.Log("EndGAME");
            endCanvas.SetActive(true);
            hud.SetActive(false);
            victoryMessage.SetActive(_isVictory);
            defeatMessage.SetActive(!_isVictory);
            eventSystem.SetSelectedGameObject(menuButton);
        }
    }
}