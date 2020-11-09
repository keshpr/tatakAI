using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roundz : MonoBehaviour
{
    private Game_Controller gc;

    public GameObject Player1;
    public GameObject Player2;
    private PlayerHealth health1;
    private PlayerHealth health2;
    private int roundsp1;
    private int roundsp2;

    public float start_x1;
    public float start_y1;
    public float start_x2;
    public float start_y2;

    public GameObject[] roundSprites1;
    public GameObject[] roundSprites2;

    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<Game_Controller>();
        roundsp1 = 0;
        roundsp2 = 0;

        health1 = Player1.GetComponent<PlayerHealth>();
        health2 = Player2.GetComponent<PlayerHealth>();

      //  roundSprites1 = new GameObject[3];
       // roundSprites2 = new GameObject[3];

        for(int i = 0; i < 3; i++)
        {
            roundSprites1[i].SetActive(false);
            roundSprites2[i].SetActive(false);
        }


        
    }

    void newRound()
    {
        Debug.Log("Reset Round");
        health1.HPreset();
        health2.HPreset();

        Player1.transform.position = new Vector2(start_x1, start_y1);
        Player2.transform.position = new Vector2(start_x2, start_y2);


    }


    // Update is called once per frame
    void Update()
    {
        if(health1.currentHealth <= 0)
        {
            roundsp2 += 1;
            roundSprites2[roundsp2 - 1].SetActive(true);
            gc.Reset();
        }
        if (health2.currentHealth <= 0)
        {
            roundsp1 += 1;
            roundSprites1[roundsp1 - 1].SetActive(true);
            gc.Reset();
        }


    }
}
