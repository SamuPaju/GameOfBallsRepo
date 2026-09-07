using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public float force;
    public Rigidbody playerRB;

    public float health;
    public float highPoint;
    public bool goingDown;
    public float damage;

    Vector3 startPos;

    public GameObject obstacleEffect;

    public GUIStyle myStyle;

    private void Start()
    {
        myStyle.normal.textColor = Color.white;
        myStyle.fontSize = 26;

        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            force += 20 * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 dir = (mousePos - transform.position).normalized;
            if (dir.y < 0)
            {
                dir *= -1;
            }

            Launch(force, dir);
        }

        // Tarkastetaan joka frame onko pallo alkanut menemään alaspäin.
        if (playerRB.linearVelocity.y < -0.01 && goingDown == false)
        {
            GetComponent<Renderer>().material.color = Color.red;
            goingDown = true;
            highPoint = transform.position.y;
        }

    }

    public void Launch(float launchForce, Vector3 launchDir)
    {
        GetComponent<Renderer>().material.color = Color.white;
        playerRB.AddForce(launchDir * launchForce, ForceMode.Impulse);
        force = 0;
        goingDown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Tämä funktio ajetaan aina kun pelaaja osuu mihin tahansa jossa on collider

        if (collision.gameObject.CompareTag("Platform"))
        {
            if (goingDown)
            {
                float height = highPoint - transform.position.y;
                damage = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * Mathf.Abs(height));
                TakeDamage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameObject effect = Instantiate(obstacleEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3);
            TakeDamage(20);
        }

        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            SceneManager.LoadScene(collision.gameObject.GetComponent<LevelEnd>().nextLevel);
        }
    }

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        if (health < 0)
        {
            Die();
        }
    }

    void Die()
    {
        transform.position = startPos;
        health = 100;
        playerRB.linearVelocity = Vector3.zero;
        goingDown = false;

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            platform.layer = LayerMask.NameToLayer("PlatformInactive");
            platform.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Force: " + force, myStyle);
        GUI.Label(new Rect(10, 30, 100, 20), "High Point: " + highPoint, myStyle);
        GUI.Label(new Rect(10, 50, 100, 20), "Going Down: " + goingDown, myStyle);
        GUI.Label(new Rect(10, 70, 100, 20), "Damage: " + damage, myStyle);
        GUI.Label(new Rect(10, 90, 100, 20), "Health: " + health, myStyle);
        GUI.Label(new Rect(10, 110, 100, 20), "Velocity Y: " + playerRB.linearVelocity.y, myStyle);
    }
}
