using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCBehaviourForces : MonoBehaviour {
    private Rigidbody _rb;

    [SerializeField]
    private float _acceleration;

    private bool _jump;

    [SerializeField]
    private float _jumpHeight;
	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
	}

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }
    private void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        _rb.AddForce(input * _acceleration, ForceMode.Impulse);
    }

    private void Jump()
    {
        if (_jump && this.gameObject.transform.position.y <=0)
        {
            _rb.AddForce(Vector3.up * _rb.mass * _jumpHeight, ForceMode.Impulse);
            _jump = false;
        }
    }

    private void LimitMaxVelocity()
    {

    }

 
}
