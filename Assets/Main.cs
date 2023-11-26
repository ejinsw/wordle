using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Main : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text display;
    [SerializeField] private TextAsset wordleBank;
    [SerializeField] private TextAsset guessBank;
    [SerializeField] private Transform guessParent;

    private List<List<TMP_Text>> rows = new();
    
    private string answer = "";
    private int attempt = 0;
    
    private List<string> wordleBankList = new();
    private List<string> guessBankList = new();
    public List<string> guessHistory = new();


    private void Start()
    {
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
        answer = wordleBankList[Random.Range(0, wordleBankList.Count)].ToUpper();
    }

    public void CheckAnswer()
    {
        //set display to empty
        display.text = "";
        string output = "";

        userInput.text = userInput.text.ToUpper();

        //check if the word is too long
        if (userInput.text.Length > answer.Length)
        {
            display.text = "Input too long!";
        }
        //check if the word is too short
        else if (userInput.text.Length < answer.Length)
        {
            display.text = "Input too short!";
        }
        //check if the word is in the guess bank
        else if (!guessBankList.Contains(userInput.text.ToLower()))
        {
            display.text = "Not a valid word!";
        }
        //run code if it passes checks
        else
        {
            //put user input into the guess history
            guessHistory.Add(userInput.text);

            //add all the matching characters into the greenDict
            Dictionary<char, int> greenDict = new();
            for (int i = 0; i < userInput.text.Length; i++)
            {
                if (userInput.text[i] == answer[i])
                {
                    if (!greenDict.ContainsKey(userInput.text[i]))
                    {
                        greenDict.Add(userInput.text[i], 0);
                    }

                    greenDict[userInput.text[i]]++;
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
            for (int i = 0; i < userInput.text.Length; i++)
            {
                char inputChar = userInput.text[i];
             
                //set G if matching
                if (userInput.text[i] == answer[i])
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
        }

        attempt++;
        GuessAttempt(output);
    }

    private void GuessAttempt(string output)
    {
        string guessWord = guessHistory[attempt - 1];
        string matches = display.text;
        

        for (int i = 0; i < 5; i++)
        {
            rows[attempt - 1][i].color = Color.white;
            rows[attempt - 1][i].text = guessWord[i].ToString();
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
}