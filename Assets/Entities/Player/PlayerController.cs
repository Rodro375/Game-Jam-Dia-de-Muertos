using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]float _speed;
    Vector3 _move;
    [Header ("Jump")]
    [SerializeField]float _jumpForce;
    [SerializeField]int _jumpCount;
    [SerializeField] float _raydistance;
    bool _isJumping;

    [Header("Attack")]
    [SerializeField] Transform _positionAttack;
    [SerializeField] float _rangeAttack;
    [SerializeField] float _coldDownAttack;
    bool _hitWall;

    [Header("SpecialAttack")]
    [SerializeField] float _damageSpecialAttack;
    bool _startCharge;
    float _chargeDamage;


    [Header("Parry")]
    bool _parryActive;


    [Header ("HandleWall")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    
    private bool isTouchingWall;
    private bool isWallSliding;

    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpForce;

    private bool isWallJumping;
    private float wallJumpTime = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;


    [Header("Layers")]
    [SerializeField] LayerMask _layerJump;
    [SerializeField] private LayerMask wallLayer;


    //Components
    Rigidbody2D _rigidbody;
    CapsuleCollider2D _capsuleCollider;
    Animator _animator;
    UIPlayer _uiplayer;

    enum States
    {
        Normal,
        Attack,
        SpecialAttack,
        Parry,
        Dash,
        charge
    }

    [SerializeField]States _state;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _uiplayer = GetComponent<UIPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _isJumping = false;
        _startCharge = false;

         _state = States.Normal;
        _hitWall = false;
        _parryActive = false;
        _jumpCount = 1;
    }

    // Update is called once per frame
    void Update()
    {

        switch(_state)
        {
            case States.Normal:
                HandleAnimations();
                SpecialAttack();
                
                

                HandleWallSlide();
                HandleWallJump();
                Jump();
                StartDash();
                ResetJumpCount();
                ResetUICantJump();

                if (Input.GetKeyDown(KeyCode.J)) { RestartHorizontalVelocity();  Attack(); }

                

                    break;

            case States.Attack:               
                break;
            case States.SpecialAttack:
                break;
            case States.Parry:
                RestartHorizontalVelocity();
                break;
            case States.Dash:
                break;
            case States.charge:
                StartChargeAttack();
                SpecialAttackDamage();
                break;
        }

        Parry();
        
        

        if (Input.GetKeyDown(KeyCode.P)) SpawnProyectile();


    }
    void FixedUpdate()
    {
        switch (_state)
        {
            case States.Normal:

                
                ScaleDirection();

                if (!isWallJumping)
                    Movement();

                break;
        }


    }

    void Attack()
    {

        _state = States.Attack;
        _animator.CrossFade("attack", 0.0001f);
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_positionAttack.position, _rangeAttack);
     
        foreach(Collider2D _target in _colliders)
        {
            if(_target.CompareTag("Enemy"))
            {
                //Realizar Accion               
                
            }

        }
        
    }

    public void BackToNormal() => _state = States.Normal;

    void SpecialAttack()
    {
        if (Input.GetKey(KeyCode.M))
        {
            _startCharge = true;
            _state = States.charge;
            _animator.CrossFade("cargado", 0.001f);
        }
    }
    void StartChargeAttack()
    {
         _chargeDamage += Time.deltaTime;
    }

    void SpecialAttackDamage()
    {

        if (Input.GetKeyUp(KeyCode.M))
        {
            //Spawnea Bala y se le asigna esa cantidad de daño cargada al proyectil

            GameObject _bala =(GameObject) Instantiate(Resources.Load("BalaPlayer"),transform.position,Quaternion.identity);

            _bala.GetComponent<Bala_Player>().SetDamage(_chargeDamage);
            _state = States.Normal;

            _chargeDamage = 0;
        }
    }

    void ResetJumpCount()
    {
        if(GroundCheck() || (isTouchingWall && _move.x != 0))
            _jumpCount = 1;
    }

    void ResetUICantJump() 
    {
        if (GroundCheck() && _rigidbody.velocity.y == 0)
        { _uiplayer.SetCantJump(2); _isJumping = false; }
    }

  

    void Parry()
    {
        if(Input.GetKey(KeyCode.I) && GroundCheck())
        {
            _rigidbody.velocity = Vector2.zero;
            _parryActive = true;
            _state = States.Parry;
            _animator.CrossFade("parry", 0.0001f);
            
        }

        if (Input.GetKeyUp(KeyCode.I) && GroundCheck())
        {
            _parryActive = false;
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
            _isJumping=true;
            RestartVerticalVelocity();
            StarJump();
            _jumpCount--;
            _uiplayer.CantJump(1);
            _animator.CrossFade("jump", 0.0001f);
        }
            
    }

    void AdditionalJump()
    {
        if(_isJumping && _jumpCount>0 && Input.GetKeyDown(KeyCode.W))
        {
            RestartVerticalVelocity();
            StarJump();
            _jumpCount--;
            _uiplayer.CantJump(1);
        }
    }

    bool GroundCheck()
    {
        RaycastHit2D _ray = Physics2D.Raycast(transform.position, Vector2.down, _raydistance, _layerJump);

        return _ray.collider;
    }

    

    void RestartVerticalVelocity() => _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
    void RestartHorizontalVelocity() => _rigidbody.velocity = new Vector2(0f,_rigidbody.velocity.y);



    public void ResetAttack() => _state = States.Normal;

    void HandleWallSlide()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);

        if (isTouchingWall && !GroundCheck() && _move.x != 0)
        {
            isWallSliding = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    void HandleWallJump()
    {
        if (isWallSliding && Input.GetKeyDown(KeyCode.W))
        {
            isWallJumping = true;
            Invoke(nameof(StopWallJump), wallJumpTime);

            Vector2 force =Vector2.right*-transform.localScale.x;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(force * wallJumpForce, ForceMode2D.Impulse);

            StarJump();
        }
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    void StartDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
       
        _state = States.Dash;

        
        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;

        _rigidbody.velocity = Vector2.zero;

        _rigidbody.AddForce(Vector2.right * transform.localScale.x * dashForce, ForceMode2D.Impulse);

        _animator.CrossFade("dash", 0.05f);

        yield return new WaitForSeconds(dashDuration);

        _rigidbody.gravityScale = originalGravity;
        _state = States.Normal;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void SpawnProyectile() => Instantiate(Resources.Load("proyectil"), transform.position + Vector3.right * 9f, Quaternion.identity);

    void HandleAnimations()
    {

        // --- Si está en el suelo ---
        if(!_isJumping && _rigidbody.velocity == Vector2.zero && GroundCheck())
        {
            _animator.CrossFade("idle", 0.0001f);
        }
        else if(!_isJumping && _rigidbody.velocity != Vector2.zero && GroundCheck())
        {
            _animator.CrossFade("walk", 0.0001f);
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_positionAttack.position, _rangeAttack);


        Gizmos.DrawLine(transform.position, transform.position + Vector3.down *_raydistance);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("proyectil") )
        {
            //Realizar accion
        }



        if (collision.CompareTag("proyectil") )
        {
            if (_parryActive)
            {
                collision.GetComponent<TestProyectile>().SetSpeed();
                Debug.Log("enmter");
            }
            
        }



    }


   
}
