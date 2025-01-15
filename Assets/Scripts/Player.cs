using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public int speed;
    public int rotationSpeed;
    public int maxRotationVel;
    public float health;
    public Enemy currentBoss;
    public Rigidbody2D enemysRigid;
    public int damage;

    private bool canDash;
    public ParticleSystem dashReadyParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        canDash = true;
        dashReadyParticles.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        //Application.targetFrameRate = 60;
        Rigidbody2D topRigid = topPiece.GetComponent<Rigidbody2D>();
        Rigidbody2D bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();


        if (Input.GetKey("w"))
        {
            topRigid.AddForce(new Vector2(0,speed));
        }
        if (Input.GetKey("a"))
        {
            topRigid.AddForce(new Vector2(-speed, 0));
        }
        if (Input.GetKey("s"))
        {
            topRigid.AddForce(new Vector2(0, -speed));
        }
        if (Input.GetKey("d"))
        {
            topRigid.AddForce(new Vector2(speed, 0));
        }
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") )
        {
            if(bottomRigid.angularVelocity< maxRotationVel)
            {
                topRigid.AddTorque(rotationSpeed);
                bottomRigid.AddTorque(rotationSpeed);
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            bottomRigid.angularVelocity = maxRotationVel;

            topRigid.AddForce(topRigid.totalForce *  10, ForceMode2D.Impulse);

            dashReadyParticles.gameObject.SetActive(false);

            canDash = false;
            Invoke(nameof(resetDash), 6);
        }


    }

    private void resetDash()
    {
        canDash = true;
        dashReadyParticles.gameObject.SetActive(true);
    }

    //private void OnCollisonEnter2D(Collider2D collision)
    //{
    //if(collision.gameObject.tag== "Enemy Spike")
    //{
    //Debug.Log("hit");
    //health-=(collision.GetComponent<Rigidbody2D>().angularVelocity) * currentBoss.getDamage();
    // }
    // }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy Spike")
        {
            Debug.Log("hit");
            health -= Mathf.Abs(((enemysRigid.angularVelocity) * currentBoss.getDamage() / 100) * ((enemysRigid.linearVelocity.x + enemysRigid.linearVelocity.y) / 20));
        }
            
            
    }
    public int getDamage()
    {
        return damage;
    }
}
