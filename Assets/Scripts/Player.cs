using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float _playerSpeed = 5f; //player velocity m/s
    private Vector3 _playerVelocity;
    private int _coins;
    private UIManager _uiManager;
    private int _lives =  3;
    [SerializeField]
    private Transform _respawn;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager in Player is null!");
        }
        _controller = GetComponent<CharacterController>();
        if(_controller == null)
        {
            Debug.LogError("Character Controller is null in Player");
        }

        _uiManager.UpdateLives(_lives);
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

    public void CollectCoin()
    {
        _coins++;
        _uiManager.UpdateCoins(_coins);
    }

    public void PlayerDies()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            gameObject.transform.position = _respawn.position;
            controller.enabled = true;
        }
        _lives--;
        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            _uiManager.UpdateLives(_lives);
        }
    }
}
