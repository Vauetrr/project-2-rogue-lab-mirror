using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementScript : MonoBehaviour
{
    

    public Animator anim;
    private Rigidbody Player;
    public Camera PlayerCamera;
    // public GameObject Projectile;
    public Material LowHealth;

    public Transform ShootLoc;
    public Transform Head;
    public Transform hand;
    public Transform PlayerModel;
    public HealthBar HealthBar;
    public HealthBar StaminaBar;
    public HealthBar ManaBar;
    private float Health = 200.0f;
    private float Mana = 200.0f;
    private float Stamina = 100.0f;

    //public Transform PlayerTransform;

    //private double fireDelay = 0;
    private int curDash = 0;
    private bool dashStart = false;
    private float dashHorizontal;
    private float dashVertical;
    private float dashTime = 0;
    private float dashTimeDefault = 2.3f;//1.0f;
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
    private bool alive = true;
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

    public float attackSlowdown = 0.75f; // slow% during attack. //original = 0.75
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
    public float MaxMana = 200.0f;

    public void SetHealthBlur(){
        float visualHealth = (Health / MaxHealth) > 0.5f? 1: (Health/MaxHealth)*2;
        LowHealth.SetFloat("_BlurSize", Mathf.Pow(1.0f - visualHealth,100));
        LowHealth.SetFloat("_Greyscale", (1.0f-visualHealth) );
        LowHealth.SetFloat("_Radius", 2.0f*visualHealth);
    }
    
    public void restartLevel(){

        //restart the scene
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
            // Debug.Log("Implement Dying here");
            Health = 0;
            alive = false;
            restartLevel();
            anim.SetBool("Dead",true);
        }

        HealthBar.SetHealthBar(Health, MaxHealth);
        SetHealthBlur();
        //_StdDeviation
        //_Radius
        //_Feather

    }
    
    public void DecreaseMana(float value){
        Mana -= value;
        if (Mana > MaxMana) { 
            Health = MaxMana; 
        }
        else if (Mana < 0){
            Mana = 0;
        }
        ManaBar.SetHealthBar(Mana, MaxMana);
    }

    void Start()
    {
        //sHealth = MaxHealth;
        LowHealth.SetFloat("_BlurSize", 0.0f);
        LowHealth.SetFloat("_Greyscale", 0.0f);
        LowHealth.SetFloat("_Radius", 2.0f);

        Stamina = MaxStamina;
        Player = this.GetComponent<Rigidbody>();
        HealthBar.SetHealthBar(Health, MaxHealth);
        ManaBar.SetHealthBar(Mana, MaxMana);
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
    
    private int preMove =0;
    private float[] RollDir = { 0.0f, 0.0f };

    public GameObject Target;
    private GameObject CurrentTarget;
    //private Vector2 RollDir = new Vector2(0.0f,0.0f);
    void PreMove() 
    {
        defaultState = (!guarding && !dashing);
        attacking = currentWeapon.attacking() || altWeapon.attacking();
        if (dashTime <= 0)
        {
            dashing = false;
            curDash = 0;
            anim.SetBool("Roll", false);

        }

        switch (preMove) 
        {
        case 0:

            break;
        case 1:
            if (dashing&&!iframed) 
            {
                dashTime = 0; //end the dash early
                AttackChainCounter =1;
                currentWeapon.normalDown(this);anim.SetBool("Roll", false);
                preMove = 0;
               // currentWeapon.normalUp(this);
               // currentWeapon.normalHold(this);

                //sprinting = false;
                
                
            }
            break;
        case 4:
            if (!attacking) 
            {
                dashing = true;
                anim.SetBool("Roll", true);
                anim.SetInteger("AttackChain", 0);
                
                dashCooldown = dashCooldownDefault;
                curDash++;
                runButtonHeld = true;
                updateStamina(-dashCost);
                dashHorizontal = RollDir[0]; // no momentum
                dashVertical = RollDir[1]; // no momentum
                
                dashTime = dashTimeDefault;
                dashStart = false;
                iframed = true;
                preMove = 0;
            }
            break;
           
        
        }
        
    }
    void readInput(){

       /* if (Input.GetKeyDown(KeyCode.Q)) 
        {
            GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("EnemyUI");
            //Vector3 ClosestEnemy = new Vector3(0.0f,0.0f,0.0f);
            Transform ClosestEnemy = EnemyList[0].GetComponent<Transform>();
            float EnemyDist = (this.transform.position -ClosestEnemy.position).magnitude;
            for (int i = 1; i < EnemyList.Length; i++) 
            {
                Transform Enemyi = EnemyList[i].GetComponent<Transform>();
                float EnemyiDist = (this.transform.position - Enemyi.position).magnitude;
                if (EnemyDist > EnemyiDist) { EnemyDist = EnemyiDist; ClosestEnemy = Enemyi; }
            }
            Destroy(CurrentTarget);
            CurrentTarget = Instantiate(Target, ClosestEnemy.position,Quaternion.identity);
            CurrentTarget.transform.parent = ClosestEnemy;
        }*/

        if (Input.GetButtonDown("Fire1"))
        {

            if (preMove==0 && defaultState) {
                AttackChainCounter++; 
            }
            if (dashing) { preMove =1; }
        }

        if (!attacking)
        {

            anim.SetInteger("AttackChain", 0);
            //left mouse, normal attack
            if (Input.GetButtonDown("Fire1") && defaultState)
            {
                if (dashing) {preMove = 1;
                }
                else
                {
                    //anim.SetBool("Roll", false);
                    currentWeapon.normalDown(this);
                    sprinting = false;
                }
                
            }

            if (Input.GetButtonUp("Fire1") && defaultState)
            {
                if (dashing) { preMove = 1; }
                else 
                {
                    currentWeapon.normalUp(this);
                    sprinting = false;
                }
            }

            if (Input.GetButton("Fire1") && defaultState)
            {
                if (dashing) { preMove = 1; }
                else
                {
                    currentWeapon.normalHold(this);
                    sprinting = false;
                }
            }

            // right mouse, alt fire 
            if (Input.GetButtonDown("Fire2") && defaultState)
            {
                if (Mana > altWeapon.normalDownCost){
                    altWeapon.normalDown(this);
                    DecreaseMana(altWeapon.normalDownCost);
                    sprinting = false;
                }
            }

            if (Input.GetButtonUp("Fire2") && defaultState)
            {
                altWeapon.normalUp(this);
                sprinting = false;
            }

            if (Input.GetButton("Fire2") && defaultState)
            {
                if (dashing) { preMove = 2; }
                else
                {
                    altWeapon.normalHold(this);
                    sprinting = false;
                }
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
            if (attacking) 
            {
              
                preMove = 4;
                RollDir[0] = Input.GetAxisRaw("Horizontal"); 
                RollDir[1] = Input.GetAxisRaw("Vertical");
            }
            else
            {
                dashing = true;
                anim.SetBool("Roll", true);
                dashStart = true;
                dashCooldown = dashCooldownDefault;
                curDash++;
                runButtonHeld = true;
                updateStamina(-dashCost);
            }
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
        PlayerModel.rotation = Quaternion.RotateTowards(PlayerModel.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.y)), 500.0f * Time.deltaTime);

        float dashMod = (dashTime < dashTimeDefault/2.0f) ? dashTime : 1.0f;
        if (dashMod < (moveSpeed/ dashSpeed )) { dashMod = (moveSpeed / dashSpeed); }
        Player.velocity = new Vector3(dir.x * dashSpeed * dashMod, Player.velocity.y, dir.y * dashSpeed * dashMod);

        dashTime -= Time.deltaTime*3.0f;

        if (dashTime < iframedDefault){
            iframed = false;
            if (runButtonHeld){
                sprinting = true;
                dashTime = 0; //end the dash early
                anim.SetBool("Roll", false);
                preMove = 0;
            }
        }

        
    }

    void movePlayer() {
        if (dashing) {
            dashPlayer();
            return;
        }
        Vector2 Left = new Vector3(1.0f, -1.0f);
        Vector2 Forward = new Vector3(1.0f, 1.0f);
        // Player.AddForce( Input.GetAxis("Horizontal")*Left +  Input.GetAxis("Vertical")*Forward);

        // Vector3 dir = new Vector2(0.0f,Player.velocity.y,0.0f)+ Input.GetAxisRaw("Horizontal") * Left + Input.GetAxisRaw("Vertical") * Forward;
        Vector2 dir = Input.GetAxisRaw("Horizontal") * Left + Input.GetAxisRaw("Vertical") * Forward;
        dir = dir.normalized;

        //PlayerModel.LookAt(new Vector3(PlayerModel.position.x+dir.x, PlayerModel.position.y, PlayerModel.position.z + dir.y),Vector3.up);

        if (dir.x != 0 || dir.y != 0)
        {
            PlayerModel.rotation = Quaternion.RotateTowards(PlayerModel.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.y)), 500.0f * Time.deltaTime);
        }

        // if ((dir.x != 0 || dir.y != 0)
        //     && (!attacking || ((currentWeapon.attacking() && !currentWeapon.lockDirectionDuringAttack)
        //     || (altWeapon.attacking() && !altWeapon.lockDirectionDuringAttack)))
        // )
        // { PlayerModel.rotation = Quaternion.RotateTowards(PlayerModel.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.y)), 500.0f * Time.deltaTime); }
        
        
        float slowV = sprinting?sprintSpeed:(guarding?guardSlowdown:(attacking?attackSlowdown:1));
        //testSpeed.SetText(slowV.ToString());
        Player.velocity = new Vector3(dir.x * moveSpeed * slowV, Player.velocity.y, dir.y * moveSpeed * slowV);

        //Debug
        // if (!((currentWeapon.attacking() && currentWeapon.lockMovementDuringAttack)
        //       || (altWeapon.attacking() && altWeapon.lockMovementDuringAttack)))
        // { Player.velocity = new Vector3(dir.x * moveSpeed * slowV, Player.velocity.y, dir.y * moveSpeed * slowV); }
        // else { Player.velocity = new Vector3(0.0f, Player.velocity.y, 0.0f); }
        
        
        if (dir.sqrMagnitude > 0.1f) { if (sprinting) { anim.SetInteger("MoveSpeed", 2); } else { anim.SetInteger("MoveSpeed", 1); } }
        else { anim.SetInteger("MoveSpeed", 0); }
        if (Player.velocity.y < -5.0f) { anim.SetBool("Falling", true); } else { anim.SetBool("Falling", false); }
    }

    // Update is called once per frame
    void Update()
    {

        PreMove();
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

        // testAtt.SetText(this.attacking.ToString());
        // testCur.SetText(this.currentWeapon.attacking().ToString());
        // testAlt.SetText(this.altWeapon.attacking().ToString());

        movePlayer();
    }
}
