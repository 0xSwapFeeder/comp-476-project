using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorPlayerTeam : MonoBehaviour
{
    public GameObject[] skinsToChangeColor;
    public Material hufflepuffMaterial;
    public Material ravenclawMaterial;
    public Material slytherinMaterial;
    public Material gryffindorMaterial;
    void Start()
    {
        string team = PlayerPrefs.GetString("Team");
        switch (team)
        {
            case "Gryffindor":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = gryffindorMaterial;
                }
                break;
            case "Slytherin":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = slytherinMaterial;
                }
                break;
            case "Hufflepuff":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = hufflepuffMaterial;
                }
                break;
            case "Ravenclaw":
                foreach (GameObject skin in skinsToChangeColor)
                {
                    skin.GetComponent<Renderer>().material = ravenclawMaterial;
                }
                break;
        }
        
    }

}
