using System;
using System.Numerics;
using TMPro;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Vector2 = UnityEngine.Vector2;


public class PlayerController : MonoBehaviour
    {
        [field:SerializeField]
        public PlayerStatsSO Stats { get; private set; }
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private SpriteRenderer playerVisual;
        [SerializeField] private Rigidbody2D playerRb;

        [Header("UI Management")]
        [SerializeField] private UIManager _uiManager;
        
        private int _health = 3;
        private int _score = 0;


        private float horizontalInput;
        private int _direction = 1;
        public bool _isGround { get; private set; }= true;

        public bool isWalking { get; private set; } = false;
        public float yVelocity { get; private set; } = 0f;

        private int _jumpsRemaining = 0;
        private float _coyoteTimeRemaining = 0;

    void Awake()
    {
        _uiManager.UpdateUI(_health, _score);
    }

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

        public void GetCollectible(int value){
            _score += value;
            _uiManager.UpdateUI(_health, _score);
        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy"){
            _health -= 1;
            _uiManager.UpdateUI(_health, _score);
        }

        if(_health <= 0){
            _health = 0; //incase you're playing in the editor.
            Application.Quit();
        }
    }
}
