using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Condor : MonoBehaviour
{
    [SerializeField] float _minPosY;
    [SerializeField] float _maxPosY;
    [SerializeField] float _verticalSpeed = 1.5f;
    [SerializeField] float _horizontalAmplitude = 0.5f;
    [SerializeField] float _horizontalSpeed = 1f;

    private float _startX;
    private float _startY;

    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _startX = transform.position.x;
        _startY = transform.position.y;

        StartCoroutine(SpawnBullet());
        StartCoroutine(BlinkLoop());
    }

    void Update()
    {
       
        float t = Mathf.PingPong(Time.time * _verticalSpeed, 1f);
        float newY = Mathf.Lerp(_minPosY, _maxPosY, t);

        float newX = _startX + Mathf.Sin(Time.time * _horizontalSpeed) * _horizontalAmplitude;

        transform.position = new Vector3(newX, _startY+ newY, transform.position.z);
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0;
        Color c = _spriteRenderer.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / duration);
            c.a = a;
            _spriteRenderer.color = c;
            yield return null;
        }
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return Fade(1f, 0f, 5f); 
            yield return Fade(0f, 1f, 3f);
        }
    }


    IEnumerator SpawnBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Instantiate(Resources.Load("Bala_Condor"), transform.position, quaternion.identity);

        }
    }
}
