using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    //General Objective List
    //ToDo:
        //Camera Movement
        //What to do when one player is atop another
        //Health System (Should be Continuous)
        //Start on Animator
        //What to do with two players walking into one another

    /*Variables Related to Flipping Function*/
    public GameObject Player1; //Player1 should always start on left
    public GameObject Player2; //Player2 should always start on right
    public GameObject mainCamera;

    Transform transform1; //Player1's Transform
    Transform transform2; //Player2's Transform

    public float Distance_Between_Players;

    public bool P1LtoP2R = true; //Used to define how to calculate the difference
                                 //between two players' positions


    //Components needed for Reset Variables
    Punch_Player_Movement P1_playerMovement;
    Punch_Player_Movement P2_playerMovement;
    PlayerHealth P1_playerHealth;
    PlayerHealth P2_playerHealth;

    //Variables to Reset
    Vector3 P1_position;
    Vector3 P2_position;
    Vector3 camera_position;
    int P1_health;
    int P2_health;
    Rigidbody2D P1_rb;
    Rigidbody2D P2_rb;


    // Start is called before the first frame update
    void Start()
    {
        transform1 = Player1.GetComponent<Transform>();
        transform2 = Player2.GetComponent<Transform>();
        FlipPlayer(transform2);

        //Grab other Variables needed to get Reset Variables
        P1_playerMovement = Player1.GetComponent<Punch_Player_Movement>();
        P2_playerMovement = Player2.GetComponent<Punch_Player_Movement>();
        P1_playerHealth = Player1.GetComponent<PlayerHealth>();
        P2_playerHealth = Player2.GetComponent<PlayerHealth>();

        //Grab all the variables needed for a reset during startup
        P1_position = transform1.position;
        P2_position = transform2.position;
        camera_position = mainCamera.transform.position;
        P1_health = P1_playerMovement.health;
        P2_health = P2_playerMovement.health;
        P1_rb = Player1.GetComponent<Rigidbody2D>();
        P2_rb = Player2.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

    }

    void FixedUpdate()
    {
        Maintain_2Player_Orientation();
    }


    //Check the distance between two players
    //X-axis Position: Player2 - Player1 
    //If the result is positive then their orienation is correct
    // Player1 Face Right           Player2 Face Left

    //If the result is negative then then the orientation is incorrect
    //Should be
    // Player2 Face Right           Player1 Face Left

    //This time for checking distance, should check if Player1 -Player2
    //is instead positive 

    //There may be issues concerning this imple around the center of 
    //crossing between the two players

    private void FlipPlayer(Transform playerTransform)
    {
        Vector2 newScale = playerTransform.localScale;
        newScale.x = -1 * newScale.x;
        playerTransform.localScale = newScale;
    }


    
    private void Maintain_2Player_Orientation ()
    {
        if (P1LtoP2R)
        {
            Distance_Between_Players = transform2.position.x - transform1.position.x;
        }
        else
        {
            Distance_Between_Players = transform1.position.x - transform2.position.x;
        }

        if (Distance_Between_Players < 0) //probably need to change 0 to some epsilon
        {
            FlipPlayer(transform1);
            FlipPlayer(transform2);

            P1LtoP2R = !P1LtoP2R;
        }
    }

    public void Reset()
    {
        Player1.transform.position = P1_position;
        Player2.transform.position = P2_position;
        mainCamera.transform.position = camera_position;
        P1_playerMovement.health = P1_health;
        P2_playerMovement.health = P2_health;
        P1_rb.bodyType = RigidbodyType2D.Dynamic;
        P2_rb.bodyType = RigidbodyType2D.Dynamic;
        P1_rb.velocity = Vector2.zero;
        P2_rb.velocity = Vector2.zero;
        P1_playerHealth.HPreset();
        P2_playerHealth.HPreset();
    }

}
