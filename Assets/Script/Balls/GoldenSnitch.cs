using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InfoManager;

public class GoldenSnitch : MonoBehaviour
{
    public InfoManager infoManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GetCatch(Teams team)
    {
        infoManager.SetGameEnded(team);
    }
}
