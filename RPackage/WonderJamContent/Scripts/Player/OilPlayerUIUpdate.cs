// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class OilPlayerUIUpdate : MonoBehaviour
    {
        [SerializeField] private OilComponent oilComponent = null!;

        [FormerlySerializedAs("HungerBar")] [SerializeField]
        private Slider hungerBar = null!;

        [SerializeField] private Text hungerTxt;

        // Start is called before the first frame update
        private void Start()
        {
            oilComponent.OnOilModified += UpdateOilUI;
        }

        private void UpdateOilUI(int _hungerValue)
        {
            hungerTxt.text = oilComponent.Oil.ToString("F0");
            hungerBar.value = oilComponent.OilPercentage;
        }

        private void OnDestroy() { }
    }
}