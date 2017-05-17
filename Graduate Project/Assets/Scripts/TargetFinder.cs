using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour {

    public Player target;
    public Vector2 targetTile;

    public TargetFinder(string monsterType, string facing )
    {
        if(monsterType.Equals("Thorgren") || monsterType.Equals("Wraith") )
        {
            target = findValidTarget(monsterType, facing);
            targetTile = findTargetTile(monsterType);
        }
        else
        {
            target = skeletonFindValidTarget(monsterType, facing);
        }
    }

    private Vector2 findTargetTile( string monsterType)
    {
        float targetX = target.gridPos.x;
        float targetY = target.gridPos.y;

        Vector2 topLeftPos = new Vector2();
        Vector2 topRightPos = new Vector2();
        Vector2 bottomLeftPos = new Vector2();
        Vector2 bottomRightPos = new Vector2();

        if (monsterType.Equals("Thorgren"))
        {
        topLeftPos = GameManager.instance.thorgrenScript.thor.topLeftPos;
        topRightPos = GameManager.instance.thorgrenScript.thor.topRightPos;
        bottomLeftPos = GameManager.instance.thorgrenScript.thor.bottomLeftPos;
        bottomRightPos = GameManager.instance.thorgrenScript.thor.bottomRightPos;
        }
        else if(monsterType.Equals("Wraith"))
        {
            topLeftPos = GameManager.instance.wraithScript.wraith.topLeftPos;
            topRightPos = GameManager.instance.wraithScript.wraith.topRightPos;
            bottomLeftPos = GameManager.instance.wraithScript.wraith.bottomLeftPos;
            bottomRightPos = GameManager.instance.wraithScript.wraith.bottomRightPos;
       }

        if ( targetX <= topLeftPos.x && targetY >= topLeftPos.y)
        {
            targetTile = topLeftPos;
        }
        else if (targetX >= topRightPos.x && targetY >= topRightPos.y)
        {
            targetTile = topRightPos;
        }
        else if (targetX <= bottomLeftPos.x && targetY <= bottomLeftPos.y)
        {
            targetTile = bottomLeftPos;
        }
        else if (targetX >= bottomRightPos.x && targetY <= bottomRightPos.y)
        {
            targetTile = bottomRightPos;
        }
        return targetTile;
    }
    
    private Player findValidTarget(string monsterType, string facing )
    {
        List<Player> List = new List<Player>();
        List<Player> players = GameManager.instance.playerScript.players;
        
        Player targ = new Player();

       if (monsterType.Equals("Thorgren"))
       {
            Thorgren thor = GameManager.instance.thorgrenScript.thor;
            if (facing.Equals("down"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.y < thor.bottomLeftPos.y && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("right"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.x > thor.bottomRightPos.x && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("left"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.x < thor.bottomLeftPos.x && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("up"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.y > thor.topLeftPos.y && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
       }
       
        else if(monsterType.Equals("Wraith"))
        {
            Wraith wraith = GameManager.instance.wraithScript.wraith;
            if (facing.Equals("down"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.y < wraith.bottomLeftPos.y && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("right"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.x > wraith.bottomRightPos.x && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("left"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.x < wraith.bottomLeftPos.x && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
            else if (facing.Equals("up"))
            {
                foreach (Player p in players)
                {
                    if (p.gridPos.y > wraith.topLeftPos.y && p.dead == false)
                    {
                        List.Add(p);
                    }
                }
            }
        }

        if (List.Count == 1)
        {
            targ = List[0];
        }
        else if (List.Count > 1)
        {
            targ = findClosestTarget(monsterType, List);
        }
        else
        {
            targ = findClosestTarget(monsterType, players);
        }

        return targ;
    }

    private Player skeletonFindValidTarget(string monsterType, string facing)
    {
        List<Player> List = new List<Player>();
        List<Player> players = GameManager.instance.playerScript.players;

        Player targ = new Player();
        Skeleton skeleton = new Skeleton();
        List<Skeleton> skeletons = GameManager.instance.skeletons;

        if( monsterType.Equals(skeletons[0].skelName) )
        {
            skeleton = skeletons[0];
        }
        else if (monsterType.Equals(skeletons[1].skelName))
        {
            skeleton = skeletons[1];
        }
        else if(monsterType.Equals(skeletons[2].skelName))
        {
            skeleton = skeletons[2];
        }
        else if (monsterType.Equals(skeletons[3].skelName))
        {
            skeleton = skeletons[3];
        }
        
        if (facing.Equals("down"))
        {
            foreach (Player p in players)
            {
                if (p.gridPos.y < skeleton.gridPos.y && p.dead == false)
                {
                    List.Add(p);
                }
            }
        }
        else if (facing.Equals("right"))
        {
            foreach (Player p in players)
            {
                if (p.gridPos.x > skeleton.gridPos.x && p.dead == false)
                {
                    List.Add(p);
                }
            }
        }
        else if (facing.Equals("left"))
        {
            foreach (Player p in players)
            {
                if (p.gridPos.x < skeleton.gridPos.x && p.dead == false)
                {
                    List.Add(p);
                }
            }
        }
        else if (facing.Equals("up"))
        {
            foreach (Player p in players)
            {
                if (p.gridPos.y > skeleton.gridPos.y && p.dead == false)
                {
                    List.Add(p);
                }
            }
        }

        if (List.Count == 1)
        {
            targ = List[0];
        }
        else if (List.Count > 1)
        {
            targ = skeletonFindClosestTarget(monsterType, List);
        }
        else
        {
            targ = skeletonFindClosestTarget(monsterType, players);
        }

        return targ;

    }

    private Player findClosestTarget(string monsterType, List<Player> validTargets )
    {
        float leastDist = 0;
        Player closestPlayer = new Player();

        float enemyX = 0;
        float enemyY = 0;
        if (monsterType.Equals("Thorgren"))
        {
            enemyX = GameManager.instance.thorgrenScript.thor.transform.position.x;
            enemyY = GameManager.instance.thorgrenScript.thor.transform.position.y;
        }
        if (monsterType.Equals("Wraith"))
        {
            enemyX = GameManager.instance.wraithScript.wraith.transform.position.x;
            enemyY = GameManager.instance.wraithScript.wraith.transform.position.y;
        }

        foreach ( Player p in validTargets )
        {
            float playerX = p.transform.position.x;
            float playerY = p.transform.position.y;
            float playerDist =( Mathf.Abs(enemyX - playerX) + Mathf.Abs(enemyY - playerY) );

            if ( leastDist == 0 && p.dead == false)
            {
                leastDist = playerDist;
                closestPlayer = p;
            }
            else if( leastDist > playerDist && p.dead == false)
            {
                leastDist = playerDist;
                closestPlayer = p;
            }
            else if( leastDist == playerDist && p.dead == false )
            {
                if( p.healthPoints < closestPlayer.healthPoints )
                {
                    closestPlayer = p;
                }
            }
        }
        return closestPlayer;
    }

    private Player skeletonFindClosestTarget(string monsterType, List<Player> validTargets)
    {
        float leastDist = 0;
        Player closestPlayer = new Player();
        Skeleton skeleton = new Skeleton();
        List<Skeleton> skeletons = GameManager.instance.skeletons;

        if (monsterType.Equals(skeletons[0].skelName))
        {
            skeleton = skeletons[0];
        }
        else if (monsterType.Equals(skeletons[1].skelName))
        {
            skeleton = skeletons[1];
        }
        if (monsterType.Equals(skeletons[2].skelName))
        {
            skeleton = skeletons[2];
        }
        if (monsterType.Equals(skeletons[3].skelName))
        {
            skeleton = skeletons[3];
        }

        float enemyX = skeleton.gridPos.x;
        float enemyY = skeleton.gridPos.y;

        foreach (Player p in validTargets)
        {
            float playerX = p.transform.position.x;
            float playerY = p.transform.position.y;
            float playerDist = (Mathf.Abs(enemyX - playerX) + Mathf.Abs(enemyY - playerY));

            if (leastDist == 0 && p.dead == false)
            {
                leastDist = playerDist;
                closestPlayer = p;
            }
            else if (leastDist > playerDist && p.dead == false)
            {
                leastDist = playerDist;
                closestPlayer = p;
            }
            else if (leastDist == playerDist && p.dead == false)
            {
                if (p.healthPoints < closestPlayer.healthPoints)
                {
                    closestPlayer = p;
                }
            }
        }
        return closestPlayer;
    }
        // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
