using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AIControllerScript : Agent
{
    public float proiximity_radius = 4.0f;
    public int damage = 10;
    
    private float currentHorizontal; // last horizontal movement decision by the agent
    private bool currentJumpState; // last jump decision by the agent
    // private Queue<int> currentAttackState; // last attack decision by the agent
    public bool currentAttackState;
    // Start is called before the first frame update
    public GameObject otherPlayer;
    public Rigidbody2D otherR2D;
    public GameObject thisPlayer;
    public Rigidbody2D thisR2D;
    public AIControllerScript otherScript;
    public AIOverController overlord;
    public int health;
    public int oldHealth;
    public float start_x;
    public float start_y;
    public bool useHeuristic;
    public int startTimer = 5000;
    public int timer;
    public bool isResetter; // should only be set for one of the agents

    public Punch_Player_Movement player;

    void Start()
    {
        currentHorizontal = 0;
        currentJumpState = false;
        currentAttackState = false;
        timer = startTimer;

        otherR2D = otherPlayer.GetComponent<Rigidbody2D>();
        thisR2D = thisPlayer.GetComponent<Rigidbody2D>();
        health = player.health;
        oldHealth = player.health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void finishEpisode() {
        Done();
    }

    public void giveReward(float r) {
        AddReward(r);
    }

    public override void AgentReset() {
        this.timer = startTimer;
        if (isResetter) {
            Debug.Log(this.name + "reset");
            overlord.trainingReset();
        }
    }

    //sets the observation vector that is passed to the model
    public override void CollectObservations(VectorSensor sensor) {
        //position of other and this player
        sensor.AddObservation(thisPlayer.transform.position.x);
        sensor.AddObservation(thisPlayer.transform.position.y);

        sensor.AddObservation(otherPlayer.transform.position.x);
        sensor.AddObservation(otherPlayer.transform.position.y);

        //velocities
        sensor.AddObservation(thisR2D.velocity.x);
        sensor.AddObservation(thisR2D.velocity.y);

        sensor.AddObservation(otherR2D.velocity.x);
        sensor.AddObservation(otherR2D.velocity.y);

        //health
        sensor.AddObservation(health);
        sensor.AddObservation(otherScript.health);
    }

    //gets the output from the model
    public override void AgentAction(float[] vectorAction) {
		Debug.Log("getting qaction");
        if (useHeuristic) {
            vectorAction = this.Heuristic();
        }
        //set horizontal action
        //set jump action
        int movementChoice = 0;
        int jumpChoice = 0;
        int attackChoice = 0;
        // int punchChoice = 0;

        float max = -1;
        //take max of the branches
        for (int i = 0, n = 3; i < n; i++) {
            if (vectorAction[i] > max) {
                max = vectorAction[i];
                movementChoice = i;
            }
        }

        max = -1;
        for (int i = 3, n = 5; i < n; i++) {
            if (vectorAction[i] > max) {
                max = vectorAction[i];
                jumpChoice = i;
            }
        }

        max = -1;
        for (int i = 5, n = 7; i < n; i++) {
            if (vectorAction[i] > max) {
                max = vectorAction[i];
                attackChoice = i;
            }
        }

        float directionX = 0f;

        //reset if dead
        if (this.health <= 0) Done();

        // Look up the index in the movement action list:

        // set horizontal action
        if (movementChoice == 0) { this.currentHorizontal = -1; }
        if (movementChoice == 1) { 
            this.currentHorizontal = 0; 
        }
        if (movementChoice == 2) { this.currentHorizontal = 1; }

        // set vertical movement
        if (jumpChoice == 3) { this.currentJumpState = true; }
        if (jumpChoice == 4) { this.currentJumpState = false; }

        // set attack state
        if (attackChoice == 5) { this.currentAttackState = true; }
        if (attackChoice == 6) { this.currentAttackState = false; }

        oldHealth = health;
        health = player.health;

        if (timer <= 0) {
            giveReward(-1.0f);
            otherScript.giveReward(-1.0f);
            Done();
        } 
        else {
            timer -= 1;
        }

        if (health <= 0) { 
            giveReward(-1.0f);
            otherScript.giveReward(1.0f);
            Done();
        }
        if (health < oldHealth) { giveReward(-0.5f); }
        if (otherScript.health <= 0) { 
            giveReward(1.0f);
            otherScript.giveReward(-1.0f);
            Done();
        }
        if (otherScript.health < otherScript.oldHealth) { giveReward(0.5f); }
    }

    public float getCurrentHorizontal() {
        return currentHorizontal;
    }

    public bool getJumpState() {
        return currentJumpState;
    }

    public bool getAttackState() {
        return currentAttackState;
    }

    public override float[] Heuristic() {
        System.Random rnd = new System.Random();
        float[] rand = new float[7];
        for (int i = 0; i < 7; i++) {
            rand[i] = rnd.Next(-10, 10);
        }
        return rand;
    }

}
