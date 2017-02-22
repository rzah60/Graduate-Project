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
    public static string[][] boardPos = readFile("C:\\Users\\Reagan\\Documents\\Graduate Project\\Assets\\Scripts\\Map.txt");
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    public Transform boardHolder;
    public int boardSize = boardPos.Length;
    public Tile[,] board = new Tile[boardPos[0].Length, boardPos.Length];

    //private List<Vector3> gridPositions = new List<Vector3>();

    private static string[][] readFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        string[] lines = Regex.Split(text, "\r\n");
        int rows = lines.Length;

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], " ");
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


    public void SetupScene(int level)
    {
        //Creates the walls and ground.
        BoardSetup();
    }
}
