using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerClass {
        Beater,
        Seeker,
        Chaser,
        Keeper
    }
    private Rigidbody rb;
    private PlayerClass playerClass;
    public float moveSpeed = 7f;
    public float rotationSpeed = 5f;
    public Camera playerCamera;
    public float sensitivity = 1.5f;
    private bool isHoldingBall = false;
    private GameObject ballHolding;
    private bool isControlEnabled = true;
    private bool canPickUpBall = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        string playerClassPref = PlayerPrefs.GetString("PlayerClass");
        switch (playerClassPref) {
            case "Beater":
                playerClass = PlayerClass.Beater;
                gameObject.tag = "Beater";
                break;
            case "Seeker":
                playerClass = PlayerClass.Seeker;
                gameObject.tag = "Seeker";
                break;
            case "Chaser":
                playerClass = PlayerClass.Chaser;
                gameObject.tag = "Chaser";
                break;
            case "Keeper":
                playerClass = PlayerClass.Keeper;
                gameObject.tag = "Keeper";
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (isControlEnabled) {

            float turnY = Input.GetAxis("Mouse Y") * sensitivity;
            float turnX = Input.GetAxis("Mouse X") * sensitivity;

            Vector3 newRotation = transform.rotation.eulerAngles;
            newRotation.y += turnX;
            newRotation.z -= turnY;

            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    void FixedUpdate()
    {
        if (isControlEnabled) {

            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0f, verticalInput).normalized);   
            rb.velocity = moveDirection * moveSpeed;

            if (Input.GetMouseButtonDown(0) && isHoldingBall) {
                ballHolding.GetComponent<Quaffle>().GetThrown(playerCamera.transform);
                isHoldingBall = false;
                StartCoroutine(WaitBeforeNextPickup());
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        // if (collision.gameObject.tag == "Quaffle") {
        //     Debug.Log("Quaffle");
        //     if (Input.GetKeyDown(KeyCode.E)) {
        //         ballHolding = collision.gameObject;
        //         ballHolding.GetComponent<Quaffle>().PickUp(transform);
        //         isHoldingBall = true;
        //     }
        // }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Quaffle" && canPickUpBall) {
            ballHolding = collision.gameObject;
            ballHolding.GetComponent<Quaffle>().PickUp(transform);
            isHoldingBall = true;
        }
        StartCoroutine(HandleCollision());
    }
    IEnumerator HandleCollision()
    {
        isControlEnabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.8f);

        isControlEnabled = true;
    }

    IEnumerator WaitBeforeNextPickup() {
        canPickUpBall = false;
        yield return new WaitForSeconds(1f);
        canPickUpBall = true;
    }
}
