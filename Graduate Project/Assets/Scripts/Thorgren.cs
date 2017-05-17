using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Thorgren : MonoBehaviour {

    public Vector2 topLeftPos = Vector2.zero;
    public Vector2 topRightPos = Vector2.zero;
    public Vector2 bottomLeftPos = Vector2.zero;
    public Vector2 bottomRightPos = Vector2.zero;

    public Text combatText;
    public Text bronHPText;
    public Text cristophHPText;
    public Text saraHPText;
    public Text veraHPText;

    private Text thorgrenHPText;
    private Text headText;
    private Text chestText;
    private Text backText;
    private Text parasiteText;
    private Text rightArmText;
    private Text leftArmText;
    private Text rightLegText;
    private Text leftLegText;

 //   private GameObject loseScreen;

    public struct FrontRight
    {
        public Vector2 position;
        public List<string> targets;

        public FrontRight( Vector2 pos )
        {
            position = pos;
            targets = new List<string> { "head", "chest", "rightArm" };

        }
    }
    public struct FrontLeft
    {
        public Vector2 position;
        public List<string> targets;

        public FrontLeft(Vector2 pos)
        {
            position = pos;
            targets = new List<string> { "head", "chest", "leftArm" };

        }
    }
    public struct BackRight
    {
        public Vector2 position;
        public List<string> targets;

        public BackRight(Vector2 pos, string type)
        {
            if (type.Equals("Afflicted Thorgren"))
            {
                position = pos;
                targets = new List<string> { "parasite", "rightLeg" };
            }
            else
            {
                position = pos;
                targets = new List<string> { "back", "rightLeg" };
            }
        }
    }
    public struct BackLeft
    {
        public Vector2 position;
        public List<string> targets;

        public BackLeft(Vector2 pos, string type)
        {
            if (type.Equals("Afflicted Thorgren"))
            {
                position = pos;
                targets = new List<string> { "parasite", "leftLeg" };
            }
            else
            {
                position = pos;
                targets = new List<string> { "back", "leftLeg" };
            }
        }
    }

    public FrontRight frontRight;
    public FrontLeft frontLeft;
    public BackRight backRight;
    public BackLeft backLeft;

    public string type;
    public string facing;
    public bool dead = false;

    public int healthPoints;
    public int leftArmHP;
    public int rightArmHP;
    public int leftLegHP;
    public int rightLegHP;
    public int chestHP;
    public int backHP;
    public int parasiteHP;
    public int headHP;

    public int maxHP = 300;
    public int maxHead = 40;
    public int maxBack = 30;
    public int maxChest = 30;
    public int maxParasite = 50;
    public int maxRightA = 25;
    public int maxLeftA = 25;
    public int maxRightL = 25;
    public int maxLeftL = 25;

    public Thorgren[] enemyPrefabs;
    //public List<Thorgren> enemies = new List<Thorgren>();
    public Thorgren thor;

    public float moveTime = 0.1f;
    public bool moving = false;
    public bool attacking = false;
    public bool healing = false;
    public bool preparingLoseScreen = false;
    public bool thorgrenTurn = false;
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    public Animator thorgrenAnimator;
    private float inverseMoveTime;


    void Awake()
    {
       // loseScreen = GameObject.FindGameObjectWithTag("Lose Screen");
    }

    void Start()
    {
        if (GameManager.instance.level.Equals("Thorgren Battle") || GameManager.instance.level.Equals("Afflicted Thorgren Battle"))
        {
            combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
            bronHPText = GameObject.FindGameObjectWithTag("Bron HP Text").GetComponent<Text>();
            cristophHPText = GameObject.FindGameObjectWithTag("Cristoph HP Text").GetComponent<Text>();
            saraHPText = GameObject.FindGameObjectWithTag("Sara HP Text").GetComponent<Text>();
            veraHPText = GameObject.FindGameObjectWithTag("Vera HP Text").GetComponent<Text>();

            thorgrenHPText = GameObject.FindGameObjectWithTag("Thorgren HP").GetComponent<Text>();
            headText = GameObject.FindGameObjectWithTag("Head HP").GetComponent<Text>();
            chestText = GameObject.FindGameObjectWithTag("Chest HP").GetComponent<Text>();
            rightArmText = GameObject.FindGameObjectWithTag("Right Arm HP").GetComponent<Text>();
            leftArmText = GameObject.FindGameObjectWithTag("Left Arm HP").GetComponent<Text>();
            rightLegText = GameObject.FindGameObjectWithTag("Right Leg HP").GetComponent<Text>();
            leftLegText = GameObject.FindGameObjectWithTag("Left Leg HP").GetComponent<Text>();
        }
        if (GameManager.instance.level.Equals("Afflicted Thorgren Battle"))
        {
            parasiteText = GameObject.FindGameObjectWithTag("Parasite HP").GetComponent<Text>();
        }
        else if(GameManager.instance.level.Equals("Thorgren Battle"))
        {
            backText = GameObject.FindGameObjectWithTag("Back HP").GetComponent<Text>();
        }

        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
        thorgrenAnimator = GetComponent<Animator>();
        //GameManager.instance.guiScript.activateLoseScreen();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void thorgrenGenerate( string thorType )
    {
        if (thorType.Equals("Thorgren"))
        {
            thor = Instantiate(enemyPrefabs[0], new Vector2(5.5f, 5.5f), Quaternion.identity);
        }
        else
        {
            thor = Instantiate(enemyPrefabs[1], new Vector2(5.5f, 5.5f), Quaternion.identity);
        }
        thor.topLeftPos = new Vector2(5, 6);
        thor.topRightPos = new Vector2(6, 6);
        thor.bottomLeftPos = new Vector2(5, 5);
        thor.bottomRightPos = new Vector2(6, 5);
        thor.facing = "down";
        thor.moveTime = 0.2f;

        thor.healthPoints = maxHP;
        thor.leftArmHP = maxLeftA;
        thor.rightArmHP = maxRightA;
        thor.leftLegHP = maxLeftL;
        thor.rightLegHP = maxRightL;
        thor.chestHP = maxChest;
        thor.headHP = maxHead;

        
        /*thor.thorgrenHPText.text = "HP: " + thor.maxHP;
        thor.headText.text = "Head: " + thor.maxHead;
        thor.chestText.text = "Chest: " + thor.maxChest;
        thor.rightArmText.text = "Right A: " + thor.maxRightA;
        thor.leftArmText.text = "Left A: " + thor.maxLeftA;
        thor.rightLegText.text = "Right L: " + thor.maxRightL;
        thor.leftLegText.text = "Left L: " + thor.maxLeftL;*/

        thor.type = thorType;
        if (thor.type.Equals("Thorgren"))
        {
            thor.backHP = maxBack;
            //thor.backText.text = "Back: " + thor.maxBack;
        }
        else if(thor.type.Equals("Afflicted Thorgren"))
        {
            thor.parasiteHP = maxParasite;
            //thor.parasiteText.text = "Parasite: " + thor.maxParasite;
        }

        thor.frontRight = new FrontRight(thor.bottomLeftPos);
        thor.frontLeft = new FrontLeft(thor.bottomRightPos);
        thor.backRight = new BackRight(thor.topLeftPos, thor.type);
        thor.backLeft = new BackLeft(thor.topRightPos, thor.type);
        //enemies.Add(thor);
        
        GameManager.instance.boardScript.board[(int)thor.topLeftPos.x, (int)thor.topLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)thor.topRightPos.x, (int)thor.topRightPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)thor.bottomLeftPos.x, (int)thor.bottomLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)thor.bottomRightPos.x, (int)thor.bottomRightPos.y].occupied = true;

    }

    public void updateTargetAreas()
    {
        if( facing.Equals("down") )
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

            Vector2 finalDest = vectorList[vectorList.Count-1];

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
        if( list.Count > 0)
        {
            StartCoroutine(SmoothMovement(list[0], list));
        }

    }

    protected IEnumerator updatePos(Vector2 finalDest, Vector2 targTile, Player target)
    {

        yield return new WaitUntil(()=> (Vector2)transform.position == finalDest);

        topLeftPos.x = transform.position.x - .5f;
        topLeftPos.y = transform.position.y + .5f;
        topRightPos.x = transform.position.x + .5f;
        topRightPos.y = transform.position.y + .5f;
        bottomLeftPos.x = transform.position.x - .5f;
        bottomLeftPos.y = transform.position.y - .5f;
        bottomRightPos.x = transform.position.x + .5f;
        bottomRightPos.y = transform.position.y - .5f;

        pivot(target);

        GameManager.instance.boardScript.board[(int)this.topLeftPos.x, (int)this.topLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.topRightPos.x, (int)this.topRightPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.bottomLeftPos.x, (int)this.bottomLeftPos.y].occupied = true;
        GameManager.instance.boardScript.board[(int)this.bottomRightPos.x, (int)this.bottomRightPos.y].occupied = true;

        updateTargetAreas();

        moving = false;
    }

    public IEnumerator activateAttack( Player target)
    {
        //Wait while the Thorgren is moving
        yield return new WaitWhile(() => moving);

        attacking = true;
        //Create an attack
        ThorgrenAttack attack = new ThorgrenAttack(Random.Range(1,3), target);

        //Apply damage to the target
        target.healthPoints -= attack.damageValue;

        //Update the combat text box with relevant info and activate the confirm button
        GUIElements.activateConfirmButton();
        combatText.text = "The Thorgen attacks " + target.name + " with " + attack.attackName + " for " + attack.damageValue + " damage.";

        //Wait until the confirm button has been pressed then update the UI
        yield return new WaitWhile(() => attacking);
        checkPlayerHPValues();
        updatePlayerHP(target);

        combatText.text = "";

        //If the Afflicted Thorgren is being fought and the parastie is alive, heal the Thorgren
        if( GameManager.instance.level.Equals("Afflicted Thorgren Battle") && parasiteHP > 0)
        {
            healing = true;
            GUIElements.activateConfirmButton();
            healAffThorgren();
            combatText.text = "The parasite heals the Afflicted Thorgren for 20 HP and each of its body parts for 5 HP.";

            yield return new WaitWhile(() => healing);

            enemyHealUpdate();
        }

        //Check if the player lost the battle 
        StartCoroutine(checkIfPlayerLost());
        //Reactivate the end turn button to signal that it is the player's turn
        GUIElements.activateEndTurnButton();


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


    public void healAffThorgren()
    {
        Thorgren thor = GameManager.instance.thorgrenScript.thor;

        thor.healthPoints += 20;
        if( thor.healthPoints > maxHP)
        {
            thor.healthPoints = maxHP;
        }

        if (thor.headHP > 0)
        {
            thor.headHP += 5;
            if (thor.headHP > maxHead)
            {
                thor.headHP = maxHead;
            }
        }

        if (thor.chestHP > 0)
        {
            thor.chestHP += 5;
            if (thor.chestHP > maxChest)
            {
                thor.chestHP = maxChest;
            }
        }

        if (thor.parasiteHP > 0)
        {
            thor.parasiteHP += 5;
            if (thor.parasiteHP > maxParasite)
            {
                thor.parasiteHP = maxParasite;
            }
        }

        if (thor.rightArmHP > 0)
        {
            thor.rightArmHP += 5;
            if (thor.rightArmHP > maxRightA)
            {
                thor.rightArmHP = maxRightA;
            }
        }

        if (thor.leftArmHP > 0)
        {
            thor.leftArmHP += 5;
            if (thor.leftArmHP > maxLeftA)
            {
                thor.leftArmHP = maxLeftA;
            }
        }

        if (thor.rightLegHP > 0)
        {
            thor.rightLegHP += 5;
            if (thor.rightLegHP > maxRightL)
            {
                thor.rightLegHP = maxRightL;
            }
        }

        if (thor.leftLegHP > 0)
        {
            thor.leftLegHP += 5;
            if (thor.leftLegHP > maxLeftL)
            {
                thor.leftLegHP = maxLeftL;
            }
        }
    }

    public void enemyHealUpdate()
    {
        thorgrenHPText.text = "HP: " + GameManager.instance.thorgrenScript.thor.healthPoints;
        
        headText.text = "Head: " + GameManager.instance.thorgrenScript.thor.headHP;
        
        chestText.text = "Chest: " + GameManager.instance.thorgrenScript.thor.chestHP;
       
        parasiteText.text = "Parasite: " + GameManager.instance.thorgrenScript.thor.parasiteHP;
        
        rightArmText.text = "Right A: " + GameManager.instance.thorgrenScript.thor.rightArmHP;
        
        leftArmText.text = "Left A: " + GameManager.instance.thorgrenScript.thor.leftArmHP;
       
        rightLegText.text = "Right L: " + GameManager.instance.thorgrenScript.thor.rightLegHP;
       
        leftLegText.text = "Left L: " + GameManager.instance.thorgrenScript.thor.leftLegHP;
    }

    public IEnumerator checkIfPlayerLost()
    {
        List<Player> players = GameManager.instance.players;
        if (players[0].dead == true && players[1].dead == true && players[2].dead == true && players[3].dead == true)
        {
            preparingLoseScreen = true;

            combatText.text = "All characters have been defeated. You lose.";

            GUIElements.activateConfirmButton();

            yield return new WaitWhile(() => preparingLoseScreen);

            combatText.text = "";

            GameManager.instance.guiScript.activateLoseScreen();// loseScreen.SetActive(true);

        }

        yield return null;
        GameManager.instance.thorgrenScript.thor.thorgrenTurn = false;
    }
    public void updateFacing(Vector2 startingPos, Vector2 destinationPos )
    {
        if (destinationPos.x < startingPos.x)
        {
            facing = "left";
            thorgrenAnimator.SetTrigger("walkLeft");
        }
        else if (destinationPos.x > startingPos.x)
        {
            facing = "right";
            thorgrenAnimator.SetTrigger("walkRight");
        }
        else if (destinationPos.y < startingPos.y)
        {
            facing = "down";
            thorgrenAnimator.SetTrigger("walkDown");
        }
        else if (destinationPos.y > startingPos.y)
        {
            facing = "up";
            thorgrenAnimator.SetTrigger("walkUp");
        }

        updateTargetAreas();
    }

    public void pivot( Player target )
    {
        if( target.gridPos.x < topLeftPos.x )
        {
            facing = "left";
            thorgrenAnimator.SetTrigger("attackLeft");
        }
        else if (target.gridPos.x > topRightPos.x)
        {
            facing = "right";
            thorgrenAnimator.SetTrigger("attackRight");
        }
        else if (target.gridPos.y < bottomLeftPos.y)
        {
            facing = "down";
            thorgrenAnimator.SetTrigger("attackDown");
        }
        else if (target.gridPos.y > topRightPos.y)
        {
            facing = "up";
            thorgrenAnimator.SetTrigger("attackUp");
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
}
