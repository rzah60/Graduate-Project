using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public BoardManager boardScript;
    public Player playerScript;
    public Thorgren thorgrenScript;
    public GUIElements guiScript;
    public Wraith wraithScript;
    public Skeleton skeletonScript;
    public string level;
    public List<Player> players;
    public List<Skeleton> skeletons;
    public string monsterType;
    
    // Use this for initialization
    void Awake()
    {
        instance = this;

        //store name of the scene for later identification
        level = SceneManager.GetActiveScene().name;

        //Get a component reference to each of the scripts attached to the GameManager
        boardScript = GetComponent<BoardManager>();
        playerScript = GetComponent<Player>();
        wraithScript = GetComponent<Wraith>();
        skeletonScript = GetComponent<Skeleton>();
        thorgrenScript = GetComponent<Thorgren>();
        guiScript = GetComponent<GUIElements>();
        
    }

    void Start()
    {
        //Call the InitGame function to initialize the level
        InitGame();
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }


    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script to set up the map
        boardScript.SetupScene();
        
        //Generate platyers
        playerScript.playerGenerate();
        
        //Generate the Thorgren for the first level
        if (level.Equals("Thorgren Battle"))
        {
            thorgrenScript.thorgrenGenerate("Thorgren");
            monsterType = "Thorgren";
        }

        //Generate the Afflicted Thorgren for the second level
        else if (level.Equals("Afflicted Thorgren Battle"))
        {
            thorgrenScript.thorgrenGenerate("Afflicted Thorgren");
            monsterType = "Afflicted Thorgren";
        }

        //Generate the Wraith and skeletons for the third level 
        else if (level.Equals("Wraith Battle"))
        {
            wraithScript.wraithGenerate();
            monsterType = "Wraith";
            skeletonScript.skeletonGenerate();
            skeletons = instance.skeletonScript.skeletons;
        }

        players = instance.playerScript.players;
    }
}
