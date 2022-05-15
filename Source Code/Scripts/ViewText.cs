using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ViewText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 18;
    }

    void Update()
    {
        gameObject.transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 20 * Time.deltaTime);
    }
    static void VibrateHand(string objectName)
    {
        if (objectName == "RightHandTriggerCollider")
        {
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
        }
        else
        {
            GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        VibrateHand(other.transform.name);
        SetText("RPreview", 0, true);
        SetText("GPreview", 1, true);
        SetText("BPreview", 2, true);
        SetText("Preview", 3, true);
    }

    private void OnTriggerExit(Collider other)
    {
        //VibrateHand(other.transform.name);
        SetText("RPreview", 0, false);
        SetText("GPreview", 1, false);
        SetText("BPreview", 2, false);
        SetText("Preview", 3, false);
    }

    void SetText(string thename, int getChild, bool setEnable)
    {
        if (gameObject.name == thename)
        {
            gameObject.transform.parent.GetChild(4).GetChild(getChild).gameObject.GetComponent<Text>().enabled = setEnable; 
        }
    }

}
