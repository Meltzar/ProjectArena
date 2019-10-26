using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    Rigidbody rb;
    Camera cam;
    Animator anim;

    Vector3 moveDir;
    Vector3 mousePos;
    float moveSpeed = 4;

    float camOffsetY = 9.5f;
    float camOffsetZ = -4.0f;

    bool attacking = false;
    bool blocking = false;
    public Collider weaponColl;
    public Collider shieldColl;
    float attackDashForce = 500;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        weaponColl.enabled = false;
        shieldColl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        InputProcessing();
    }

    void Movement()
    {
        //move camera with character
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + camOffsetY, transform.position.z + camOffsetZ);

        if (!attacking && !blocking)
        {
            //character movement
            moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDir.Normalize();

            rb.velocity = moveDir * moveSpeed;

            //changes animation for movement
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                anim.SetInteger("Condition", 1);
            }
            else
            {
                anim.SetInteger("Condition", 0);
            }
        }

        if (!attacking)
        {
            //character facing mouse cursor
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane ground = new Plane(Vector3.down, Vector3.zero);
            float rayDistance;

            if (ground.Raycast(ray, out rayDistance))
            {
                Vector3 point = (ray.GetPoint(rayDistance));
                transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
            }
        }
    }

    void InputProcessing() 
    {
        // KeyCodes need to be changed to buttons
        if (Input.GetKeyDown(KeyCode.Mouse0) && !blocking && !attacking)
        {
            //Debug.Log("Attack");
            rb.velocity = Vector3.zero;
            attacking = true;
            rb.AddForce(transform.forward * attackDashForce); //dash needs to be changed
            anim.SetBool("Attacking", true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Insert block commands here
            rb.velocity = Vector3.zero;
            blocking = true;
            anim.SetBool("Blocking", true);
            shieldColl.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            blocking = false;
            anim.SetBool("Blocking", false);
            shieldColl.enabled = false; //needs to be changed to only desable when leaving block state
        }
    }

    public void AttackEvent() 
    {
        rb.velocity = Vector3.zero;
        weaponColl.enabled = true;
    }

    public void AttackReturnEvent() 
    {
        weaponColl.enabled = false;
        attacking = false;
        anim.SetBool("Attacking", false);
    }
}
