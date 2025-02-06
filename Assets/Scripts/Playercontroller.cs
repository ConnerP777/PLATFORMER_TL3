using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Playercontroller : MonoBehaviour
{

    //inputs
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;
    [SerializeField] private KeyCode _jump = KeyCode.W;


    [SerializeField] private float _maxSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _friction = 10.0f;
    [SerializeField] private float _fallThreshold = -10f;


    private Rigidbody2D _rb = null;
    private bool _isGrounded = false;
    private Vector2 _startingPosition;

    //animation
    public Animator animator;
    private bool _facingRight = true;

    //Health
    public Image healthBar;
    private float healthAmount = 100.0f;
    private float damage = 50.0f;

    private bool bcMode = false;




    public CanvasGroup gameOverCanvas; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (!_rb)
        {
            Debug.Log("Failed to get Rb");
        }

        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
        GroundCheck();
        UpdateBCMode();

        animator.SetFloat("Speed", Mathf.Abs(_rb.linearVelocity.x));

        if (healthAmount <= 0 || transform.position.y < _fallThreshold)
        {
            resetToStart();
            gameOver();
        }


        
    }

    void GetPlayerMovement()
    {
        if (Input.GetKey(_left))
        {
            _rb.linearVelocityX = -1 * _maxSpeed;
            if(_facingRight)
            {
                Flip();
            }

        }

        else if (Input.GetKey(_right))
        {
            Debug.Log("Right");
            _rb.linearVelocityX = _maxSpeed;
            if (!_facingRight)
            {
                Flip();
            }

        }
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0.0f, Time.deltaTime * _friction);

        }

        if (Input.GetKeyDown(_jump) && _isGrounded)
        {
            _rb.linearVelocityY = _jumpForce;
            animator.SetBool("isJumping", true);
            _isGrounded = false;
        }
        else if (!Input.GetKeyDown(_jump))
        {
            animator.SetBool("isJumping", false);
        }

    }

    void GroundCheck()
    {
        // Define the length of the raycast slightly below the player's collider
        float rayLength = 0.2f; // Increased the ray length for better ground detection
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (GetComponent<BoxCollider2D>().size.y / 2));

        // Cast a ray downwards from the player's position
        RaycastHit2D groundHit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength);

        // Check if the raycast hit any collider
        if (groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        // Optional: Draw the ray in the Scene view for debugging
        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
    }

    public void hurt()
    {
        if (!bcMode)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            takeDamage(damage);
            Debug.Log("Player Hurt");
            Debug.Log("Health: " + healthAmount);
        }
    }
    public void takeDamage(float damage) 
    {
        if (!bcMode)
        {
            healthAmount -= damage;
            healthAmount = Mathf.Clamp(healthAmount, 0, 100);
            healthBar.fillAmount = healthAmount / 100f;
            _maxSpeed = 7.0f;
            Debug.Log("Health: " + healthAmount);
        }

    }

    private void UpdateBCMode()
    {
        bcMode = PlayerPrefs.GetInt("BCMode", 0) == 1;
    }

    public void gameOver()
    {
        //gameOverCanvas.alpha = 1.0f;
        //gameOverCanvas.interactable = true;
        //gameOverCanvas.blocksRaycasts = true;

        SceneManager.LoadScene(2);
    }


    public void resetToStart()
    {
        transform.position = _startingPosition;
        _rb.linearVelocity = Vector2.zero;
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
