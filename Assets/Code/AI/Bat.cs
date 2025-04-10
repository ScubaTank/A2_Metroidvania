using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _moveSpeed;
    private Vector2 _targetPos;

    private bool _moving;
    void Start()
    {
        _moving = false;
        StartCoroutine(NextTarget());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);
        if((Vector2)transform.position == _targetPos && _moving){
            _moving = false;
            StartCoroutine(NextTarget());
        }
    }

    private IEnumerator NextTarget(){
        yield return new WaitForSeconds(_delay);
        if(GameObject.Find("Player") != null){
            _targetPos = GameObject.Find("Player").transform.position;
            _moving = true;
        }
    }
}
