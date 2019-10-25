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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //character movement
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical"));
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
