using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProyectile : MonoBehaviour
{
    [SerializeField] float _speed;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }

    public void SetSpeed()
    {
        _speed *= -1f;
    }
}
