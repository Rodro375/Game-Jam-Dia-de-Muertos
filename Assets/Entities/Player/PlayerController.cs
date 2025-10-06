using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]float _speed;
    [SerializeField]float _jumpForce;
    Vector3 _move;

    [Header("Attack")]
    [SerializeField] Transform _positionAttack;
    [SerializeField] float _rangeAttack;
    [SerializeField] float _coldDownAttack;
    bool _hitWall;

    [Header ("Layers")]
    [SerializeField] LayerMask _layerJump;
    

    //Components
    Rigidbody2D _rigidbody;
    CapsuleCollider2D _capsuleCollider;
    Animator _animator;

    enum States
    {
        Normal,
        Attack,
        SpecialAttack,
        Parry
    }

    [SerializeField]States _state;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _state = States.Normal;
        _hitWall = false;
    }

    // Update is called once per frame
    void Update()
    {

        switch(_state)
        {
            case States.Normal:

                
                Jump();

                if ( Input.GetKeyDown(KeyCode.J)) Attack();

                    break;
            case States.Attack:               
                break;
            case States.SpecialAttack:
                break;
            case States.Parry:
                break;
        }


        HandleAnimations();



    }
    void FixedUpdate()
    {
        switch (_state)
        {
            case States.Normal:

                Movement();
                ScaleDirection();
                break;
        }


    }

    void Attack()
    {
        if (GroundCheck()) _hitWall = false;

        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_positionAttack.position, _rangeAttack);
     
        foreach(Collider2D _target in _colliders)
        {
            if(_target.CompareTag("Enemy"))
            {
                //Realizar Accion               
                
            }

            if (_target.CompareTag("Wall") && !GroundCheck())
            {
                //Realizar Accion
                _hitWall = true;
                RestartVerticalVelocity();
                StarJump();
            }



        }

        if (!_hitWall) 
        {
            //_animator.CrossFade("attack", 0.0001f);
            _state = States.Attack;
           
        }

        
    }

    void SpecialAttack()
    {

    }

    void Parry()
    {
        if(Input.GetKey(KeyCode.I))
        {
            _state = States.Parry;
            _animator.CrossFade("Parry", 0.0001f);
        }
        else
        {
            _state = States.Normal;
        }
    }

    void ScaleDirection()
    {
        if(_rigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        _move = new Vector2(x * _speed, _rigidbody.velocity.y) ;

        _rigidbody.velocity = _move ;

    }

    void StarJump()
    {
        _rigidbody.AddForce(Vector2.up*_jumpForce, ForceMode2D.Impulse);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && GroundCheck())
        {
            StarJump();
            //_animator.CrossFade("jump", 0.0001f);
        }
            
    }

    bool GroundCheck()
    {
        RaycastHit2D _ray = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, _layerJump);

        return _ray.collider;
    }

    void RestartVerticalVelocity() => _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);

    public void ResetAttack() => _state = States.Normal;


    IEnumerator ResetWallJump()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        _state = States.Normal;

    }

    void HandleAnimations()
    {

        // --- Si está en el suelo ---
        float x = Input.GetAxisRaw("Horizontal");
        if(GroundCheck())
        {
            /*if (Mathf.Abs(x) > 0.1f)
                _animator.CrossFade("move", 0.1f);
            else
                _animator.CrossFade("idle", 0.1f);*/
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_positionAttack.position, _rangeAttack);
    }
}
