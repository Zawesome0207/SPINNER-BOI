using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject spinner;
    public GameObject topPiece;
    public GameObject bottomPiece;
    public int speed;
    public int rotationSpeed;
    public int maxRotationVel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        
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


    }
}
