using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Camera camera;
    
    // Life Variables
    private int lives = 3;
    public Text lifeText;

    // Score Variables

    private int scoreValue = 0;
    public Text scoreText;

    public Text winText;
    public Text lossText;

    // Animator Variables
    Animator anim;
    public bool onGround = true;
    public bool isJumping = false;

    // Sound Variables
    public AudioSource musicSource;

    public AudioClip musicClip01;
    public AudioClip musicClip02;
    public AudioClip musicClip03;

    public bool loop;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        SetScoreText();
        SetLifeText();

        anim = GetComponent<Animator>();

        musicSource.clip = musicClip01;
        musicSource.Play();
        musicSource.loop = loop;
        loop = true;
    }
    
    void Update()
    {
        if (isJumping == true)
        {
            anim.SetInteger("state", 2);
        }
    }
    
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        FlipPlayer();
        JumpCheck();
        WalkAnimation();
    }

    // Animation

    private void FlipPlayer()
    {
        Vector3 characterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 1;
        }
        transform.localScale = characterScale;
    }

    private void WalkAnimation()
    {
        if (onGround == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("state", 1);
                Debug.Log("attempting to run left");
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("state", 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("state", 1);
                Debug.Log("attempting to run right");
            }

            if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.W))
            {
                anim.SetInteger("state", 1);
                Debug.Log("attempting to jump right");
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("state", 0);
            }
        }
    }

    private void JumpCheck()
    {
        if (onGround == false)
        {
            anim.SetInteger("state", 2);
            isJumping = true;
        }

        if (rd2d.velocity.y < -0.1)
        {
            onGround = false;

            if (onGround == false)
            {
                anim.SetInteger("state", 2);
                isJumping = true;
            }
        }
    }
    
    // Collisions and Triggers
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide (name) : " + collision.collider.gameObject.name);
        //Debug.Log("collide (tag) : " + collision.collider.gameObject.tag);

        if (collision.collider.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            lives--;
            SetLifeText();
        }

       if (collision.collider.tag == "Ground")
        {
            if (onGround == false)
            {
                onGround = true;
                isJumping = false;
                anim.SetInteger("state", 0);
                Debug.Log("found the ground after jumping");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Coin")
        {
            scoreValue++;
            SetScoreText();
            Destroy(collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                onGround = false;
                isJumping = true;
                anim.SetInteger("state", 2);
                Debug.Log("now off the ground");
            }
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4)
        {
            transform.position = new Vector2(-6.0f, -7.5f);
            camera.transform.position = new Vector3(-6.0f, -9.5f, -10f);
            lives = 3;
            scoreText.text = "Score: " + scoreValue.ToString();
        }

        if (scoreValue >= 8)
        {
            winText.gameObject.SetActive(true);
            winText.text = "You win! Game created by Julia Houha.";
            this.gameObject.SetActive(false);
            musicSource.clip = musicClip02;
            musicSource.Play();
            musicSource.loop = false;
        }

    }

    void SetLifeText()
    {
        lifeText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            lossText.gameObject.SetActive(true);
            lossText.text = "You lose! Game created by Julia Houha.";
            this.gameObject.SetActive(false);

            musicSource.clip = musicClip03;
            musicSource.Play();
            musicSource.loop = false;
        }
    }
}