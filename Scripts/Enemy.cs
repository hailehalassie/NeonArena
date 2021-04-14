using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 5f;

    [Header("Pick Ups")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinSpeed = 3f;

    [Header("SoundFX")]
    [SerializeField] AudioClip enemyDeadSFX;
    [SerializeField] AudioClip enemyLaserSFX;
    [SerializeField] float deadSFXVolume = 0.7f;
    [SerializeField] float LaserSFXVolume = 0.6f;

    public Animator animator;

    
    // Use this for initialization
    void Start () {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update () {
        CountDownAndShoot();
	}

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed-2f);
        AudioSource.PlayClipAtPoint(enemyLaserSFX, Camera.main.transform.position, LaserSFXVolume );
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Disc")
        {
            DamageDealer damageDealer2 = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer2) { return; }
            ProcessHit(damageDealer2);
        }
        else
        {
            DamageDealer damageDealer1 = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer1) { return; }
            ProcessHitDisc(damageDealer1);
        }

    }

    private void ProcessHit(DamageDealer damageDealer1)
    {
        health -= damageDealer1.GetDamage1();
        damageDealer1.Hit();
        if (health <= 0)
        {
            StartCoroutine(DestroyEnemy());
        }
    }

    private void ProcessHitDisc(DamageDealer damageDealer2)
    {
        health -= damageDealer2.GetDamage2();
        if (health <= 0)
        {
            StartCoroutine(DestroyEnemy());
        }
    }

    IEnumerator DestroyEnemy()
    {
            animator.SetBool("dead", true);
            AudioSource.PlayClipAtPoint(enemyDeadSFX, Camera.main.transform.position, deadSFXVolume);
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            SpawnCoin();
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
    }

    public void SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity) as GameObject;
        coin.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -coinSpeed);
    }
}
