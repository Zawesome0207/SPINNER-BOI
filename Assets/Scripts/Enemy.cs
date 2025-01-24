using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float damage;

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
    public ParticleSystem deathParticles;
    public Image healthBar;

    [Header("Debug - Runtime Filled")]
    private bool isImmune;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        canDash = true;

        topRigid = topPiece.GetComponent<Rigidbody2D>();
        bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();

        deathParticles.gameObject.SetActive(false);

        isImmune = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / 100;

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

        if(health <= 0.0)
        {
            death();
        }

        deathParticles.transform.position = topPiece.transform.position;

    }

    private void resetDash()
    {
        canDash = true;

        dashReadyParticles.gameObject.SetActive(true);
    }

    private void dash()
    {
        isImmune = true;

        Invoke(nameof(stopImmune), .2f);

        bottomRigid.angularVelocity = maxRotationVel;

        //Debug.Log("Dash Now!");
        topRigid.AddForce((player.position - topPiece.transform.position) * 200, ForceMode2D.Impulse);

        Invoke(nameof(resetDash), dashCooldown);
    }
    public float getDamage()
    {
        return damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Ehit");
        if ((collision.gameObject.tag == "Player Spike" || collision.gameObject.tag == "Player") && !isImmune)
        {

            isImmune = true;

            Invoke(nameof(stopImmune), .5f);

            float EnemyVelocityDamge = Mathf.Abs((topRigid.linearVelocityX - playersRigid.linearVelocity.x) / topRigid.linearVelocityX + playersRigid.linearVelocity.x) + (Mathf.Abs((topRigid.linearVelocityY - playersRigid.linearVelocity.y) / topRigid.linearVelocityX + playersRigid.linearVelocity.x));
            float EnemyRotationDamage = Mathf.Abs((playersRigid.angularVelocity) * playerScript.getDamage() / 50);
            if(EnemyVelocityDamge>20)
            {
                EnemyVelocityDamge = 20;
            }
            if (EnemyRotationDamage > 20)
            {
                EnemyRotationDamage = 20;
            }

            //float damn 
            Debug.Log("Enemy rotation damage: "+ EnemyRotationDamage/4 + "  Enemy velocity Damage: "+ EnemyVelocityDamge/4);//debug

            health -= ((EnemyVelocityDamge+ EnemyRotationDamage)/4);

            //topRigid.linearVelocity += playersRigid.linearVelocity * 10;


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player Spike" || collision.gameObject.tag == "Player") && canDash)
        {
            canDash = false;

            dashReadyParticles.gameObject.SetActive(false);

            dash();
        }
    }

    private void death()
    {
        deathParticles.gameObject.SetActive(true);

        Destroy(GameObject.Find("EnemyTop"));
    }

    private void stopImmune()
    {
        isImmune = false;
    }
}
