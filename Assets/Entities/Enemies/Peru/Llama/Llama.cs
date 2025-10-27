using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llama : MonoBehaviour
{
    enum states{
        idle,
        attack
    }

    

    [SerializeField] states _state=states.idle;

    [SerializeField] float _speed;
    [SerializeField] Transform _positionAttack;
    [SerializeField] float _rangeAttack;


    Transform _player;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(_state)
        {


            case states.idle:

                ScaleDirection();

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(_player.position.x, transform.position.y), _speed * Time.deltaTime);

                if (Vector2.Distance(transform.position,_player.position) < 1.4f)
                {
                    _state = states.attack;
                    StartCoroutine(Attack());
                }
                break;
            case states.attack:
                break;
        }

        
    }

    void ScaleDirection()
    {
        if (_player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void StartAttack()
    {
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_positionAttack.position, _rangeAttack);

        foreach (Collider2D _target in _colliders)
        {
            if (_target.CompareTag("Player"))
            {
                //Realizar Accion               
                Debug.Log("Hit Player");
                _target.GetComponent<UIPlayer>().TakeDamage(1);
            }

        }
    }

    

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        StartAttack();
        yield return new WaitForSeconds(2f);       
        _state = states.idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_positionAttack.position, _rangeAttack);
    }
}
