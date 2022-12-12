using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back_character_select : MonoBehaviour {
    
    public void onchick_back_character_select()
    {
        MySceneManager.Instance.LoadScene("CharSelect");
    }
}
