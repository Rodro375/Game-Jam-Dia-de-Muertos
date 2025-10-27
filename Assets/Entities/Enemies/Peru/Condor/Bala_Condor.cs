using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala_Condor : MonoBehaviour
{
    [SerializeField] float _speed;
    Vector3 _direction;
    Transform _player;

    Rigidbody2D _rig;
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();

        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _direction = (_player.position - transform.position).normalized;

        _rig.velocity = _direction * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
