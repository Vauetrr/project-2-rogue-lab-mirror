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
    public StaminaBar StaminaBar;
    private float Health = 100.0f;
    private float Stamina = 100.0f;
    
    //public Transform PlayerTransform;

    //private double fireDelay = 0;
    private int curDash = 0;
    private bool dashStart = false;
    private float dashHorizontal;
    private float dashVertical;
    private float dashTime = 0;
    private float dashTimeDefault = 1.0f;
    private float iframedDefault = 0.5f;
    private float dashControl = 0.25f; // how much movements impact mid dash direction 
    private float dashCooldown = 0.0f;
    private float dashCooldownDefault = 1.0f; // how long until you can dash again.
    private float staminaRegeneration = 30.0f; // Time.deltaTime * value
    private float runningCost = 20.0f; // Time.deltaTime * value
    private float dashCost = 50.0f;
    private bool runButtonHeld = false;

    // START player state: altered by input and gameplay
    private bool guarding = false;
    private bool dashing = false;
    private bool running = false;
    private bool iframed = false;
    private bool defaultState = true;
    // END player state


    // START Variables: these can be changed mid-game
    public Weapon currentWeapon;// = new Gun();
    public float moveSpeed = 5.0f; // movement speed.
    public float dashSpeed = 15.0f; // dash speed. 
                                    // ideally will be faster than moveSpeed.
    public float attackSpeed = 1; // Multiplier, controls delay between attacks
                                   // lower value = faster attacks
                                   // 0 = infinitely fast
    public float guardSlowdown = 0.35f; // slow% during guard.
                                        // 0 = can't move, 1 = original speed
    public float guardDamageDecrease = 0.2f; // damage decrease% during guard.
                                             // 0 = invincible, 1 = origianl dmg
    public int dashLimit = 1; // how many dashes can be chained.
                              // 0 = no dashes allowed.
                              // END Variables

    public float runSpeed = 1.5f; // multiplier for running speed
                                      // 1 = normal speed, higher = faster

    public float MaxHealth = 200.0f;
    public float MaxStamina = 200.0f;

    public void DecreaseHealth(float damage) 
    {
        //Debug.Log("e");
        if (iframed){
            return;
        }

        Health -= damage * ((guarding && damage > 0)? guardDamageDecrease : 1);
        Debug.Log(guarding);
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
        StaminaBar.SetStaminaBar(Stamina/MaxStamina);
        //PlayerTransform = this.GetComponent<Transform>();
    }

    void updateStamina(float val){
        Stamina += val;
        if (Stamina > MaxStamina){
            Stamina = MaxStamina;
        }
        else if (Stamina < 0){
            Stamina = 0;
        }
        StaminaBar.SetStaminaBar(Stamina/MaxStamina);
        return;
    }

    void updateDelay(){
        currentWeapon.updateDelay();
        dashCooldown -= Time.deltaTime;
        if (dashCooldown < dashCooldownDefault){
            dashCooldown = 0;
        }
        if (!dashing && !running){ // if not consuming/recently consumed stamina
            updateStamina(Time.deltaTime * staminaRegeneration);
        }
        
        if (running == true && Stamina <= 0){ // Stamina CAN go below zero! But barely.
            running = false;
        }
    }

    void readInput(){

        defaultState = (!guarding && !dashing);

        //left mouse, normal attack
        if (Input.GetButtonDown("Fire1") && defaultState) {
            currentWeapon.normalDown(this);
            running = false;
        }

        if (Input.GetButtonUp("Fire1") && defaultState) {
            currentWeapon.normalUp(this);
            running = false;
        }

        if (Input.GetButton("Fire1") && defaultState) {
            currentWeapon.normalHold(this);
            running = false;
        }

        // right mouse, alt fire 
        if (Input.GetButton("Fire2") && defaultState){
            Debug.Log("Using alt fire!");
            currentWeapon.altAttack(this);
            running = false;
        }

        // hold shift to guard
        if (Input.GetButton("Fire3") && !dashing){ 
            guarding = true;
            running = false; //can't dash and guard at the same time
        }
        else {
            guarding = false;
        }

        //spacebar, dash; hold post-dash to run until spacebar let go
        if (Input.GetButtonDown("Jump") && curDash < dashLimit && dashCooldown <= 0 && dashCost <= Stamina){
            dashing = true;
            dashStart = true;
            dashCooldown = dashCooldownDefault;
            curDash++;
            updateStamina(-dashCost);
        }

        if (Input.GetButton("Jump")){
            if (running) { // held dash button long enough to trigger running
                updateStamina(-(Time.deltaTime * runningCost));
            } 
            
            runButtonHeld = true;
        }
        else {
            runButtonHeld = false;
            running = false;
        }
    }

    void dashPlayer(){
        float duringDashH = 0.0f;
        float duringDashV = 0.0f;
        if (dashStart){
            dashHorizontal = Input.GetAxisRaw("Horizontal"); // no momentum
            dashVertical = Input.GetAxisRaw("Vertical"); // no momentum
            dashTime = dashTimeDefault;
            dashStart = false;
            iframed = true;
        }
        else {
            duringDashH = Input.GetAxisRaw("Horizontal"); 
            duringDashV = Input.GetAxisRaw("Vertical"); 
        }

        Vector2 Left = new Vector3( 1.0f, -1.0f );
        Vector2 Forward = new Vector3( 1.0f, 1.0f );
        Vector2 dir = (((1-dashControl) * dashHorizontal) + (dashControl * duringDashH)) * Left 
                    + (((1-dashControl) * dashVertical) + (dashControl * duringDashV)) * Forward;
                    
        dir = dir.normalized;

        float dashMod = (dashTime < dashTimeDefault/2.0f) ? dashTime : 1.0f;
        Player.velocity = new Vector3(dir.x * dashSpeed * dashMod, Player.velocity.y, dir.y * dashSpeed * dashMod);

        dashTime -= Time.deltaTime*3.0f;

        if (dashTime < iframedDefault){
            iframed = false;
            if (runButtonHeld){
                running = true;
                dashTime = 0; //end the dash early
            }
        }

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
        float slowV = running?runSpeed:(guarding?guardSlowdown:1);
        Player.velocity = new Vector3(dir.x * moveSpeed * slowV, Player.velocity.y, dir.y * moveSpeed * slowV);
    }

    // Update is called once per frame
    void Update()
    {
        readInput();
        updateDelay();

        Ray ShootLocation = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        float al = ShootLocation.direction.y;//Vector3.Dot(ShootLocation.direction, new Vector3(0.0f,1.0f,0.0f));
        float val = Head.position.y - ShootLocation.origin.y;//(PlayerTransform.position - PlayerCamera.transform.position).y; //(center - .Origin).Dot(normal);
        float T = (val / al);
        Vector3 LookLoc = T * ShootLocation.direction + ShootLocation.origin;

        Head.LookAt(LookLoc, new Vector3(0.0f,1.0f,0.0f));

        movePlayer();
    }
}
