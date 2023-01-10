using Assets.Scripts.Models;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip_attribute : MonoBehaviour {

    public Image icon;
    public Text name;
    public Text character_class;
    public Text HP;
    public Text MP;
    public Text STR;
    public Text INT;
    public Text DEX;
    public Text AD;
    public Text AP;
    public Text DEF;
    public Text MDEF;
    public Text SPD;
    public Text CRI;

    public void set_Equip_attribute(Item equip)
    {
        this.icon.overrideSprite = Resloader.Load<Sprite>(equip.itemDefine.Icon);
        this.name.text = equip.itemDefine.Name;
        this.character_class.text = CharacterDefine.CharacterClass_Dic[equip.itemDefine.LimitClass];
        EquipDefine define = equip.equipDefine;
        HP.text = define.HP.ToString();
        HP.gameObject.SetActive(define.HP == 0 ? false : true);
        HP.transform.parent.gameObject.SetActive(define.HP == 0 ? false : true);

        MP.text = define.MP.ToString();
        MP.gameObject.SetActive(define.MP == 0 ? false : true);
        MP.transform.parent.gameObject.SetActive(define.MP == 0 ? false : true);

        STR.text = define.STR.ToString();
        STR.gameObject.SetActive(define.STR == 0 ? false : true);
        STR.transform.parent.gameObject.SetActive(define.STR == 0 ? false : true);

        INT.text = define.INT.ToString();
        INT.gameObject.SetActive(define.INT == 0 ? false : true);
        INT.transform.parent.gameObject.SetActive(define.INT == 0 ? false : true);

        DEX.text = define.DEX.ToString();
        DEX.gameObject.SetActive(define.DEX == 0 ? false : true);
        DEX.transform.parent.gameObject.SetActive(define.DEX == 0 ? false : true);

        AD.text = define.AD.ToString();
        AD.gameObject.SetActive(define.AD == 0 ? false : true);
        AD.transform.parent.gameObject.SetActive(define.AD == 0 ? false : true);

        AP.text = define.AP.ToString();
        AP.gameObject.SetActive(define.AP == 0 ? false : true);
        AP.transform.parent.gameObject.SetActive(define.AP == 0 ? false : true);

        DEF.text = define.DEF.ToString();
        DEF.gameObject.SetActive(define.DEF == 0 ? false : true);
        DEF.transform.parent.gameObject.SetActive(define.DEF == 0 ? false : true);

        MDEF.text = define.MDEF.ToString();
        MDEF.gameObject.SetActive(define.MDEF == 0 ? false : true);
        MDEF.transform.parent.gameObject.SetActive(define.MDEF == 0 ? false : true);

        SPD.text = define.SPD.ToString();
        SPD.gameObject.SetActive(define.SPD == 0 ? false : true);
        SPD.transform.parent.gameObject.SetActive(define.SPD == 0 ? false : true);

        CRI.text = define.CRI.ToString();
        CRI.gameObject.SetActive(define.CRI == 0 ? false : true);
        CRI.transform.parent.gameObject.SetActive(define.CRI == 0 ? false : true);
        
    }
}
