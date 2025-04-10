using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _moveSpeed;
    private Vector2 _targetPos;
    void Start()
    {
        StartCoroutine
        //basically, coroutine waits and sets targetpos, bat then will move to target pos.
        //once it reaches targetpos, wait again.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
