using UnityEngine;

public class MultiMovePlatform : MonoBehaviour
{

    [Header("Movement Points")]
    [SerializeField] private Transform[] _positions;

    [Header("Settings")]
    [SerializeField] private bool _startOnTouch;
    [SerializeField] private bool _pingPong; //does the platform traverse backwards once it reaches it's goal.

    [SerializeField] private float _moveSpeed;

    private int _nextPosIdx = 0;
    private bool _forwards;
    private bool _activated;

    void Start()
    {
        if(!_startOnTouch){
            _activated = true;
        }
        _forwards = true;
    }

    void Update()
    {
        if(_activated){
            if(_nextPosIdx < _positions.Length){
                MovePlatform();
            } else {
                //if we get here, it means we reached the end of the array. 
                if(_pingPong){
                    _forwards = !_forwards;
                } else {
                    _nextPosIdx = 0;
                }
            }
        }
    }

    private void MovePlatform(){

        if(!_activated){
            return;
        }

         //move
        transform.position = Vector2.MoveTowards((Vector2)transform.position, (Vector2)_positions[_nextPosIdx].position, _moveSpeed * Time.deltaTime);

        //check if at target
        if((Vector2)transform.position == (Vector2)_positions[_nextPosIdx].position){

            //if we reach the edges of array, loop or pingpong.
            if((_nextPosIdx+1 == _positions.Length  && _forwards) || (_nextPosIdx == 0 && !_forwards)){
                if(_pingPong){
                    _forwards = !_forwards;
                } else {
                    _nextPosIdx = 0;
                }
            }

            //increment/decrement index. 
            if(_forwards){
                _nextPosIdx++;
            } else {
                _nextPosIdx--;
            }
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && !_activated){
            _activated = true;
        }    
    }
}
