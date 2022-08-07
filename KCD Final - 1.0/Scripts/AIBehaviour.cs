//Tien-Yi Lee (thanks to Unity's tutorial https://learn.unity.com/project/behaviour-trees?uv=2019.4&courseId=5dd851beedbc2a1bf7b72bed)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    public static AIBehaviour _instance;

    BehaviourTree tree;

    Node.Status treeStatus = Node.Status.RUNNING;

    //troopsdifference = Ai troops - enemy troops

    private static int pirateNum;
    private static int castleNum;
    
    private int troopsDiff = pirateNum - castleNum;

    //current Coin
    private static int currentCoin;

    private float TimeToPKingSpawn = 70;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Setup()
    {
        CharacterManager._instance.ForceSpawn("P2"); //spawn lvl 2 pirate at the start of the game
        CharacterManager._instance.ForceSpawn("P3"); //spawn lvl 3 pirate at the start of the game
        pirateNum = CharacterManager._instance.PirateArmyComp.ArmyList.Count; //AI troops
        castleNum = CharacterManager._instance.CastleArmyComp.ArmyList.Count; //player troops
        currentCoin = CoinManager._instance.Coins[1]; //Set up current coin
        StartCoroutine(SpawnPKing()); //start the 60 second for spawning pirate king
    }

    //p1 = lvl 1 pirate, p2 = lvl 2 pirate, p3 = lvl 3 pirate, p4 = lvl 4 pirate, p5 = lvl 5 pirate
    // Start is called before the first frame update
    void Start()
    {
        tree = new BehaviourTree();
        Sequence click = new Sequence("click Something");

        //if AI has X more or X less troops than the human player, and then do something depnds on troopsDiff
        //For example,
        //When Ai has oneMore troops than the human player, spawn p4 if possible.
        Leaf oneMore = new Leaf("One More", OneMore); //p4
        Leaf twoMore = new Leaf("Two More", TwoMore); //p3
        Leaf aLotMore = new Leaf("A Lot More", ALotMore); //p5
        Leaf oneLess = new Leaf("One Less", OneLess); //p3
        Leaf twoLess = new Leaf("Two Less", TwoLess); //p4
        Leaf aLotLess = new Leaf("A Lot Less", ALotLess); //p5
        Leaf same = new Leaf("Same", Same); //p3

        click.AddChild(oneMore);
        click.AddChild(twoMore);
        click.AddChild(aLotMore);
        click.AddChild(oneLess);
        click.AddChild(twoLess);
        click.AddChild(aLotLess);
        click.AddChild(same);

        tree.AddChild(click);

        tree.PrintTree();
    }

    public Node.Status OneMore()
    {
        if (troopsDiff == 1)
        {
            if (currentCoin >= 40)
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
            else if (currentCoin >= 25 && currentCoin < 40)
            {
                CharacterManager._instance.SpawnCharacter("P4", 2); //spawn p4
            }
        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status TwoMore()
    {
        if (troopsDiff == 2)
        {
            if (currentCoin >= 40)
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
            else if (currentCoin >= 25 && currentCoin < 40)
            {
                CharacterManager._instance.SpawnCharacter("P4", 2); //spawn p4
            }
            else if (currentCoin >= 15 && currentCoin < 25)
            {
                CharacterManager._instance.SpawnCharacter("P3", 2); //spawn p3
            }
        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status ALotMore()
    {
        if (troopsDiff > 2)
        {
            if (currentCoin >= 40)
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status OneLess()
    {
        if (troopsDiff == -1)
        {
            if (currentCoin >= 40)
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
            else if (currentCoin >= 25 && currentCoin < 40)
            {
                CharacterManager._instance.SpawnCharacter("P4", 2); //spawn p4
            }
            else if (currentCoin >= 15 && currentCoin < 25)
            {
                CharacterManager._instance.SpawnCharacter("P3", 2); //spawn p3
            }
        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status TwoLess()
    {
        if (troopsDiff == -2)
        {
            if (currentCoin >= 40)
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
            else if (currentCoin >= 25 && currentCoin < 40)
            {
                CharacterManager._instance.SpawnCharacter("P4", 2); //spawn p4
            }
        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status ALotLess()
    {
        if (troopsDiff < -2) 
        {
            if ( currentCoin >= 40 )
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }

        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status Same()
    {
        if (troopsDiff == 0)
        {
            if (currentCoin > 40 )
            {
                CharacterManager._instance.SpawnCharacter("P5", 2); //spawn p5
            }
            else if (currentCoin >= 25 && currentCoin < 40)
            {
                CharacterManager._instance.SpawnCharacter("P4", 2); //spawn p4
            }
            else if (currentCoin >= 15 && currentCoin < 25)
            {
                CharacterManager._instance.SpawnCharacter("P3", 2); //spawn p3
            }
        }
        else
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.SUCCESS;
    }

    public void CheckTroops() //Check current troops in game
    {
        pirateNum = CharacterManager._instance.PirateArmyComp.ArmyList.Count; //AI troops
        castleNum = CharacterManager._instance.CastleArmyComp.ArmyList.Count; //player troops
        troopsDiff = pirateNum - castleNum;
    }

    //coroutine { wait for time} CharacterManager._instance.SpawnCharacter("PKing", 2); //spawn PKing

    private IEnumerator SpawnPKing() //Spawn Pirate King after 70 seconds
    {
        yield return new WaitForSeconds(TimeToPKingSpawn);
        CharacterManager._instance.SpawnCharacter("PKing", 2); //spawn PKing
    }

    // Update is called once per frame
    void Update()
    {
        currentCoin = CoinManager._instance.Coins[1]; //update current coin
        CheckTroops(); //update troopDiff
        treeStatus = tree.Process(); //make sure the tree is working
    }

}