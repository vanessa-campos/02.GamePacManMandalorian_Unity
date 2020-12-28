using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform[] waypoints;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteColor;
    private Pacman pacman;
    private float speed = .15f;
    private int cur = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteColor = GetComponent<SpriteRenderer>();
        pacman = FindObjectOfType<Pacman>();
    }

    void FixedUpdate()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
            _rigidbody.MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;

        if (pacman.powerful)
        {
            LowVelocity();
            ChangeColor();
        }
        else
        {
            spriteColor.color = Color.white;
        }
    }

    void LowVelocity()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed * .5f);
            _rigidbody.MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;
    }

    void ChangeColor()
    {
        spriteColor.color = Color.green;
    }
}
