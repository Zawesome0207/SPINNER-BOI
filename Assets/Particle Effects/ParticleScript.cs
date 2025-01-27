using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public int lifespan ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lifespan>0)
        {
            lifespan--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
