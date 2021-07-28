using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [Header("Components")]
    private TMP_Text ammunitionTMP;
    public Gun currentGun;

    // Start is called before the first frame update
    void Start()
    {
        ammunitionTMP = transform.Find("AmmunitionTMP").GetComponentInChildren<TMP_Text>();
        UpdateAmmunitionText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmunitionText()
    {
        ammunitionTMP.text = currentGun.currentAmmunition.ToString() + "/" + currentGun.maxAmmunition.ToString();
    }
}
