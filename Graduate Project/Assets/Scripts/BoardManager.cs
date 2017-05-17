using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //public int columns = 10;
    //public int rows = 10;
    //"C:\\Users\\Reagan\\Documents\\Graduate Project\\Assets\\Scripts\\Map.txt"
    static string path = "C:\\Users\\Reagan\\Documents\\Graduate Project\\Assets\\Scripts\\Map.txt";
    //static string path = Application.dataPath;
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    public static TextAsset mapFile;// = new TextAsset();
    //string text = mapFile.text;
    public static string[][] boardPos;// = readFile( );

    public Transform boardHolder;
    public int boardSize;// = boardPos.Length;
    public Tile[,] board;// = new Tile[boardPos[0].Length, boardPos.Length];

    //private List<Vector3> gridPositions = new List<Vector3>();
    /*void Awake()
    {
        path = Resources.Load("Map") a
    }*/

    void Awake()
    {
        //On Awake load the Map.txt file from the Resources folder
         mapFile = (TextAsset)Resources.Load("Map", typeof(TextAsset));

        //read the text of Map.txt and store it in a 2D string array 
         boardPos = readFile();
        //get the size of the board
         boardSize = boardPos.Length;
        //Create a new board to hold the tiles
         board = new Tile[boardPos[0].Length, boardPos.Length];
    }

    private static string[][] readFile( )
    {
        //get the text of the Map.txt file as a string
        string text = mapFile.text;
        //split the string by each new line to determine the amount of rows of text
        string[] lines = Regex.Split(text, "\r\n");
        int rows = lines.Length;

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            //split the lines on spaces to get each individual value
            string[] stringsOfLine = Regex.Split(lines[i], " ");
            //populate the 2D array with the values
            levelBase[i] = stringsOfLine;
        }
        return levelBase;
    }

    /*void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }*/

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for ( int y = 0; y < boardPos.Length; y++ )
        {
            for( int x = 0; x < boardPos[0].Length; x++)
            {
                int invy = boardSize - 1 - y;
                switch(boardPos[y][x] )
                {
                    case "0":
                        Tile instance = Instantiate(wallTiles[0], new Vector2(x,invy), Quaternion.identity);
                        instance.transform.SetParent(boardHolder);
                        instance.gridPos = new Vector2(x, invy);
                        instance.isWall = true;
                        board[x, invy] = instance;
                        break;
                    case "1":
                        Tile instance2 = Instantiate(groundTiles[0], new Vector2(x, invy), Quaternion.identity);
                        instance2.transform.SetParent(boardHolder);
                        instance2.gridPos = new Vector2(x, invy);
                        board[x, invy] = instance2;
                        break;
                    case "2":
                        Tile instance3 = Instantiate(groundTiles[1], new Vector2(x, invy), Quaternion.identity);
                        instance3.transform.SetParent(boardHolder);
                        instance3.gridPos = new Vector2(x, invy);
                        board[x, invy] = instance3;
                        break;
                    case "3":
                        Tile instance4 = Instantiate(groundTiles[2], new Vector2(x, invy), Quaternion.identity);
                        instance4.transform.SetParent(boardHolder);
                        instance4.gridPos = new Vector2(x, invy);
                        board[x, invy] = instance4;
                        break;

                }
            }
        }
        /*boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = groundTiles[Random.Range(0, groundTiles.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }*/
    }


    public void SetupScene()
    {
        //Creates the walls and ground.
        BoardSetup();
    }
}
