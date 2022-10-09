using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody Player;
    public Camera PlayerCamera;
   // public GameObject Projectile;
    public Transform ShootLoc;
    public Transform Head;
    public HealthBar HealthBar;
    private float Health = 100.0f;
    public float MaxHealth = 200.0f;
    //public Transform PlayerTransform;

    //private double fireDelay = 0;
    private int curDash = 0;
    private bool dashStart = false;
    private float dashHorizontal;
    private float dashVertical;
    private float dashTime = 0;
    // START player state: altered by input and gameplay
    private bool guarding = false;
    private bool dashing = false;

    // END player state


    // START Variables: these can be changed mid-game
    public Weapon currentWeapon;// = new Gun();
    public float moveSpeed = 5.0f; // movement speed.
    public float dashSpeed = 20.0f; // dash speed. 
                                    // ideally will be faster than moveSpeed.
    public float attackSpeed = 1; // Multiplier, controls delay between attacks
                                   // lower value = faster attacks
                                   // 0 = infinitely fast
    public float guardSlowdown = 0.35f; // slow% during guard.
                                        // 0 = can't move, 1 = original speed
    public float guardDamageDecrease = 0.2f; // damage decrease% during guard.
                                             // 0 = invincible, 1 = origianl dmg
    public int dashLimit = 2; // how many dashes can be chained.
                              // 0 = no dashes allowed.
                              // END Variables


    public void DecreaseHealth(float damage) 
    {
        //Debug.Log("e");
        
        Health -= damage * ((guarding && damage > 0)? guardDamageDecrease : 1);
        if (Health > MaxHealth) { 
            Health = MaxHealth; 
        }
        else if (Health < 0) { 
            Debug.Log("Implement Dying here"); 
        }
        HealthBar.SetHealthBar(Health/MaxHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = this.GetComponent<Rigidbody>();
        HealthBar.SetHealthBar(Health/MaxHealth);
        //PlayerTransform = this.GetComponent<Transform>();
    }

    void updateDelay(){
        currentWeapon.updateDelay();
    }

    void readInput(){

        //left mouse, normal attack
        if (Input.GetButtonDown("Fire1") && !guarding) {
            currentWeapon.normalDown(this);
        }

        if (Input.GetButtonUp("Fire1") && !guarding) {
            currentWeapon.normalUp(this);
        }

        if (Input.GetButton("Fire1") && !guarding) {
            currentWeapon.normalHold(this);
        }

        //right mouse, guard
        if (Input.GetButton("Fire2")){ 
            guarding = true;
        }
        else {
            guarding = false;
        }

        //spacebar, dash
        if (Input.GetButtonDown("Jump") && curDash < dashLimit){
            dashing = true;
            dashStart = true;
            curDash++;
        }
    }

    void dashPlayer(){
        if (dashStart){
            dashHorizontal = Input.GetAxisRaw("Horizontal"); // no momentum
            dashVertical = Input.GetAxisRaw("Vertical"); // no momentum
            dashTime = 1.0f;
            dashStart = false;
        }

        Vector2 Left = new Vector3( 1.0f, -1.0f );
        Vector2 Forward = new Vector3( 1.0f, 1.0f );
        Vector2 dir = dashHorizontal * Left + dashVertical * Forward;
        dir = dir.normalized;
        Player.velocity = new Vector3(dir.x * dashSpeed * dashTime, Player.velocity.y, dir.y * dashSpeed * dashTime);

        dashTime -= 0.01f;
        if (dashTime <= 0){
            dashing = false;
            curDash = 0;
        }
    }

    void movePlayer(){
        if (dashing){
            dashPlayer();
            return;
        }
        Vector2 Left = new Vector3( 1.0f, -1.0f );
        Vector2 Forward = new Vector3( 1.0f, 1.0f );
        // Player.AddForce( Input.GetAxis("Horizontal")*Left +  Input.GetAxis("Vertical")*Forward);

        // Vector3 dir = new Vector2(0.0f,Player.velocity.y,0.0f)+ Input.GetAxisRaw("Horizontal") * Left + Input.GetAxisRaw("Vertical") * Forward;
        Vector2 dir = Input.GetAxisRaw("Horizontal") * Left + Input.GetAxisRaw("Vertical") * Forward;
        dir = dir.normalized;
        float slowV = guarding?guardSlowdown:1;
        Player.velocity = new Vector3(dir.x * moveSpeed * slowV, Player.velocity.y, dir.y * moveSpeed * slowV);
    }

    // Update is called once per frame
    void Update()
    {
        updateDelay();
        readInput();

        Ray ShootLocation = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        float al = ShootLocation.direction.y;//Vector3.Dot(ShootLocation.direction, new Vector3(0.0f,1.0f,0.0f));
        float val = Head.position.y - ShootLocation.origin.y;//(PlayerTransform.position - PlayerCamera.transform.position).y; //(center - .Origin).Dot(normal);
        float T = (val / al);
        Vector3 LookLoc = T * ShootLocation.direction + ShootLocation.origin;

        Head.LookAt(LookLoc, new Vector3(0.0f,1.0f,0.0f));

        movePlayer();
    }
}
