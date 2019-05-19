using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float fpsSpeed, speed , turnforce, mouseSense;
    public Text countText;
    public Text winText;
    public GameObject firstPerson, thirdPerson, orbit;
    public Animator anim;
    public Transform target;
    
    

    private Rigidbody rb;
    private float verticalLookRotation;
    private int count;
    private bool isFirstPerson,aiming;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        count = 0;
        setCountText();
        winText.text = "";
        isFirstPerson = false;
        aiming = false;
        firstPerson.SetActive(false);
        orbit.SetActive(false);
        thirdPerson.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = Input.GetAxis("Jump");

        Vector3 movHor = transform.right * moveHorizontal;
        Vector3 movVert = transform.forward * moveVertical;
        Vector3 movUp = transform.up * moveUp;

        velocity = (movHor + movVert + movUp).normalized * fpsSpeed;
        if (isFirstPerson)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSense);
            
            verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSense;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -30, 30);
            firstPerson.transform.localEulerAngles = Vector3.left * verticalLookRotation;   
        }
        
        

        if (Input.GetKeyDown("v"))
        {
            if(isFirstPerson)
            {
                firstPerson.SetActive(false);
                orbit.SetActive(false);
                thirdPerson.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isFirstPerson = false;
            }
            else
            {
                orbit.SetActive(false);
                thirdPerson.SetActive(false);
                firstPerson.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                isFirstPerson = true;
            }
        }

        if(Input.GetKeyDown("b"))
        {
            thirdPerson.SetActive(false);
            firstPerson.SetActive(false);
            orbit.SetActive(true);

        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //Animations
        if(Input.GetKey(KeyCode.W))
        {
            if (aiming)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAiming", true);
            }
            else
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAiming", false);
            }

        }
        else
        {
            if(aiming)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isAiming", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isAiming", false);
            }
            
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            if (aiming)
            {
                anim.CrossFade("Pistol Walk", 0.1f);  
            }
            else
            {
                anim.CrossFade("Walking", 0.1f);
            }
        }

        if(Input.GetButton("Fire2"))
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");



        if (!isFirstPerson)
        {
            moveHorizontal *= turnforce * Time.deltaTime;
            
            moveVertical *= speed * Time.deltaTime;

            rb.AddRelativeForce(0,0,moveVertical, ForceMode.Force);
            rb.AddTorque(transform.up * moveHorizontal, ForceMode.VelocityChange);

            
        }
        else
        {
           if(velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectibles"))
        {
            other.gameObject.SetActive(false);
            count++;
            setCountText();
        }
    }

    private void setCountText()
    {
        countText.text = "Score: " + count.ToString();
        if(count >= 12)
        {
            winText.text = "Winner!";
        }
    }

    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
    }
}
