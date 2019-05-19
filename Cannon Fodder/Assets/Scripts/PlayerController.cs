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

    private Rigidbody rb;
    private float verticalLookRotation;
    private int count;
    private bool isFirstPerson;
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
        firstPerson.SetActive(false);
        orbit.SetActive(false);
        thirdPerson.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movHor = transform.right * moveHorizontal;
        Vector3 movVert = transform.forward * moveVertical;
        Vector3 movUp = Vector3.zero;

        velocity = (movHor + movVert + movUp).normalized * fpsSpeed;
        if (isFirstPerson)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSense);
            verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSense;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
            firstPerson.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }
        if(Input.GetButtonDown("Fire2"))
        {
            
            anim.SetBool("isAiming", true);
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
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveVertical > 0.1)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }


        if (!isFirstPerson)
        {
            moveHorizontal *= turnforce * Time.deltaTime;
            
            moveVertical *= speed;

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
