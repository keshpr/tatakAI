using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Player_Movement : MonoBehaviour
{
    //Controllers
    public Game_Controller gameController;
    public SoundManager soundManager;

    //Player Components
    private Rigidbody2D rb2D;

    //Movement Variables;
    public float jumpForce = 800f;
    Vector2 referenceVelocity = Vector2.zero;
    [Range(0f, 0.3f)] [SerializeField] private float m_smoothFactor = 0.05f;

    //Player Properties
    public bool m_grounded;
    public bool m_playerHeadCollision;
    public bool m_playerFootCollision;
    public bool m_playerHitReceived;
    public bool double_jump = false;

    //Object Detection Properties
    private float m_groundRadius = 0.2f;
    [SerializeField] private LayerMask m_whatIsGround;
    [SerializeField] private LayerMask m_whatIsP2;
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private Transform m_headCheck;
    [SerializeField] private Transform m_footCheck;

    private bool footImpulse = true;
    private bool headImpulse = true;

    public float box_X = 0.8f;
    public float box_Y = 0.2f;
    private Vector2 boxCheck;


    //GameObjects
    public GameObject AttackBox;

    //Player Attack Testing Variables
    public float duration = 1;
    public bool attackStarted = false;
    public bool attackWaiting = false;
    public float currentTime = 0;
    public float waitUntil = 0;

    //Player Health
    public int health = 100;
    public int damage = 10;

    //Player Flashing Variables
    private SpriteRenderer playerSprite;
    public float flashDuration = 2f;
    public float flashInterval = 0.2f;
    public bool iFrames = false;
    public bool flash = false;
    public float iFrameDuration = 0f;
    public float flashTime = 0f;
    public float opacity = 0.5f;

    //Player Attacks
    public bool attack2Flag = false;
    public bool attack3Flag = false;

    //health bar shenanigans
    public PlayerHealth healthbar;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCheck = new Vector2(box_X, box_Y);
        playerSprite = GetComponent<SpriteRenderer>();

        //added healthbar getcomponent
        healthbar = GetComponent<PlayerHealth>();

    }


    // Update is called once per frame
    void Update()
    {
        m_playerHeadCollision = Physics2D.OverlapBox(m_headCheck.position, boxCheck, 0, m_whatIsP2,
                                                  -1, 1);
        m_playerFootCollision = Physics2D.OverlapBox(m_footCheck.position, boxCheck, 0, m_whatIsP2,
                                                  -1, 1);


        if (!m_playerHeadCollision)
        {
            headImpulse = true;
        }

        if (!m_playerFootCollision)
        {
            footImpulse = true;
        }

        if (m_playerHeadCollision)
        {
            HeadCollision(m_whatIsP2);
        }
        else if (m_playerFootCollision)
        {
            FootCollision(m_whatIsP2);
        }

        /* 
         * Beginning of Timer Section of the Update Loop
         * Contains:
         *      - Attack Timers
         *      - Invicibility Frames Timers
         */
        currentTime = Time.time;

        Manage_Attack_Box();
        ManageSound();

        /*
        if (attackStarted)
        {
            waitUntil = Time.time + duration;
            attackStarted = false;
            attackWaiting = true;
        }

        if (attackWaiting)
        {
            if (currentTime > waitUntil)
            {
                AttackBox.SetActive(false);
                attackWaiting = false;
            }
        }
        */

        // iFrames triggers true when a player is hit, false when invuln duration is over
        // Visual effect of iFrames:
        //   - The player that was hit flashed in the game (can still move and perform commands during this)
        //   - Currently visual effect is done through changes to opacity in the Sprite Renderer
        //          - Opacity alternates between 1 and 0.5
        if (iFrames)
        {
            if (currentTime < iFrameDuration)
            {
                if (currentTime > flashTime)
                {
                    //playerSprite.enabled = !flash;
                    //flash = !flash;
                    playerSprite.color = new Color(1f, 1f, 1f, opacity);

                    //Arbitrary 0.7f value compared against
                    if (opacity > 0.7f)
                    {
                        opacity = 0.5f;
                    }
                    else
                    {
                        opacity = 1f;
                    }

                    flashTime += flashInterval;
                }
            }
            else
            {
                //playerSprite.enabled = true;
                playerSprite.color = new Color(1f, 1f, 1f, 1f);
                iFrames = false;
            }
        }

        /*
         * End of Timer Section of Update Loop
         */
    }

    void FixedUpdate()
    {
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_whatIsGround);

        if (m_grounded)
        {
            double_jump = true;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject attacker = collision.gameObject.transform.parent.gameObject;
        GameObject recipient = this.gameObject;

        //If you've hit P1
        if (recipient.layer == 9)
        {
            Debug.Log("You've Hit P1");
            //Call GameController function to damage P1, update health bar visual, and trigger invuln frames
        }

        //If you've hit P2
        if (recipient.layer == 10)
        {
            Debug.Log("You've Hit P2");
            //Call GameController function to damage P2, update health bar visual, and trigger invuln frames
        }

        //If the player hit does not currently have Invuln-Frames(iFrames):
        //  - Do damage to the player that was hit
        //  - Set the timers for the entire iFrame duration and for the first interval of visual flashing
        if (!iFrames && health > 0)
        {
            health -= damage;
            //take big damage!
            healthbar.TakeDamage(damage);

            iFrameDuration = Time.time + flashDuration;
            flashTime = Time.time + flashInterval;

            playerSprite.color = new Color(1f, 1f, 1f, opacity);
            opacity = 1f;

            iFrames = true;
        }
    }

    //Attacking currently occurs in a 3 step process (NEEDS TO BE CHANGED TO ALLOW FOR 3-COMBOS)
    //(Step 1) Hitting the attack button causes the AttackBox collider to be set active true
    //(Step 2) In the Update loop, a timer is set to be a given constant duration
    //(Step 3) Once the timer is exceeded, the AttackBox is set active false
    public void Attack(bool attack)
    {
        if(attack)
        {
            
        }

        /*
        if (attack)
        {
            AttackBox.SetActive(true);
            attackStarted = true;
        }
        */
    }

    private void Manage_Attack_Box()
    {
        if (playerSprite.sprite.name == "L_punch10002" ||
            playerSprite.sprite.name == "L_punch10003" ||
            playerSprite.sprite.name == "L_punch10004" ||
            playerSprite.sprite.name == "L_punch10005" ||
            playerSprite.sprite.name == "L_punch20003" ||
            playerSprite.sprite.name == "L_punch20004" ||
            playerSprite.sprite.name == "L_punch20005" ||
            playerSprite.sprite.name == "L_swipe0004" ||
            playerSprite.sprite.name == "L_swipe0005" ||
            playerSprite.sprite.name == "L_swipe0006" ||
            playerSprite.sprite.name == "punch10002" ||
            playerSprite.sprite.name == "punch10003" ||
            playerSprite.sprite.name == "punch10004" ||
            playerSprite.sprite.name == "punch10005" ||
            playerSprite.sprite.name == "punch20003" ||
            playerSprite.sprite.name == "punch20004" ||
            playerSprite.sprite.name == "punch20005" ||
            playerSprite.sprite.name == "swipe0004" ||
            playerSprite.sprite.name == "swipe0005" ||
            playerSprite.sprite.name == "swipe0006")
        {
            AttackBox.SetActive(true);
        }


        if (playerSprite.sprite.name != "L_punch10002" &&
            playerSprite.sprite.name != "L_punch10003" &&
            playerSprite.sprite.name != "L_punch10004" &&
            playerSprite.sprite.name != "L_punch10005" &&
            playerSprite.sprite.name != "L_punch20004" &&
            playerSprite.sprite.name != "L_punch20005" &&
            playerSprite.sprite.name != "L_swipe0004" &&
            playerSprite.sprite.name != "L_swipe0005" &&
            playerSprite.sprite.name != "L_swipe0006" &&
            playerSprite.sprite.name != "punch10002" &&
            playerSprite.sprite.name != "punch10003" &&
            playerSprite.sprite.name != "punch10004" &&
            playerSprite.sprite.name != "punch10005" &&
            playerSprite.sprite.name != "punch20004" &&
            playerSprite.sprite.name != "punch20005" &&
            playerSprite.sprite.name != "swipe0004" &&
            playerSprite.sprite.name != "swipe0005" &&
            playerSprite.sprite.name != "swipe0006")
        {
            AttackBox.SetActive(false);
        }

        

        if (playerSprite.sprite.name == "L_punch10001" ||
           playerSprite.sprite.name == "L_punch10002" ||
           playerSprite.sprite.name == "L_punch10003" ||
           playerSprite.sprite.name == "L_punch10004" ||
           playerSprite.sprite.name == "L_punch10005" ||
           playerSprite.sprite.name == "punch10001" ||
          playerSprite.sprite.name == "punch10002" ||
          playerSprite.sprite.name == "punch10003" ||
          playerSprite.sprite.name == "punch10004" ||
          playerSprite.sprite.name == "punch10005")
        {
            attack2Flag = true;
        }

        if (playerSprite.sprite.name == "L_punch10006" ||
           playerSprite.sprite.name == "L_punch10007" ||
           playerSprite.sprite.name == "L_punch20007" ||
           playerSprite.sprite.name == "L_swipe0002" ||
           playerSprite.sprite.name == "L_swipe0003" ||
           playerSprite.sprite.name == "punch10006" ||
           playerSprite.sprite.name == "punch10007" ||
           playerSprite.sprite.name == "punch20007" ||
           playerSprite.sprite.name == "swipe0002" ||
           playerSprite.sprite.name == "swipe0003")
        {
            attack2Flag = false;
        }

        if (playerSprite.sprite.name == "L_punch20001" ||
           playerSprite.sprite.name == "L_punch20002" ||
           playerSprite.sprite.name == "L_punch20003" ||
           playerSprite.sprite.name == "L_punch20004" ||
           playerSprite.sprite.name == "L_punch20005" ||
           playerSprite.sprite.name == "L_punch20006" ||
           playerSprite.sprite.name == "punch20001" ||
           playerSprite.sprite.name == "punch20002" ||
           playerSprite.sprite.name == "punch20003" ||
           playerSprite.sprite.name == "punch20004" ||
           playerSprite.sprite.name == "punch20005" ||
           playerSprite.sprite.name == "punch20006")
        {
            attack3Flag = true;
        }

        if (playerSprite.sprite.name == "L_swipe0002" ||
           playerSprite.sprite.name == "L_punch20007" ||
           playerSprite.sprite.name == "swipe0002" ||
           playerSprite.sprite.name == "punch20007")
        {
            attack3Flag = false;
        }


    }

    bool punch1_playOnce = true;
    bool punch2_playOnce = true;
    bool punch3_playOnce = true;
    private void ManageSound()
    {
        if ((playerSprite.sprite.name == "L_punch10001" && punch1_playOnce) ||
            (playerSprite.sprite.name == "punch10001" && punch1_playOnce))
        {
            soundManager.PlayAudioClip("punch");
            punch1_playOnce = false;
        }

        if ((playerSprite.sprite.name == "L_punch20001" && punch2_playOnce) ||
            (playerSprite.sprite.name == "punch20001" && punch2_playOnce))
        {
            soundManager.PlayAudioClip("punch");
            punch2_playOnce = false;
        }

        if ((playerSprite.sprite.name == "L_swipe0001" && punch3_playOnce) ||
            (playerSprite.sprite.name == "swipe0001" && punch3_playOnce))
        {
            soundManager.PlayAudioClip("swipe");
            punch3_playOnce = false;
        }

        if (playerSprite.sprite.name == "L_punch10007" ||
            playerSprite.sprite.name == "L_punch20001" ||
            playerSprite.sprite.name == "punch10007" ||
            playerSprite.sprite.name == "punch20001")
        {
            punch1_playOnce = true;
        }

        if (playerSprite.sprite.name == "L_punch20007" ||
            playerSprite.sprite.name == "L_swipe0001" ||
            playerSprite.sprite.name == "punch20007" ||
            playerSprite.sprite.name == "swipe0001")
        {
            punch2_playOnce = true;
        }

        if (playerSprite.sprite.name == "L_swipe0008" ||
            playerSprite.sprite.name == "swipe0008")
        {
            punch3_playOnce = true;
        }

    }

    //Function that controls horizontal and vertical movement of a player
    //A player is sped up from rest to a target velocity (the max speed limit of a player)
    //Jumping is controlled through a simple jump-doublejump system
    public void Move(float Speed, bool Jump)
    {
        Vector2 targetSpeed = new Vector2(Speed, rb2D.velocity.y);
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetSpeed, ref referenceVelocity, m_smoothFactor);

        if (Jump)
        {
            if (m_grounded)
            {
                rb2D.AddForce(new Vector2(0f, jumpForce));
            }
            else
            {
                if (double_jump)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
                    rb2D.AddForce(new Vector2(0f, jumpForce));
                    double_jump = false;
                }
            }
        }
    }


    //Function to check collision with a player's head and moves player getting
    //out of the way so the player falling can fall to the floor correctly
    public float XForce;

    //If P1 get stepped on by P2 and and P1LP2R, move P1 left
    //else move P1 right, and vice versa
    /*
     *           P2
     *       <--P1
     */
    public void HeadCollision(LayerMask otherPlayer)
    {
        if (headImpulse)
        {
            if (otherPlayer == (1 << 10))
            {
                if (gameController.P1LtoP2R)
                {

                    rb2D.AddForce(new Vector2(-XForce, 0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb2D.AddForce(new Vector2(XForce, 0f), ForceMode2D.Impulse);
                }
            }
            else if (otherPlayer == (1 << 9))
            {
                if (gameController.P1LtoP2R)
                {
                    rb2D.AddForce(new Vector2(XForce, 0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb2D.AddForce(new Vector2(-XForce, 0f), ForceMode2D.Impulse);
                }
            }
            headImpulse = false;
        }
    }

    //If P1 steps on P2 and P1LP2R, move P1 left
    //else move P1 right, and vice versa
    /*
     *      <-- P1
     *            P2
     */
    public void FootCollision(LayerMask otherPlayer)
    {
        if (footImpulse)
        {
            if (otherPlayer == (1 << 10))
            {
                if (gameController.P1LtoP2R)
                {
                    rb2D.AddForce(new Vector2(-XForce, 0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb2D.AddForce(new Vector2(XForce, 0f), ForceMode2D.Impulse);
                }
            }
            else if (otherPlayer == (1 << 9))
            {
                if (gameController.P1LtoP2R)
                {
                    rb2D.AddForce(new Vector2(XForce, 0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb2D.AddForce(new Vector2(-XForce, 0f), ForceMode2D.Impulse);
                }
            }
            footImpulse = false;
        }
    }
}

