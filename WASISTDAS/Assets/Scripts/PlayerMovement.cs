using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D player;
    public float player_speed;
    Vector2 movement;
    void Start()
    {
        movement = new Vector2(0, 0);        
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));        
    }
    private void FixedUpdate()
    {
        Debug.Log(Time.deltaTime);
        player.MovePosition(player.position + movement * player_speed * Time.deltaTime);
    }
}
