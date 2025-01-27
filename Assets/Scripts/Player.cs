using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxhealth;
    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public int speed;
    public int rotationSpeed;
    public int maxRotationVel;
    public float health;
    public Enemy currentBoss;
    //public Rigidbody2D enemysRigid;
    public float damage;
    public Camera cameras;
    public Image healthBar;

    private Rigidbody2D topRigid;
    private Rigidbody2D bottomRigid;

    float dodgex;
    float dodgey;

    [Header("Debug - Runtime Filled")]
    public bool isImmune;
    public int dodgeCooldown;
    public float dashCooldown;

    [Header("Particle Effects")]
    public ParticleSystem dashReadyParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem hitEffectPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        isImmune = false;

        topRigid = topPiece.GetComponent<Rigidbody2D>();
        bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();

        deathParticles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (health/100)/(maxhealth / 100);

        deathParticles.transform.position = topPiece.transform.position;


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
        if(dodgeCooldown> 0)
        {
            dodgeCooldown--;
        }

        if(dashCooldown == 0)
        {
            dashReadyParticles.gameObject.SetActive(true);
        }
        if (dashCooldown == 0 && Input.GetMouseButtonDown(0))
        {
            isImmune = true;

            Invoke(nameof(stopImmune), .5f);

            dashCooldown = 3 * 60;
            Vector3 mousePos = cameras.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            //Debug.Log("mouseX:" + mousePos.x + "   mouseY: " + mousePos.y);
            //Debug.Log("playerX:" + topPiece.transform.position.x + "   playerY: " + topPiece.transform.position.y);
            //topRigid.AddForce(Vector2.MoveTowards(topRigid.position, mousePos,1000)*new Vector2(200,200));

            float posNumMathX = mousePos.x - topPiece.transform.position.x;
            float posNumMathY = mousePos.y - topPiece.transform.position.y;
            float posNumMathAbs = Mathf.Abs(posNumMathX) + Mathf.Abs(posNumMathY);
            topRigid.AddForce(new Vector2((posNumMathX / posNumMathAbs) * speed * 500, (posNumMathY / posNumMathAbs) * speed * 500));

            dashReadyParticles.gameObject.SetActive(false);
        }
        if (dodgeCooldown == 0 && Input.GetMouseButtonDown(1))
        {
            dodgeCooldown = 3 * 60;

            Vector3 mousePos = cameras.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            dodgex=  mousePos.x - topPiece.transform.position.x;
            dodgey = mousePos.y - topPiece.transform.position.y;
            float posNumMathAbs = Mathf.Abs(dodgex) + Mathf.Abs(dodgey);

            dodgex = dodgex / posNumMathAbs;
            dodgey = dodgey / posNumMathAbs;
        }
        if(dodgeCooldown>(2.4*60))
        {
            //Debug.Log(1);
            topRigid.linearVelocity = new Vector2(dodgex * speed * .07f, dodgey * speed * .07f);
            bottomRigid.linearVelocity = new Vector2(dodgex * speed * .07f, dodgey * speed * .07f);
        }

        if (health <= 0)
        {
            death();
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
        if ((collision.gameObject.tag == "Enemy") && isImmune== false)
        {
            //Debug.Log("hit");
            //health -= Mathf.Abs((enemysRigid.angularVelocity) * currentBoss.getDamage() / 500) + ((Mathf.Abs(topRigid.linearVelocityX - PlayersRigid.linearVelocity.x) + Mathf.Abs(topRigid.linearVelocityY - PlayersRigid.linearVelocity.y)) / 5);
            isImmune = true;
            Rigidbody2D enemysRigid = collision.gameObject.GetComponent<Rigidbody2D>();
            Enemy CURenemScript = collision.gameObject.GetComponent<Enemy>();
            float pain = CURenemScript.getDamage();
            Invoke(nameof(stopImmune), .5f);

            float PlayerVelocityDamge = pain * Mathf.Abs((topRigid.linearVelocityX - enemysRigid.linearVelocity.x) / topRigid.linearVelocityX + enemysRigid.linearVelocity.x) + (Mathf.Abs((topRigid.linearVelocityY - enemysRigid.linearVelocity.y) / topRigid.linearVelocityX + enemysRigid.linearVelocity.x));
            float PlayerRotationDamage = pain * Mathf.Abs((enemysRigid.angularVelocity) * currentBoss.getDamage() / 50);

            if (PlayerVelocityDamge > 20* pain)
            {
                PlayerVelocityDamge = 20* pain;
            }
            if (PlayerRotationDamage > 20* pain)
            {
                PlayerRotationDamage = 20* pain;
            }
            
            Debug.Log("Player rotation damage: " + PlayerRotationDamage/4 + "  Player velocity Damage: " + PlayerVelocityDamge/4);//debug

            health -= ((PlayerVelocityDamge + PlayerRotationDamage) / 4);

            //topRigid.linearVelocity += enemysRigid.linearVelocity * 10;

            //topRigid.linearVelocity += enemysRigid.linearVelocity * 10;
        }

        ParticleSystem hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);
    }
    public float getDamage()
    {
        return damage;
    }

    private void stopImmune()
    {
        isImmune = false;
    }

    private void death()
    {
        deathParticles.gameObject.SetActive(true);

        //Destroy(GameObject.Find("PlayerTop"));
        spinner.SetActive(false);
    }
}
