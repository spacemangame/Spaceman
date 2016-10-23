using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class UpgradesTabManager : MonoBehaviour {

	public GameObject spaceshipUpgradePanel;
    public GameObject gunsPanel;
    public GameObject ammoPanel;	

	public UserProfile profile { get; set; }

	void Start() {
        spaceshipUpgradePanel.SetActive(true);
	}

	public void showSpaceshipPanel() {
        spaceshipUpgradePanel.SetActive (true);
        gunsPanel.SetActive(false);
        ammoPanel.SetActive(false);
	}

    public void showGunsPanel()
    {
        spaceshipUpgradePanel.SetActive(false);
		ammoPanel.SetActive(false);
		gunsPanel.SetActive(true);

		GunBuyController controller = gunsPanel.gameObject.GetComponentInChildren<GunBuyController> ();
		controller.Render ();
    }

    public void showAmmoPanel()
    {
        spaceshipUpgradePanel.SetActive(false);
        gunsPanel.SetActive(false);
        ammoPanel.SetActive(true);

		AmmoBuyController controller = ammoPanel.gameObject.GetComponentInChildren<AmmoBuyController> ();
		controller.Render ();
    }





}
