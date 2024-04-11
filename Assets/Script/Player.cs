using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InfoManager;

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
    private Teams team = Teams.Gryffindor;
    [Header("Player Settings")]
    public float moveSpeed = 20f;
    public float rotationSpeed = 12f;
    public Camera playerCamera;
    public float sensitivity = 2f;
    private bool isHoldingBall = false;
    private GameObject ballHolding;
    private bool isControlEnabled = true;
    private bool canPickUpBall = true;
    private Animator animator;
    public float boostSpeed = 2f;
    public float boostDuration = 2f;
    private bool isBoosting = false;
    
    
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        string playerClassPref = PlayerPrefs.GetString("PlayerClass");
        string teamPref = PlayerPrefs.GetString("Team");
        switch (teamPref) {
            case "Gryffindor":
                team = Teams.Gryffindor;
                break;
            case "Slytherin":
                team = Teams.Slytherin;
                break;
            case "Hufflepuff":
                team = Teams.Hufflepuff;
                break;
            case "Ravenclaw":
                team = Teams.Ravenclaw;
                break;
            default:
                break;
        }
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
            if (Input.GetMouseButton(1)) {
                newRotation.x += turnY;
            } else {
                newRotation.y += turnX;
                newRotation.z -= turnY;
            }

            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    void FixedUpdate()
    {
        if (isControlEnabled) {

            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            float UpDownInput = 0f;
            if (Input.GetKey(KeyCode.Space)) {
                UpDownInput += 1f;
            }
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C)) {
                UpDownInput += -1f;
            }
            Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, UpDownInput, verticalInput).normalized);   
            float currentSpeed = isBoosting ? moveSpeed * boostSpeed : moveSpeed;
            rb.velocity = moveDirection * currentSpeed;

            if (Input.GetMouseButtonDown(0) && isHoldingBall) {
                ballHolding.GetComponent<Quaffle>().GetThrown(playerCamera.transform);
                isHoldingBall = false;
                StartCoroutine(WaitBeforeNextPickup());
                animator.SetBool("Throwing", true);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                StartCoroutine(Boost());
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (ballHolding != null) {
            ballHolding.GetComponent<Quaffle>().GetThrown(collision.transform);
            isHoldingBall = false;
        }
        if (collision.gameObject.CompareTag("Quaffle") && (playerClass == PlayerClass.Chaser || playerClass == PlayerClass.Keeper)) {
            ballHolding = collision.gameObject;
            ballHolding.GetComponent<Quaffle>().PickUp(transform);
            isHoldingBall = true;
        }
        if (collision.gameObject.CompareTag("Snitch") && playerClass == PlayerClass.Seeker) {
            if (Input.GetKey(KeyCode.E)) {
                collision.gameObject.GetComponent<GoldenSnitch>().GetCatch(team);
            }
        }
        if (collision.gameObject.CompareTag("Bludger") && playerClass == PlayerClass.Beater) {
            if (Input.GetMouseButton(0)) {
                collision.gameObject.GetComponent<Bludger>().Throw(gameObject.transform, 10, team);
            }
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

    IEnumerator Boost() {
        isBoosting = true;
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
    }
}
