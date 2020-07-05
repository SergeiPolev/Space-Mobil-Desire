using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameSession gameSession;
    float gameDifficult = 1f;
    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 100;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float projectileFirePeriod = 0.1f;
    [Header("Death Settings")]
    [SerializeField] GameObject deathExplosion;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] Level level;

    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    bool dead = false;
    bool deathDone = false;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        SetWorldBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession) gameDifficult = gameSession.GetDifficultModiffier();
        if (!dead)
        {
            Move();
            Fire();
        }
        else if (!deathDone)
        {
            StopMusic();
            Invoke("Explosion", 2f);
            Invoke("GameOver", 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        DamageDealer damageDealer = otherObject.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        if (health > 0) { ProcessHit(damageDealer); }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            dead = true;
        }
    }

    public int GetHealth()
    {
        return health;
    }
    public void AddHealth()
    {
        if (health < 300)
        {
            health += 100;
        }
    }
    private void Explosion()
    {
        GameObject explosion = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        gameObject.SetActive(false);
    }
    private void StopMusic()
    {
        StopCoroutine(firingCoroutine);
        deathDone = true;
        gameObject.GetComponent<AudioSource>().Stop();
    }
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFirePeriod * gameDifficult);
        }
    }
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2 (newXPos, newYPos);
    }
    private void SetWorldBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    void GameOver()
    {
        level.LoadGameOver();
    }
}
