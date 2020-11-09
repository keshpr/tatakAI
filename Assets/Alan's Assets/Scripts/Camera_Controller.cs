using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Movement_Controller Movement;

    //Variables used to determine the bounds of a level
    private const float halfCamWidth = 8.9f;
    public bool ReachedLevelR;
    public bool ReachedLevelL;


    //Game Objects
    public GameObject camera; //equivalent to this.(...)
    public GameObject P1; //Player1
    public GameObject P2; //Player2
    public GameObject LevelBoundR;
    public GameObject LevelBoundL;


    private Rigidbody2D camera_rb2d; //Rigid body of camera
    //private Rigidbody2D P1_rb2d; //Rigid body of P1
    //private Rigidbody2D P2_rb2d; //Rigid body of P2

    

    //Variables used to detect when a play touches boundary of the camera
    public bool m_BoundCollision1;
    public bool m_BoundCollision2;

    [SerializeField] private LayerMask m_whatIsBound;
    [SerializeField] private Transform m_boundCheck1;
    [SerializeField] private Transform m_boundCheck2;

    public float box_X = 0.8f;
    public float box_Y = 0.8f;
    private Vector2 boxCheck;


    //Variables used to control camera movement
    private float fromCameraCenter = 4f;
    private Vector2 referenceVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera_rb2d = this.GetComponent<Rigidbody2D>();
        //P1_rb2d = P1.GetComponent<Rigidbody2D>();
        //P2_rb2d = P2.GetComponent<Rigidbody2D>();
        boxCheck = new Vector2(box_X, box_Y);
    }

    // Update is called once per frame
    void Update()
    {
        moveCamera();
    }

    void FixedUpdate()
    {
        
        //Checks whether P1 or P2 came into contect with a bound of the camera
        m_BoundCollision1 = Physics2D.OverlapBox(m_boundCheck1.position, boxCheck, 0, m_whatIsBound,
                                                  -1, 1);
        m_BoundCollision2 = Physics2D.OverlapBox(m_boundCheck2.position, boxCheck, 0, m_whatIsBound,
                                                  -1, 1);
        

        //Might need to add epsilons here when defining where level bounds are
        ReachedLevelL = ((camera.transform.position.x - halfCamWidth) <= LevelBoundL.transform.position.x);
        ReachedLevelR = ((camera.transform.position.x + halfCamWidth) >= LevelBoundR.transform.position.x);

    }


    float getMax(float speed1, float speed2)
    {
        float maxSpeed;

        if(Mathf.Abs(speed1) > Mathf.Abs(speed2))
        {
            maxSpeed = speed1;
        }
        else
        {
            maxSpeed = speed2;
        }

        return maxSpeed;
    }

    /***************************************************************************
     * Will probably need to rewrite this function to operate based on P1 and P2
     * movement inputs rather than looking at the gameObjects' velocities
     * directly.
     * An edge case occurs when the players are near the right or left bound of
     * the camera but can no longer move with any significant amount of
     * velocity.
     * This results in the camera no longer moving with an player's inputs until
     * one of the players moves far enough from the other to obtain the spacing
     * required to generate enough velocity to move the camera.
     **************************************************************************/
    //Function used to move the game camera based on the movement and locations of the two players
    public void moveCamera()
    {
        //If the camera has already reached the rightmost or leftmost portion of the level
        //do not try to move the camera any further in that direction
       
        //If the two players are both on a right region of the camera
        //and either:
        // 1) Both P1 and P2 are moving in the same direction (right)
        // 2) One player is against the camera bound and the other is trying to move right
        // 3) None of the above
        if ((P1.transform.position.x > (camera.transform.position.x + fromCameraCenter)) &&
            (P2.transform.position.x > (camera.transform.position.x + fromCameraCenter)))
        {
            
            float maxSpeed;
            if ((Movement.h_move_1 > 1f) && (Movement.h_move_2 > 1f) && !ReachedLevelR)
            {
                maxSpeed = getMax(Movement.h_move_1, Movement.h_move_2);
            }
            
            else if ((m_BoundCollision1 || m_BoundCollision2) &&
                    (Movement.h_move_1 > 1f || Movement.h_move_2 > 1f) &&
                     !ReachedLevelR)
            {
                maxSpeed = getMax(Movement.h_move_1, Movement.h_move_2);
            }
            
            else
            {
                maxSpeed = 0f;
            }

            Vector2 targetSpeed = new Vector2(maxSpeed, 0f);
            camera_rb2d.velocity = Vector2.SmoothDamp(camera_rb2d.velocity, targetSpeed, ref referenceVelocity, 0.05f);
        }

        //If the two players are both on a left region of the camera
        //and either:
        // 1) Both P1 and P2 are moving in the same direction (left)
        // 2) One player is against the camera bound and the other is trying to move left
        // 3) None of the above
        else if ((P1.transform.position.x < (camera.transform.position.x - fromCameraCenter)) &&
                (P2.transform.position.x < (camera.transform.position.x - fromCameraCenter)))
        {
        
            float maxSpeed;
            if ((Movement.h_move_1 < -1f) && (Movement.h_move_2 < -1f) && !ReachedLevelL)
            {
                maxSpeed = getMax(Movement.h_move_1, Movement.h_move_2);
            }
            
            else if ((m_BoundCollision1 || m_BoundCollision2) &&
                    (Movement.h_move_1 < -1f || Movement.h_move_2 < -1f) &&
                    !ReachedLevelL)
            {
                maxSpeed = getMax(Movement.h_move_1, Movement.h_move_2);
            }
            
            else
            {
                maxSpeed = 0f;
            }

            Vector2 targetSpeed = new Vector2(maxSpeed, 0f);
            camera_rb2d.velocity = Vector2.SmoothDamp(camera_rb2d.velocity, targetSpeed, ref referenceVelocity, 0.05f);
        }
        //Otherwise the P1 and P2 are not gonna move the camera
        else
        {
            Vector2 targetSpeed = new Vector2(0f, 0f);
            camera_rb2d.velocity = Vector2.SmoothDamp(camera_rb2d.velocity, targetSpeed, ref referenceVelocity, 0.05f);
        }
    }


}
