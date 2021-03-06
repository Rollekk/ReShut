using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [Header("Components")]
    private TMP_Text ammunitionTMP;
    private TMP_Text pickupTMP;

    // Start is called before the first frame update
    void Start()
    {
        ammunitionTMP = transform.Find("AmmunitionTMP").GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAmmunitionText(bool shouldShow)
    {
        ammunitionTMP.enabled = shouldShow;
    }

    public void UpdateAmmunitionText(int currentAmmunition, int maxAmmunition)
    {
        ammunitionTMP.text = currentAmmunition.ToString() + "/" + maxAmmunition.ToString();
    }
}
