using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [HideInInspector] public Pacman pacman;
    [HideInInspector] public GameController GC;
    SpriteRenderer spriteColor;
    private float speed = .15f;
    public Transform[] waypoints;
    int cur = 0;

    private void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position,
                                            waypoints[cur].position,
                                            speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;

        if (pacman.powerful)
        {
            LowVelocity();
            ChangeColor();
        }
        if (!pacman.powerful)
        {            
            spriteColor.color = Color.red;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && pacman.powerful == false)
        {
            Destroy(other.gameObject);
            GC.GameOver();
        }
        if (other.tag == "Player" && pacman.powerful)
        {
            Destroy(gameObject);
        }
    }

    void LowVelocity()
    {
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position,
                                            waypoints[cur].position,
                                            (speed * .5f));
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;
    }
    
    void ChangeColor()
    {
        spriteColor.color = Color.blue;
    }

}
