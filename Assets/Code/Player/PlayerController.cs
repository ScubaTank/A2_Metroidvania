using System;
using System.Numerics;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;


public class PlayerController : MonoBehaviour
    {
        [field:SerializeField]
        public PlayerStatsSO Stats { get; private set; }
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private SpriteRenderer playerVisual;
        [SerializeField] private Rigidbody2D playerRb;

        
        private float horizontalInput;
        private int _direction = 1;
        public bool _isGround { get; private set; }= true;

        public bool isWalking { get; private set; } = false;
        public float yVelocity { get; private set; } = 0f;

        private int _jumpsRemaining = 0;
        private float _coyoteTimeRemaining = 0;
        
        private void OnEnable()
        {
            inputManager.OnMove += SetHorizontal;
            inputManager.OnJump += HandleJump;
        }
        
        private void OnDisable()
        {
            inputManager.OnMove -= SetHorizontal;
            inputManager.OnJump -= HandleJump;
        }

        private void SetHorizontal(Vector2 moveVector)
        {
            horizontalInput = moveVector.x;
        }

        public void HandleMove()
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                transform.position + transform.right * horizontalInput,
                Stats.Speed * Time.deltaTime);
        }

        private void Update()
        {
            isWalking = horizontalInput != 0;
            
            if(horizontalInput>0) _direction = 1;
            else if(horizontalInput<0) _direction = -1;
            
            playerVisual.flipX = _direction < 0;

            if(!_isGround){
                _coyoteTimeRemaining -= Time.deltaTime;
            }
        }
        
        private void FixedUpdate()
        {
            CheckGround();
            HandleMove();
            
            yVelocity = playerRb.linearVelocity.y;
        }

        private void HandleJump()
        {
            
            if(_coyoteTimeRemaining <= 0 && _jumpsRemaining <= 0){
                //Debug.Log("coyote time and jumps remaining were 0 or less.");
                return;  
            } 
            if(_jumpsRemaining <= 0){
                //Debug.Log("jumps remaining were 0 or less.");
                return;
            } 

            _jumpsRemaining--;
            
            playerRb.linearVelocity += Vector2.up * Stats.JumpVelocity;
            
        }

        private void CheckGround()
        {
            bool _prevGround = _isGround;
            
            _isGround = Physics2D.Raycast(
                (Vector2)transform.position + Stats.GroundCheckOffset, 
                Vector2.down,
                Stats.GroundCheckDistance,
                Stats.GroundLayer);

            //reset jumpsremaining and coyotetime on entering ground.
            if(_isGround && !_prevGround){
                _jumpsRemaining = Stats.JumpCount;
                _coyoteTimeRemaining = Stats.CoyoteTime;
            }
        }
    }
