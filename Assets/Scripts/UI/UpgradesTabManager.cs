using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UpgradesTabManager : MonoBehaviour {

	public GameObject spaceshipUpgradePanel;
    public GameObject gunsPanel;
    public GameObject ammoPanel;

	public Button spaceshipButton;
	public Button gunsButton;
	public Button ammoButton;

	public UserProfile profile { get; set; }

	void Start() {
		showSpaceshipPanel ();
	}

	public void showSpaceshipPanel() {
        spaceshipUpgradePanel.SetActive (true);
        gunsPanel.SetActive(false);
        ammoPanel.SetActive(false);
		disableAllExceptOne (spaceshipButton);

		SpaceshipUpgradeController controller = GameObject.Find("SpaceshipUpgradeController").GetComponentInParent<SpaceshipUpgradeController> ();
		controller.Render ();
	}

    public void showGunsPanel()
    {
        spaceshipUpgradePanel.SetActive(false);
		ammoPanel.SetActive(false);
		gunsPanel.SetActive(true);

		disableAllExceptOne (gunsButton);

		GunBuyController controller = gunsPanel.gameObject.GetComponentInChildren<GunBuyController> ();
		controller.Render ();
    }

    public void showAmmoPanel()
    {
        spaceshipUpgradePanel.SetActive(false);
        gunsPanel.SetActive(false);
        ammoPanel.SetActive(true);

		disableAllExceptOne (ammoButton);

		AmmoBuyController controller = ammoPanel.gameObject.GetComponentInChildren<AmmoBuyController> ();
		controller.Render ();
    }

	private void disableAllExceptOne(Button button) {
		List<Button> buttons = new List<Button> (){ spaceshipButton, gunsButton, ammoButton};
		foreach(Button b in buttons) {
			if (!b.Equals (button)) {
				b.interactable = true;
			}
		}
		button.interactable = false;
	}





}
