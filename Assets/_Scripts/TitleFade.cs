using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class TitleFade : MonoBehaviour {

    private float timer;
    private bool canLoad = false;
    public int FadeTime;
    public Image image;
    public int BuildIndex;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        image.DOFade(0, FadeTime);
	}

    public void Update()
    {
        var state = GamePad.GetState(PlayerIndex.One);
        if (state.Buttons.Start == ButtonState.Pressed)
        {
            image.DOFade(1, FadeTime);
            canLoad = true;
 
        }
        if (image.color.a == 1 && canLoad == true)
        {
            SceneManager.LoadScene(BuildIndex);
        }

    }




}
