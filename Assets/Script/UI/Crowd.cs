using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    private Material colorGame;
    public GameObject[] cards;

    public void SetMaterial(Material color)
    {
        colorGame = color;
        foreach(GameObject card in cards)
        {
            card.GetComponent<Renderer>().material = colorGame;
        }
    }
}
