using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public GameObject optionPrefab;    // Prefab for the buttons the player clicks
    public Transform optionContainer;  // Where buttons will be instantiated in the UI

    public void Open(List<CursedPower> options)
    {
        gameObject.SetActive(true);

        // Clear old buttons
        foreach (Transform child in optionContainer)
            Destroy(child.gameObject);

        // Create a button for each option
        foreach (var power in options)
        {
            GameObject buttonObj = Instantiate(optionPrefab, optionContainer);
            buttonObj.GetComponent<PowerButton>().Setup(power);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
