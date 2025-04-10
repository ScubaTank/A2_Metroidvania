using NUnit.Framework.Constraints;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player"){
            PlayerController _pc = (PlayerController)collision.gameObject.GetComponent("PlayerController");
            _pc.GetCollectible(_value);
            Destroy(gameObject);
        }
    }
}
