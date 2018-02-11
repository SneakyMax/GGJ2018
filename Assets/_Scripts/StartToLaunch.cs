using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class StartToLaunch : MonoBehaviour
{

    float timer; 
    public float FlashInterval;
    public int BuildIndex;
    public MonoBehaviour Target;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FlashLoop());
	}
	
    private IEnumerator FlashLoop ()
    {
        while (true)
        {
            Target.enabled = false;
            yield return new WaitForSeconds(FlashInterval);
            Target.enabled = true;
            yield return new WaitForSeconds(FlashInterval);

            

        }

    }
    public void Update()
    {
        var state = GamePad.GetState(PlayerIndex.One);
        if (state.Buttons.Start == ButtonState.Pressed)
        {
            StopCoroutine(FlashLoop());
            Target.enabled = false;
        }

        
    }
}
