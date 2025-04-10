using UnityEngine;

public class Worm : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private Vector2 _edgeOffset;
    [SerializeField] private float _edgeDropLength;
    [SerializeField] private LayerMask _groundLayer;


    private bool _movingRight;


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

    void Update()
    {
        if(CheckEdge()){
            _movingRight = !_movingRight;
            Debug.Log("worm switched dir!");
        }

        if(_movingRight){
            transform.position = new Vector2(transform.position.x + (_walkingSpeed * Time.deltaTime), transform.position.y);
        } else {
            transform.position = new Vector2(transform.position.x - (_walkingSpeed * Time.deltaTime), transform.position.y);
        }
    }
}
