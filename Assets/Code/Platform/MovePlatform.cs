using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _moveSpeed;

    private Vector2 _targetPos;

    void Start()
    {
        _targetPos = _pointB.position;   
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);
        if((Vector2)transform.position == _targetPos){
            if(_targetPos == (Vector2)_pointB.position){
                _targetPos = _pointA.position;
            } else {
                _targetPos = _pointB.position;
            }
            
        }

    }
}
