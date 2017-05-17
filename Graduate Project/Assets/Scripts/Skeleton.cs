using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    public Text combatText;
    public Text bronHPText;
    public Text cristophHPText;
    public Text saraHPText;
    public Text veraHPText;

   // private GameObject loseScreen;

    public string skelName = "name";
    public int healthPoints = 8;
    public string facing = "down";
    public bool dead = true;

    public Vector2 gridPos = Vector2.zero;
    public bool moving = false;
    public bool attacking = false;
    public bool preparingLoseScreen = false;
    public bool skeletonActing = false;

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    public Animator skeletonAnimator;
    private float inverseMoveTime;

    public Skeleton[] skeletonChars;
    public List<Skeleton> skeletons = new List<Skeleton>();

    void Awake()
    {
        combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
        bronHPText = GameObject.FindGameObjectWithTag("Bron HP Text").GetComponent<Text>();
        cristophHPText = GameObject.FindGameObjectWithTag("Cristoph HP Text").GetComponent<Text>();
        saraHPText = GameObject.FindGameObjectWithTag("Sara HP Text").GetComponent<Text>();
        veraHPText = GameObject.FindGameObjectWithTag("Vera HP Text").GetComponent<Text>();
    }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;

        skeletonAnimator = GetComponent<Animator>();
    }

    public void skeletonGenerate()
    {
        Skeleton skeleton;

        skeleton = Instantiate(skeletonChars[0], new Vector2(2, 7), Quaternion.identity);
        skeleton.gridPos = new Vector2(2, 7);
        skeleton.facing = "down";
        skeleton.skelName = "Skeleton 1";
        skeleton.dead = true;
        skeleton.healthPoints = 0;
        skeleton.moveTime = 0.2f;
        skeletons.Add(skeleton);
        GameManager.instance.boardScript.board[(int)skeleton.gridPos.x, (int)skeleton.gridPos.y].occupied = true;

        skeleton = Instantiate(skeletonChars[1], new Vector2(2, 3), Quaternion.identity);
        skeleton.gridPos = new Vector2(2, 3);
        skeleton.facing = "down";
        skeleton.skelName = "Skeleton 2";
        skeleton.dead = true;
        skeleton.healthPoints = 0;
        skeleton.moveTime = 0.2f;
        skeletons.Add(skeleton);
        GameManager.instance.boardScript.board[(int)skeleton.gridPos.x, (int)skeleton.gridPos.y].occupied = true;

        skeleton = Instantiate(skeletonChars[2], new Vector2(9, 7), Quaternion.identity);
        skeleton.gridPos = new Vector2(9, 7);
        skeleton.facing = "down";
        skeleton.skelName = "Skeleton 3";
        skeleton.dead = true;
        skeleton.healthPoints = 0;
        skeleton.moveTime = 0.2f;
        skeletons.Add(skeleton);
        GameManager.instance.boardScript.board[(int)skeleton.gridPos.x, (int)skeleton.gridPos.y].occupied = true;

        skeleton = Instantiate(skeletonChars[3], new Vector2(9, 3), Quaternion.identity);
        skeleton.gridPos = new Vector2(9, 3);
        skeleton.facing = "down";
        skeleton.skelName = "Skeleton 4";
        skeleton.dead = true;
        skeleton.healthPoints = 0;
        skeleton.moveTime = 0.2f;
        skeletons.Add(skeleton);
        GameManager.instance.boardScript.board[(int)skeleton.gridPos.x, (int)skeleton.gridPos.y].occupied = true;

    }

    public IEnumerator moveAndAttack(Tile destTile, Player target)
    {
        List<Skeleton> skels = GameManager.instance.skeletonScript.skeletons;
        yield return null;
        //skeletonActing = true;
        /*if (skelName.Equals("Skeleton 1"))
        {
            yield return new WaitWhile(() => GameManager.instance.wraithScript.wraith.wraithActing);
        }*/
        if (skelName.Equals("Skeleton 2"))
        {
            yield return new WaitWhile(() => skels[0].skeletonActing);
        }
        else if (skelName.Equals("Skeleton 3"))
        {
            yield return new WaitWhile(() => skels[0].skeletonActing || skels[1].skeletonActing);
        }
        else if (skelName.Equals("Skeleton 4"))
        {
            yield return new WaitWhile(() => skels[0].skeletonActing || skels[1].skeletonActing || skels[2].skeletonActing);
        }

        List<Tile> list = PathGenerate.FindPath(GameManager.instance.boardScript.board[(int)gridPos.x, (int)gridPos.y], destTile);

        list.Remove(destTile);

        if (list.Count != 0)
        {
            GameManager.instance.boardScript.board[(int)gridPos.x, (int)gridPos.y].occupied = false;

            Tile finalDest = list[list.Count - 1];
            moving = true;
            StartCoroutine(SmoothMovement(list[0], list));

            StartCoroutine(updatePos(finalDest, target));
        }
        else
        {
            pivot(target);
        }
        StartCoroutine(activateAttack(target));
    }

    protected IEnumerator SmoothMovement(Tile targTile, List<Tile> list)
    {
        Vector3 end = targTile.gridPos;

        updateFacing(transform.position, end);
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

        yield return new WaitUntil(() => transform.position == end);

        list.Remove(targTile);
        if (list.Count > 0)
        {
            StartCoroutine(SmoothMovement(list[0], list));
        }

    }

    protected IEnumerator updatePos(Tile finalDest, Player target)
    {
        yield return new WaitUntil(() => (Vector2)transform.position == finalDest.gridPos);
        gridPos = finalDest.gridPos;
        finalDest.occupied = true;
        pivot(target);
        moving = false;
    }

    public IEnumerator activateAttack(Player target)
    {
       yield return new WaitWhile(() => moving);

        attacking = true;

        target.healthPoints -= 5;

        GUIElements.activateConfirmButton();

        //Text combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
        combatText.text = "The Skeleton attacks " + target.name + " for 5 damage.";

        yield return new WaitWhile(() => attacking);
        checkPlayerHPValues();
        updatePlayerHP(target);

        combatText.text = "";

        StartCoroutine(checkIfPlayerLost());
    }

    public void updatePlayerHP(Player targetPlayer)
    {
        if (targetPlayer.name == "Bron")
        {
            bronHPText.text = "Bron \n\nHP: " + targetPlayer.healthPoints;
        }
        else if (targetPlayer.name == "Cristoph")
        {
            cristophHPText.text = "Cristoph \n\nHP: " + targetPlayer.healthPoints;
        }
        else if (targetPlayer.name == "Sara")
        {
            saraHPText.text = "Sara \n\nHP: " + targetPlayer.healthPoints;
        }
        else if (targetPlayer.name == "Vera")
        {
            veraHPText.text = "Vera \n\nHP: " + targetPlayer.healthPoints;
        }
    }
    private void checkPlayerHPValues()
    {
        List<Player> players = GameManager.instance.players;

        if (players[0].healthPoints <= 0 && players[0].dead == false)
        {
            players[0].healthPoints = 0;
            players[0].dead = true;
            players[0].playerAnimator.SetTrigger("die");
        }
        if (players[1].healthPoints <= 0 && players[1].dead == false)
        {
            players[1].healthPoints = 0;
            players[1].dead = true;
            players[1].playerAnimator.SetTrigger("die");
        }
        if (players[2].healthPoints <= 0 && players[2].dead == false)
        {
            players[2].healthPoints = 0;
            players[2].dead = true;
            players[2].playerAnimator.SetTrigger("die");
        }
        if (players[3].healthPoints <= 0 && players[3].dead == false)
        {
            players[3].healthPoints = 0;
            players[3].dead = true;
            players[3].playerAnimator.SetTrigger("die");
        }
    }

    public IEnumerator checkIfPlayerLost()
    {
        List<Player> players = GameManager.instance.players;
        List<Skeleton> skels = GameManager.instance.skeletonScript.skeletons;
        if (players[0].dead == true && players[1].dead == true && players[2].dead == true && players[3].dead == true)
        {
            preparingLoseScreen = true;

            combatText.text = "All characters have been defeated. You lose.";

            GUIElements.activateConfirmButton();

            yield return new WaitWhile(() => preparingLoseScreen);

            GameManager.instance.guiScript.activateLoseScreen();// loseScreen.SetActive(true);

        }

        yield return null;

        if (skelName.Equals("Skeleton 1"))
        {
            skels[0].skeletonActing = false;
        }
        else if (skelName.Equals("Skeleton 2"))
        {
            skels[1].skeletonActing = false;
        }
        else if (skelName.Equals("Skeleton 3"))
        {
            skels[2].skeletonActing = false;
        }
        else if (skelName.Equals("Skeleton 4"))
        {
            skels[3].skeletonActing = false;
        }

        if (skels[0].skeletonActing == false && skels[1].skeletonActing == false && skels[2].skeletonActing == false && skels[3].skeletonActing == false)
        {
            GUIElements.activateEndTurnButton();
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

    public void updateFacing(Vector2 startingPos, Vector2 destinationPos)
    {
        if (destinationPos.x < startingPos.x)
        {
            facing = "left";
            skeletonAnimator.SetTrigger("walkLeft");
        }
        else if (destinationPos.x > startingPos.x)
        {
            facing = "right";
            skeletonAnimator.SetTrigger("walkRight");
        }
        else if (destinationPos.y < startingPos.y)
        {
            facing = "down";
            skeletonAnimator.SetTrigger("walkDown");
        }
        else if (destinationPos.y > startingPos.y)
        {
            facing = "up";
            skeletonAnimator.SetTrigger("walkUp");
        }
    }

    public void pivot(Player target)
    {
        if (target.gridPos.x < gridPos.x)
        {
            facing = "left";
            skeletonAnimator.SetTrigger("attackLeft");
        }
        else if (target.gridPos.x > gridPos.x)
        {
            facing = "right";
            skeletonAnimator.SetTrigger("attackRight");
        }
        else if (target.gridPos.y < gridPos.y)
        {
            facing = "down";
            skeletonAnimator.SetTrigger("attackDown");
        }
        else if (target.gridPos.y > gridPos.y)
        {
            facing = "up";
            skeletonAnimator.SetTrigger("attackUp");
        }
    }
}
