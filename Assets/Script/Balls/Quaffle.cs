using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaffle : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform playerToFollow;
    void Start()
    {
        
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
}
