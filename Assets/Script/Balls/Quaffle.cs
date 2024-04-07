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
    void Start()
    {
        startGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFollow != null) {
            transform.localPosition = new Vector3(0.8f, 0, 1f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
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
            Destroy(gameObject);
            Instantiate(gameObject, positionSpawn, Quaternion.identity);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("GoalSecond")) {
            infoManager.scoreTeam2 += 10;
            Destroy(gameObject);
            Instantiate(gameObject, positionSpawn, Quaternion.identity);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
        }
    }

    public void startGame()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
    }
}
