using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    int level = 0;
    string password = "";
    string[] level1password = { "books", "shelf", "password", "font", "borrow" };
    string[] level2password = { "prisoner", "handcuffs", "holster", "uniform", "arrest", "lieutenant" };
    string[] level3password = { "spacestation", "apolo15", "astronaut", "science", "rocket", "information", "environment", "moonjun201969" };

    enum Screen { MainMenu, Password, Win };
    Screen currentScreen = Screen.MainMenu;

    void Start()
    {
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        currentScreen = Screen.MainMenu;

        Terminal.ClearScreen();
        Terminal.WriteLine("Welcome to the hacking station. Who is your target this time?\n");
        Terminal.WriteLine("1. Library\n2. Police Station\n3. NASA\n");
        Terminal.WriteLine("Enter the number of your target: ");
    }

    void OnUserInput(string input)
    {
        if (input == "menu")
        {
            ShowMainMenu();
        }
        else if (input == "quit" || input == "close" || input == "exit")
        {
            Application.Quit();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currentScreen == Screen.Password)
        {
            CheckPassword(input);
        }
    }

    void RunMainMenu(string input)
    {
        bool isValidLevelNumber = (input == "1" || input == "2" || input == "3");
        if (isValidLevelNumber)
        {
            level = int.Parse(input);
            AskPassword();
        }
        else
        {
            Terminal.WriteLine("invalid command");
        }
    }

    void CheckPassword(string input)
    {
        if (input == password)
        {
            WinScreen();
        }
        else
        {
            AskPassword();
        }
    }

    void AskPassword()
    {
        currentScreen = Screen.Password;

        GeneratePassword();

        Terminal.ClearScreen();
        Terminal.WriteLine("Enter the password: 'hint: " + password.Anagram());
    }

    private void GeneratePassword()
    {
        switch (level)
        {
            case 1:
                password = level1password[Random.Range(0, level1password.Length)];
                break;
            case 2:
                password = level2password[Random.Range(0, level2password.Length)];
                break;
            case 3:
                password = level3password[Random.Range(0, level3password.Length)];
                break;
            default:
                Debug.LogError("Invalid level was chosen");
                break;
        }
    }

    void WinScreen()
    {
        currentScreen = Screen.Win;
        Terminal.ClearScreen();
        Terminal.WriteLine("logged in");
        ShowLevelReward();
    }

    void ShowLevelReward()
    {
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Have a book...");
                Terminal.WriteLine(@"
    _______
   /      //
  /      //
 /______//
(______(/
"               );
                break;
            case 2:
                Terminal.WriteLine("Have a badge...");
                Terminal.WriteLine(@"
   ,   /\   ,
  / '-'  '-' \
  |  POLICE  |
  |   .--.   |
  |  ( 19 )  |
  \   '--'   /
   '--.  .--'
       \/
"               );
                break;
            case 3:
                Terminal.WriteLine("Launching the rocket!");
                Terminal.WriteLine(@"
    /\
   /  \
  |    |
  |NASA|
  |    |
 '      `
 |      |
 |______|
  '-`'-`  
  / . \'\ 
");
                break;
            default:
                Debug.LogError("Invalid level");
                break;
        }
    }
}
