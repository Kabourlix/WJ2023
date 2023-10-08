// Copyrighted by team Rézoskour
// Created by alexandre buzon on 08

#nullable enable

using System;
using Rezoskour.Content.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 08

namespace Rezoskour.Content
{
    public class PerksUI : MonoBehaviour
    {
        public PerksData perksData = null!;
        private PerksUpdateManager perksUpdateManager = null!;
        [SerializeField] private Image perksImage = null!;
        [SerializeField] private TextMeshProUGUI perksTitle = null!;
        [SerializeField] private TextMeshProUGUI perksDescription = null!;
        [SerializeField] private Button button = null!;

        public void Init(PerksUpdateManager _perksUpdateManager, PerksData _perksData)
        {
            perksUpdateManager = _perksUpdateManager;
            perksImage.sprite = perksData.StatImage;
            perksTitle.text = perksData.StatTitle;
            perksDescription.text = perksData.StatDescription;
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            perksUpdateManager.ApplyPerks(perksData.StatName, perksData.AddValue, perksData.Multiplier);
        }
    }
}