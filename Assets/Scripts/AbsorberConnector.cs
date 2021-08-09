using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberConnector : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbsorberController absorber;
    [SerializeField] private GameObject[] devicesToActivate;

    [Header("Delay")]
    [SerializeField] private bool repeatDelay;
    [SerializeField] private float delayRepeatTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (absorber) StartCoroutine(TurnOnDevices(false, false));
    }

    // Update is called once per frame
    void Update()
    {
        if (absorber.isActive)
        {
            if (!devicesToActivate[0].activeInHierarchy) StartCoroutine(TurnOnDevices(true, repeatDelay));
        }
        else
        {
            if (devicesToActivate[0].activeInHierarchy) StartCoroutine(TurnOnDevices(false, repeatDelay));
        }
    }

    private IEnumerator TurnOnDevices(bool turnOn, bool rDelay)
    {
        foreach (GameObject device in devicesToActivate)
        {
            device.SetActive(turnOn);
            if (rDelay) yield return new WaitForSeconds(delayRepeatTime);
        }
    }
}
