using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invuln_Controller : MonoBehaviour
{
    public Punch_Player_Movement P1;
    public Punch_Player_Movement P2;

    [SerializeField] private int damageLimit1;
    [SerializeField] private int damageLimit2;
    public float refreshWindow;

    [SerializeField] private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        
    }
}
