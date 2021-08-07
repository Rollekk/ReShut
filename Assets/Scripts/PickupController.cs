using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    protected TMP_Text pickupTMP;
    protected float initialTextHeight;

    protected virtual void Start()
    {
        pickupTMP =  transform.parent.GetComponentInChildren<TMP_Text>();
        initialTextHeight = pickupTMP.transform.localPosition.y;
        pickupTMP.enabled = false;
    }

    public virtual void PickupInteraction(PlayerController playerController)
    {

    }

    public void ShowPickupText(bool shouldShow)
    {
       pickupTMP.enabled = shouldShow;
    }

    public void UpdatePickupText(KeyCode pickupKeybind, string pickupName)
    {
        pickupTMP.text = "[" + pickupKeybind.ToString() + "]" + pickupName;
    }
}
