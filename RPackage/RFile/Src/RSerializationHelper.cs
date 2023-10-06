// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 06

#nullable enable

using System.Collections.Generic;
using System.Text;

namespace Rezoskour.RFile
{
    public static class RSerializationHelper
    {
        public static string ToJson<TKey,TValue>(this Dictionary<TKey,TValue> _dictionary)
        {
            StringBuilder sb = new("{");
            foreach (KeyValuePair<TKey,TValue> kv in _dictionary)
            {

            }



            sb.Append("}");
            return sb.ToString();
        }
    }
}