using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class options : MonoBehaviour
{
    public bool AIvsAI;
    public bool HumanVsAI;
    public bool HumanVsHuman;
    public AIControllerScript AI_1;
    public AIControllerScript AI_2;
    public Punch_Movement_Controller move_con;
    // Start is called before the first frame update
    void Start()
    {
        if (AIvsAI) {
            move_con.useAIAssets = true;
            move_con.HumanPlayer1VsAI = false;
            AI_1.useHeuristic = false;
            AI_2.useHeuristic = false;
        }
        else if (HumanVsAI){
            move_con.useAIAssets = true;
            move_con.HumanPlayer1VsAI = true;
            AI_1.useHeuristic = true;
            AI_2.useHeuristic = false;
        }
        else if (HumanVsHuman) {
            move_con.useAIAssets = false;
            move_con.HumanPlayer1VsAI = false;
            AI_1.useHeuristic = true;
            AI_2.useHeuristic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
