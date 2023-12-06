using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class KeyboardButton : MonoBehaviour
{
    public static KeyboardButton instance;
    
    [SerializeField] private bool isBackspace;
    [SerializeField] private List<TMP_Text> characters;
    private string input;
    
    private void Start()
    {
        instance = this;
        if (!isBackspace)
            GetComponent<Button>().onClick.AddListener(Clicked);
        else
            GetComponent<Button>().onClick.AddListener(DeleteChar);

    }

    private void Update()
    {
        input = Main.instance.userInput;
    }

    private void DeleteChar()
    {
        if (input.Length > 0)
           Main.instance.Backspace();
    }

    private void Clicked()
    {
        Main.instance.GetUserInput(input += GetComponent<TMP_Text>().text);
    }

    public void ChangeColors(Dictionary<char, int> greenDict, Dictionary<char, int> yellowDict, Dictionary<char, int> redDict)
    {
        foreach (KeyValuePair<char, int> reds in redDict)
        {
            string charToFind = reds.Key.ToString().ToUpper();
            foreach (TMP_Text character in characters)
            {
                if (character.text == charToFind)
                {
                    character.GetComponentInParent<Image>().color = Color.grey;
                }
            }
        }
        foreach (KeyValuePair<char, int> yellows in yellowDict)
        {
            string charToFind = yellows.Key.ToString().ToUpper();
            foreach (TMP_Text character in characters)
            {
                if (character.text == charToFind)
                {
                    character.GetComponentInParent<Image>().color = Color.yellow;
                }
            }
        }
        foreach (KeyValuePair<char, int> greens in greenDict)
        {
            string charToFind = greens.Key.ToString().ToUpper();
            foreach (TMP_Text character in characters)
            {
                if (character.text == charToFind)
                {
                    character.GetComponentInParent<Image>().color = Color.green;
                }
            }
        }
    }
}
