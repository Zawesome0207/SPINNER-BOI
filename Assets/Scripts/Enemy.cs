using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public int damage;

    private bool canDash;
    public int dashCooldown;

    public Transform player;
    public Player playerScript;

    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public int speed;
    public int rotationSpeed;
    public int maxRotationVel;
    public Rigidbody2D playersRigid;

    private Rigidbody2D topRigid;
    private Rigidbody2D bottomRigid;

    public ParticleSystem dashReadyParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        canDash = true;

        topRigid = topPiece.GetComponent<Rigidbody2D>();
        bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (bottomRigid.angularVelocity < maxRotationVel)
        {
            topRigid.AddTorque(rotationSpeed);
            bottomRigid.AddTorque(rotationSpeed);
        }

        //topRigid.AddForce((player.position - topPiece.transform.position) * speed, ForceMode2D.Impulse);
        float posNumMathX = player.position.x - topPiece.transform.position.x;
        float posNumMathY = player.position.y - topPiece.transform.position.y;
        float posNumMathAbs = Mathf.Abs(posNumMathX) + Mathf.Abs(posNumMathY);
        topRigid.AddForce(new Vector2((posNumMathX / posNumMathAbs) * speed, (posNumMathY / posNumMathAbs) * speed));

    }

    private void resetDash()
    {
        canDash = true;

        dashReadyParticles.gameObject.SetActive(true);
    }

    private void dash()
    {
        bottomRigid.angularVelocity = maxRotationVel;

        Debug.Log("Dash Now!");
        topRigid.AddForce((player.position - topPiece.transform.position) * 200, ForceMode2D.Impulse);

        Invoke(nameof(resetDash), dashCooldown);
    }
    public int getDamage()
    {
        return damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Ehit");
        if (collision.gameObject.tag == "Player Spike")
        {
            Debug.Log("Ehit");
            health -= Mathf.Abs(((playersRigid.angularVelocity) * playerScript.getDamage() /100) * ((playersRigid.linearVelocity.x + playersRigid.linearVelocity.y)/20));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Spike" && canDash)
        {
            canDash = false;

            dashReadyParticles.gameObject.SetActive(false);

            dash();
        }
    }

    private void death()
    {
        
    }
}
