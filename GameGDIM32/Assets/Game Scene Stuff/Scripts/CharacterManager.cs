using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //Since Unity doesn't serialize dictionaries from what I can see, this is a sort of work around though a bit rough
    [SerializeField]
    private List<string> CharacterName;
    [SerializeField]
    private List<GameObject> CharacterPrefab;

    [SerializeField]
    private GameObject CastleArmy;
    [SerializeField]
    private GameObject PirateArmy;

    [SerializeField]
    private float MaxSpawnY;
    [SerializeField]
    private float MinSpawnY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCharacter(string characterName)
    {
        int index = CharacterName.IndexOf(characterName);
        if (index != -1)
        {
            //creates variation in spawn location
            float randomY = Random.Range(MinSpawnY, MaxSpawnY);
            //for spawning a castle character, 6 represents king character/final castle character as seen in unity editor inspector list
            if (index < 6)
            {
                Vector3 spawnPos = new Vector3(CastleArmy.transform.position.x, randomY, CastleArmy.transform.position.z);
                GameObject newCharacter = Instantiate(CharacterPrefab[index], CastleArmy.transform);
                newCharacter.transform.position = spawnPos;
                if (index == 5)
                {
                    KingCharacterDecorator KCD = new KingCharacterDecorator(newCharacter.GetComponent<Character>());
                }
                else
                {
                    CastleArmy.GetComponent<Army>().Add(newCharacter.GetComponent<Character>());
                }
            }
            //for spawning a pirate character, 12 represents king character/final pirate character as seen in unity editor inspector list
            else
            {
                Vector3 spawnPos = new Vector3(PirateArmy.transform.position.x, randomY, PirateArmy.transform.position.z);
                GameObject newCharacter = Instantiate(CharacterPrefab[index], PirateArmy.transform);
                newCharacter.transform.position = spawnPos;
                PirateArmy.GetComponent<Army>().Add(newCharacter.GetComponent<Character>());
            }
        }
    }
}
