using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Scriptable_Objects.Player
{
    [CreateAssetMenu(fileName = "Player Hud Data", menuName = "Scrapyard/Data/Player/Player Hud Data")]
    public class PlayerHudDataScriptableObject : ScriptableObject
    {
        #region Serialized Variables

        [SerializeField] private GameObject hudPrefab;
        [FormerlySerializedAs("levelSprites")] [SerializeField] private List<Sprite> waveSprites;

        #endregion

        #region Public Variables

        public GameObject HudPrefab => hudPrefab;
        public List<Sprite> WaveSprites => waveSprites;

        #endregion
    }
}