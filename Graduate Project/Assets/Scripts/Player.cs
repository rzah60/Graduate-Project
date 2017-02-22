using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Vector2 gridPos = Vector2.zero;
    public Vector3 moveDestination;
    public bool moving = false;

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;

    public Player[] playerChars;
    public List<Player> players = new List<Player>();

    void Awake()
    {
        moveDestination = transform.position;
    }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    public void move(Tile destTile)
    {
        StartCoroutine(SmoothMovement(destTile.gridPos));
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
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
    }

    public void playerGenerate()
    {
        Player player;

        player = Instantiate(playerChars[0], new Vector2(1, 1), Quaternion.identity);
        player.gridPos = new Vector2(1, 1);
        players.Add(player);
    }
}
