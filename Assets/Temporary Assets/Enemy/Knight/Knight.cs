using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Knight : MonoBehaviour
{
    public float FollowDist;
    private Transform Player;
    public GameObject Sword;
    public float AttackCoolDown;
    NavMeshAgent Agent;
    private float time = 0.0f;
    public HealthBar HealthBar;
    private float Health = 100.0f;
    public float MaxHealth = 200.0f;
    public float AttackDistance = 100.0f;

    public GameObject model;
    public Animator anim;

    private GamePlayManager manager;
    private bool EnemyEngaged=false;
    public void DecreaseHealth(float damage)
    {
        //anim.SetBool("Stagger", true);
        anim.SetInteger("Stagger", Random.Range(0,4));
        Health -= damage;
        HealthBar.SetHealthBar(Health / MaxHealth);
        if (Health < 0.0f)
        {
            //Instantiate(EnemyDeath, this.transform.position, this.transform.rotation);
            anim.enabled = false;
            model.transform.parent = null;
            manager.EnemyKilled();
            Destroy(transform.parent.gameObject);
            //Destroy(Enemy);
        }
        else
        {
            HealthBar.SetHealthBar(Health / MaxHealth);
        }
    }


    private float Speed = 0.0f;
    private float AngularSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Agent = this.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        manager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GamePlayManager>();
        Speed = Agent.speed;
        AngularSpeed = Agent.angularSpeed;
    }

    bool Attacking = false;
    IEnumerator FireBallCooldown()
    {
        Attacking = true;
        Agent.angularSpeed = 10000;//AngularSpeed*;
        Sword.SetActive(true);
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Attack", false);
        Sword.SetActive(false);
        //Agent.angularSpeed = AngularSpeed;
        yield return new WaitForSeconds(AttackCoolDown);

        Attacking = false;

    }

    bool Stopped = false;
    IEnumerator StopNav()
    {
        //   anim.SetInteger("MoveState", 0);
        //   Stopped = true;
        //   Agent.Stop();
        Agent.speed = 0.01f;
        
        yield return new WaitForSeconds(1);
        Agent.speed = Speed;
        //  Agent.Resume();
        //  Stopped = false;
        //anim.SetInteger("MoveState", 1);
    }
    // Update is called once per frame
    void Update()
    {

        float Dist2 = (this.transform.position - Player.position).sqrMagnitude;
        if (Dist2 < FollowDist)
        {
            //anim.SetInt("MoveState",1);
            if (!EnemyEngaged) { EnemyEngaged = true; manager.EnemyEngaged(); }
            
            if (Dist2 > AttackDistance)
            {
                 
                
                if (anim.GetInteger("Stagger") != 0)
                {
                    Agent.speed = 0.5f;
                    //Agent.Stop();
                }
                else
                {
                    Agent.speed = Speed;
                    //Agent.Resume();
                    Agent.SetDestination(Player.position);
                
                }
                
                anim.SetInteger("MoveState", 1);
            }
            else
            { //this.transform.rotation * (Vector3.forward);
                
                if (!Attacking)
                 {
                     Agent.speed = Speed; StartCoroutine(FireBallCooldown()); 
                 }
                 else 
                 { 
                    Agent.speed = 1.0f;
                    this.transform.LookAt(new Vector3(Player.position.x, this.transform.position.y, Player.position.z), Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards( this.transform.rotation, Quaternion.LookRotation(new Vector3(Player.position.x, 0, Player.position.z)), 500*Time.deltaTime);
                 }
                 

                //if (Stopped) { this.transform.LookAt(new Vector3(Player.position.x,this.transform.position.y, Player.position.z), Vector3.up); }
                //else
                //{ StartCoroutine(StopNav()); }


                
                
            }
           

        }
        else { }

    }

}
