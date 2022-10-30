using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Animator anim;
    public float FollowDist;
    public GameObject EnemyDeath;
    // public GameObject Enemy;
    public GameObject Projectile;
    //public 
    private Transform Player;
    public Transform Head;
    public Transform ShootLoc;
    public float ShootCoolDown;
    public int shots = 5;
    public bool isBoss = true;
    NavMeshAgent Agent;
    private float time = 0.0f;
    public HealthBar HealthBar;
    private float Health = 100.0f;
    public float MaxHealth = 200.0f;

    [SerializeField] PlayerMovementScript finalScreen;
    private GamePlayManager manager;
    private bool EnemyEngaged = false;

    public void DecreaseHealth(float damage)
    {

        Health -= damage;
        if (Health < 0.0f)
        {
            finalScreen.winSequence();
            GamePlayManager.manager.musicManager.lockTrack = false;
            GamePlayManager.manager.musicManager.FadeTrack(3, 5.0f);
            GamePlayManager.manager.musicManager.lockTrack = true;
            manager.EnemyKilled();
            Instantiate(EnemyDeath, this.transform.position, this.transform.rotation);
            Destroy(transform.parent.gameObject);

            

            //Destroy(Enemy);
        }
        else
        {
            HealthBar.SetHealthBar(Health / MaxHealth);
        }
    }
    
    
    private float Speed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        HealthBar.SetHealthBar(Health / MaxHealth);
        Agent = this.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        Speed = Agent.speed;
        manager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GamePlayManager>();
        
    }
    
    private bool Attacking = false;
    public float AttackDistance = 5.0f;
    public float AttackCoolDown = 1.0f;
    public GameObject Arm;

    IEnumerator RSwing()
    {
        Attacking = true;
        Agent.angularSpeed = 10000;//AngularSpeed*;
        //Arm.SetActive(true);
        anim.SetInteger("AttackState", 1);
        yield return new WaitForSeconds(2.0f);
        anim.SetInteger("AttackState", 0);
        //Arm.SetActive(false);
        //Agent.angularSpeed = AngularSpeed;
        yield return new WaitForSeconds(AttackCoolDown);

        Attacking = false;

    }

    IEnumerator JumpAttack()
    {
        Attacking = true;
        Agent.angularSpeed = 10000;//AngularSpeed*;
        //Arm.SetActive(true);
        anim.SetInteger("AttackState", 2);
        yield return new WaitForSeconds(2.0f);
        anim.SetInteger("AttackState", 0);
        //Arm.SetActive(false);
        //Agent.angularSpeed = AngularSpeed;
        yield return new WaitForSeconds(AttackCoolDown);

        Attacking = false;

    }
 
    IEnumerator FireAttack()
    {
        Attacking = true;
        Agent.speed = Speed*0.5f;
        anim.SetInteger("AttackState", 0);

        //StartCoroutine(FireShot());
        for (int i = 0; i < shots; i++) 
        { 
           var rot = this.transform.rotation * Quaternion.Euler(0, 180f, 0);
           GameObject o = Instantiate(Projectile, ShootLoc.position, rot);
           o.GetComponent<Rigidbody>().velocity = 10.0f * (ShootLoc.position - Head.position);

           //yield return new WaitForSeconds(1);
           yield return new WaitForSeconds(ShootCoolDown);
        }
        
        

        yield return new WaitForSeconds(AttackCoolDown);

        Attacking = false;

    }

    // Update is called once per frame
    void Update()
    {

        float Dist2 = (this.transform.position - Player.position).sqrMagnitude;
        if (Dist2 < FollowDist)
        {
            //anim.SetInt("MoveState",1);
            //if (!EnemyEngaged) { if (isBoss) { manager.musicManager.FadeTrack(1,1.0f); EnemyEngaged = true; } else { EnemyEngaged = true; manager.EnemyEngaged(); } }
            if (!EnemyEngaged) {EnemyEngaged = true; manager.EnemyEngaged();  }

            if (Dist2 > AttackDistance)
            {

                if (!Attacking)
                {
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(new Vector3(Player.position.x-this.transform.position.x, 0,Player.position.z -this.transform.position.z)), 1000 * Time.deltaTime);
                    int p = Random.Range(0, 30);
                    if (p ==0)
                    { 
                        StartCoroutine(FireAttack());
                    }
                        
                }
                else
                {
                    Agent.speed = Speed;
                    Agent.SetDestination(Player.position);
                    anim.SetInteger("MoveState", 1);
                    
                }
            }
            else
            {
                Agent.speed = 0.0f;
                anim.SetInteger("MoveState", 0);
                if (!Attacking)
                {
                    //Agent.speed = Speed;
                    int p = Random.Range(0,6);
                    if (p > 1)
                    {
                        StartCoroutine(RSwing());
                    }
                    else if (isBoss) { StartCoroutine(JumpAttack()); }

                    
                }
                else
                {
                    //Agent.speed = 1.0f;
                    //this.transform.LookAt(new Vector3(Player.position.x, this.transform.position.y, Player.position.z), Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(new Vector3(Player.position.x-this.transform.position.x, 0, Player.position.z-this.transform.position.z)), 5000 * Time.deltaTime);
                }
            }
        }


       /* if ((this.transform.position - Player.position).sqrMagnitude < FollowDist)
        {
            Agent.SetDestination(Player.position);
            time += Time.deltaTime;
            if (time >= ShootCoolDown)
            {
                var rot = this.transform.rotation * Quaternion.Euler(0, 180f, 0);
                GameObject o = Instantiate(Projectile, ShootLoc.position, rot);
                o.GetComponent<Rigidbody>().velocity = 10.0f * (ShootLoc.position - Head.position);
                time = 0.0f;
            }
        }
        else { }*/

    }
}
