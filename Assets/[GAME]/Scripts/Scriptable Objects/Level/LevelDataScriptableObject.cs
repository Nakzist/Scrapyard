using System;
using System.Collections.Generic;
using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Level
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Scrapyard/Data/Level/Level Data")]
    public class LevelDataScriptableObject : ScriptableObject
    {
        [SerializeField] private Level[] levels;
        
        public Level[] Levels => levels;
    }
    
    [Serializable]
    public class Level
    {
        public List<GameObject> enemiesToSpawn;
        public float spawnRate;
        public WinConditions winCondition1;
        public float winCondition1Value;
        public WinConditions winCondition2;
        public float winCondition2Value;
        public WinConditions winCondition3;
        public float winCondition3Value;
    }
}