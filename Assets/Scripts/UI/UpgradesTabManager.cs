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
        gunsPanel.SetActive(true);
        ammoPanel.SetActive(false);
    }

    public void showAmmoPanel()
    {
        spaceshipUpgradePanel.SetActive(false);
        gunsPanel.SetActive(false);
        ammoPanel.SetActive(true);
    }





}
