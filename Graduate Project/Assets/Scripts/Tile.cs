using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public Vector2 gridPos = Vector2.zero;

    public List<Tile> neighbors = new List<Tile>();

    public bool isWall = false;

    // Use this for initialization
    void Start() {
        generateNeighbors();
    }

    public void generateNeighbors()
    {
       neighbors = new List<Tile>();
        //get neighbor above
        if( isWall == false && gridPos.y < GameManager.instance.boardScript.boardSize - 2 )
        {
            Vector2 neighborPos = new Vector2(gridPos.x, gridPos.y + 1);
            neighbors.Add(GameManager.instance.boardScript.board[ (int)Mathf.Round(neighborPos.x), (int)Mathf.Round(neighborPos.y) ]);
        }
        //get neighbor below
        if ( isWall == false && gridPos.y > 1)
        {
            Vector2 neighborPos = new Vector2(gridPos.x, gridPos.y -1);
            neighbors.Add(GameManager.instance.boardScript.board[(int)Mathf.Round(neighborPos.x), (int)Mathf.Round(neighborPos.y)]);
        }
        //get neighbor right
        if ( isWall == false && gridPos.x < GameManager.instance.boardScript.boardSize - 2)
        {
            Vector2 neighborPos = new Vector2(gridPos.x + 1, gridPos.y);
           neighbors.Add(GameManager.instance.boardScript.board[(int)Mathf.Round(neighborPos.x), (int)Mathf.Round(neighborPos.y)]);
        }
        //get neighbor left
        if ( isWall == false && gridPos.x > 1 )
        { 
            Vector2 neighborPos = new Vector2(gridPos.x - 1, gridPos.y);
            neighbors.Add(GameManager.instance.boardScript.board[(int)Mathf.Round(neighborPos.x), (int)Mathf.Round(neighborPos.y)]);
        }
    }

    void OnMouseDown()
    {
        GameManager.instance.playerScript.players[0].move(this);
    }
        // Update is called once per frame
    void Update () {
		
	}
}
