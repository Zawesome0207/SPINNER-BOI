using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public Transform player;
    private Rigidbody2D rb;
    public float moveSpeed;

    private bool canDash;
    private float dashCooldown;
    public CircleCollider2D playerDashCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce((player.position - transform.position) * moveSpeed, ForceMode2D.Impulse);
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
}
