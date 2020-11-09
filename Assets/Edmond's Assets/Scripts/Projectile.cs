using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D body;
    private float creationTime;
    public int speed;
    public int vertspeed;
   // public AudioClip start;
   // public AudioClip end;
   // public AudioSource starts;
   // public AudioSource ends;

    public string objectName;
    

    void Awake()
    {
        //source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        body = GetComponent<Rigidbody2D>();
        body.AddRelativeForce(new Vector2(speed, vertspeed));

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - creationTime > 3)
        {
            Destroy(gameObject);
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name != objectName)
        {
            Debug.Log("boom!");
            //Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }*/
}
