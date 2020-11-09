using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
   
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal;
        float vertical;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 forceVector = new Vector3(horizontal, 0, vertical);
        this.GetComponent<Rigidbody2D>().AddForce(forceVector * speed);
        
    }
}
