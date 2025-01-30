using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxhealth;
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

    public Image healthBar;

    public AudioSource noiseMaker;
    public AudioClip metalPipe;

    [Header("Particle Effects")]
    public ParticleSystem dashReadyParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem hitEffectPrefab;


    [Header("Debug - Runtime Filled")]
    private bool isImmune;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        canDash = false;
        Invoke(nameof(resetDash), dashCooldown);

        topRigid = topPiece.GetComponent<Rigidbody2D>();
        bottomRigid = bottomPiece.GetComponent<Rigidbody2D>();

        isImmune = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (health / 100) / (maxhealth / 100);

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
        noiseMaker.PlayOneShot(metalPipe);
        if ((collision.gameObject.tag == "Player Spike" || collision.gameObject.tag == "Player") && !isImmune)
        {
            float pain = playerScript.getDamage();

            isImmune = true;

            Invoke(nameof(stopImmune), .5f);

            float EnemyVelocityDamge = pain * Mathf.Abs((topRigid.linearVelocityX - playersRigid.linearVelocity.x) / topRigid.linearVelocityX + playersRigid.linearVelocity.x) + (Mathf.Abs((topRigid.linearVelocityY - playersRigid.linearVelocity.y) / topRigid.linearVelocityX + playersRigid.linearVelocity.x));
            float EnemyRotationDamage = pain * Mathf.Abs((playersRigid.angularVelocity) * playerScript.getDamage() / 50);
            if(EnemyVelocityDamge>20* pain)
            {
                EnemyVelocityDamge = 20* pain;
            }
            if (EnemyRotationDamage > 20* pain)
            {
                EnemyRotationDamage = 20* pain;
            }
            //hi
            //float damn 
            Debug.Log("Enemy rotation damage: "+ EnemyRotationDamage/4 + "  Enemy velocity Damage: "+ EnemyVelocityDamge/4);//debug

            health -= ((EnemyVelocityDamge+ EnemyRotationDamage)/4);

            //topRigid.linearVelocity += playersRigid.linearVelocity * 10;
        }

        ParticleSystem hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);

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
        topPiece.gameObject.SetActive(false);
    }

    private void stopImmune()
    {
        isImmune = false;
    }
}
