using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private bool _groundPlayer;
    private float _yVelocity;
    [SerializeField]
    private float _gravity = 9.8f; //gravity acceleration m/s/s
    private bool _doubleJump;
    [SerializeField]
    private float _jumpHeight = 6.0f;
    private float _xVelocity;
    [SerializeField]
    private float _playerSpeed = 3f; //player velocity m/s
    private Vector3 _playerVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if(_controller == null)
        {
            Debug.LogError("Character Controller is null in Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check is player grounded on previous move 
        _groundPlayer = _controller.isGrounded;
        if(_groundPlayer == true && _yVelocity < 0)
        {
            _yVelocity = 0f;
        }
        //add jump function
        if (_groundPlayer == true && Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = _jumpHeight;
            _doubleJump = true;
        }
        else
        {
            //add double jump
            if (_doubleJump == true && Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _doubleJump = false;
            }
            //add gravity
            else
            {
                _yVelocity -= _gravity * Time.deltaTime;
            }
        }
        //player input to move jump, left & right
        _xVelocity = Input.GetAxis("Horizontal") * _playerSpeed;
        _playerVelocity = new Vector3(_xVelocity, _yVelocity, 0f);
        _controller.Move(Time.deltaTime * _playerVelocity);
    }
}
