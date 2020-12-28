using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pacman : MonoBehaviour
{
    public bool powerful;
    public float speed = 5;
    public float time;    
    public Transform[] border = new Transform[4];
    public Text timeText;
    public Text pointsText;
    
    public AudioSource bonusSound;
    public AudioSource ghostSound;

    private int points;    
    private int enemyDestroyed = 0;
    private int dots = 0;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private GameController GC;

    /*
    private float horizontalInput;
    private float verticalInput;
    */

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        GC = FindObjectOfType<GameController>();
        time = 0;
        points = 0;
    }

    void Update()
    {
        // Check if powerful is active, start the countdown and check it again
        _animator.SetBool("Powerful", powerful);
        if (powerful)
        {
            if (time >= 5)
            {
                time = 0;
                powerful = false;
                ghostSound.Stop();
            }
            
            time += Time.deltaTime; 
            timeText.text = Mathf.Ceil(time).ToString();
        }

        // Updates points values to points text 
        pointsText.text = points.ToString();
    }

    void FixedUpdate()
    {
        // Move - Capture arrow keys and move towards of borders points using speed
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[0].position, speed));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[1].position, speed));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[2].position, speed));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[3].position, speed));
        }

        /*
        // Move
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        // Animation
        _animator.SetFloat("DirX", horizontalInput);
        _animator.SetFloat("DirY", verticalInput);
        */
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Check collision with the SuperDots and if so, activate powerful and destroy the object
        if (other.gameObject.CompareTag("Bonus"))
        {
            powerful = true; 
            time = 0;
            bonusSound.Play();
            Destroy(other.gameObject); 
            ghostSound.Play();
        }

        // Check collision with the Dots and if so, add 10 to points value and then destroy the object
        if (other.gameObject.CompareTag("Ponto"))
        {
            points += 10;
            dots++;
            Destroy(other.gameObject);
        }

        /* Check collision with the Ghosts and then check if powerful is activate, if so, add 100 to points value 
        and then destroy the object, if not, destroy the object and call the Game Over method */
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (powerful)
            {
                points += 100;
                enemyDestroyed++;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
                GC.GameOver();
            }
        }

        // Check if the player arrived at the endline and if it destroyed all the enemies or collected all the dots
        if (other.gameObject.CompareTag("Finish"))
        {
            if(enemyDestroyed == 4)
            {
                Destroy(gameObject);
                GC.Win();
            }
            if(dots == 327)
            {
                GC.Win();
            }
        }
    }
}

