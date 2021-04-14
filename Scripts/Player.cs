using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float padding = 1f;
    [SerializeField] float paddingTop = 6f;
    

    [Header("Shooting")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserPrefab1;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileFiringPeriod = 0.2f;
    [SerializeField] float projectileSpeedSecondary = 4f;
    [SerializeField] int scoreValue = 100;
    [SerializeField] int projectileSecRestriction = 5;

    [Header ("Health")]
    [SerializeField] int health = 200;
    bool shot = false;
    int timesDied = 0;
    [SerializeField] int numberOfLives;
    public Image[] lives;
    public Sprite life;
    

    [Header("Sound FX")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] AudioClip laserSound1;
    [SerializeField] AudioClip playerDeadSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.9f;
    [SerializeField] float deadSFXVolume = 0.9f;

    Coroutine fireCoroutine;

    GameSession gameSession1;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    int scoreForUpgrade;


    // Setting move boundaries and game session
    void Start()
    {
        SetUpMoveBoundaries();
        gameSession1 = FindObjectOfType<GameSession>();
       

    }

    // Calling methods for moving, firing and upgrading weapon
    void Update()
    {
        Move();
        Fire();
        scoreForUpgrade = gameSession1.GetScore();
        FireSecondary();
        
    }

   
    // Method for player shooting
    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
           fireCoroutine =  StartCoroutine(FireContinuously());
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);
        }
    }

    private void FireSecondary()
    {
        if (scoreForUpgrade >= 400)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (shot == false)
                {
                    GameObject laserSecondary = Instantiate(laserPrefab1, transform.position, Quaternion.identity) as GameObject;
                    laserSecondary.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeedSecondary);
                    AudioSource.PlayClipAtPoint(laserSound1, Camera.main.transform.position, laserSFXVolume);
                    shot = true;
                    RestricFire2();
                }
            }
        }
    }

    private void RestricFire2()
    {
        if (shot)
        {
            StartCoroutine(CountdownForFire2());
        }
    }

    IEnumerator CountdownForFire2()
    {
        yield return new WaitForSeconds(projectileSecRestriction);
        shot = false;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);

        

    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingTop;

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSFXVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.gameObject.tag == "Coin")
            {
                Destroy(other.gameObject);
                FindObjectOfType<GameSession>().AddToScore(scoreValue);
            }
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

         /*   if (!damageDealer)
            {
                for (int i = 0; i < lives.Length; i++)
                {
                    if (i < numberOfLives)
                    {
                        lives[i].enabled = true;
                        
                    }
                    else
                    {
                        lives[i].enabled = false;
                    }
                    
                }

                return;
            }    */
            ProcessHit(damageDealer);


            /*
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }  
            ProcessHit(damageDealer); */

        }
    }
    
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage1();
        damageDealer.Hit();
        numberOfLives--;
        if (health <= 0)
        {
           /*
            if (numberOfLives >= 1 )
            {
                Destroy(gameObject);
                AudioSource.PlayClipAtPoint(playerDeadSFX, Camera.main.transform.position, deadSFXVolume);
                
               
                
            }
            else
            {   */
                Destroy(gameObject);
                AudioSource.PlayClipAtPoint(playerDeadSFX, Camera.main.transform.position, deadSFXVolume);
                FindObjectOfType<Level>().LoadGameOver();
         //   }
        }
    }

   

    public int GetHealth()
    {
        return health;
    }

    

}


