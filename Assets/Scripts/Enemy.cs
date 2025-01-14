using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public Transform player;
    //public float moveSpeed;

    private DashEnemy dashTrigger;
    private bool canDash;
    public float dashCooldown;
    public float dashChargeTime;

    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public float speed;
    public int rotationSpeed;
    public int maxRotationVel;

    private Rigidbody2D topRigid;
    private Rigidbody2D bottomRigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        dashTrigger = topPiece.GetComponent<DashEnemy>();
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

        topRigid.AddForce((player.position - topPiece.transform.position) * speed / 100, ForceMode2D.Impulse);

        if (dashTrigger.dash)
        {
            if (canDash)
            {
                canDash = false;

                topRigid.AddForce(-(player.position - topPiece.transform.position) * speed, ForceMode2D.Impulse);
                speed = speed / 10;
                rotationSpeed = rotationSpeed * 3;

                Invoke(nameof(dash), dashChargeTime);
                Debug.Log("Dash Charge");
            }
        }
    }

    private void resetDash()
    {
        canDash = true;
    }

    private void dash()
    {
        speed = speed * 10;
        rotationSpeed = rotationSpeed / 3;

        Debug.Log("Dash Now!");
        topRigid.AddForce((player.position - topPiece.transform.position) * 200, ForceMode2D.Impulse);

        Invoke(nameof(resetDash), dashCooldown);
    }
}
