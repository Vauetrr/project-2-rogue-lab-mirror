using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFollow : MonoBehaviour
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
    public void DecreaseHealth(float damage)
    {

        Health -= damage;
        if (Health < 0.0f) 
        {
            Instantiate(EnemyDeath, this.transform.position, this.transform.rotation);
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

    // Update is called once per frame
    void Update()
    {
        if ((this.transform.position - Player.position).sqrMagnitude < FollowDist)
        {
            Agent.SetDestination(Player.position);
            time += Time.deltaTime;
            if (time >= ShootCoolDown) 
            {
                GameObject o = Instantiate(Projectile, ShootLoc.position, Quaternion.identity);
                o.GetComponent<Rigidbody>().velocity = 10.0f * (ShootLoc.position - Head.position);
                time = 0.0f;
            }
        }
        else { }
       
    }

}
