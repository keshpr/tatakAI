using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Anim_Controller : MonoBehaviour
{
   
    //Other Scripts Used
    public Punch_Movement_Controller movementController;
    public Punch_Player_Movement playerController1;
    public Punch_Player_Movement playerController2;
    public SoundManager soundManager;

    //Animators grabbed from each individual player
    private Animator animator1;
    private Animator animator2;

    //Players
    public GameObject P1;
    public GameObject P2;
    Rigidbody2D rb1;
    Rigidbody2D rb2;
    public GameObject WalkSound1;
    public GameObject WalkSound2;

    //Animator Variables, such as states
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_JUMP = 2;
    const int STATE_FALL = 3;
    const int STATE_BACKPEDAL = 4;
    const int STATE_ATTACK1 = 5;
    const int STATE_ATTACK2 = 6;
    const int STATE_ATTACK3 = 7;
    public int currentState1 = STATE_IDLE;
    public int currentState2 = STATE_IDLE;

    //Testing Variables
    UnityEngine.KeyCode p1_attack = KeyCode.LeftShift;
    UnityEngine.KeyCode p2_attack = KeyCode.Space;
    private bool transition_attack2 = false;
    private bool transition_attack3 = false;

    //use the ai controllers to control players
    //must be enabled to train
    public bool useAIAssets;

    public AIControllerScript AI_Controller_1;
    public AIControllerScript AI_Controller_2;

    // Start is called before the first frame update
    void Start()
    {
        animator1 = P1.GetComponent<Animator>();
        animator2 = P2.GetComponent<Animator>();
        rb1 = P1.GetComponent<Rigidbody2D>();
        rb2 = P2.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        ControlStates(1, ref currentState1, ref movementController.m_anim_attack_1);
        ControlStates(2, ref currentState2, ref movementController.m_anim_attack_2);
    }



    //Function used to change a state by specifying and animator and a state
    private void changeState(Animator animator, int state, ref int currentState)
    {
        if (currentState == state)
            return;

        switch (state)
        {
            case STATE_IDLE:
                animator.SetInteger("state", STATE_IDLE);
                break;
            case STATE_WALK:
                animator.SetInteger("state", STATE_WALK);
                break;
            case STATE_JUMP:
                animator.SetInteger("state", STATE_JUMP);
                break;
            case STATE_FALL:
                animator.SetInteger("state", STATE_FALL);
                break;
            case STATE_BACKPEDAL:
                animator.SetInteger("state", STATE_BACKPEDAL);
                break;
            case STATE_ATTACK1:
                animator.SetInteger("state", STATE_ATTACK1);
                break;
            case STATE_ATTACK2:
                animator.SetInteger("state", STATE_ATTACK2);
                break;
            case STATE_ATTACK3:
                animator.SetInteger("state", STATE_ATTACK3);
                break;
            default:
                animator.SetInteger("state", STATE_IDLE);
                break;
        }
        currentState = state;
    }

    private void ControlStates(int stateNum, ref int currentState, ref bool attack)
    {
        Punch_Player_Movement playerController;
        float h_move;
        Rigidbody2D rb;
        Animator animator;
        UnityEngine.KeyCode attack_key;
        Game_Controller gameController;
        AIControllerScript AI_Controller;
        GameObject WalkSound;

        switch (stateNum)
        {
            case 1:
                playerController = playerController1;
                h_move = movementController.h_move_1;
                rb = rb1;
                animator = animator1;
                attack_key = p1_attack;
                gameController = playerController.gameController;
                AI_Controller = AI_Controller_1;
                WalkSound = WalkSound1;
                break;
            case 2:
                playerController = playerController2;
                h_move = movementController.h_move_2;
                rb = rb2;
                animator = animator2;
                attack_key = p2_attack;
                gameController = playerController.gameController;
                AI_Controller = AI_Controller_2;
                WalkSound = WalkSound2;
                break;
            default:
                playerController = playerController1;
                h_move = movementController.h_move_1;
                rb = rb1;
                animator = animator1;
                attack_key = p1_attack;
                gameController = playerController.gameController;
                AI_Controller = AI_Controller_1;
                WalkSound = WalkSound1;
                break;
        }

       

        if (playerController.health > 0)
        {
            if ((useAIAssets && AI_Controller.getAttackState()) || Input.GetKeyDown(attack_key))
            {
                Debug.Log("actually punching");
                if (playerController.attack2Flag == false) {
                    Debug.Log("2flag is down");
                }
                if (playerController.attack3Flag == false) {
                    Debug.Log("3flag is down");
                }
                if (playerController.attack2Flag == false &&
                    playerController.attack3Flag == false)
                {
                    Debug.Log("actually punching1");
                    changeState(animator, STATE_ATTACK1, ref currentState);
                    WalkSound.SetActive(false);
                }
                else if (playerController.attack2Flag == true && playerController.attack3Flag == false)
                {
                    Debug.Log("actually punching2");
                    changeState(animator, STATE_ATTACK2, ref currentState);
                    WalkSound.SetActive(false);
                }
                else if (playerController.attack3Flag == true)
                {
                    Debug.Log("actually punching3");
                    changeState(animator, STATE_ATTACK3, ref currentState);
                    WalkSound.SetActive(false);
                }
            }

            else if (Mathf.Abs(h_move) < 1f
                     && playerController.m_grounded
                     && playerController.attack2Flag == false
                     && playerController.attack3Flag == false)
            {
                changeState(animator, STATE_IDLE, ref currentState);
                WalkSound.SetActive(false);
            }

            else if (h_move > 1f
                     && playerController.m_grounded
                     && playerController.attack2Flag == false
                     && playerController.attack3Flag == false)
            {
                if (gameController.P1LtoP2R)
                {
                    if (stateNum == 1)
                    {
                        changeState(animator, STATE_WALK, ref currentState);
                        WalkSound.SetActive(true);
                        
                    }
                    else if (stateNum == 2)
                    {
                        changeState(animator, STATE_BACKPEDAL, ref currentState);
                        WalkSound.SetActive(true);
                    }
                }
                else
                {
                    if (stateNum == 1)
                    {
                        changeState(animator, STATE_BACKPEDAL, ref currentState);
                        WalkSound.SetActive(true);
                    }
                    else if (stateNum == 2)
                    {
                        changeState(animator, STATE_WALK, ref currentState);
                        WalkSound.SetActive(true);
                    }
                }

            }

            else if (h_move < -1f
                     && playerController.m_grounded
                     && playerController.attack2Flag == false
                     && playerController.attack3Flag == false)
            {
                if (gameController.P1LtoP2R)
                {
                    if (stateNum == 1)
                    {
                        changeState(animator, STATE_BACKPEDAL, ref currentState);
                        WalkSound.SetActive(true);
                    }
                    else if (stateNum == 2)
                    {
                        changeState(animator, STATE_WALK, ref currentState);
                        WalkSound.SetActive(true);
                    }
                }
                else
                {
                    if (stateNum == 1)
                    {
                        changeState(animator, STATE_WALK, ref currentState);
                        WalkSound.SetActive(true);
                    }
                    else if (stateNum == 2)
                    {
                        changeState(animator, STATE_BACKPEDAL, ref currentState);
                        WalkSound.SetActive(true);
                    }
                }
            }

            else if (rb.velocity.y > 0.1f
                     && !playerController.m_grounded
                     && playerController.attack2Flag == false
                     && playerController.attack3Flag == false)
            {
                changeState(animator, STATE_JUMP, ref currentState);
                WalkSound.SetActive(false);
            }

            else if (rb.velocity.y < -0.1f
                     && !playerController.m_grounded
                     && playerController.attack2Flag == false
                     && playerController.attack3Flag == false)
            {
                changeState(animator, STATE_FALL, ref currentState);
                WalkSound.SetActive(false);
            }  
               
                
        }
        else
        {
            WalkSound.SetActive(false);
            Vector2 referenceVelocity = Vector2.zero;
            Vector2 targetSpeed = new Vector2(0f, rb.velocity.y);
            //If a player dies set the animation state to the death state
            //For now as a placeholder use the IDLE state
            changeState(animator, STATE_IDLE, ref currentState);
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, targetSpeed, ref referenceVelocity, 0.05f);
            }
            if (rb.bodyType != RigidbodyType2D.Static && rb.velocity == Vector2.zero)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
