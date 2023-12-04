using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class KeyboardButton : MonoBehaviour
{
    [SerializeField] private bool isBackspace;
    private string input;
    
    private void Start()
    {
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
           Main.instance.Backspace(input.Substring(0, input.Length - 1).ToUpper());
    }

    private void Clicked()
    {
        Main.instance.GetUserInput(input += GetComponent<TMP_Text>().text);
    }
}
