// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using UnityEngine;
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

        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject KeyboardControls;
        [SerializeField] private GameObject GamepadControls;

        private bool isUsingGamepad = false;

        private void Start()
        {
            KeyboardControls.gameObject.SetActive(false);
            GamepadControls.gameObject.SetActive(false);
        }

        private void Update()
        {
            var controllerUsed = Gamepad.current.wasUpdatedThisFrame;

            if (controllerUsed && !isUsingGamepad)
            {
                // Si la manette est utilisée et que le joueur ne l'utilisait pas précédemment,
                // activez l'image du contrôleur et désactivez l'image du clavier
                GamepadControls.gameObject.SetActive(true);
                KeyboardControls.gameObject.SetActive(false);
                isUsingGamepad = true;
            }
            else if (!controllerUsed && isUsingGamepad)
            {
                // Si le clavier est utilisé et que le joueur utilisait précédemment un contrôleur,
                // activez l'image du clavier et désactivez l'image du contrôleur
                GamepadControls.gameObject.SetActive(false);
                KeyboardControls.gameObject.SetActive(true);
                isUsingGamepad = false;
            }
        }

        public void Play()
        {
            SceneManager.LoadScene("Main");
        }

        public void Credits()
        {
            creditsPanel.SetActive(true);
        }

        public void Back()
        {
            creditsPanel.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}