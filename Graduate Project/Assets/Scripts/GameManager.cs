using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public BoardManager boardScript;
    public Player playerScript;
    private int level = 1;
    
    // Use this for initialization
    void Awake()
    {
        instance = this;
        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();
        playerScript = GetComponent<Player>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene(level);
        playerScript.playerGenerate();
    }
}
