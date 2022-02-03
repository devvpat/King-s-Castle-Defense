//Class written by: Dev Patel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectManager : MonoBehaviour
{
    public static SpecialEffectManager _instance;

    //Two composite objects for the composite pattern based on ISpecialEffect
    private SpecialEffect_Group SE_Group_1;
    private SpecialEffect_Group SE_Group_2;

    //Five leaf objects for the composite pattern based on ISpecialEffect
    private SpecialEffect_TakeDamage SE_TD;
    private SpecialEffect_Resistance SE_RE;
    private SpecialEffect_AttackDamage SE_AD;
    private SpecialEffect_Range SE_RA;
    private SpecialEffect_SpeedMod SE_SM;

    //Values to initalize the leaf objects
    [SerializeField]
    private List<int> SE_TD_Vals;
    [SerializeField]
    private List<int> SE_RE_Vals;
    [SerializeField]
    private List<int> SE_AD_Vals;
    [SerializeField]
    private List<float> SE_RA_Vals;
    [SerializeField]
    private List<float> SE_SM_Vals;

    //references to the army composite for the TakeDamage special effect
    private Army CastleArmy;
    private Army PirateArmy;

    //how often special effects occur
    [SerializeField]
    private float Interval;

    //References to items for displaying the special effect on screen
    [SerializeField]
    private GameObject SpecialEffect_Canvas;
    [SerializeField]
    private TMPro.TextMeshProUGUI Text;
    [SerializeField]
    private float DisplayTime;

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

    // Start is called before the first frame update
    //Initializes the composites and leafs and finds appropirate references (to the armies), and starts the interval timer
    public void Setup()
    {
        CastleArmy = CharacterManager._instance.CastleArmyComp;
        PirateArmy = CharacterManager._instance.PirateArmyComp;

        SE_TD = new SpecialEffect_TakeDamage();
        SE_TD.minDamage = SE_TD_Vals[0];
        SE_TD.maxDamage = SE_TD_Vals[1];
        SE_TD.CastleArmy = CastleArmy;
        SE_TD.PirateArmy = PirateArmy;

        SE_RE = new SpecialEffect_Resistance();
        SE_RE.minResitanceChange = SE_RE_Vals[0];
        SE_RE.maxResistanceChange = SE_RE_Vals[1];

        SE_AD = new SpecialEffect_AttackDamage();
        SE_AD.minAttackDamageChange = SE_AD_Vals[0];
        SE_AD.maxAttackDamageChange = SE_AD_Vals[1];

        SE_RA = new SpecialEffect_Range();
        SE_RA.minAttackRangeChange = SE_RA_Vals[0];
        SE_RA.maxAttackRangeChange = SE_RA_Vals[1];

        SE_SM = new SpecialEffect_SpeedMod();
        SE_SM.minSpeedModChange = SE_SM_Vals[0];
        SE_SM.maxSpeedModChange = SE_SM_Vals[1];

        SE_Group_1 = new SpecialEffect_Group();
        SE_Group_2 = new SpecialEffect_Group();

        //group 1 will change attack damage and resistance
        SE_Group_1.Add(SE_AD);
        SE_Group_1.Add(SE_RE);

        //group 2 will change speed mod and attack range
        SE_Group_2.Add(SE_RA);
        SE_Group_2.Add(SE_SM);

        StartCoroutine(Timer(Interval));
    }

    private IEnumerator Timer(float interval)
    {
        yield return new WaitForSeconds(interval);
        ExecuteSpecialEffect();
        StartCoroutine(Timer(interval));
    }

    //Does one of three random special effect:
    //1) (r == 0) Deal damage to all alive characters (using army references)
    //2) (r == 1) Composite 1 = Change Attack Damage and change Resistance values
    //3) (r == 2) Composite 2 = Change Attack Range and change Speed Mod
    private void ExecuteSpecialEffect()
    {
        int r = Random.Range(0, 3);
        if (r == 0)
        {
            SE_TD.Effect(new Character());
        }
        else
        {
            foreach (ICharacter character in CastleArmy.ArmyList)
            {
                if (r == 1) SE_Group_1.Effect(character.GetCharacter());
                if (r == 2) SE_Group_2.Effect(character.GetCharacter());
            }
            foreach (ICharacter character in PirateArmy.ArmyList)
            {
                if (r == 1) SE_Group_1.Effect(character.GetCharacter());
                if (r == 2) SE_Group_2.Effect(character.GetCharacter());
            }
        }
        StartCoroutine(DisplaySpecialEffect(r));
    }

    //Displays the special effect on screen for the player for a specified amount of time
    private IEnumerator DisplaySpecialEffect(int r)
    {
        if (r == 0) Text.text = $"Special Effect:\nRandomly Dealing Damage ({SE_TD_Vals[0]} - {SE_TD_Vals[1]}) to ALL UNITS";
        if (r == 1) Text.text = $"Special Effect:\nRandomly Changing ALL UNITS' Attack Damage ({SE_AD_Vals[0]} - {SE_AD_Vals[1]}) and Resistance ({SE_RE_Vals[0]} - {SE_RE_Vals[1]})";
        if (r == 2) Text.text = $"Special Effect:\nRandomly Changing ALL UNITS' Speed ({SE_SM_Vals[0]} - {SE_SM_Vals[1]}) and Attack Range ({SE_RA_Vals[0]} - {SE_RA_Vals[1]})";
        SpecialEffect_Canvas.SetActive(true);
        yield return new WaitForSeconds(DisplayTime);
        SpecialEffect_Canvas.SetActive(false);
    }
}
