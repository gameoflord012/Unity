using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static PlayerDelegate OnPlayerDied;
    public static PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    Rigidbody2D rigidBody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager gameManager;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        rigidBody.simulated = false;

        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (gameManager.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            gameManager.FlyAudio.Play();
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero; 
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnEnable()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager.GameOver) return;
        if (collision.collider.tag == "DeadZone")
        {
            gameManager.HitAudio.Play();
            transform.rotation = downRotation;
            OnPlayerDied(); // event sent to GameManager
        }
        else if (collision.collider.tag == "ScoreZone")
        {
            gameManager.PointAudio.Play();
            OnPlayerScored(); // event sent to GameManager
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GameOver) return;
        if (collision.tag == "ScoreZone")
        {
            gameManager.PointAudio.Play();
            OnPlayerScored(); // event sent to GameManager
        } else if (collision.tag == "DeadZone")
        {
            gameManager.HitAudio.Play();
            transform.rotation = downRotation;
            OnPlayerDied(); // event sent to GameManager
        }
    }

    private void OnGameOverConfirmed()
    {
        
    }

    private void OnGameStart()
    {
        rigidBody.simulated = true;
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
