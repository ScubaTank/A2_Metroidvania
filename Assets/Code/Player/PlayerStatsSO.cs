using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/Player/Stats", order = 0)]
    public class PlayerStatsSO : ScriptableObject
    {
        [field:SerializeField] public float Speed { get; private set; }
        [field:SerializeField] public float JumpVelocity { get; private set; }
        [field:SerializeField] public int JumpCount { get; private set; }
        
        [Header("Ground Check")]
        public float GroundCheckDistance = 0.1f;
        public Vector2 GroundCheckOffset;
        public LayerMask GroundLayer;

    }