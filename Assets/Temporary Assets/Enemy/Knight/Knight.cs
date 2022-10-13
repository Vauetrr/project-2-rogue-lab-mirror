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
    public void DecreaseHealth(float damage)
    {

        Health -= damage;
        HealthBar.SetHealthBar(Health / MaxHealth);
        if (Health < 0.0f)
        {
            //Instantiate(EnemyDeath, this.transform.position, this.transform.rotation);
            anim.enabled = false;
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
        Sword.SetActive(true);
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Attack", false);
        Sword.SetActive(false);
        yield return new WaitForSeconds(AttackCoolDown);

        Attacking = false;

    }

    bool Stopped = false;
    IEnumerator StopNav()
    {
        anim.SetInteger("MoveState", 0);
        Stopped = true;
        Agent.Stop();
        yield return new WaitForSeconds(1);
        Agent.Resume();
        Stopped = false;
        //anim.SetInteger("MoveState", 1);
    }
    // Update is called once per frame
    void Update()
    {

        float Dist2 = (this.transform.position - Player.position).sqrMagnitude;
        if (Dist2 < FollowDist)
        {
            //anim.SetInt("MoveState",1);

            if (Dist2 > AttackDistance)
            {
                Agent.SetDestination(Player.position);
                anim.SetInteger("MoveState", 1);
            }
            else
            {
                if (Stopped) { this.transform.LookAt(Player, Vector3.up); }
                else
                { StartCoroutine(StopNav()); }
            }
            if (!Attacking)
            {
                StartCoroutine(FireBallCooldown());
            }

        }
        else { }

    }

}
