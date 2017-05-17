using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIElements : MonoBehaviour
{
    private Text thorgrenHPText;
    private Text headText;
    private Text chestText;
    private Text backText;
    private Text parasiteText;
    private Text rightArmText;
    private Text leftArmText;
    private Text rightLegText;
    private Text leftLegText;
    private Text blueOrbText;
    private Text redOrbText;
    private Text greenOrbText;
    private Text yellowOrbText;
    private Text skeleton1Text;
    private Text skeleton2Text;
    private Text skeleton3Text;
    private Text skeleton4Text;
    private Text combatText;

    private GameObject transitionImage;
    private GameObject winScreen;
    private GameObject loseScreen;
    private GameObject blueOrbX;
    private GameObject redOrbX;
    private GameObject greenOrbX;
    private GameObject yellowOrbX;

    public bool preparingWinScreen = false;

    void Awake()
    {
        combatText = GameObject.FindGameObjectWithTag("Combat Text").GetComponent<Text>();
       
        transitionImage = GameObject.FindGameObjectWithTag("Transition Image");
        winScreen = GameObject.FindGameObjectWithTag("Win Screen");
        loseScreen = GameObject.FindGameObjectWithTag("Lose Screen");

    }
    void Start()
    {
        
        if (GameManager.instance.level.Equals("Thorgren Battle") || GameManager.instance.level.Equals("Afflicted Thorgren Battle"))
        {
            thorgrenHPText = GameObject.FindGameObjectWithTag("Thorgren HP").GetComponent<Text>();
            headText = GameObject.FindGameObjectWithTag("Head HP").GetComponent<Text>();
            chestText = GameObject.FindGameObjectWithTag("Chest HP").GetComponent<Text>();
            rightArmText = GameObject.FindGameObjectWithTag("Right Arm HP").GetComponent<Text>();
            leftArmText = GameObject.FindGameObjectWithTag("Left Arm HP").GetComponent<Text>();
            rightLegText = GameObject.FindGameObjectWithTag("Right Leg HP").GetComponent<Text>();
            leftLegText = GameObject.FindGameObjectWithTag("Left Leg HP").GetComponent<Text>();
        }

        if (GameManager.instance.level.Equals("Thorgren Battle"))
        {
            backText = GameObject.FindGameObjectWithTag("Back HP").GetComponent<Text>();
        }
        else if (GameManager.instance.level.Equals("Afflicted Thorgren Battle"))
        {
            parasiteText = GameObject.FindGameObjectWithTag("Parasite HP").GetComponent<Text>();
        }

        if (GameManager.instance.level.Equals("Wraith Battle"))
        {
            blueOrbText = GameObject.FindGameObjectWithTag("Blue Orb HP").GetComponent<Text>();
            redOrbText = GameObject.FindGameObjectWithTag("Red Orb HP").GetComponent<Text>();
            greenOrbText = GameObject.FindGameObjectWithTag("Green Orb HP").GetComponent<Text>();
            yellowOrbText = GameObject.FindGameObjectWithTag("Yellow Orb HP").GetComponent<Text>();
            skeleton1Text = GameObject.FindGameObjectWithTag("Skeleton 1 HP").GetComponent<Text>();
            skeleton2Text = GameObject.FindGameObjectWithTag("Skeleton 2 HP").GetComponent<Text>();
            skeleton3Text = GameObject.FindGameObjectWithTag("Skeleton 3 HP").GetComponent<Text>();
            skeleton4Text = GameObject.FindGameObjectWithTag("Skeleton 4 HP").GetComponent<Text>();

            blueOrbX = GameObject.FindGameObjectWithTag("Blue Orb X");
            redOrbX = GameObject.FindGameObjectWithTag("Red Orb X");
            greenOrbX = GameObject.FindGameObjectWithTag("Green Orb X");
            yellowOrbX = GameObject.FindGameObjectWithTag("Yellow Orb X");
        }
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        if(GameManager.instance.level.Equals("Wraith Battle"))
        {
            deactivateBlueOrbX();
            deactivateRedOrbX();
            deactivateGreenOrbX();
            deactivateYellowOrbX();
        }
    }

    public void fightButton()
    {
        transitionImage.SetActive(false);
    }

    public void firstBattleContinueButton()
    {
        SceneManager.LoadScene("Afflicted Thorgren Battle");
    }
    public void secondBattleContinueButton()
    {
        SceneManager.LoadScene("Wraith Battle");
    }
    public void thirdBattleContinueButton()
    {
        Application.Quit();
    }
    public void firstBattleRetry()
    {
        SceneManager.LoadScene("Thorgren Battle");
    }
    public void secondBattleRetry()
    {
        SceneManager.LoadScene("Afflicted Thorgren Battle");
    }
    public void thirdBattleRetry()
    {
        SceneManager.LoadScene("Wraith Battle");
    }
    public void mainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void exitButton()
    {
        Application.Quit();
    }

    public void moveClick()
    {
        //In case the player has pressed the attack button, deactivate target buttons
        deactivateTargetButtons();

        Player player = new Player();
        bool activePlayer = false;
        //Find the selected character
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
                activePlayer = true;
            }
        }

        //If the character has sufficeient movement tokens, highlight the tiles they can move to
        if( activePlayer == true && player.movementTokens > 0)
        {
            player.highlightTilesAt(player.gridPos, player.movementPoints);
        }
    }

    public void attackClick()
    {

        Player player = new Player();
        bool activePlayer = false;

        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
                activePlayer = true;
                
                if (player.highlightedTiles.Count > 0)
                {
                    foreach (Tile t in p.highlightedTiles)
                    {
                        t.highlighted = false;
                        t.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }

        //If a player is selected and has an action token, determine if there are any valid
        //targets arounf them
        if (activePlayer == true && player.actionTokens > 0)
        {
            bool adjacentToEnemy = false;
            List<string> targets = new List<string>();
            //Loop through each of the neighboring tiles
            foreach( Tile t in GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].neighbors)
            {
                //Check if the tile is occupied and if the Thorgren is occupying it 
                if (GameManager.instance.boardScript.board[(int)t.gridPos.x, (int)t.gridPos.y].occupied == true && checkThorgrenTiles(t.gridPos))
                {
                    
                    adjacentToEnemy = true;
                    //Get the target parts based on which section of the Thorgren the 
                    //character is next to
                    targets = getValidTargets(t.gridPos);
                }
            }
            //If the character is next to the Thorgren, activate the necessary target buttons
            if( adjacentToEnemy == true)
            {
                foreach( string s in targets )
                {
                    activateThorgrenTargetButton(s);
                }
            }
        }
    }

    public void wraithAttackClick()
    {

        Player player = new Player();
        bool activePlayer = false;

        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
                activePlayer = true;

                if (player.highlightedTiles.Count > 0)
                {
                    foreach (Tile t in p.highlightedTiles)
                    {
                        t.highlighted = false;
                        t.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }

        if (activePlayer == true && player.actionTokens > 0)
        {
            bool adjacentToWraith = false;
            bool adjacentToSkeleton = false;
            List<string> targets = new List<string>();
            foreach (Tile t in GameManager.instance.boardScript.board[(int)player.gridPos.x, (int)player.gridPos.y].neighbors)
            {
                if (GameManager.instance.boardScript.board[(int)t.gridPos.x, (int)t.gridPos.y].occupied == true && checkWraithTiles(t.gridPos))
                {

                    adjacentToWraith = true;

                    targets = getValidTargets(t.gridPos);
                }
                else if(GameManager.instance.boardScript.board[(int)t.gridPos.x, (int)t.gridPos.y].occupied == true )
                {
                    string skeleton = checkSkeletonTiles(t.gridPos);

                    if(!skeleton.Equals(""))
                    {
                        adjacentToSkeleton = true;
                        targets.Add(skeleton);
                    }
                }
            }

            if (adjacentToWraith == true || adjacentToSkeleton == true)
            {
                foreach (string s in targets)
                {
                    activateWraithTargetButton(s);
                }
            }
        }
    }

    public void headButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.headHP <= 0)
        {
            hpDamage = (5 + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (5 + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.headHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.headHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.headHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "head"));

    }

    public void chestButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        int chestDamage = 3;
        if( GameManager.instance.thorgrenScript.thor.chestHP == 0)
        {
            chestDamage = 6;
        }
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.chestHP <= 0)
        {
            hpDamage = (chestDamage + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (chestDamage + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.chestHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.chestHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.chestHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "chest"));

    }

    public void backButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        int backDamage = 3;
        if (GameManager.instance.thorgrenScript.thor.backHP == 0)
        {
            backDamage = 6;
        }
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.backHP <= 0)
        {
            hpDamage = (backDamage + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (backDamage + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.backHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.backHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.backHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "back"));

    }

    public void parasiteButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        int parasiteDamage = 2;
        if (GameManager.instance.thorgrenScript.thor.parasiteHP == 0)
        {
            parasiteDamage = 4;
        }
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.parasiteHP <= 0)
        {
            hpDamage = (parasiteDamage + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (parasiteDamage + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.parasiteHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.parasiteHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.parasiteHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "parasite"));

    }

    public void rightArmButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.rightArmHP <= 0)
        {
            hpDamage = (1 + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (1 + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.rightArmHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.rightArmHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.rightArmHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "right arm"));

    }

    public void leftArmButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.leftArmHP <= 0)
        {
            hpDamage = (1 + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (1 + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.leftArmHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.leftArmHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.leftArmHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "left arm"));

    }

    public void rightLegButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.rightLegHP <= 0)
        {
            hpDamage = (1 + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (1 + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.rightLegHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.rightLegHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.rightLegHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "right leg"));

    }

    public void leftLegButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }

        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);

        if (player.weapon.Equals("rapier") && GameManager.instance.thorgrenScript.thor.leftLegHP <= 0)
        {
            hpDamage = (1 + (weaponDamage * 2));
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }
        else
        {
            hpDamage = (1 + weaponDamage);
            GameManager.instance.thorgrenScript.thor.healthPoints -= hpDamage;
        }

        if (player.weapon.Equals("mace") && GameManager.instance.thorgrenScript.thor.leftLegHP > 0)
        {
            partDamage = weaponDamage * 2;
            GameManager.instance.thorgrenScript.thor.leftLegHP -= partDamage;
        }
        else
        {
            partDamage = weaponDamage;
            GameManager.instance.thorgrenScript.thor.leftLegHP -= partDamage;
        }
        deactivateTargetButtons();

        player.pivot("Thorgren");

        StartCoroutine(displayPlayerCombatText(player, hpDamage, partDamage, "left leg"));

    }

    public void blueOrbButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        partDamage = weaponDamage;

        if (player.weapon.Equals("rapier"))
        {
            partDamage = weaponDamage * 2;
        }
        GameManager.instance.wraithScript.wraith.blueOrbHP -= partDamage;


        deactivateTargetButtons();

        player.pivot("Wraith");

        StartCoroutine(displayPlayerCombatText(player, 0, partDamage, "blue orb"));
    }
    public void yellowOrbButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        partDamage = weaponDamage;

        if (player.weapon.Equals("rapier"))
        {
            partDamage = weaponDamage * 2;
        }
        GameManager.instance.wraithScript.wraith.yellowOrbHP -= partDamage;


        deactivateTargetButtons();

        player.pivot("Wraith");

        StartCoroutine(displayPlayerCombatText(player, 0, partDamage, "yellow orb"));
    }
    public void redOrbButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        partDamage = weaponDamage;

        if (player.weapon.Equals("rapier"))
        {
            partDamage = weaponDamage * 2;
        }
        GameManager.instance.wraithScript.wraith.redOrbHP -= partDamage;


        deactivateTargetButtons();

        player.pivot("Wraith");

        StartCoroutine(displayPlayerCombatText(player, 0, partDamage, "red orb"));

    }
    public void greenOrbButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int partDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        partDamage = weaponDamage;

        if (player.weapon.Equals("rapier"))
        {
            partDamage = weaponDamage * 2;
        }
        GameManager.instance.wraithScript.wraith.greenOrbHP -= partDamage;


        deactivateTargetButtons();

        player.pivot("Wraith");

        StartCoroutine(displayPlayerCombatText(player, 0, partDamage, "green orb"));

    }

    public void skeleton1ButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        hpDamage = weaponDamage;

        if (player.weapon.Equals("mace"))
        {
            hpDamage = weaponDamage * 2;
        }
        GameManager.instance.skeletons[0].healthPoints -= hpDamage;

        deactivateTargetButtons();

        player.pivot("Skeleton 1");

        StartCoroutine(skeletonDisplayPlayerCombatText(player, hpDamage, "Skeleton 1"));

    }

    public void skeleton2ButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        hpDamage = weaponDamage;

        if (player.weapon.Equals("mace"))
        {
            hpDamage = weaponDamage * 2;
        }
        GameManager.instance.skeletons[1].healthPoints -= hpDamage;


        deactivateTargetButtons();

        player.pivot("Skeleton 2");

        StartCoroutine(skeletonDisplayPlayerCombatText(player, hpDamage, "Skeleton 2"));

    }
    public void skeleton3ButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        hpDamage = weaponDamage;

        if (player.weapon.Equals("mace"))
        {
            hpDamage = weaponDamage * 2;
        }
        GameManager.instance.skeletons[2].healthPoints -= hpDamage;


        deactivateTargetButtons();

        player.pivot("Skeleton 3");
  
        StartCoroutine(skeletonDisplayPlayerCombatText(player, hpDamage, "Skeleton 3"));

    }
    public void skeleton4ButtonClick()
    {
        Player player = new Player();
        int weaponDamage = 0;
        int hpDamage = 0;
        foreach (Player p in GameManager.instance.playerScript.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        targetButtonPressed(player);

        weaponDamage = generateWeaponDamage(player.weapon);
        hpDamage = weaponDamage;

        if (player.weapon.Equals("mace"))
        {
            hpDamage = weaponDamage * 2;
        }
        GameManager.instance.skeletons[3].healthPoints -= hpDamage;


        deactivateTargetButtons();

        player.pivot("Skeleton 4");

        StartCoroutine(skeletonDisplayPlayerCombatText(player, hpDamage, "Skeleton 4"));

    }


    public void targetButtonPressed(Player player)
    {
        deactivateEndTurnButton();
        player.attacking = true;
        player.actionTokens -= 1;
        deactivateAttackButton();
    }
    public void enemyStatusUpdate( Player player, string targetPart)
    {
        combatText.text = "";
        checkEnemyHPValues();
        if (!GameManager.instance.level.Equals("Wraith Battle"))
        {
            thorgrenHPText.text = "HP: " + GameManager.instance.thorgrenScript.thor.healthPoints;
        }

        if (targetPart.Equals("head"))
        {
            headText.text = "Head: " + GameManager.instance.thorgrenScript.thor.headHP;
        }
        else if (targetPart.Equals("chest"))
        {
            chestText.text = "Chest: " + GameManager.instance.thorgrenScript.thor.chestHP;
        }
        else if (targetPart.Equals("back"))
        {
            backText.text = "Back: " + GameManager.instance.thorgrenScript.thor.backHP;
        }
        else if (targetPart.Equals("parasite"))
        {
            parasiteText.text = "Parasite: " + GameManager.instance.thorgrenScript.thor.parasiteHP;
        }
        else if (targetPart.Equals("right arm"))
        {
            rightArmText.text = "Right A: " + GameManager.instance.thorgrenScript.thor.rightArmHP;
        }
        else if (targetPart.Equals("left arm"))
        {
            leftArmText.text = "Left A: " + GameManager.instance.thorgrenScript.thor.leftArmHP;
        }
        else if (targetPart.Equals("right leg"))
        {
            rightLegText.text = "Right L: " + GameManager.instance.thorgrenScript.thor.rightLegHP;
        }
        else if (targetPart.Equals("left leg"))
        {
            leftLegText.text = "Left L: " + GameManager.instance.thorgrenScript.thor.leftLegHP;
        }
        else if (targetPart.Equals("blue orb"))
        {
            blueOrbText.text = "Blue Orb:" + GameManager.instance.wraithScript.wraith.blueOrbHP;
        }
        else if (targetPart.Equals("red orb"))
        {
            redOrbText.text = "Red Orb:" + GameManager.instance.wraithScript.wraith.redOrbHP;
        }
        else if (targetPart.Equals("green orb"))
        {
            greenOrbText.text = "Green Orb:" + GameManager.instance.wraithScript.wraith.greenOrbHP;
        }
        else if (targetPart.Equals("yellow orb"))
        {
            yellowOrbText.text = "Yellow Orb:" + GameManager.instance.wraithScript.wraith.yellowOrbHP;
        }
        else if (targetPart.Equals("Skeleton 1"))
        {
            skeleton1Text.text = "Skeleton 1:" + GameManager.instance.skeletons[0].healthPoints;
        }
        else if (targetPart.Equals("Skeleton 2"))
        {
            skeleton2Text.text = "Skeleton 2:" + GameManager.instance.skeletons[1].healthPoints;
        }
        else if (targetPart.Equals("Skeleton 3"))
        {
            skeleton3Text.text = "Skeleton 3:" + GameManager.instance.skeletons[2].healthPoints;
        }
        else if (targetPart.Equals("Skeleton 4"))
        {
            skeleton4Text.text = "Skeleton 4:" + GameManager.instance.skeletons[3].healthPoints;
        }

        player.attacking = false;

        if (player.actionTokens > 0)
        {
            activateAttackButton();
        }

        activateEndTurnButton();
    }

    protected IEnumerator displayPlayerCombatText( Player player, int hpDamage, int partDamage, string targetPart )
    {
        string monsterType = GameManager.instance.monsterType;

        if (monsterType.Equals("Wraith"))
        {
            combatText.text = player.name + " hits the " + monsterType + "'s " + targetPart + " for " + partDamage + ".";
        }
        else
        {
            combatText.text = player.name + " hits the " + monsterType + "'s " + targetPart + " for " + partDamage + " damage and deals " + hpDamage + " damage to the " + monsterType + ".";
        }

        activateConfirmButton();

        yield return new WaitWhile(() => player.attacking);

        enemyStatusUpdate(player, targetPart);
        if (monsterType.Equals("Wraith"))
        {
            if (GameManager.instance.wraithScript.wraith.dead == true)
            {
                preparingWinScreen = true;

                combatText.text = "The Wraith has been conquered. You win!";

                GUIElements.activateConfirmButton();

                yield return new WaitWhile(() => preparingWinScreen);

                combatText.text = "";

                winScreen.SetActive(true);

            }
        }
        else if (monsterType.Equals("Thorgren") || monsterType.Equals("Afflicted Thorgren"))
        {
            if (GameManager.instance.thorgrenScript.thor.dead == true)
            {
                preparingWinScreen = true;

                combatText.text = "The Thorgrn has been conquered. You win!";

                GUIElements.activateConfirmButton();

                yield return new WaitWhile(() => preparingWinScreen);

                combatText.text = "";

                winScreen.SetActive(true);

            }
        }

    }

    protected IEnumerator skeletonDisplayPlayerCombatText(Player player, int hpDamage, string skeleton)
    {
        combatText.text = player.name + " hits " + skeleton + " for " + hpDamage + ".";

        activateConfirmButton();

        yield return new WaitWhile(() => player.attacking);

        enemyStatusUpdate(player, skeleton);

    }
    public void endTurnClick()
    {
        deactivateMoveButton();
        deactivateAttackButton();
        deactivateEndTurnButton();
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
            p.moving = false;
            p.attacking = false;
        }

        Thorgren thor = GameManager.instance.thorgrenScript.thor;
        thor.thorgrenTurn = true;
        TargetFinder targFind = new TargetFinder("Thorgren", thor.facing);

        thor.moveAndAttack(GameManager.instance.boardScript.board[(int)targFind.target.gridPos.x, (int)targFind.target.gridPos.y], targFind.targetTile, targFind.target);
        
        foreach( Player p in GameManager.instance.players)
        {
            p.movementTokens = 1;

            if (p.weapon.Equals("dagger"))
            {
                p.actionTokens = 2;
            }
            else
            {
                p.actionTokens = 1;
            }
        }

    }

    public void endTurnWraithClick()
    {

        deactivateMoveButton();
        deactivateAttackButton();
        deactivateEndTurnButton();
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
                deactivateTargetButtons();
            }
            p.selected = false;
            p.moving = false;
            p.attacking = false;
        }

        Wraith wraith = GameManager.instance.wraithScript.wraith;
        TargetFinder targFind = new TargetFinder( "Wraith", wraith.facing);

        wraith.moveAndAttack(GameManager.instance.boardScript.board[(int)targFind.target.gridPos.x, (int)targFind.target.gridPos.y], targFind.targetTile, targFind.target);

        StartCoroutine(skeletonAttacks());

        foreach (Player p in GameManager.instance.players)
        {
            p.movementTokens = 1;

            if (p.weapon.Equals("dagger"))
            {
                p.actionTokens = 2;
            }
            else
            {
                p.actionTokens = 1;
            }
        }
    }

    protected IEnumerator skeletonAttacks()
    {
        yield return new WaitWhile(() => GameManager.instance.wraithScript.wraith.wraithActing);
        List<Skeleton> skeletons = GameManager.instance.skeletons;
        if (skeletons[0].dead == false)
        {
            skeletons[0].skeletonActing = true;
            TargetFinder skel1TargFind = new TargetFinder(skeletons[0].skelName, skeletons[0].facing);
            StartCoroutine(skeletons[0].moveAndAttack(GameManager.instance.boardScript.board[(int)skel1TargFind.target.gridPos.x, (int)skel1TargFind.target.gridPos.y], skel1TargFind.target));
        }
        if (skeletons[1].dead == false)
        {
            skeletons[1].skeletonActing = true;
            TargetFinder skel2TargFind = new TargetFinder(skeletons[1].skelName, skeletons[1].facing);
            StartCoroutine(skeletons[1].moveAndAttack(GameManager.instance.boardScript.board[(int)skel2TargFind.target.gridPos.x, (int)skel2TargFind.target.gridPos.y], skel2TargFind.target));
        }
        if (skeletons[2].dead == false)
        {
            skeletons[2].skeletonActing = true;
            TargetFinder skel3TargFind = new TargetFinder(skeletons[2].skelName, skeletons[0].facing);
            StartCoroutine(skeletons[2].moveAndAttack(GameManager.instance.boardScript.board[(int)skel3TargFind.target.gridPos.x, (int)skel3TargFind.target.gridPos.y], skel3TargFind.target));
        }
        if (skeletons[3].dead == false)
        {
            skeletons[3].skeletonActing = true;
            TargetFinder skel4TargFind = new TargetFinder(skeletons[3].skelName, skeletons[0].facing);
            StartCoroutine(skeletons[3].moveAndAttack(GameManager.instance.boardScript.board[(int)skel4TargFind.target.gridPos.x, (int)skel4TargFind.target.gridPos.y], skel4TargFind.target));
        }

        //yield return new WaitWhile(() => skeletons[0].skeletonActing && skeletons[1].skeletonActing && skeletons[2].skeletonActing && skeletons[3].skeletonActing);
        //activateEndTurnButton();
    }
    public void thorgrenConfirmClick()
    {
        Player player = GameManager.instance.players[0];

        foreach(Player p in GameManager.instance.players)
        {
            if(p.selected == true)
            {
                player = p;
            }
        }

        if (GameManager.instance.thorgrenScript.thor.attacking == true)
        {
            GameManager.instance.thorgrenScript.thor.attacking = false;
        }

        if (GameManager.instance.thorgrenScript.thor.healing == true)
        {
            GameManager.instance.thorgrenScript.thor.healing = false;
        }

        if (GameManager.instance.thorgrenScript.thor.preparingLoseScreen == true)
        {
            GameManager.instance.thorgrenScript.thor.preparingLoseScreen = false;
        }

        if (preparingWinScreen == true)
        {
            preparingWinScreen = false;
        }

        if (player.attacking == true)
        {
            player.attacking = false;
        }

        combatText.text = "";

        deactivateConfirmButton();


    }

    public void wraithConfirmClick()
    {
        Player player = GameManager.instance.players[0];

        foreach (Player p in GameManager.instance.players)
        {
            if (p.selected == true)
            {
                player = p;
            }
        }
        
        if (GameManager.instance.wraithScript.wraith.attacking == true)
        {
            GameManager.instance.wraithScript.wraith.attacking = false;
        }

        if (GameManager.instance.wraithScript.wraith.preparingLoseScreen == true)
        {
            GameManager.instance.wraithScript.wraith.preparingLoseScreen = false;
        }

        if (GameManager.instance.wraithScript.wraith.resurrectingSkeleton == true)
        {
            GameManager.instance.wraithScript.wraith.resurrectingSkeleton = false;
        }

        if(GameManager.instance.skeletonScript.skeletons[0].attacking == true)
        {
            GameManager.instance.skeletonScript.skeletons[0].attacking = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[1].attacking == true)
        {
            GameManager.instance.skeletonScript.skeletons[1].attacking = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[2].attacking == true)
        {
            GameManager.instance.skeletonScript.skeletons[2].attacking = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[3].attacking == true)
        {
            GameManager.instance.skeletonScript.skeletons[3].attacking = false;
        }

        if (GameManager.instance.skeletonScript.skeletons[0].preparingLoseScreen == true)
        {
            GameManager.instance.skeletonScript.skeletons[0].preparingLoseScreen = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[1].preparingLoseScreen == true)
        {
            GameManager.instance.skeletonScript.skeletons[1].preparingLoseScreen = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[2].preparingLoseScreen == true)
        {
            GameManager.instance.skeletonScript.skeletons[2].preparingLoseScreen = false;
        }
        else if (GameManager.instance.skeletonScript.skeletons[3].preparingLoseScreen == true)
        {
            GameManager.instance.skeletonScript.skeletons[3].preparingLoseScreen = false;
        }

        if (preparingWinScreen == true)
        {
            preparingWinScreen = false;
        }

        if (player.attacking == true)
        {
            player.attacking = false;
        }

        combatText.text = "";

        deactivateConfirmButton();


    }

    //public void get

    private bool checkThorgrenTiles( Vector2 targetTilePos)
    {
        if( targetTilePos == GameManager.instance.thorgrenScript.thor.topLeftPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.thorgrenScript.thor.topRightPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.thorgrenScript.thor.bottomLeftPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.thorgrenScript.thor.bottomRightPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkWraithTiles(Vector2 targetTilePos)
    {
        if (targetTilePos == GameManager.instance.wraithScript.wraith.topLeftPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.wraithScript.wraith.topRightPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.wraithScript.wraith.bottomLeftPos)
        {
            return true;
        }
        else if (targetTilePos == GameManager.instance.wraithScript.wraith.bottomRightPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private string checkSkeletonTiles(Vector2 targetTilePos)
    {
        List<Skeleton> skeletons = GameManager.instance.skeletonScript.skeletons;
        string skeleton = "";
        if (targetTilePos == skeletons[0].gridPos && skeletons[0].dead == false)
        {
            skeleton = "skeleton1";
        }
        if (targetTilePos == skeletons[1].gridPos && skeletons[1].dead == false)
        {
            skeleton = "skeleton2";
        }
        if (targetTilePos == skeletons[2].gridPos && skeletons[2].dead == false)
        {
            skeleton = "skeleton3";
        }
        if (targetTilePos == skeletons[3].gridPos && skeletons[3].dead == false)
        {
            skeleton = "skeleton4";
        }
        
        return skeleton;
    }

    private List<string> getValidTargets( Vector2 targetTilePos )
    {
        if (GameManager.instance.level.Equals("Wraith Battle"))
        {
            Wraith wraith = GameManager.instance.wraithScript.wraith;
            List<Skeleton> skeletons = GameManager.instance.skeletons;

            if (targetTilePos == wraith.frontRight.position && skeletons[2].dead == true)
            {
                return wraith.frontRight.targets;
            }
            else if (targetTilePos == wraith.frontLeft.position && skeletons[3].dead == true)
            {
                return wraith.frontLeft.targets;
            }
            else if (targetTilePos == wraith.backRight.position && skeletons[0].dead == true)
            {
                return wraith.backRight.targets;
            }
            else if (targetTilePos == wraith.backLeft.position && skeletons[1].dead == true)
            {
                return wraith.backLeft.targets;
            }
            else
            {
                List<string> empty = new List<string>(0);
                return empty;
            }
        }
        else
        {
            Thorgren thor = GameManager.instance.thorgrenScript.thor;
            if (targetTilePos == thor.frontRight.position)
            {
                return thor.frontRight.targets;
            }
            else if (targetTilePos == thor.frontLeft.position)
            {
                return thor.frontLeft.targets;
            }
            else if (targetTilePos == thor.backRight.position)
            {
                return thor.backRight.targets;
            }
            else if (targetTilePos == thor.backLeft.position)
            {
                return thor.backLeft.targets;
            }
            else
            {
                List<string> empty = new List<string>(0);
                return empty;
            }
        }
    }

    private void activateThorgrenTargetButton( string target)
    {
        if (target.Equals("head"))
        {
            GameObject.FindGameObjectWithTag("Head Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("chest"))
        {
            GameObject.FindGameObjectWithTag("Chest Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("back"))
        {
            GameObject.FindGameObjectWithTag("Back Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("parasite"))
        {
            GameObject.FindGameObjectWithTag("Parasite Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("rightArm"))
        {
            GameObject.FindGameObjectWithTag("Right Arm Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("leftArm"))
        {
            GameObject.FindGameObjectWithTag("Left Arm Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("rightLeg"))
        {
            GameObject.FindGameObjectWithTag("Right Leg Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("leftLeg"))
        {
            GameObject.FindGameObjectWithTag("Left Leg Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    private void activateWraithTargetButton(string target)
    {
        if (target.Equals("blueOrb"))
        {
            GameObject.FindGameObjectWithTag("Blue Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("redOrb"))
        {
            GameObject.FindGameObjectWithTag("Red Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("greenOrb"))
        {
            GameObject.FindGameObjectWithTag("Green Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("yellowOrb"))
        {
            GameObject.FindGameObjectWithTag("Yellow Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("skeleton1"))
        {
            GameObject.FindGameObjectWithTag("Skeleton 1 Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("skeleton2"))
        {
            GameObject.FindGameObjectWithTag("Skeleton 2 Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("skeleton3"))
        {
            GameObject.FindGameObjectWithTag("Skeleton 3 Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else if (target.Equals("skeleton4"))
        {
            GameObject.FindGameObjectWithTag("Skeleton 4 Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public static void deactivateTargetButtons()
    {
        if (GameManager.instance.level.Equals("Wraith Battle"))
        {
            GameObject.FindGameObjectWithTag("Blue Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Red Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Green Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Yellow Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Skeleton 1 Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Skeleton 2 Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Skeleton 3 Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Skeleton 4 Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Head Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Chest Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Right Arm Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Left Arm Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Right Leg Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.FindGameObjectWithTag("Left Leg Button").GetComponent<UnityEngine.UI.Button>().interactable = false;

            if (GameManager.instance.level.Equals("Thorgren Battle"))
            {
                GameObject.FindGameObjectWithTag("Back Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
            else if (GameManager.instance.level.Equals("Afflicted Thorgren Battle"))
            {
                GameObject.FindGameObjectWithTag("Parasite Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }
    }

    /*public static void deactivateWraithTargetButtons()
    {
        GameObject.FindGameObjectWithTag("Blue Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.FindGameObjectWithTag("Red Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.FindGameObjectWithTag("Green Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.FindGameObjectWithTag("Yellow Orb Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }*/

    public static void activateMoveButton()
    {
        GameObject.FindGameObjectWithTag("Move Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    public static void activateAttackButton()
    {
        GameObject.FindGameObjectWithTag("Attack Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    public static void activateEndTurnButton()
    {
        GameObject.FindGameObjectWithTag("End Turn Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    public static void activateConfirmButton()
    {
        GameObject.FindGameObjectWithTag("Confirm Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    public void activateLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void activateBlueOrbX()
    {
        blueOrbX.SetActive(true);
    }
    public void activateRedOrbX()
    {
        redOrbX.SetActive(true);
    }
    public void activateGreenOrbX()
    {
        greenOrbX.SetActive(true);
    }
    public void activateYellowOrbX()
    {
        yellowOrbX.SetActive(true);
    }

    public static void deactivateMoveButton()
    {
        GameObject.FindGameObjectWithTag("Move Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    public static void deactivateAttackButton()
    {
        GameObject.FindGameObjectWithTag("Attack Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    public static void deactivateEndTurnButton()
    {
        GameObject.FindGameObjectWithTag("End Turn Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    public static void deactivateConfirmButton()
    {
        GameObject.FindGameObjectWithTag("Confirm Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    public void deactivateBlueOrbX()
    {
        blueOrbX.SetActive(false);
    }
    public void deactivateRedOrbX()
    {
        redOrbX.SetActive(false);
    }
    public void deactivateGreenOrbX()
    {
        greenOrbX.SetActive(false);
    }
    public void deactivateYellowOrbX()
    {
        yellowOrbX.SetActive(false);
    }

    private int generateWeaponDamage(string weaponType )
    {
        if( weaponType.Equals("dagger") )
        {
            return Random.Range(2, 6);
        }
        else if (weaponType.Equals("mace"))
        {
            return Random.Range(2, 8);
        }
        else if (weaponType.Equals("rapier"))
        {
            return Random.Range(2, 8);
        }
        else if (weaponType.Equals("sword"))
        {
            return Random.Range(4, 12);
        }
        else
        {
            return 0;
        }
    }

    private void checkEnemyHPValues()
    {
        if (GameManager.instance.level.Equals("Wraith Battle"))
        {
            Wraith wraith = GameManager.instance.wraithScript.wraith;
            List<Skeleton> skeletons = GameManager.instance.skeletons;
            if (wraith.blueOrbHP < 0)
            {
                wraith.blueOrbHP = 0;
            }
            if (wraith.redOrbHP < 0)
            {
                wraith.redOrbHP = 0;
            }
            if (wraith.greenOrbHP < 0)
            {
                wraith.greenOrbHP = 0;
            }
            if (wraith.yellowOrbHP < 0)
            {
                wraith.yellowOrbHP = 0;
            }
            if(wraith.blueOrbHP == 0 && wraith.redOrbHP == 0 && wraith.greenOrbHP == 0 && wraith.yellowOrbHP == 0)
            {
                wraith.dead = true;
            }

            if(skeletons[0].healthPoints <= 0)
            {
                skeletons[0].healthPoints = 0;
                skeletons[0].dead = true;
                skeletons[0].skeletonAnimator.SetTrigger("die");
                deactivateBlueOrbX();
            }
            if (skeletons[1].healthPoints <= 0)
            {
                skeletons[1].healthPoints = 0;
                skeletons[1].dead = true;
                deactivateRedOrbX();
            }
            if (skeletons[2].healthPoints <= 0)
            {
                skeletons[2].healthPoints = 0;
                skeletons[2].dead = true;
                deactivateGreenOrbX();
            }
            if (skeletons[3].healthPoints <= 0)
            {
                skeletons[3].healthPoints = 0;
                skeletons[3].dead = true;
                deactivateYellowOrbX();
            }
        }
        else
        {
            Thorgren thor = GameManager.instance.thorgrenScript.thor;

            if (thor.healthPoints <= 0)
            {
                thor.healthPoints = 0;
                thor.dead = true;
            }
            if (thor.headHP < 0)
            {
                thor.headHP = 0;
            }
            if (thor.chestHP < 0)
            {
                thor.headHP = 0;
            }
            if (thor.backHP < 0)
            {
                thor.backHP = 0;
            }
            if (thor.parasiteHP < 0)
            {
                thor.parasiteHP = 0;
            }
            if (thor.rightArmHP < 0)
            {
                thor.rightArmHP = 0;
            }
            if (thor.leftArmHP < 0)
            {
                thor.leftArmHP = 0;
            }
            if (thor.rightLegHP < 0)
            {
                thor.rightLegHP = 0;
            }
            if (thor.leftLegHP < 0)
            {
                thor.leftLegHP = 0;
            }
        }
    }
}
