using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rezoskour.Content.waves
{
    
    [Serializable]
    public struct WaveStruct
    {
        public EnemyType enemyType;
        public AnimationCurve curve;
    }

    [CreateAssetMenu(fileName = "WaveData", menuName = "Rezoskour/WaveData", order = 0)]
    public class WaveData : ScriptableObject
    {
        public List<WaveStruct> chasingEnemies;
    }
    
}