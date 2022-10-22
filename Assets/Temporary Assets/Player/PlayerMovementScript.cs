using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Animator anim;
    private Rigidbody Player;
    public Camera PlayerCamera;
    // public GameObject Projectile;
    public Material LowHealth;

    public Transform ShootLoc;
    public Transform Head;
    public HealthBar HealthBar;
    public HealthBar StaminaBar;
    private float Health = 200.0f;
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
    private float sprintCost = 20.0f; // Time.deltaTime * value
    private float dashCost = 50.0f;
    private bool runButtonHeld = false;

    // START player state: altered by input and gameplay
    private bool guarding = false;
    private bool dashing = false;
    private bool sprinting = false;
    public bool iframed = false;
    private bool defaultState = true;
    private bool attacking = false;
    // END player state


    // START Variables: these can be changed mid-game
    public Weapon currentWeapon;
    public Weapon altWeapon;
    public float moveSpeed = 5.0f; // movement speed.
    public float dashSpeed = 15.0f; // dash speed. 
                                    // ideally will be faster than moveSpeed.
    public float attackSpeed = 1; // Multiplier, controls delay between attacks
                                   // lower value = faster attacks
                                   // 0 = infinitely fast
    public float guardSlowdown = 0.35f; // slow% during guard.
                                        // 0 = can't move, 1 = original speed

    public float attackSlowdown = 0.75f; // slow% during attack.
                                        // 0 = can't move, 1 = original speed
    public float guardDamageDecrease = 0.2f; // damage decrease% during guard.
                                             // 0 = invincible, 1 = origianl dmg
    public int dashLimit = 1; // how many dashes can be chained.
                              // 0 = no dashes allowed.
                              // END Variables

    public float sprintSpeed = 1.75f; // multiplier for sprinting speed
                                      // 1 = normal speed, higher = faster

    public float MaxHealth = 200.0f;
    public float MaxStamina = 200.0f;

    public void SetHealthBlur(){
        float visualHealth = (Health / MaxHealth) > 0.5f? 1: (Health/MaxHealth)*2;
        LowHealth.SetFloat("_BlurSize", Mathf.Pow(1.0f - visualHealth,100));
        LowHealth.SetFloat("_Greyscale", (1.0f-visualHealth) );
        LowHealth.SetFloat("_Radius", 2.0f*visualHealth);
    }
    
    public void DecreaseHealth(float damage) 
    {
        //Debug.Log("e");
        if (iframed){
            return;
        }

        Health -= damage * ((guarding && damage > 0)? guardDamageDecrease : 1);
        //Debug.Log(guarding);
        if (Health > MaxHealth) { 
            Health = MaxHealth; 
        }
        else if (Health < 0) { 
            Debug.Log("Implement Dying here"); 
        }

        HealthBar.SetHealthBar(Health / MaxHealth);
        SetHealthBlur();
        //_StdDeviation
        //_Radius
        //_Feather

    }
    // Start is called before the first frame update

    void Start()
    {
        //sHealth = MaxHealth;
        LowHealth.SetFloat("_BlurSize", 0.0f);
        LowHealth.SetFloat("_Greyscale", 0.0f);
        LowHealth.SetFloat("_Radius", 2.0f);

        Stamina = MaxStamina;
        Player = this.GetComponent<Rigidbody>();
        HealthBar.SetHealthBar(Health/MaxHealth);
        StaminaBar.SetHealthBar(Stamina/MaxStamina);

        SetHealthBlur();

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
        StaminaBar.SetHealthBar(Stamina/MaxStamina);
        return;
    }

    void updateDelay(){
        currentWeapon.updateDelay();
        altWeapon.updateDelay();
        dashCooldown -= Time.deltaTime;
        if (dashCooldown < dashCooldownDefault){
            dashCooldown = 0;
        }
        if (!dashing && !sprinting){ // if not consuming/recently consumed stamina
            updateStamina(Time.deltaTime * staminaRegeneration);
        }
        
        if (sprinting && Stamina <= 0){ // Stamina CAN go below zero! But barely.
            sprinting = false;
        }
    }

    public int AttackChainCounter = 0;

    
    void readInput(){

        defaultState = (!guarding && !dashing);
        attacking = currentWeapon.attacking() || altWeapon.attacking();

        if (Input.GetButtonDown("Fire1") && defaultState)
        { 
            AttackChainCounter++;
        }

        if (!attacking)
        {

            //left mouse, normal attack
            if (Input.GetButtonDown("Fire1") && defaultState)
            {
                
                //if (!Attacking) { StartCoroutine(AnimationChain()); }

                currentWeapon.normalDown(this);
                sprinting = false;
            }

            if (Input.GetButtonUp("Fire1") && defaultState)
            {
                currentWeapon.normalUp(this);
                sprinting = false;
            }

            if (Input.GetButton("Fire1") && defaultState)
            {

                currentWeapon.normalHold(this);
                sprinting = false;
            }

            // right mouse, alt fire 
            if (Input.GetButton("Fire2") && defaultState)
            {
                Debug.Log("Using alt fire!");
                altWeapon.normalHold(this);
                sprinting = false;
            }
        }


        // hold shift to guard
        if (Input.GetButton("Fire3") && !dashing){ 
            guarding = true;
            sprinting = false; //can't sprint and guard at the same time
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
            runButtonHeld = true;
            updateStamina(-dashCost);
        }

        if (Input.GetButton("Jump")){
            if (sprinting) { // held dash button long enough to trigger sprint
                updateStamina(-(Time.deltaTime * sprintCost));
            } 
        }
        else {
            runButtonHeld = false;
            sprinting = false;
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
                sprinting = true;
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
        float slowV = sprinting?sprintSpeed:(guarding?guardSlowdown:(attacking?attackSlowdown:1));
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

        if (!attacking || ((currentWeapon.attacking() && !currentWeapon.lockDirectionDuringAttack)
            || (altWeapon.attacking() && !altWeapon.lockDirectionDuringAttack))){
            Head.LookAt(LookLoc, new Vector3(0.0f,1.0f,0.0f));
        }


        movePlayer();
    }
}
