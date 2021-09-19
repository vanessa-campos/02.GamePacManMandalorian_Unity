using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pacman : MonoBehaviour
{
    [HideInInspector] public bool powerful;

    public float speed = 5;
    public float time;    
    public Transform[] border = new Transform[4];

    public AudioSource bonusSound;
    public AudioSource ghostSound;          
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private GameManager GM;

    /*
    private float horizontalInput;
    private float verticalInput;
    */

    /*private float Time
    {
        get { return time; }
        set
        {
            time = value;
            GM.timeText.text = "LEVEL: " + time;
        }
    }*/        

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        GM = FindObjectOfType<GameManager>();
        time = 0;
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
        }
    }

    void FixedUpdate()
    {
        // Move - Capture arrow keys and move towards of borders points using speed
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[0].position, speed));
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[1].position, speed));
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, border[2].position, speed));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
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
            time = 0;
            powerful = true; 
            bonusSound.Play();
            Destroy(other.gameObject); 
            ghostSound.Play();
        }

        // Check collision with the Dots and if so, add 10 to points value and then destroy the object
        if (other.gameObject.CompareTag("Ponto"))
        {
            GM.Score += 10;
            Destroy(other.gameObject);
        }

        /* Check collision with the Ghosts and then check if powerful is activate, if so, add 100 to points value 
        and then destroy the object, if not, destroy the object and call the Game Over method */
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (powerful)
            {
                GM.Score += 100;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
                GM.Life -= 1;
            }
        }

        // Check if the player arrived at the endline and if it destroyed all the enemies or collected all the dots
        if (other.gameObject.CompareTag("Finish"))
        {
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Destroy(gameObject);
                GM.SwitchState(GameManager.State.LEVELCOMPLETED);
            }
            if(GameObject.FindGameObjectsWithTag("Ponto").Length == 0)
            {
                Destroy(gameObject);
                GM.SwitchState(GameManager.State.LEVELCOMPLETED);
            }
        }
    }
}

