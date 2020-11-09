using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Movement_Controller : MonoBehaviour
{
    public Punch_Player_Movement controller1;
    public Punch_Player_Movement controller2;
    public GameObject player1;
    public GameObject player2;

    //use the ai controllers to control players
    //must be enabled to train
    public bool useAIAssets;

    //use the arrow keys to play as player, and AI to control player2
    public bool HumanPlayer1VsAI;

    [Range(0, 100)] [SerializeField] private float moveSpeed = 3f;

    public float h_move_1;//Player1's Horizontal Movement
    public float h_move_2;//Player2's Horizontal Movement
    public bool m_jump_1 = false;//Player1's jump condition
    public bool m_jump_2 = false;//Player2's jump condition
    public bool m_attack_1 = false;
    public bool m_anim_attack_1 = false;
    public bool m_attack_2 = false;
    public bool m_anim_attack_2 = false;

    //AI Movement
    public AIControllerScript AI_Controller_1;
    public AIControllerScript AI_Controller_2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (useAIAssets)
        {
            if (HumanPlayer1VsAI)
            {
                h_move_1 = Input.GetAxisRaw("Horizontal_1") * moveSpeed;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    m_jump_1 = true;
                }
                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    m_attack_1 = true;
                    m_anim_attack_1 = true;
                }
            }
            else
            {
                h_move_1 = AI_Controller_1.getCurrentHorizontal() * moveSpeed;
                if (AI_Controller_1.getJumpState())
                {
                    m_jump_1 = true;
                }
                if (AI_Controller_1.getAttackState()) 
                {
                    m_attack_1 = true;
                    m_anim_attack_1 = true;
                }
            }

            h_move_2 = AI_Controller_2.getCurrentHorizontal() * moveSpeed;
            if (AI_Controller_2.getJumpState())
            {
                m_jump_2 = true;
            }
            if (AI_Controller_2.getAttackState()) 
            {
                m_attack_2 = true;
                m_anim_attack_2 = true;
            }
        }
        else
        {
            h_move_1 = Input.GetAxisRaw("Horizontal_1") * moveSpeed;
            if (Input.GetKeyDown(KeyCode.W))
            {
                m_jump_1 = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_attack_1 = true;
                m_anim_attack_1 = true;
            }

            h_move_2 = Input.GetAxisRaw("Horizontal_2") * moveSpeed;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_jump_2 = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_attack_2 = true;
                m_anim_attack_2 = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (controller1.health > 0)
        {
            controller1.Move(h_move_1, m_jump_1);
            m_jump_1 = false;
            controller1.Attack(m_attack_1);
            m_attack_1 = false;
        }

        if (controller2.health > 0)
        {
            controller2.Move(h_move_2, m_jump_2);
            m_jump_2 = false;
            controller2.Attack(m_attack_2);
            m_attack_2 = false;
        }
    }
}