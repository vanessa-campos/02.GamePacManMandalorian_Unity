using System.Collections;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    [HideInInspector] public bool powerful;
    public float speed = 5;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private float horizontalInput;
    private float verticalInput;
    private GameController GC;
    private float timeI = 0;
    public float timeF = 200;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Move
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        // Animation
        _animator.SetFloat("DirX", horizontalInput);
        _animator.SetFloat("DirY", verticalInput);

        if (powerful)
        {
            timeI++;
        }
        if (powerful && timeI == timeF)
        {
            powerful = false;
            timeI = 0;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bonus")
        {
            timeI = 0;
            powerful = true;
            Destroy(other.gameObject);            
                    
        }
    }

}
