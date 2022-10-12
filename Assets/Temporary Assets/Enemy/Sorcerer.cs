using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Sorcerer : MonoBehaviour
{
    public float FollowDist;
    public GameObject EnemyDeath;
    // public GameObject Enemy;
    public GameObject Projectile;
    //public 
    private Transform Player;
    public Transform Head;
    public Transform ShootLoc;
    public float ShootCoolDown;
    NavMeshAgent Agent;
    private float time = 0.0f;
    public HealthBar HealthBar;
    private float Health = 100.0f;
    public float MaxHealth = 200.0f;
    public float FireDistance = 100.0f;

    public GameObject model;
    public Animator anim;
    public void DecreaseHealth(float damage)
    {

        Health -= damage;
        HealthBar.SetHealthBar(Health / MaxHealth);
        if (Health < 0.0f)
        {
            //Instantiate(EnemyDeath, this.transform.position, this.transform.rotation);
            
            anim.enabled=false;
            model.transform.parent = null;
            Destroy(transform.parent.gameObject);
            //Destroy(Enemy);
        }
        else
        {
            HealthBar.SetHealthBar(Health / MaxHealth);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Agent = this.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
    }

    bool Attacking = false;
    IEnumerator FireBallCooldown() 
    {
        Attacking = true;
        anim.SetFloat("Speed",-1);
        yield return new WaitForSeconds(0.26f); 
        
        GameObject o = Instantiate(Projectile, ShootLoc.position, Quaternion.identity);
        o.GetComponent<Rigidbody>().velocity = 30.0f * (ShootLoc.position - Head.position);
        
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Speed", 2);
        yield return new WaitForSeconds(ShootCoolDown);

        Attacking = false;

    }

    bool Stopped = false;
    IEnumerator StopNav() 
    {
        Stopped = true;
        Agent.Stop();
        yield return new WaitForSeconds(1);
        Agent.Resume();
        Stopped = false;
    }
    // Update is called once per frame
    void Update()
    {

        float Dist2 = (this.transform.position - Player.position).sqrMagnitude;
        if (Dist2< FollowDist)
        {
            if (Dist2 > FireDistance)
            {
                Agent.SetDestination(Player.position);
               
            }
            else {
                if (Stopped) { this.transform.LookAt(Player, Vector3.up); } else 
                { StartCoroutine(StopNav()); }
                 }
            if (!Attacking) 
            {
                StartCoroutine(FireBallCooldown());
            }
           
            /*time += Time.deltaTime;
            if (time >= ShootCoolDown)
            {
                GameObject o = Instantiate(Projectile, ShootLoc.position, Quaternion.identity);
                o.GetComponent<Rigidbody>().velocity = 10.0f * (ShootLoc.position - Head.position);
                time = 0.0f;
            }*/
        }
        else { }

    }

}

