using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MissionPanelController : MonoBehaviour {
	public Text kidDeliveryCoins;
	public Text kidDeliveryMedals;
	public Text drugDeliveryCoins;
	public Text drugDeliveryMedals;
	public Text kidPickupCoins;
	public Text kidPickupMedals;

	void Start() {

		int targetItemCountKD = GameController.Instance.missions [0].targetItemCount;
		int itemValueKD = GameController.Instance.missions [0].item.value;
		kidDeliveryCoins.text = (targetItemCountKD * itemValueKD).ToString ();
		kidDeliveryMedals.text = GameController.Instance.missions [0].maxMedalEarned.ToString ();

		int targetItemCountKP = GameController.Instance.missions [1].targetItemCount;
		int itemValueKP = GameController.Instance.missions [1].item.value;
		kidPickupCoins.text = (targetItemCountKP * itemValueKP).ToString ();
		kidPickupMedals.text = GameController.Instance.missions [1].maxMedalEarned.ToString ();

		int targetItemCountDD = GameController.Instance.missions [2].targetItemCount;
		int itemValueDD = GameController.Instance.missions [2].item.value;
		drugDeliveryCoins.text = (targetItemCountDD * itemValueDD).ToString ();
		drugDeliveryMedals.text = GameController.Instance.missions [2].maxMedalEarned.ToString ();




	}	
}
