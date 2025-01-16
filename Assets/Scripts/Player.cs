using System;
using UnityEngine;
using UnityEngine.UI;

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
    public float dashCooldown;
    public Camera cameras;
    public Slider healthBar;

    public ParticleSystem dashReadyParticles;
    private bool isImmune;

    private Rigidbody2D topRigid;
    private Rigidbody2D bottomRigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        isImmune = false;

        topRigid = topPiece.GetComponent<Rigidbody2D>();
        bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;


        if (Input.GetKey("w"))
        {
            topRigid.AddForce(new Vector2(0, speed));
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
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            if (bottomRigid.angularVelocity < maxRotationVel)
            {
                topRigid.AddTorque(rotationSpeed);
                bottomRigid.AddTorque(rotationSpeed);
            }

        }

        if (dashCooldown > 0)
        {
            dashCooldown--;
        }

        if(dashCooldown == 0)
        {
            dashReadyParticles.gameObject.SetActive(true);
        }
        if (dashCooldown == 0 && Input.GetMouseButtonDown(0))
        {
            isImmune = true;

            Invoke(nameof(stopImmune), 1);

            dashCooldown = 3 * 60;
            Vector3 mousePos = cameras.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            Debug.Log("mouseX:" + mousePos.x + "   mouseY: " + mousePos.y);
            Debug.Log("playerX:" + topPiece.transform.position.x + "   playerY: " + topPiece.transform.position.y);
            //topRigid.AddForce(Vector2.MoveTowards(topRigid.position, mousePos,1000)*new Vector2(200,200));

            float posNumMathX = mousePos.x - topPiece.transform.position.x;
            float posNumMathY = mousePos.y - topPiece.transform.position.y;
            float posNumMathAbs = Mathf.Abs(posNumMathX) + Mathf.Abs(posNumMathY);
            topRigid.AddForce(new Vector2((posNumMathX / posNumMathAbs) * speed * 500, (posNumMathY / posNumMathAbs) * speed * 500));

            dashReadyParticles.gameObject.SetActive(false);
        }
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
        if (collision.gameObject.tag == "Enemy Spike" && !isImmune)
        {
            Debug.Log("hit");
            health -= Mathf.Abs(((enemysRigid.angularVelocity) * currentBoss.getDamage() / 500) + ((enemysRigid.linearVelocity.x + enemysRigid.linearVelocity.y) / 2));

            topRigid.linearVelocity += enemysRigid.linearVelocity;
        }
            
            
    }
    public int getDamage()
    {
        return damage;
    }

    private void stopImmune()
    {
        isImmune = false;
    }
}
