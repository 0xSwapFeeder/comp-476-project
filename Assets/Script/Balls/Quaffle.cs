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
    private Rigidbody rb;
    void Start()
    {
        startGame();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFollow != null) {
            transform.localPosition = new Vector3(0.8f, 0, 1f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        } else if (transform.position.y >= maxHeight) {
            rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
        } else if (transform.position.y <= minHeight) {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    public void PickUp(Transform player) {
        playerToFollow = player.GetChild(0);
        transform.SetParent(playerToFollow);
        transform.localPosition = new Vector3(0, 0, 1);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void GetThrown(Transform direction) {
        transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(direction.forward * 10, ForceMode.Impulse);
        GetComponent<Collider>().enabled = true;
        playerToFollow = null;
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("GoalFirst")) {
            infoManager.scoreTeam1 += 10;
            startGame();
        }
        if (collision.gameObject.CompareTag("GoalSecond")) {
            infoManager.scoreTeam2 += 10;
            startGame();
        }
        if (collision.gameObject.CompareTag("Wall")) {
            rb.velocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
        }
    }

    public void startGame()
    {
        transform.position = positionSpawn;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
    }
}
