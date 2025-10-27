using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala_Player : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _damage;

    Transform _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _speed *= _player.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag ("Wall")) { Destroy(gameObject); }
    }

    public void SetDamage(float damage) => _damage = damage;     
    
}
