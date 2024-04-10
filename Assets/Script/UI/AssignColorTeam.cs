using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignColorTeam : MonoBehaviour
{
    public Material Gryffindor;
    public Material Slytherin;
    public Material Hufflepuff;
    public Material Ravenclaw;
    void Start()
    {
        string team = PlayerPrefs.GetString("Team");
        Material color = Gryffindor;
        switch (team)
        {
            case "Gryffindor":
                color = gameObject.CompareTag("TeamPlayer") ? Slytherin : Gryffindor;
                break;
            case "Slytherin":
                color = gameObject.CompareTag("TeamPlayer") ? Gryffindor : Slytherin;
                break;
            case "Hufflepuff":
                color = gameObject.CompareTag("TeamPlayer") ? Hufflepuff : Ravenclaw;
                break;
            case "Ravenclaw":
                color = gameObject.CompareTag("TeamPlayer") ? Ravenclaw : Hufflepuff;
                break;
        }
        AssignColorToChilds(color);
    }

    private void AssignColorToChilds(Material color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Crowd crowdComponent = child.GetComponent<Crowd>();
            if (crowdComponent != null)
            {
                crowdComponent.SetMaterial(color);
            }
        }
    }
}
