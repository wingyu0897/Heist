using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int money;
    public int clearStageCount;
	public float effectVolume;
	public float bgmVolume;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
	private string path;

	public SaveData nowData;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("DataManager 다중 실행 중");
			Destroy(this);
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
		DontDestroyOnLoad(Instance.gameObject);

		path = Application.persistentDataPath + "/saves";
		print($"Data Path : {path}");
	}

	public void LoadData()
	{
		nowData = new SaveData();

		if (!File.Exists(path))
		{
			PlayerData.Instance.Money = 100000;
			PlayerData.Instance.clearStageCount = 0;
			SoundController.bgmVolume = 1;
			SoundController.effectVolume = 1;
		}
		else
		{
			string loadJson = File.ReadAllText(path);
			nowData = JsonUtility.FromJson<SaveData>(loadJson);

			PlayerData.Instance.Money = nowData.money;
			PlayerData.Instance.clearStageCount = nowData.clearStageCount;
			SoundController.bgmVolume = nowData.bgmVolume;
			SoundController.effectVolume = nowData.effectVolume;

			print(loadJson);
		}
	}

	public void SaveData()
	{
		nowData = new SaveData();

		nowData.money = PlayerData.Instance.Money;
		nowData.clearStageCount = PlayerData.Instance.clearStageCount;
		nowData.effectVolume = SoundController.effectVolume;
		nowData.bgmVolume = SoundController.bgmVolume;

		string json = JsonUtility.ToJson(nowData, true);
		File.WriteAllText(path, json);
		print(json);
	}
}
