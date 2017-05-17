using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wraith : MonoBehaviour {

    public Vector2 topLeftPos = Vector2.zero;
    public Vector2 topRightPos = Vector2.zero;
    public Vector2 bottomLeftPos = Vector2.zero;
    public Vector2 bottomRightPos = Vector2.zero;

    public Text combatText;
    public Text bronHPText;
    public Text cristophHPText;
    public Text saraHPText;
    public Text veraHPText;

    private Text blueOrbText;
    private Text redOrbText;
    private Text yellowOrbText;
    private Text greenOrbText;

    private Text skeleton1Text;
    private Text skeleton2Text;
    private Text skeleton3Text;
    private Text skeleton4Text;

   // private GameObject loseScreen;

    public struct FrontRight
    {
        public Vector2 position;
        public List<string> targets;

        public FrontRight(Vector2 pos)
        {
            position = pos;
            targets = new List<string> { "greenOrb" };

        }
    }
    public struct FrontLeft
    {
        public Vector2 position;
        public List<string> targets;

        public FrontLeft(Vector2 pos)
        {
            position = pos;
            targets = new List<string> { "yellowOrb" };

        }
    }
    public struct BackRight
    {
        public Vector2 position;
        public List<string> targets;

        public BackRight(Vector2 pos)
        {
            position = pos;
            targets = new List<string> { "blueOrb" };
        }
    }
    public struct BackLeft
    {
        public Vector2 position;
        public List<string> targets;

        public BackLeft(Vector2 pos)
        {
            position = pos;
            targets = new List<string> { "redOrb" };
        }
    }

    public FrontRight frontRight;
    public FrontLeft frontLeft;
    public BackRight backRight;
    public BackLeft backLeft;

    public string type;
    public string facing;
    public bool dead = false;

    public int blueOrbHP;
    public int redOrbHP;
    public int greenOrbHP;
    public int yellowOrbHP;

    public Wraith[] enemyPrefabs;
    //public List<Thorgren> enemies = new List<Thorgren>();
    public Wraith wraith;

    public float moveTime = 0.1f;
    public bool moving = false;
    public bool attacking = false;
    public bool checkingLoseStatus = false;
    public bool preparingLoseScreen = false;
    public bool resurrectingSkeleton = false;
    public bool wraithActing = false;
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    public Animator wraithAnimator;
    private float inverseMoveTime;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;

        wraithAnimator = GetComponent<Animator>();
        //GameManager.instance.guiScript.activateLoseScreen();//loseScreen.SetActive(false);
    }

    void Awake()
    {
        if (GameManager.instance.level.Equals("Wraith Battle"))
        {
            combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
            bronHPText = GameObject.FindGameObjectWithTag("Bron HP Text").GetComponent<Text>();
            cristophHPText = GameObject.FindGameObjectWithTag("Cristoph HP Text").GetComponent<Text>();
            saraHPText = GameObject.FindGameObjectWithTag("Sara HP Text").GetComponent<Text>();
            veraHPText = GameObject.FindGameObjectWithTag("Vera HP Text").GetComponent<Text>();

            blueOrbText = GameObject.FindGameObjectWithTag("Blue Orb HP").GetComponent<Text>();
            redOrbText = GameObject.FindGameObjectWithTag("Red Orb HP").GetComponent<Text>();
            greenOrbText = GameObject.FindGameObjectWithTag("Green Orb HP").GetComponent<Text>();
            yellowOrbText = GameObject.FindGameObjectWithTag("Yellow Orb HP").GetComponent<Text>();

            skeleton1Text = GameObject.FindGameObjectWithTag("Skeleton 1 HP").GetComponent<Text>();
            skeleton2Text = GameObject.FindGameObjectWithTag("Skeleton 2 HP").GetComponent<Text>();
            skeleton3Text = GameObject.FindGameObjectWithTag("Skeleton 3 HP").GetComponent<Text>();
            skeleton4Text = GameObject.FindGameObjectWithTag("Skeleton 4 HP").GetComponent<Text>();
        }
        //loseScreen = GameObject.FindGameObjectWithTag("Lose Screen");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void wraithGenerate()
    {
        wraith = Instantiate(enemyPrefabs[0], new Vector2(5.5f, 5.5f), Quaternion.identity);
        wraith.topLeftPos = new Vector2(5, 6);
        wraith.topRightPos = new Vector2(6, 6);
        wraith.bottomLeftPos = new Vector2(5, 5);
        wraith.bottomRightPos = new Vector2(6, 5);
        wraith.facing = "down";
        wraith.moveTime = 0.2f;

        wraith.blueOrbHP = 50;
        wraith.redOrbHP = 50;
        wraith.greenOrbHP = 50;
        wraith.yellowOrbHP = 50;

        wraith.frontRight = new FrontRight(wraith.bottomLeftPos);
        wraith.frontLeft = new FrontLeft(wraith.bottomRightPos);
        wraith.backRight = new BackRight(wraith.topLeftPos);
        wraith.backLeft = new BackLeft(wraith.topRightPos);
        //enemies.Add(thor);

        GameManager.instance.boardScript.board[(int)wraith.topLeftPos.x, (int)wraith.topLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)wraith.topRightPos.x, (int)wraith.topRightPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)wraith.bottomLeftPos.x, (int)wraith.bottomLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)wraith.bottomRightPos.x, (int)wraith.bottomRightPos.y].occupied = true;

    }

    public void updateTargetAreas()
    {
        if (facing.Equals("down"))
        {
            frontRight.position = bottomLeftPos;
            frontLeft.position = bottomRightPos;
            backRight.position = topLeftPos;
            backLeft.position = topRightPos;
        }
        else if (facing.Equals("right"))
        {
            frontRight.position = bottomRightPos;
            frontLeft.position = topRightPos;
            backRight.position = bottomLeftPos;
            backLeft.position = topLeftPos;
        }
        else if (facing.Equals("up"))
        {
            frontRight.position = topRightPos;
            frontLeft.position = topLeftPos;
            backRight.position = bottomRightPos;
            backLeft.position = bottomLeftPos;
        }
        else if (facing.Equals("left"))
        {
            frontRight.position = topLeftPos;
            frontLeft.position = bottomLeftPos;
            backRight.position = topRightPos;
            backLeft.position = bottomRightPos;
        }
    }

    public void moveAndAttack(Tile destTile, Vector2 targTile, Player target)
    {
        wraithActing = true;
        List<Tile> list = PathGenerate.FindPath(GameManager.instance.boardScript.board[(int)targTile.x, (int)targTile.y], destTile);

        list.Remove(destTile);

        if (list.Count != 0)
        {
            GameManager.instance.boardScript.board[(int)this.topLeftPos.x, (int)this.topLeftPos.y].occupied = false;
            GameManager.instance.boardScript.board[(int)this.topRightPos.x, (int)this.topRightPos.y].occupied = false;
            GameManager.instance.boardScript.board[(int)this.bottomLeftPos.x, (int)this.bottomLeftPos.y].occupied = false;
            GameManager.instance.boardScript.board[(int)this.bottomRightPos.x, (int)this.bottomRightPos.y].occupied = false;

            //list updated with transform destinations
            List<Vector2> vectorList = updatePosValues(targTile, list);

            Vector2 finalDest = vectorList[vectorList.Count - 1];

            moving = true;


            //foreach (Vector2 v in vectorList)
            //{
            StartCoroutine(SmoothMovement(vectorList[0], vectorList));
            //yield return new WaitForSeconds(2);
            //}

            //float waitTime = (inverseMoveTime * Time.deltaTime * list.Count) + 1f;
            StartCoroutine(updatePos(finalDest, targTile, target));

        }
        else
        {
            pivot(target);
        }
        StartCoroutine(activateAttack(target));
    }

    protected IEnumerator SmoothMovement(Vector3 end, List<Vector2> list)
    {
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
        
        list.Remove(end);
        if (list.Count > 0)
        {
            StartCoroutine(SmoothMovement(list[0], list));
        }

    }

    protected IEnumerator updatePos(Vector2 finalDest, Vector2 targTile, Player target)
    {

        yield return new WaitUntil(() => (Vector2)transform.position == finalDest);
        
        topLeftPos.x = transform.position.x - .5f;
        topLeftPos.y = transform.position.y + .5f;
        topRightPos.x = transform.position.x + .5f;
        topRightPos.y = transform.position.y + .5f;
        bottomLeftPos.x = transform.position.x - .5f;
        bottomLeftPos.y = transform.position.y - .5f;
        bottomRightPos.x = transform.position.x + .5f;
        bottomRightPos.y = transform.position.y - .5f;
        checkCollisions();

        pivot(target);

        GameManager.instance.boardScript.board[(int)this.topLeftPos.x, (int)this.topLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.topRightPos.x, (int)this.topRightPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.bottomLeftPos.x, (int)this.bottomLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.bottomRightPos.x, (int)this.bottomRightPos.y].occupied = true;

        updateTargetAreas();

        moving = false;
    }

    public IEnumerator activateAttack(Player target)
    {
        yield return new WaitWhile(() => moving);

        attacking = true;
        WraithAttack attack = new WraithAttack(Random.Range(1, 1), target);

        target.healthPoints -= attack.damageValue;

        GUIElements.activateConfirmButton();

        //Text combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
        combatText.text = "The Wraith attacks " + target.name + " with " + attack.attackName + " for " + attack.damageValue + " damage.";

        yield return new WaitWhile(() => attacking);
        checkPlayerHPValues();
        updatePlayerHP(target);

        StartCoroutine(checkIfPlayerLost());

        StartCoroutine(resurrectSkeleton());
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
    public void updateSkeletonHP(Skeleton skeleton)
    {
        if (skeleton.skelName == "Skeleton 1")
        {
            skeleton1Text.text = "Skeleton 1:" + skeleton.healthPoints;
        }
        else if (skeleton.skelName == "Skeleton 2")
        {
            skeleton2Text.text = "Skeleton 2:" + skeleton.healthPoints;
        }
        else if (skeleton.skelName == "Skeleton 3")
        {
            skeleton3Text.text = "Skeleton 3:" + skeleton.healthPoints;
        }
        else if (skeleton.skelName == "Skeleton 4")
        {
            skeleton4Text.text = "Skeleton 4:" + skeleton.healthPoints;
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

    private void checkSkeletonHPValues()
    {
        List<Skeleton> skeletons = GameManager.instance.skeletons;

        if (skeletons[0].healthPoints <= 0)
        {
            skeletons[0].healthPoints = 0;
            skeletons[0].dead = true;
        }
        if (skeletons[1].healthPoints <= 0)
        {
            skeletons[1].healthPoints = 0;
            skeletons[1].dead = true;
        }
        if (skeletons[2].healthPoints <= 0)
        {
            skeletons[2].healthPoints = 0;
            skeletons[2].dead = true;
        }
        if (skeletons[3].healthPoints <= 0)
        {
            skeletons[3].healthPoints = 0;
            skeletons[3].dead = true;
        }
    }

    public IEnumerator checkIfPlayerLost()
    {
        checkingLoseStatus = true;

        List<Player> players = GameManager.instance.players;
        if (players[0].dead == true && players[1].dead == true && players[2].dead == true && players[3].dead == true)
        {
            preparingLoseScreen = true;

            combatText.text = "All characters have been defeated. You lose.";

            GUIElements.activateConfirmButton();

            yield return new WaitWhile(() => preparingLoseScreen);

            combatText.text = "";

            GameManager.instance.guiScript.activateLoseScreen();//    loseScreen.SetActive(true);

        }

        checkingLoseStatus = false;
        yield return null;
    }

    private IEnumerator resurrectSkeleton()
    {
        //Wait until it has been determined if the player has lost or not
        yield return new WaitWhile(() => checkingLoseStatus);

        resurrectingSkeleton = true;
        List<Skeleton> skeletons = GameManager.instance.skeletons;
        GUIElements.activateConfirmButton();

        //Check the skeletons in order to see if they are dead and their corresponding orb is still above 0 hp.
        //If one of these is found, that skeleton is ressurected. Only one skeleton can be resurrected per turn.
        if (skeletons[0].dead == true && blueOrbHP > 0)
        {
            skeletons[0].healthPoints = 8;
            skeletons[0].dead = false;
            combatText.text = "The Wraith ressurects " + skeletons[0].skelName + ".";
            updateSkeletonHP(skeletons[0]);
            //Block out the orb's hp value to show it cannot be attacked
            GameManager.instance.guiScript.activateBlueOrbX();
        }
        else if (skeletons[1].dead == true && redOrbHP > 0)
        {
            skeletons[1].healthPoints = 8;
            skeletons[1].dead = false;
            combatText.text = "The Wraith ressurects " + skeletons[1].skelName + ".";
            updateSkeletonHP(skeletons[1]);
            GameManager.instance.guiScript.activateRedOrbX();
        }
        else if (skeletons[2].dead == true && greenOrbHP > 0)
        {
            skeletons[2].healthPoints = 8;
            skeletons[2].dead = false;
            combatText.text = "The Wraith ressurects " + skeletons[2].skelName + ".";
            updateSkeletonHP(skeletons[2]);
            GameManager.instance.guiScript.activateGreenOrbX();
        }
        else if (skeletons[3].dead == true && yellowOrbHP > 0)
        {
            skeletons[3].healthPoints = 8;
            skeletons[3].dead = false;
            combatText.text = "The Wraith ressurects " + skeletons[3].skelName + ".";
            updateSkeletonHP(skeletons[3]);
            GameManager.instance.guiScript.activateYellowOrbX();
        }

        yield return new WaitWhile(() => resurrectingSkeleton);
        wraithActing = false;
    }

    public void updateFacing(Vector2 startingPos, Vector2 destinationPos)
    {
        if (destinationPos.x < startingPos.x)
        {
            facing = "left";
            wraithAnimator.SetTrigger("walkLeft");
        }
        else if (destinationPos.x > startingPos.x)
        {
            facing = "right";
            wraithAnimator.SetTrigger("walkRight");
        }
        else if (destinationPos.y < startingPos.y)
        {
            facing = "down";
            wraithAnimator.SetTrigger("walkDown");
        }
        else if (destinationPos.y > startingPos.y)
        {
            facing = "up";
            wraithAnimator.SetTrigger("walkUp");
        }

        updateTargetAreas();
    }

    public void pivot(Player target)
    {
        if (target.gridPos.x < topLeftPos.x)
        {
            facing = "left";
            wraithAnimator.SetTrigger("attackLeft");
        }
        else if (target.gridPos.x > topRightPos.x)
        {
            facing = "right";
            wraithAnimator.SetTrigger("attackRight");
        }
        else if (target.gridPos.y < bottomLeftPos.y)
        {
            facing = "down";
            wraithAnimator.SetTrigger("attackDown");
        }
        else if (target.gridPos.y > topRightPos.y)
        {
            facing = "up";
            wraithAnimator.SetTrigger("attackUp");
        }

        updateTargetAreas();
    }

    public List<Vector2> updatePosValues(Vector2 targTile, List<Tile> list)
    {
        Vector2 newVector = new Vector2(0, 0);
        List<Vector2> vectorList = new List<Vector2>();
        foreach (Tile t in list)
        {
            if (targTile.x < transform.position.x && targTile.y < transform.position.y)
            {
                newVector.x = t.gridPos.x + .5f;
                newVector.y = t.gridPos.y + .5f;
            }
            else if (targTile.x < transform.position.x && targTile.y > transform.position.y)
            {
                newVector.x = t.gridPos.x + .5f;
                newVector.y = t.gridPos.y - .5f;
            }
            else if (targTile.x > transform.position.x && targTile.y < transform.position.y)
            {
                newVector.x = t.gridPos.x - .5f;
                newVector.y = t.gridPos.y + .5f;
            }
            else if (targTile.x > transform.position.x && targTile.y > transform.position.y)
            {
                newVector.x = t.gridPos.x - .5f;
                newVector.y = t.gridPos.y - .5f;
            }
            vectorList.Add(newVector);
        }

        return vectorList;
    }

    public void checkCollisions()
    {
        List<Skeleton> skeletons = GameManager.instance.skeletons;
        List<Player> players = GameManager.instance.players;

        foreach( Skeleton s in skeletons)
        {
            bool successfulShift = false;
            if ( s.gridPos == topLeftPos || s.gridPos == topRightPos || s.gridPos == bottomLeftPos || s.gridPos == bottomRightPos)
            {
                foreach (Tile t in GameManager.instance.boardScript.board[(int)s.gridPos.x, (int)s.gridPos.y].neighbors)
                {
                    if(t.occupied == false)
                    {
                        StartCoroutine(s.shiftPosition(t));
                        successfulShift = true;
                        break;
                    }
                }

                if (successfulShift == false)
                {
                    foreach (Tile t in GameManager.instance.boardScript.board[(int)s.gridPos.x, (int)s.gridPos.y].neighbors)
                    {
                        foreach( Tile n in t.neighbors)
                        {
                            StartCoroutine(s.shiftPosition(n));
                            break;
                        }
                    }
                }
            }
        }

        foreach (Player p in players)
        {
            bool successfulShift = false;
            if (p.gridPos == topLeftPos || p.gridPos == topRightPos || p.gridPos == bottomLeftPos || p.gridPos == bottomRightPos)
            {
                foreach (Tile t in GameManager.instance.boardScript.board[(int)p.gridPos.x, (int)p.gridPos.y].neighbors)
                {
                    if (t.occupied == false)
                    {
                        StartCoroutine(p.shiftPosition(t));
                        successfulShift = true;
                        break;
                    }
                }

                if (successfulShift == false)
                {
                    foreach (Tile t in GameManager.instance.boardScript.board[(int)p.gridPos.x, (int)p.gridPos.y].neighbors)
                    {
                        foreach (Tile n in t.neighbors)
                        {
                            StartCoroutine(p.shiftPosition(n));
                            break;
                        }
                    }
                }
            }
        }
    }
}

