using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody Player;
    public Camera PlayerCamera;
    public GameObject Projectile;
    public Transform ShootLoc;
    public Transform Head;
    public HealthBar HealthBar;
    private float Health = 100.0f;
    public float MaxHealth = 200.0f;
    //public Transform PlayerTransform;

    private int fireDelay = 0;
    private int fireSpeed = 100;
    
    private bool guarding = false; // hold right mouse to guard

    public void DecreaseHealth(float damage) 
    {
        //Debug.Log("e");
        
        Health -= damage;
        if (Health > MaxHealth) { Health = MaxHealth; }
        else if (Health < 0) { Debug.Log("Implement Dying here"); }
        HealthBar.SetHealthBar(Health/MaxHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = this.GetComponent<Rigidbody>();
        //PlayerTransform = this.GetComponent<Transform>();
    }

    void updateDelay(){
        fireDelay --;
        if (fireDelay < 0){
            fireDelay = 0;
        }
    }

    void getInput(){
        if (Input.GetButton("Fire1") && fireDelay <= 0)
        {
             //Instantiate(Projectile, new Vector3(0, 1, 0), Quaternion.identity);
            fireDelay = fireSpeed;
            GameObject o = Instantiate(Projectile, ShootLoc.position, Quaternion.identity);
            o.GetComponent<Rigidbody>().velocity = 10.0f*(ShootLoc.position - Head.position);
            //Debug.Log(Input.mousePosition);
            Debug.Log(fireDelay);
        }
        Debug.Log(guarding);
        if (Input.GetButton("Fire2")){
            guarding = true;
        }
        else {
            guarding = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        updateDelay();
        getInput();

        Ray ShootLocation = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        float al = ShootLocation.direction.y;//Vector3.Dot(ShootLocation.direction, new Vector3(0.0f,1.0f,0.0f));
        float val = Head.position.y - ShootLocation.origin.y;//(PlayerTransform.position - PlayerCamera.transform.position).y; //(center - .Origin).Dot(normal);
        float T = (val / al);
        Vector3 LookLoc = T * ShootLocation.direction + ShootLocation.origin;

        Head.LookAt(LookLoc, new Vector3(0.0f,1.0f,0.0f));
        //Vector3 Left = new Vector3( 5.0f, 0.0f, 0.0f );
        //Vector3 Forward = new Vector3( 0.0f, 0.0f, 5.0f ); 
        Vector3 Left = guarding?(new Vector3( 1.5f, 0.0f, -1.5f )):(new Vector3( 5.0f, 0.0f, -5.0f ));
        Vector3 Forward = guarding?(new Vector3( 1.5f, 0.0f, 1.5f )):(new Vector3( 5.0f, 0.0f, 5.0f ));

        // Player.AddForce( Input.GetAxis("Horizontal")*Left +  Input.GetAxis("Vertical")*Forward);
        Player.velocity = new Vector3(0.0f,Player.velocity.y,0.0f)+ Input.GetAxis("Horizontal") * Left + Input.GetAxis("Vertical") * Forward;
    }
}
