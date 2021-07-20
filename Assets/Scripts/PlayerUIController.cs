using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [Header("Components")]
    private TMP_Text ammunitionTMP;

    // Start is called before the first frame update
    void Start()
    {
        ammunitionTMP = transform.Find("AmmunitionTMP").GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmunitionText(int currentAmmunition)
    {
        ammunitionTMP.text = currentAmmunition.ToString() + "/1";
    }
}
