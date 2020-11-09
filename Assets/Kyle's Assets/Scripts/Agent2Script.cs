using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Agent2Script : Agent
{


    int success;
    int epoch;
    public float proiximity_radius = 2f;
    public int damage = 10;
    public float start_x;
    public int health;
    public Agent2Script script;
    public SpriteRenderer spriteRenderer;
    public float baseRewardPerTick;
    //for handling color
    private bool wasDamaged;
    private bool choseAttack;

    public float speed = 10;

    //for jumping

    public Rigidbody2D rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        success = 0;
        epoch = 0;
        health = 100;
        spriteRenderer = GetComponent<SpriteRenderer>();
        wasDamaged = false;
        choseAttack = false;
    }

    void Update() {
        if (wasDamaged && choseAttack) {
            spriteRenderer.color = new Color(1, 0, 1, 1);
        }
        else if (wasDamaged) {
            spriteRenderer.color = new Color(1, 0, 0, 1);
        }
        else if (choseAttack) {
            spriteRenderer.color = new Color(0, 0, 1, 1);
        }
        else {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        wasDamaged = false;
        choseAttack = false;
    }

    public void giveReward(float r)
    {
        SetReward(r);
    }
    private bool outside()
    {
        return this.transform.position.x <  - 8.0f || this.transform.position.x >   8.0f;
    }

    private bool isNearTarget()
    {
        float distanceToTarget = Mathf.Sqrt(Mathf.Pow((this.transform.position.x - Target.position.x), 2) + Mathf.Pow((this.transform.position.y - Target.position.y), 2));
        return distanceToTarget < proiximity_radius;
    }

    public bool causeDamage()
    {
        Agent2.GetComponent<Agent2Script>().doDamage(damage);
        Agent2.GetComponent<Agent2Script>().giveReward(-1.0f);
        giveReward(1.0f);
        if (Agent2.GetComponent<Agent2Script>().health <= 0)
        {
            Agent2.GetComponent<Agent2Script>().giveReward(-15.0f);
            giveReward(15.0f);
            Debug.Log(Agent2.name + "was killed.");
            return true;
        }
        return false;
    }

    //does damage to this object, and changes the color
    public void doDamage(int amount) {
        this.health -= amount;
        this.wasDamaged = true;
    }

    public Transform Target;
    public GameObject Agent2;
    public override void AgentReset()
    {
        this.transform.position = new Vector2(start_x, this.transform.position.y);
        health = 100;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.position.x);
        sensor.AddObservation(this.transform.position.x );

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(Agent2.GetComponent<Agent2Script>().rBody.velocity.x);
        sensor.AddObservation(this.health);
        sensor.AddObservation(Agent2.GetComponent<Agent2Script>().health);
        
    }


    public override void AgentAction(float[] vectorAction)
    {

        int choice = 0;
        float max = -1;
        //take max of the branches
        for (int i = 0, n = vectorAction.Length; i < n; i++) {
            if (vectorAction[i] > max) {
                max = vectorAction[i];
                choice = i;
            }
        }
        float directionX = 0f;

        //reset if dead
        if (this.health <= 0) Done();

        // Look up the index in the movement action list:
        if (choice == 0) { directionX = -1; }
        if (choice == 1) { directionX = 1; }
        if (choice == 2) {  
            this.choseAttack = true;
            if (isNearTarget())
            {
                if(causeDamage()) {
                     Done();
                }
            }
        }
      

        // Apply the action results to move the Agent
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(directionX * speed, 0));


        // Rewards
        float distanceToTarget = Vector2.Distance(this.transform.position, Target.position);

        //reward for being alive
        //giveReward(baseRewardPerTick);

        // Fell off platform
        if (outside())
        {
            Debug.Log("outside!!");
            SetReward(-1.0f);
            Done();
        }

        // Reached target


    }

    public override float[] Heuristic() {
        System.Random rand = new System.Random();


        var action = new float[3];
        action[0] = (float) rand.NextDouble() * 4 + 1;
        return action;
    }



}
