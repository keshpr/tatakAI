using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Controller : MonoBehaviour
{
    //Other Scripts Used
    public Movement_Controller movementController;
    public Player_Movement playerController1;
    public Player_Movement playerController2;

    //Animators grabbed from each individual player
    private Animator animator1;
    private Animator animator2;

    //Players
    public GameObject P1;
    public GameObject P2;
    Rigidbody2D rb1;
    Rigidbody2D rb2;

    //Animator Variables, such as states
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_JUMP = 2;
    const int STATE_FALL = 3;
    const int STATE_BACKPEDAL = 4;
    const int STATE_ATTACK = 5;
    public int currentState1 = STATE_IDLE;
    public int currentState2 = STATE_IDLE;


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
            case STATE_ATTACK:
                animator.SetInteger("state", STATE_ATTACK);
                break;
            default:
                animator.SetInteger("state", STATE_IDLE);
                break;
        }
        currentState = state;
    }

    private void ControlStates(int stateNum, ref int currentState, ref bool attack)
    {
        Player_Movement playerController;
        float h_move;
        Rigidbody2D rb;
        Animator animator;


        switch (stateNum)
        {
            case 1:
                playerController = playerController1;
                h_move = movementController.h_move_1;
                rb = rb1;
                animator = animator1;
                break;
            case 2:
                playerController = playerController2;
                h_move = movementController.h_move_2;
                rb = rb2;
                animator = animator2;
                break;
            default:
                playerController = playerController1;
                h_move = movementController.h_move_1;
                rb = rb1;
                animator = animator1;
                break;
        }
        if (playerController.health > 0)
        {
            if (!attack)
            {
                if (Mathf.Abs(h_move) < 1f && playerController.m_grounded)
                {
                    changeState(animator, STATE_IDLE, ref currentState);
                }
                else if ((h_move > 1f || rb.velocity.x > 0.1f)
                        && playerController.m_grounded)
                {
                    changeState(animator, STATE_WALK, ref currentState);
                }
                else if ((h_move < -1f || rb.velocity.x < -0.1f)
                        && playerController.m_grounded)
                {
                    changeState(animator, STATE_BACKPEDAL, ref currentState);
                }
                else if (rb.velocity.y > 0.1f && !playerController.m_grounded)
                {
                    changeState(animator, STATE_JUMP, ref currentState);
                }
                else if (rb.velocity.y < -0.1f && !playerController.m_grounded)
                {
                    changeState(animator, STATE_FALL, ref currentState);
                }
            }
            else if (attack)
            {
                changeState(animator, STATE_ATTACK, ref currentState);
                attack = false;
            }
        }
        else
        {
            Vector2 referenceVelocity = Vector2.zero;
            Vector2 targetSpeed = new Vector2(0f, rb.velocity.y);
            //If a player dies set the animation state to the death state
            //For now as a placeholder use the IDLE state
            changeState(animator, STATE_IDLE, ref currentState);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetSpeed, ref referenceVelocity, 0.05f); ;
            if(rb.bodyType != RigidbodyType2D.Static && rb.velocity == Vector2.zero)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
