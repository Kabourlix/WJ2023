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

        private string inputDevice;
        private string currentDevice;

        // Init inputDevice with the current device
        private void Update()
        {
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