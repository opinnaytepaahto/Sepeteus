using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float maxHealth = 50f;
    public float currentHealth = 0f;

    private float timer = 3f;

    private bool firstDestroy = true;

    private float shootTimer = 1.5f;

    private bool isFacingRight;

    public GameObject healthBar;

    public GameObject bullet;

    public GameObject player;

    public GameObject coin;
    public GameObject oxygen;
    public GameObject health;

    private Rigidbody2D physics;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;

        physics = GetComponent<Rigidbody2D>();

        // InvokeRepeating("decreaseHealth", 1f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (player)
        {
            if (player.transform.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                isFacingRight = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                isFacingRight = false;
            }
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = 0;
        }

        if (shootTimer == 0 && currentHealth > 0)
        {
            bullet.GetComponent<BulletController>().facingRight = isFacingRight;

            transform.GetChild(2).GetComponent<AudioSource>().Play();

            if (isFacingRight)
            {
                Instantiate(bullet, transform.position + new Vector3(-0.65f, 0.1225f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(bullet, transform.position + new Vector3(0.68f, 0.1225f, 0), Quaternion.identity);
            }

            shootTimer = 1.5f;
        }

        if (currentHealth <= 0)
        {
            if (firstDestroy)
            {
                GameObject.Find("Spaceman").GetComponent<PlayerController>().score += 10;

                transform.GetChild(1).GetComponent<AudioSource>().Play();

                GetComponent<Rigidbody2D>().isKinematic = true;
                Destroy(transform.GetChild(0).gameObject);
                Destroy(GetComponent<PolygonCollider2D>());

                StartCoroutine(spawnPickups());

                firstDestroy = false;
            }

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Destroy(gameObject);
                Destroy(this);
            }
        }
	}
    
    public void decreaseHealth(float amount)
    {
        currentHealth -= amount;

        float tempHealth = currentHealth / maxHealth;
        setHealthBar(tempHealth);
    }

    public void setHealthBar(float health)
    {
        healthBar.transform.localScale = new Vector2(Mathf.Clamp(health, 0f, 1f), healthBar.transform.localScale.y);
    }

    IEnumerator spawnPickups()
    {
        for (int i = 0; i < Random.Range(1, 5); i++)
        {
            Instantiate(coin, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }

        if (Random.Range(0, 10) > 8)
        {
            for (int i = 0; i < 1; i++)
            {
                Instantiate(oxygen, transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < 1; i++)
            {
                Instantiate(health, transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }

        
    }
}
