using System;
using System.Numerics;
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
        public bool _isWall { get; private set; }= false;
        

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
            //Debug.Log("Coyote Time: " + _coyoteTimeRemaining);
            
            
        }
        
        private void FixedUpdate()
        {
            CheckGround();
            CheckWall();
            HandleMove();
            
            yVelocity = playerRb.linearVelocity.y;
        }

        private void HandleJump()
        {

            if(_isWall){
                if(_direction > 0){
                    playerRb.linearVelocity += (Vector2.up + Vector2.left) * Stats.JumpVelocity;
                } else {
                    playerRb.linearVelocity += (Vector2.up + Vector2.right) * Stats.JumpVelocity;
                }
                
            }
            
            if(_coyoteTimeRemaining <= 0 && _jumpsRemaining <= 0){
                Debug.Log("coyote time and jumps remaining were 0 or less.");
                return;  
            } 
            if(_jumpsRemaining <= 0){
                Debug.Log("jumps remaining were 0 or less.");
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

            //reset coyote time
            if(_isGround && !_prevGround){
                //Debug.Log("Touched Ground!");
                _jumpsRemaining = Stats.JumpCount;
                _coyoteTimeRemaining = Stats.CoyoteTime;
            }
        }

        private void CheckWall()
        {
            bool _prevWall = _isWall;
            
            Vector2 dir = Vector2.right;
            if(playerVisual.flipX)
            {
               dir = Vector2.left;
            } 

            _isWall = Physics2D.Raycast(
                (Vector2)transform.position + Stats.WallCheckOffset,
                dir,
                Stats.WallCheckDistance,
                Stats.WallLayer);
            
            if(!_prevWall && _isWall){
                Debug.Log("Touched Wall!");
            }
        }

        public void HandleJumpCancel()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine((Vector2)transform.position + Stats.GroundCheckOffset, 
            (Vector2)transform.position + Stats.GroundCheckOffset +Vector2.down * Stats.GroundCheckDistance);

            //Gizmos.color = Color.green;
            Gizmos.DrawLine((Vector2)transform.position + Stats.WallCheckOffset, 
            (Vector2)transform.position + Stats.WallCheckOffset +Vector2.right * Stats.WallCheckDistance);
        }
    }
