using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public string name = "name";
    public int healthPoints = 100;
    public int movementTokens = 1;
    public int actionTokens = 1;
    public int movementPoints = 5;
    public string facing = "up";
    public string weapon;
    public bool selected = false;
    public bool dead = false;

    public Vector2 gridPos = Vector2.zero;
    public bool moving = false;
    public bool attacking = false;

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;
    public Animator playerAnimator;

    public Player[] playerChars;
    public List<Player> players = new List<Player>();
    public List<Tile> highlightedTiles;

    void Awake()
    {
    }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;

        playerAnimator = GetComponent<Animator>();
    }

    public void playerGenerate()
    {
        Player player;

        player = Instantiate(playerChars[0], new Vector2(4, 1), Quaternion.identity);
        player.gridPos = new Vector2(4, 1);
        player.facing = "up";
        player.name = "Bron";
        player.healthPoints = 100;
        player.moveTime = 0.2f;
        player.weapon = "dagger";
        player.actionTokens = 2;
        players.Add(player);
        GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].occupied = true;

        player = Instantiate(playerChars[1], new Vector2(5, 1), Quaternion.identity);
        player.gridPos = new Vector2(5, 1);
        player.facing = "up";
        player.name = "Cristoph";
        player.healthPoints = 100;
        player.moveTime = 0.2f;
        player.weapon = "mace";
        player.actionTokens = 1;
        players.Add(player);
        GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].occupied = true;

        player = Instantiate(playerChars[2], new Vector2(6, 1), Quaternion.identity);
        player.gridPos = new Vector2(6, 1);
        player.facing = "up";
        player.name = "Sara";
        player.healthPoints = 100;
        player.moveTime = 0.2f;
        player.weapon = "rapier";
        player.actionTokens = 1;
        players.Add(player);
        GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].occupied = true;

        player = Instantiate(playerChars[3], new Vector2(7, 1), Quaternion.identity);
        player.gridPos = new Vector2(7, 1);
        player.facing = "up";
        player.name = "Vera";
        player.healthPoints = 100;
        player.moveTime = 0.2f;
        player.weapon = "sword";
        player.actionTokens = 1;
        players.Add(player);
        GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].occupied = true;

    }

    public void move(Tile destTile)
    {
        GameManager.instance.boardScript.board[(int)gridPos.x, (int)gridPos.y].occupied = false;
        List<Tile> list = PathGenerate.FindPath(GameManager.instance.boardScript.board[(int)gridPos.x, (int)gridPos.y], destTile);

        StartCoroutine(SmoothMovement( list[0], list));
        //yield return new WaitForSeconds(2);
           
        gridPos = destTile.gridPos;
        destTile.occupied = true;
        movementTokens = movementTokens - 1;

        if( movementTokens < 1)
        {
            GUIElements.deactivateMoveButton();
        }
    }

    protected IEnumerator SmoothMovement(Tile targTile, List<Tile> list)
    {
        Vector3 end = targTile.gridPos;

        //Change the direction the character is facing based on the destination
        updateFacing(transform.position, end);
        //Calculates the remaining distance to the end and squares it as using the 
        //square magnitude is computaationally cheaper
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.fixedDeltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        //Wait to progress until the character has reached their destination
        yield return new WaitUntil(() => transform.position == end);
        if( boxCollider.isTrigger == true )
        {
            boxCollider.isTrigger = false;
        }
        else
        {
            boxCollider.isTrigger = true;
        }
        //Remove the tile that was just moved to from the list
        list.Remove(targTile);
        //If the list is not empty recursively start the SmoothMovement coroutine
        if (list.Count > 0)
        {
            StartCoroutine(SmoothMovement(list[0], list));
        }

    }

    public void updateFacing(Vector2 startingPos, Vector2 destinationPos)
    {
        if (destinationPos.x < startingPos.x)
        {
            facing = "left";
            playerAnimator.SetTrigger("walkLeft");
        }
        else if (destinationPos.x > startingPos.x)
        {
            facing = "right";
            playerAnimator.SetTrigger("walkRight");
        }
        else if (destinationPos.y < startingPos.y)
        {
            facing = "down";
            playerAnimator.SetTrigger("walkDown");
        }
        else if (destinationPos.y > startingPos.y)
        {
            facing = "up";
            playerAnimator.SetTrigger("walkUp");
        }
    }

    public void pivot(string target)
    {
        if (target.Equals("Wraith"))
        {
            Wraith wraith = GameManager.instance.wraithScript.wraith;
            if (gridPos.x < wraith.topLeftPos.x)
            {
                facing = "right";
                playerAnimator.SetTrigger("attackRight");
            }
            else if (gridPos.x > wraith.topRightPos.x)
            {
                facing = "left";
                playerAnimator.SetTrigger("attackLeft");
            }
            else if (gridPos.y < wraith.bottomLeftPos.y)
            {
                facing = "up";
                playerAnimator.SetTrigger("attackUp");
            }
            else if (gridPos.y > wraith.topRightPos.y)
            {
                facing = "down";
                playerAnimator.SetTrigger("attackDown");
            }
        }

        else if (target.Equals("Thorgren"))
        {
            Thorgren thor = GameManager.instance.thorgrenScript.thor;
            if (gridPos.x < thor.topLeftPos.x)
            {
                facing = "right";
                playerAnimator.SetTrigger("attackRight");
            }
            else if (gridPos.x > thor.topRightPos.x)
            {
                facing = "left";
                playerAnimator.SetTrigger("attackLeft");
            }
            else if (gridPos.y < thor.bottomLeftPos.y)
            {
                facing = "up";
                playerAnimator.SetTrigger("attackUp");
            }
            else if (gridPos.y > thor.topRightPos.y)
            {
                facing = "down";
                playerAnimator.SetTrigger("attackDown");
            }
        }

        else
        {
            List<Skeleton> skeletons = GameManager.instance.skeletons;
            Skeleton skeleton = new Skeleton();

            switch (target)
            {
                case "Skeleton 1":
                    skeleton = skeletons[0];
                    break;

                case "Skeleton 2":
                    skeleton = skeletons[1];
                    break;

                case "Skeleton 3":
                    skeleton = skeletons[2];
                    break;

                case "Skeleton 4":
                    skeleton = skeletons[3];
                    break;
            }

            if (gridPos.x < skeleton.gridPos.x)
            {
                facing = "right";
                playerAnimator.SetTrigger("attackRight");
            }
            else if (gridPos.x > skeleton.gridPos.x)
            {
                facing = "left";
                playerAnimator.SetTrigger("attackLeft");
            }
            else if (gridPos.y < skeleton.gridPos.y)
            {
                facing = "up";
                playerAnimator.SetTrigger("attackUp");
            }
            else if (gridPos.y > skeleton.gridPos.y)
            {
                facing = "down";
                playerAnimator.SetTrigger("attackDown");
            }

        }

    }


    void OnMouseDown()
    {
        if (GameManager.instance.level.Equals("Wraith Battle") )
        {
            List<Skeleton> skeletons = GameManager.instance.skeletons;
            if (dead == false && GameManager.instance.wraithScript.wraith.wraithActing == false && skeletons[0].skeletonActing == false && skeletons[1].skeletonActing == false && skeletons[2].skeletonActing == false && skeletons[3].skeletonActing == false)
            {
                foreach (Player p in GameManager.instance.playerScript.players)
                {
                    if (p.highlightedTiles.Count > 0)
                    {
                        foreach (Tile t in p.highlightedTiles)
                        {
                            t.highlighted = false;
                            t.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    if (p.attacking == true)
                    {
                        GUIElements.deactivateTargetButtons();
                    }

                    p.selected = false;
                }
                this.selected = true;
                GUIElements.deactivateAttackButton();
                GUIElements.deactivateMoveButton();
                if (this.movementTokens > 0)
                {
                    GUIElements.activateMoveButton();
                }

                if (this.actionTokens > 0)
                {
                    GUIElements.activateAttackButton();
                }
                //highlightTilesAt(this.gridPos, this.movementPoints);
            }
        }
    
        else
        {
            //if (dead == false && GameManager.instance.thorgrenScript.thor.moving == false && GameManager.instance.thorgrenScript.thor.attacking == false && GameManager.instance.thorgrenScript.thor.healing == false )
            if(dead == false && GameManager.instance.thorgrenScript.thor.thorgrenTurn == false)
            {
                foreach (Player p in GameManager.instance.playerScript.players)
                {
                    if (p.highlightedTiles.Count > 0)
                    {
                        foreach (Tile t in p.highlightedTiles)
                        {
                            t.highlighted = false;
                            t.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    if (p.attacking == true)
                    {
                        GUIElements.deactivateTargetButtons();
                    }

                    p.selected = false;
                }
                this.selected = true;
                GUIElements.deactivateAttackButton();
                GUIElements.deactivateMoveButton();

                if (this.movementTokens > 0)
                {
                    GUIElements.activateMoveButton();
                }

                if (this.actionTokens > 0)
                {
                    GUIElements.activateAttackButton();
                }

                //highlightTilesAt(this.gridPos, this.movementPoints);
            }
        }
    }

    public void highlightTilesAt(Vector2 originLocation, int distance)
    {
        highlightedTiles = PathHighlight.FindHighlight(GameManager.instance.boardScript.board[(int)originLocation.x, (int)originLocation.y], distance);

        foreach (Tile t in highlightedTiles)
        {
            t.highlighted = true;
            t.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public IEnumerator shiftPosition(Tile targTile)
    {
        Vector3 end = targTile.gridPos;

        updateFacing(transform.position, end);
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        yield return new WaitUntil(() => transform.position == end);
        gridPos = transform.position;
        targTile.occupied = true;
    }
}
