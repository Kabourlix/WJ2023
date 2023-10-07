// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using UnityEngine;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content.Misc
{
    public static class Helper
    {
        public static bool HasNot(this LayerMask _layerMask, int _layer)
        {
            return ((1 << _layer) & _layerMask) == 0;
        }

        public static bool Has(this LayerMask _layerMask, int _layer)
        {
            return ((1 << _layer) & _layerMask) == _layerMask;
        }
    }
}