using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Main : MonoBehaviour
{
    public static Main instance;

    // [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text display;
    [SerializeField] private TextAsset wordleBank;
    [SerializeField] private TextAsset guessBank;
    [SerializeField] private Transform guessParent;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TMP_Text answerLose;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TMP_Text answerVictory;

    private List<List<TMP_Text>> rows = new();

    private string answer;
    private int attempt;
    private bool victory;
    public string userInput;

    private List<string> wordleBankList = new();
    private List<string> guessBankList = new();
    public List<string> guessHistory = new();


    private void Start()
    {
        instance = this;
        gameScreen.SetActive(true);
        loseScreen.SetActive(false);
        victoryScreen.SetActive(false);

        for (int i = 0; i < 6; i++)
        {
            rows.Add(new List<TMP_Text>());
            Transform row = guessParent.GetChild(i);
            for (int j = 0; j < 5; j++)
            {
                rows[i].Add(row.GetChild(j).GetChild(0).GetComponent<TMP_Text>());
            }
        }

        foreach (string s in wordleBank.text.Split("\n"))
        {
            wordleBankList.Add(s);
        }

        foreach (string s in guessBank.text.Split("\n"))
        {
            guessBankList.Add(s);
        }

        attempt = 0;
        victory = false;
        userInput = "";
        answer = wordleBankList[Random.Range(0, wordleBankList.Count)].ToLower();
    }

    private void Update()
    {
        if (victory)
        {
            gameScreen.SetActive(false);
            victoryScreen.SetActive(true);

            answerVictory.text = "The word was:\n" + answer.FirstCharacterToUpper();
        }

        if (attempt == 6 && !victory)
        {
            gameScreen.SetActive(false);
            loseScreen.SetActive(true);

            answerLose.text = "The word was:\n" + answer.FirstCharacterToUpper();
        }
    }

    public void GetUserInput(string s)
    {
        if (userInput.Length < 5)
        {
            userInput = s;
            UpdateWord(false);
        }

        Debug.Log(userInput);
    }

    public void Backspace(string s)
    {
        userInput = s;
        UpdateWord(true);
        Debug.Log(userInput);
    }

    public void CheckAnswer()
    {
        //set display to empty
        display.text = "";
        string output = "";

        userInput = userInput.ToLower();

        //check if the word is too long
        if (userInput.Length > answer.Length)
        {
            display.text = "Input too long!";
        }
        //check if the word is too short
        else if (userInput.Length < answer.Length)
        {
            display.text = "Input too short!";
        }
        //check if the word is in the guess bank
        else if (!guessBankList.Contains(userInput.ToLower()))
        {
            display.text = "Not a valid word!";
        }
        //run code if it passes checks
        else
        {
            //put user input into the guess history
            guessHistory.Add(userInput);

            //add all the matching characters into the greenDict
            Dictionary<char, int> greenDict = new();
            for (int i = 0; i < userInput.Length; i++)
            {
                if (userInput[i] == answer[i])
                {
                    if (!greenDict.ContainsKey(userInput[i]))
                    {
                        greenDict.Add(userInput[i], 0);
                    }

                    greenDict[userInput[i]]++;
                }
            }

            //add all the available characters into the answerDict
            Dictionary<char, int> answerDict = new();
            for (int i = 0; i < answer.Length; i++)
            {
                if (!answerDict.ContainsKey(answer[i]))
                {
                    answerDict.Add(answer[i], 0);
                }

                answerDict[answer[i]]++;
            }

            //set G, R, or Y
            Dictionary<char, int> usedYellows = new();
            for (int i = 0; i < userInput.Length; i++)
            {
                char inputChar = userInput[i];

                //set G if matching
                if (userInput[i] == answer[i])
                {
                    output += "G";
                }
                //set R if the input character isn't in the answerDict
                else if (!answerDict.ContainsKey(inputChar))
                {
                    output += "R";
                }
                //set Y if input character isn't G but is in the answerDict
                else
                {
                    usedYellows.TryAdd(inputChar, 0);
                    answerDict.TryAdd(inputChar, 0);
                    greenDict.TryAdd(inputChar, 0);

                    //set Y if the usedYellows + the matches is less than the value in the answerDict
                    if (usedYellows[inputChar] + greenDict[inputChar] < answerDict[inputChar])
                    {
                        output += "Y";

                        //+1 to the usedYellows
                        usedYellows[inputChar]++;
                    }
                    else
                    {
                        output += "R";
                    }
                }
            }

            if (output == "GGGGG") victory = true;
            attempt++;
            GuessAttempt(output);
            userInput = "";
        }
    }

    private void GuessAttempt(string output)
    {
        string guessWord = guessHistory[attempt - 1];

        for (int i = 0; i < 5; i++)
        {
            rows[attempt - 1][i].color = Color.white;
            rows[attempt - 1][i].text = guessWord[i].ToString().ToUpper();
            if (output[i] == 'G')
            {
                rows[attempt - 1][i].transform.parent.GetComponent<Image>().color = Color.green;
            }
            else if (output[i] == 'R')
            {
                rows[attempt - 1][i].transform.parent.GetComponent<Image>().color = Color.red;
            }
            else if (output[i] == 'Y')
            {
                rows[attempt - 1][i].transform.parent.GetComponent<Image>().color = Color.yellow;
            }
        }
    }

    private void UpdateWord(bool delete)
    {
        if (!delete)
        {
            for (int i = 0; i < userInput.Length; i++)
            {
                rows[attempt][i].text = userInput[i].ToString();
            }
        }
        
        if (delete)
        {
            for (int i = 5; i > 0; i--)
            {
                if (i > userInput.Length)
                    rows[attempt][i - 1].text = "";
            }
        }
    }
}