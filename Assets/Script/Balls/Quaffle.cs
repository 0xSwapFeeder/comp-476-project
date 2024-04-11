using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quaffle : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform playerToFollow;
    public InfoManager infoManager;
    public Vector3 positionSpawn;
    private readonly float maxHeight = 20f;
    private readonly float minHeight = -8f;
    public float maxBounceSpeed = 10;
    private Rigidbody rb;
    AudioManager audioManager;
    void Start()
    {
        startGame();
        transform.SetParent(null);
        rb = GetComponent<Rigidbody>();
        GameObject particleSystem = transform.GetChild(1).gameObject;
        string playerClassPref = PlayerPrefs.GetString("PlayerClass");
        if (playerClassPref ==  "Chaser" || playerClassPref == "Keeper") {
            particleSystem.SetActive(true);
        } else {
            particleSystem.SetActive(false);
        }
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFollow != null) {
            transform.localPosition = new Vector3(0.8f, 0, 1f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        } else if (transform.position.y > maxHeight) {
            float speed = (float)Mathf.Abs((float)rb.velocity.y) * 1.02f;
            if (speed > maxBounceSpeed)
                speed = maxBounceSpeed;

            rb.velocity = new Vector3(rb.velocity.x, -speed, rb.velocity.z);
            transform.position.Set(transform.position.x, maxHeight, transform.position.z);
        } else if (transform.position.y < minHeight) {
            float speed = (float)Mathf.Abs((float)rb.velocity.y) * 1.02f;
            if (speed > maxBounceSpeed)
                speed = maxBounceSpeed;

            rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
            transform.position.Set(transform.position.x, minHeight, transform.position.z);
        }
    }

    public void PickUp(Transform player) {
        playerToFollow = player.GetChild(0);
        transform.SetParent(playerToFollow);
        transform.localPosition = new Vector3(0, 0, 1);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void GetThrown(Transform direction) {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(direction.forward * 1000, ForceMode.Impulse);
        GetComponent<Collider>().enabled = true;
        playerToFollow = null;
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("GoalFirst")) {
            infoManager.GoalFirstTeam();
            startGame();
            audioManager.PlaySFX(audioManager.goal);
        }
        if (collision.gameObject.CompareTag("GoalSecond")) {
            infoManager.GoalSecondTeam();
            startGame();
            audioManager.PlaySFX(audioManager.goal);
        }
        if (collision.gameObject.CompareTag("Wall")) {
            rb.velocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
        }
    }

    public void startGame()
    {
        transform.position = positionSpawn;
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
    }
}
