using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASeekerAgent : IAAgent
{
    public float boostRadius = 10;
    public float catchRadius = 3;
    private GoldenSnitch goldenSnitch;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        goldenSnitch = GameObject.FindGameObjectWithTag("Snitch").GetComponent<GoldenSnitch>();
        movement.Target = goldenSnitch.transform;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        movement.Pursue();
        if (Vector3.Distance(transform.position, goldenSnitch.transform.position) < boostRadius)
            movement.Boost();
        if (Vector3.Distance(transform.position, goldenSnitch.transform.position) < catchRadius)
            goldenSnitch.GetCatch(team);
    }
}
