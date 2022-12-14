using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using System;
using System.IO;

using Common.Data;

using Newtonsoft.Json;

public class DataManager : Singleton<DataManager>
{
    public string DataPath;
    
    /// <summary>
    /// ??ͼ?ֵ?
    /// </summary>
    public Dictionary<int, MapDefine> Maps = null;
    /// <summary>
    /// ??ɫ?ֵ?
    /// </summary>
    public Dictionary<int, CharacterDefine> Characters = null;
    /// <summary>
    /// ???͵??ֵ?
    /// </summary>
    public Dictionary<int, TeleporterDefine> Teleporters = null;
    /// <summary>
    /// ˢ?ֵ??ֵ?
    /// </summary>
    public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
    /// <summary>
    /// NPC?ֵ?
    /// </summary>
    public Dictionary<int, NpcDefine> Npcs = null;
    /// <summary>
    /// ??Ʒ?ֵ?
    /// </summary>
    public Dictionary<int, ItemDefine> Items = null;

    internal Dictionary<int, ShopDefine> Shops = null;

    public Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;

    public Dictionary<int, EquipDefine> Equips = null;
    public DataManager()
    {
        this.DataPath = "Data/";
        Debug.LogFormat("DataManager > DataManager()");
    }
    /// <summary>
    /// ?????????õ?
    /// </summary>
    public void Load()
    {
        string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
        this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

        json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
        this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

        json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
        this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

        /*
        json = File.ReadAllText(this.DataPath + "SpawnPointDefine.txt");
        this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>> (json);
        */
        json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
        this.Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

        /*
        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
        */
        json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
        this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);
    }

    /// <summary>
    /// ???ͻ????õ?
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadData()
    {
        string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
        this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);
        Debug.LogFormat("MapDefine.txt");

        yield return null;

        json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
        this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
        Debug.LogFormat("CharacterDefine.txt");
        yield return null;

        json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
        this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);
        Debug.LogFormat("TeleporterDefine.txt");
        yield return null;

        /*
        json = File.ReadAllText(this.DataPath + "SpawnPointDefine.txt");
        this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);
        
        yield return null;
        */


        Debug.LogFormat("SpawnPointDefine.txt");
        json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
        this.Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);
        Debug.LogFormat("NpcDefine.txt");
        yield return null;


        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
        Debug.LogFormat("ItemDefine.txt");
        yield return null;

        json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
        this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);
        yield return null;
        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);
        yield return null;
        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);
    }

#if UNITY_EDITOR
    public void SaveTeleporters()
    {
        string json = JsonConvert.SerializeObject(this.Teleporters, Formatting.Indented);
        File.WriteAllText(this.DataPath + "TeleporterDefine.txt", json);
    }

    public void SaveSpawnPoints()
    {
        string json = JsonConvert.SerializeObject(this.SpawnPoints, Formatting.Indented);
        File.WriteAllText(this.DataPath + "SpawnPointDefine.txt", json);
    }

#endif
}
