using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAddForceBehaviour : MonoBehaviour {

    private Rigidbody _rb;
    [SerializeField]
    private float _acceleration;
    private bool _jump;
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
        if (_jump)
        {
            _rb.AddForce(Vector3.up *  _rb.mass * _acceleration,ForceMode.Impulse);
            _jump = false;
        }
    }
}
