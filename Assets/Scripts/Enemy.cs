using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public int damage;

    public Transform player;
    //public float moveSpeed;
    public Player playerScript;
    private bool canDash;
    private float dashCooldown;
    public CircleCollider2D playerDashCollider;

    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public int speed;
    public int rotationSpeed;
    public int maxRotationVel;
    public Rigidbody2D playersRigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D topRigid = topPiece.GetComponent<Rigidbody2D>();
        Rigidbody2D bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();

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

    private void OnTriggerEnter(Collider playerDashCollider)
    {
        if (canDash)
        {
            canDash = false;


            Invoke(nameof(resetDash), dashCooldown);
        }

    }

    private void resetDash()
    {
        canDash = true;
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
            health -= (playersRigid.angularVelocity) * playerScript.getDamage() /100;
        }
    }
}
