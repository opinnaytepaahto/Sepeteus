using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D physics;

    public float maxOxygen = 500f;
    public float currentOxygen = 0f;

    public Transform groundDetector;
    public bool grounded = true;

    public float maxSpeed = 10f;

    public float speed = 300f;
    public float jumpSpeed = 30f;

    public bool isFacingRight = false;

    public float shootTimer = 0.3f;

    public GameObject bullet;
    public GameObject oxygenBar;

    public int score = 0;

	// Use this for initialization
	void Start ()
    {
        physics = GetComponent<Rigidbody2D>();

        InvokeRepeating("decreaseOxygen", 0.5f, 0.5f);

        currentOxygen = maxOxygen;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            physics.AddForce(new Vector2(speed * Time.deltaTime, 0f));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            physics.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
        }
        else
            if (grounded)
                physics.velocity = new Vector2(Vector2.zero.x, physics.velocity.y);

        physics.velocity = new Vector2(Mathf.Clamp(physics.velocity.x, -maxSpeed, maxSpeed), physics.velocity.y);

        if (Input.GetKey(KeyCode.A) && !isFacingRight)
            Flip();
        else if (Input.GetKey(KeyCode.D) && isFacingRight)
            Flip();

        grounded = Physics2D.OverlapCircle(groundDetector.position, 0.15f);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            transform.GetChild(2).GetComponent<AudioSource>().Play();
            physics.AddForce(new Vector2(0, jumpSpeed));
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = 0;
        }

        if (Input.GetKey(KeyCode.LeftControl) && shootTimer == 0)
        {
            transform.GetChild(1).GetComponent<AudioSource>().Play();

            bullet.GetComponent<BulletController>().facingRight = isFacingRight;

            if (isFacingRight)
            {
                Instantiate(bullet, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
                physics.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
            }
            else
            {
                Instantiate(bullet, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                physics.AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
            }

            shootTimer = 0.3f;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void decreaseOxygen()
    {
        currentOxygen -= 5f;

        float tempHealth = currentOxygen / maxOxygen;
        setOxygenBar(tempHealth);
    }

    public void setOxygenBar(float oxygen)
    {
        oxygenBar.transform.localScale = new Vector2(Mathf.Clamp(oxygen, 0f, 1f), oxygenBar.transform.localScale.y);
    }
}
