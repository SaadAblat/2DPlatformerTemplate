using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData SaveInstance;


	// Game Variable
	public int totalCoinCount;


	//CurrentLevel Variable
	public int coinCount;

	//CheckPointVariable
	public bool firstCheckPointActivated;
	public Vector2 LastCheckPointPosition;
	public int LastCheckPointCoin;
	public int LastCheckPointPistolAmmo;
	public int LastCheckPointShotgunAmmo;
	public List<int> CheckPointIDActivated = new List<int>();

	//weapons
	internal bool HavePistol;
	internal bool HaveShotgun;

    private void Update()
    {
    }

    void Awake()
	{
		if (SaveInstance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			SaveInstance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	public void ResetLevelData()
    {
		firstCheckPointActivated = false;
		HavePistol = false;
		HaveShotgun = false;
		LastCheckPointPistolAmmo = 0;
		LastCheckPointShotgunAmmo = 0;
		LastCheckPointCoin = 0;
		CheckPointIDActivated = new List<int>();
	}
}


