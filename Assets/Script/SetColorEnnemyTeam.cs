using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorEnnemyTeam : MonoBehaviour
{
    public GameObject[] skinsToChangeColor;
    public Material hufflepuffMaterial;
    public Material ravenclawMaterial;
    public Material slytherinMaterial;
    public Material gryffindorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        string ennemyTeam = PlayerPrefs.GetString("Team");
        switch (ennemyTeam)
        {
            case "Gryffindor":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = slytherinMaterial;
                }
                break;
            case "Slytherin":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = gryffindorMaterial;
                }
                break;
            case "Hufflepuff":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = ravenclawMaterial;
                }
                break;
            case "Ravenclaw":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = hufflepuffMaterial;
                }
                break;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
