using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectionUIManager : MonoBehaviour
{
    [SerializeField]
    Text MaxUnitText;
    [SerializeField]
    Text RocketThiefNumberText;
    [SerializeField]
    Text RocketGruntNumberText;
    [SerializeField]
    Text RocketTamerNumberText;
    [SerializeField]
    Text RocketSquadNumberText;
    [SerializeField]
    GameObject unitSelectCanvas;

    public int MaxUnitsToTake;
    public int UnitsTaken;
    public int RocketThiefsRecruited;
    public int RocketGruntsRecruited;
    public int RocketTamersRecruited;
    public int RocketSquadsRecruited;


    public void ThiefAddButtonPressed()
    {
        if(UnitsTaken < MaxUnitsToTake)
        {
            UnitsTaken++;
            RocketThiefsRecruited++;
            RocketThiefNumberText.text = RocketThiefsRecruited.ToString();
        }
    }

    public void ThiefRemoveButtonPressed()
    {
        if(RocketThiefsRecruited > 0)
        {
            UnitsTaken--;
            RocketThiefsRecruited--;
            RocketThiefNumberText.text = RocketThiefsRecruited.ToString();
        }
    }

    public void GruntAddButtonPressed()
    {
        if (UnitsTaken < MaxUnitsToTake)
        {
            UnitsTaken++;
            RocketGruntsRecruited++;
            RocketGruntNumberText.text = RocketGruntsRecruited.ToString();
        }
    }
    public void GruntRemoveButtonPressed()
    {
        if (RocketGruntsRecruited > 0)
        {
            UnitsTaken--;
            RocketGruntsRecruited--;
            RocketGruntNumberText.text = RocketGruntsRecruited.ToString();
        }
    }

    public void TamerAddButtonPressed()
    {
        if (UnitsTaken < MaxUnitsToTake)
        {
            UnitsTaken++;
            RocketTamersRecruited++;
            RocketTamerNumberText.text = RocketTamersRecruited.ToString();
        }
    }
    public void TamerRemoveButtonPressed()
    {
        if (RocketTamersRecruited > 0)
        {
            UnitsTaken--;
            RocketTamersRecruited--;
            RocketTamerNumberText.text = RocketTamersRecruited.ToString();
        }
    }

    public void SquadAddButtonPressed()
    {
        if (UnitsTaken < MaxUnitsToTake)
        {
            UnitsTaken++;
            RocketSquadsRecruited++;
            RocketSquadNumberText.text = RocketSquadsRecruited.ToString();
        }
    }
    public void SquadRemoveButtonPressed()
    {
        if (RocketSquadsRecruited > 0)
        {
            UnitsTaken--;
            RocketSquadsRecruited--;
            RocketSquadNumberText.text = RocketSquadsRecruited.ToString();
        }
    }

    public void StartBattleButtonPressed()
    {
        GameObject map = GameObject.Find("Map");
        TileMap tileMapScript = map.GetComponent<TileMap>();
		GameObject gameManager = GameObject.Find ("Game Manager");
		GameManager gameManagerScript = gameManager.GetComponent<GameManager> ();

		for (int i = 0; i < RocketThiefsRecruited; i++)
        {
            tileMapScript.SpawnRocketThiefUnits();
        }
		for (int i = 0; i < RocketGruntsRecruited; i++)
		{
			tileMapScript.SpawnRocketGruntUnits();
		}
		for (int i = 0; i < RocketTamersRecruited; i++)
		{
			tileMapScript.SpawnRocketTamerUnits();
		}
		for (int i = 0; i < RocketSquadsRecruited; i++)
		{
			tileMapScript.SpawnRocketSquadUnits();
		}

		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Rocket Thief"))
		{
			gameManagerScript.unitList.Add(unit);
			gameManagerScript.rocketThiefList.Add (unit);
		}
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Rocket Grunt"))
		{
			gameManagerScript.unitList.Add(unit);
			gameManagerScript.rocketGruntList.Add (unit);
		}
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Rocket Tamer"))
		{
			gameManagerScript.unitList.Add(unit);
			gameManagerScript.rocketTamerList.Add (unit);
		}
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Rocket Squad"))
		{
			gameManagerScript.unitList.Add(unit);
			gameManagerScript.rocketSquadList.Add (unit);
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			gameManagerScript.enemyList.Add(enemy);
		}

		tileMapScript.SelectUnit ();

        unitSelectCanvas.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        MaxUnitText.text = "Max Squad Size: " + MaxUnitsToTake;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
