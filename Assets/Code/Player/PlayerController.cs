using System;
using UnityEngine;


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
        }
        
        private void FixedUpdate()
        {
            CheckGround();
            HandleMove();
            
            yVelocity = playerRb.linearVelocity.y;
        }

        private void HandleJump()
        {
            //if (!_isGround) return;
            _jumpsRemaining--;
            if(_jumpsRemaining <= 0) return;
            playerRb.linearVelocity += Vector2.up * Stats.JumpVelocity;
        }

        private void CheckGround()
        {
            _isGround = Physics2D.Raycast(
                (Vector2)transform.position + Stats.GroundCheckOffset, 
                Vector2.down,
                Stats.GroundCheckDistance,
                Stats.GroundLayer);
            if(_isGround){
                _jumpsRemaining = Stats.JumpCount;
            }
        }

        public void HandleJumpCancel()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawLine((Vector2)transform.position + Stats.GroundCheckOffset, (Vector2)transform.position + Stats.GroundCheckOffset +Vector2.down * Stats.GroundCheckDistance);
        }
    }
