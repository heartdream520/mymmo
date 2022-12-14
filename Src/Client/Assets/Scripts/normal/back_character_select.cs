using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
public class back_character_select : MonoBehaviour {
    
    public void onchick_back_character_select()
    {
        MapService.Instance.CurrentMapId = 0;
        MySceneManager.Instance.LoadScene("CharSelect");
    }
}
