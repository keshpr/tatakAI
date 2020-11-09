using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powpow : MonoBehaviour
{
    //Alan
    //Adding game controller to fix bug with projectile forces
    public Game_Controller gameController;


    public Transform firepoint;
    //linear fireball
    public GameObject fireballPrefab;
    public int projectileSpeed;
    public string fireballPush;

    //archingfireball
    public GameObject archProjPrefab;
    public int archverticalSpeed;
    public int archhorizontalSpeed;
    public string archPush;

    //name of creator of projectiole
    public string playerName;
    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(fireballPush)) 
        {
            ShootFireball();
        }
        if (Input.GetButtonDown(archPush))
        {
            ShootArch();
        }
    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firepoint.position, firepoint.rotation) as GameObject;
        //Alan
        //Changed to accound for player flipping
        if (gameController.P1LtoP2R)
        {
            fireball.GetComponent<Hadoken>().speed = projectileSpeed;
        }
        else
        {
            fireball.GetComponent<Hadoken>().speed = -projectileSpeed;
        }
        fireball.GetComponent<Hadoken>().objectName = playerName; 

    }

    void ShootArch()
    {
        GameObject archProjectile = Instantiate(archProjPrefab, firepoint.position, firepoint.rotation) as GameObject;
        //Alan
        //Changed to account for player flipping
        if (gameController.P1LtoP2R)
        {
            archProjectile.GetComponent<Arch>().speed = archhorizontalSpeed;
        }
        else
        {
            archProjectile.GetComponent<Arch>().speed = -archhorizontalSpeed;
        }
        archProjectile.GetComponent<Arch>().vertspeed = archverticalSpeed;
        archProjectile.GetComponent<Arch>().objectName = playerName;
    }
}
