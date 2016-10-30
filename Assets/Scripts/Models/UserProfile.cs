using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class UserProfile {
	public Spaceship spaceship {set; get;}
	public int coins {set; get;}
	public int medals {set; get;}
	public int clues {set; get;}
	public int bonusMission {set; get;}
	public List<Gun> guns = new List<Gun>();

	public Gun secondaryGun { get; set; }

	public bool isSoundEnabled { set; get; }
	public bool isAccelerometerEnabled { get; set;}

    public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/userProfile.gd");
		bf.Serialize(file,  GameController.Instance.profile);
		file.Close();
	}

	public static void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/userProfile.gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/userProfile.gd", FileMode.Open);
			GameController.Instance.profile = (UserProfile)bf.Deserialize (file);
			file.Close ();
		} else {
			GameController.Instance.profile = DataGenerator.PopulateUserProfile ();
			UserProfile.Save ();
		}
	}
}
