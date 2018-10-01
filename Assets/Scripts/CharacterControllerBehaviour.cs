using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterControllerBehaviour : MonoBehaviour {

    [SerializeField]
    private Transform _absoluteForward;

    [SerializeField]
    private float _acceleration;
    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rb;

    private Vector3 _movement;
    private bool _jump;

    private Vector3 _velocity;

    [SerializeField]
    private float _jumpHeight;

	void Start () {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
	}

#if DEBUG
    //assertions
#endif

    void Update () {

        //Debug.Log(_jump);
        if (Input.GetButtonDown("Jump")&& IsGrounded())
        {
            _jump = true;
        }

        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
	}


    private void FixedUpdate()
    {
        ApplyGround();

        IsGrounded();

        ApplyMovement();

        ApplyJump();


        _rb.MovePosition(transform.position + (_velocity * Time.deltaTime));
    }
    private void ApplyGround()
    {
        if (_isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
        }
    }
    private void LimitMaxSpeed()
    {


    }


    private void ApplyMovement()
    {
        if (IsGrounded())
        {
            Vector3 absoluteTransform = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));
            Quaternion absoluteRotation = Quaternion.LookRotation(absoluteTransform);
            Vector3 relativeForward = absoluteRotation * _movement;

            _velocity += relativeForward * _acceleration * Time.fixedDeltaTime;

        }
    }
    private void ApplyJump()
    {
        if (_jump && IsGrounded())
        {
            Debug.Log(_jump);
            //_isGroundedNeedsUpdate = true;
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
        }
    }

    bool _isGrounded = false;
    bool _isGroundedNeedsUpdate = true;


    private bool IsGrounded() 
    {
        if (_isGroundedNeedsUpdate)
        {

        Vector3 rayCenter = _capsuleCollider.center;
        float rayLength = _capsuleCollider.bounds.extents.y + 0.1f ;
        float sphereRadius = _capsuleCollider.radius * 0.9f;
        RaycastHit hitInfo;
        bool isGround =  Physics.Raycast(rayCenter, Vector3.down,out hitInfo, rayLength);
        bool isGround2 = Physics.SphereCast(rayCenter, sphereRadius,Vector3.down, out hitInfo, rayLength);

            //if (isGround)
            //{
            //    Vector3 surfaceNormal = hitInfo.normal;
            //    return Vector3.Dot(Vector3.up, surfaceNormal) > 0.5f;
            //}

            ApplyGround();

            // return false;
         _isGrounded = isGround && Vector3.Dot(hitInfo.normal, Vector3.up) > 0.5f;
         _isGroundedNeedsUpdate = false;
        }

        return _isGrounded;
    }

    private List<GameObject>  _ground = new List<GameObject>();
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if(Vector3.Dot(contact.normal,Vector3.up) > 0.5)
            {
                _isGrounded = true;
                _ground.Add(  collision.gameObject);
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.Equals(_ground))
        {
            _isGrounded = false;
            _ground.Remove(collision.gameObject);
        }

        if (_ground.Remove(collision.gameObject))
        {
            _isGrounded = (_ground.Count > 0);
        }

    }
}
