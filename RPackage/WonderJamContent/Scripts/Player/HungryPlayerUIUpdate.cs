// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class HungryPlayerUIUpdate : MonoBehaviour
    {
        public Slider HungerBar;
        public int maxHunger = 50;
        public float currentHunger = 50;
        public Text hungerTxt;
        public GameManager gameManager;

        private bool isBerzerk = false;

        // Start is called before the first frame update
        private void Start()
        {
            gameManager.OnBerserkModeChange += Switch;
        }

        private void Update()
        {
            if (currentHunger >= 0)
            {
                if (isBerzerk)
                {
                    currentHunger -= 4 * Time.deltaTime;
                    hungerTxt.text = currentHunger.ToString("F0");
                    HungerBar.value = currentHunger / maxHunger;
                }
                else
                {
                    currentHunger -= 1 * Time.deltaTime;
                    hungerTxt.text = currentHunger.ToString("F0");
                    HungerBar.value = currentHunger / maxHunger;
                }
            }
        }

        private void Switch(bool value)
        {
            isBerzerk = value;
        }
    }
}