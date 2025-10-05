using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]float _speed;
    [SerializeField]float _jumpForce;
    Vector3 _move;

    [Header ("Layers")]
    [SerializeField] LayerMask _layerJump;
    

    //Components
    Rigidbody2D _rigidbody;
    CapsuleCollider2D _capsuleCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && GroundCheck()) Jump();
    }
    void FixedUpdate()
    {
        Movement();
        
        
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        _move = new Vector2(x, 0) *_speed;

        transform.position += _move * Time.fixedDeltaTime;

    }

    void Jump()
    {
        _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, _jumpForce ), ForceMode2D.Impulse);
    }

    bool GroundCheck()
    {
        RaycastHit2D _ray = Physics2D.Raycast(_capsuleCollider.bounds.size, Vector2.down, 1f, _layerJump);

        return !_ray;
    }
}
