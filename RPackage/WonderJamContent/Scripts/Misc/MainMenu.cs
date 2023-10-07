// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content.Misc
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private GameObject playButtonObj;
        [SerializeField] private GameObject backButton;

        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject KeyboardControls;
        [SerializeField] private GameObject GamepadControls;

        private bool isUsingGamepad = false;
        private string currentController = null;

        [SerializeField] private EventSystem eventSystem;

        private void Start()
        {
            KeyboardControls.gameObject.SetActive(true);
            GamepadControls.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Gamepad.current != null && Keyboard.current != null)
            {
                if (isUsingGamepad && currentController != "Keyboard")
                {
                    if (Keyboard.current.wasUpdatedThisFrame)
                    {
                        isUsingGamepad = false;
                        currentController = "Keyboard";
                        eventSystem.SetSelectedGameObject(playButtonObj);
                    }
                }
                else if (!isUsingGamepad && currentController != "Gamepad")
                {
                    if (Gamepad.current.wasUpdatedThisFrame)
                    {
                        isUsingGamepad = true;
                        currentController = "Gamepad";
                        eventSystem.SetSelectedGameObject(playButtonObj);
                    }
                }

                switch (currentController)
                {
                    case "Keyboard":
                        KeyboardControls.gameObject.SetActive(true);
                        GamepadControls.gameObject.SetActive(false);
                        break;
                    case "Gamepad":
                        KeyboardControls.gameObject.SetActive(false);
                        GamepadControls.gameObject.SetActive(true);
                        break;
                }
            }
        }

        public void Play()
        {
            SceneManager.LoadScene("Main");
        }

        public void Credits()
        {
            playButton.interactable = false;
            creditsButton.interactable = false;
            quitButton.interactable = false;
            creditsPanel.SetActive(true);
            eventSystem.SetSelectedGameObject(backButton);
        }

        public void Back()
        {
            playButton.interactable = true;
            creditsButton.interactable = true;
            quitButton.interactable = true;
            creditsPanel.SetActive(false);
            eventSystem.SetSelectedGameObject(playButtonObj);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}