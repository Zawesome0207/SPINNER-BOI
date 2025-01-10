using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public Transform player;
    //public float moveSpeed;

    private bool canDash;
    public float dashCooldown;
    public float dashChargeTime;
    public CircleCollider2D playerDashCollider;

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

        topRigid.AddForce((player.position - topPiece.transform.position) * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter(Collider playerDashCollider)
    {
        Debug.Log("trigger");

        if (canDash)
        {
            canDash = false;

            speed = speed / 3;

            Invoke(nameof(dash), dashChargeTime);
            Invoke(nameof(resetDash), dashCooldown);
        }
    }

    private void resetDash()
    {
        canDash = true;
    }

    private void dash()
    {
        speed = speed * 3;

        topRigid.AddForce((player.position - topPiece.transform.position) * 1000, ForceMode2D.Impulse);
    }
}
