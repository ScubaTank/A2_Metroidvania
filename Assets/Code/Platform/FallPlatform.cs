using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField] private float _despawnTime;

    private bool _falling;

    void Awake()
    {
        _rb.gravityScale = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && !_falling){
            _rb.gravityScale = 1;
            StartCoroutine(Despawn());
            _falling = true;
        }    
    }

    private IEnumerator Despawn(){
         yield return new WaitForSeconds(_despawnTime);
         Destroy(gameObject);
    }
}
