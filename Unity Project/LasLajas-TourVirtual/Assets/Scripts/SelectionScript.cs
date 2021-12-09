using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    public GameObject player;
    public GameObject menu;
    public GameObject lights;
    FirstPersonController script;
    TurnOnLights script2;

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<FirstPersonController>();
        script2 = lights.GetComponent<TurnOnLights>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectPC()
    {
        menu.SetActive(false);
        script.mobile = false;
        script2.mobile = false;
    }

    public void selectMobile()
    {
        menu.SetActive(false);
        script.mobile = true;
        script2.mobile = true;
    }

}
