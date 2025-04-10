using UnityEngine;

public class Sprinter : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _sprintingSpeed;

    [Header("Edge Detection")]
    [SerializeField] private Vector2 _edgeOffset;
    [SerializeField] private float _edgeDropLength;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Player Detection")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightRange;

    [Header("Sprites")]
    [SerializeField] private Sprite _walkSprite;
    [SerializeField] private Sprite _sprintSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _roaming;
    private bool _movingRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _movingRight = true;
        _roaming = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckEdge() && _roaming){
            _movingRight = !_movingRight;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        if(CheckPlayer()){
            _roaming = false;
            _spriteRenderer.sprite = _sprintSprite;
        }

        if(_roaming){   
            if(_movingRight){
                transform.position = new Vector2(transform.position.x + (_walkingSpeed * Time.deltaTime), transform.position.y);
            } else {
                transform.position = new Vector2(transform.position.x - (_walkingSpeed * Time.deltaTime), transform.position.y);
            }
        } else {
            if(_movingRight){
                transform.position = new Vector2(transform.position.x + (_sprintingSpeed * Time.deltaTime), transform.position.y);
            } else {
                transform.position = new Vector2(transform.position.x - (_sprintingSpeed * Time.deltaTime), transform.position.y);
            }
        }
    }

    private bool CheckEdge(){
        if(_movingRight){
            return !Physics2D.Raycast(
            (Vector2)transform.position + _edgeOffset, 
            Vector2.down,
            _edgeDropLength,
            _groundLayer);
        } else {
            return !Physics2D.Raycast(
            (Vector2)transform.position - _edgeOffset, 
            Vector2.down,
            _edgeDropLength,
            _groundLayer);
        }
    }

    private bool CheckPlayer(){
        if(_movingRight){
            return Physics2D.Raycast(
            (Vector2)transform.position, 
            Vector2.right,
            _sightRange,
            _playerLayer);
        } else {
            return Physics2D.Raycast(
            (Vector2)transform.position, 
            Vector2.left,
            _sightRange,
            _playerLayer);
        }
    }
}
