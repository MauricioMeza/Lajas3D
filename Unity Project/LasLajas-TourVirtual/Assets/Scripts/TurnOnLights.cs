using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    
    public List<Animator> animators = new List<Animator>();
    public Animator sunAnim = new Animator();

    string state = "day";
    bool animationPlaying = false;
    
    //Variables to check the mobile double tap
    public bool mobile;
    bool firstTap = false;
    Touch touchSave;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnim();

        if (!mobile && Input.GetButtonDown("Fire2") && animationPlaying == false) 
        {
            if(state.Equals("day"))
            {
                TurnOn(); 
            }
            else if(state.Equals("night"))
            {
                TurnOff();
            }
        }

        if (mobile && TouchCount() && animationPlaying == false)
        {
            if (state.Equals("day"))
            {
                TurnOn();
            }
            else if (state.Equals("night"))
            {
                TurnOff();
            }
        }
    }

    //Turn on the Lights
    void TurnOn() 
    {
        sunAnim.Play("sunFade");
        foreach (Animator animador in animators)
        {
            animador.Play("lightFade");
        }
        state = "night";
    }

    //Turn off the Lights
    void TurnOff()
    {
        sunAnim.Play("sunRise");
        foreach (Animator animador in animators)
        {
            animador.Play("lightRise");
        }
        state = "day";
    }

    //Check if the animation is playing during the current frame
    void CheckAnim() 
    {
        if (sunAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") || sunAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
        {
            animationPlaying = false;
        }
        else
        {
            animationPlaying = true;
        }
    }

    // Check if there is a double tap
    bool TouchCount()
    {
        bool state = false;
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended  && firstTap == false)
        {
            firstTap = true;
            touchSave = Input.GetTouch(0);
            timer = Time.fixedTime;
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && firstTap == true)
        {
            float timeInterval = Time.fixedTime - timer;
            Vector2 posInterval = touchSave.position - Input.GetTouch(0).position;
            if (timeInterval < 0.5 && posInterval.magnitude < Screen.width/10)
            {
                state = true;
            }
            Debug.Log(posInterval.magnitude);
            
        }
        else if ((Time.fixedTime - timer) > 0.5)
        {
            firstTap = false;
            timer = 0;
        }
        return state;
    }
}
