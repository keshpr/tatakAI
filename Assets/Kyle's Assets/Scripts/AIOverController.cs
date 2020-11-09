using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOverController : MonoBehaviour
{

    public AIControllerScript AI_1;
    public AIControllerScript AI_2;
    public Camera_Controller Camera_Controller;
    public Game_Controller game_controller;
    private bool flipStart;

    //Speed up environment
    public float timeScale = 5f;

    // Start is called before the first frame update
    void Start()
    {
        AI_1.thisPlayer.transform.position = new Vector3(AI_1.start_x, AI_1.start_y, 0);
        AI_1.health = 100;
        AI_2.thisPlayer.transform.position = new Vector3(AI_2.start_x, AI_2.start_y, 0);
        AI_2.health = 100;
        flipStart = true;
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void trainingReset() {
        /*
        if (!flipStart) {
            AI_1.thisPlayer.transform.position = new Vector2(AI_1.start_x, AI_1.start_y);
            AI_2.thisPlayer.transform.position = new Vector2(AI_2.start_x, AI_2.start_y);
        }
        else {
            AI_1.thisPlayer.transform.position = new Vector2(AI_2.start_x, AI_2.start_y);
            AI_2.thisPlayer.transform.position = new Vector2(AI_1.start_x, AI_1.start_y);
        }

        flipStart = !flipStart;
        AI_1.health = 100;
        AI_2.health = 100;
        Camera_Controller.transform.position = new Vector3(0, 0, -10);
        */
        
        game_controller.Reset();
        Debug.Log("reset bug");
    }
}
