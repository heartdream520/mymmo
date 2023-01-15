using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Common.Data;

public class MapTools : MonoBehaviour {

	[MenuItem("Map Tools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();

        Scene scene = EditorSceneManager.GetActiveScene();

        string currentScene = scene.name;
        if(scene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }
        List<TeleportObject> allteleportObjects = new List<TeleportObject>();
        foreach(var map in DataManager.Instance.Maps)
        {
            //获取地图所在位置
            string sceneFile = "Assets/Scenes/" + map.Value.Resource + ".unity";
            if(!System.IO.File.Exists(sceneFile))
            {
                Debug.LogErrorFormat("Scene :{0} not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            TeleportObject[] teleports = FindObjectsOfType<TeleportObject>();
            foreach(var teleporter in teleports)
            {
                if(!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("错误",
                        string.Format("地图：{0}中配置的 TeleportID：{1}不存在！", map.Value.ID, teleporter.ID),
                        " 确定");
                    return;
                }
                TeleporterDefine teleporter_Define = DataManager.Instance.Teleporters[teleporter.ID];
                if(teleporter_Define.MapID!=map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误",
                        string.Format("地图：{0}中不应存在的 TeleportID：{1}！", map.Value.ID, teleporter.ID),
                          "确定");
                }
                teleporter_Define.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                teleporter_Define.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Scenes/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完毕", "确定");
    }
    [MenuItem("Map Tools/Export SpawnPoints")]
    public static void ExportSpawnPoints()
    {
        DataManager.Instance.Load();

        Scene scene = EditorSceneManager.GetActiveScene();

        string currentScene = scene.name;
        if (scene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }
        if (DataManager.Instance.SpawnPoints == null)
            DataManager.Instance.SpawnPoints = new Dictionary<int, Dictionary<int, SpawnPointDefine>>();


        foreach (var map in DataManager.Instance.Maps)
        {
            //获取地图所在位置
            string sceneFile = "Assets/Scenes/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogErrorFormat("Scene :{0} not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            SpawnPoint[] SpawnPoints = FindObjectsOfType<SpawnPoint>();
            if(!DataManager.Instance.SpawnPoints.ContainsKey(map.Value.ID))
            {
                DataManager.Instance.SpawnPoints[map.Value.ID] = new Dictionary<int, SpawnPointDefine>();
            }
            foreach (var sp in SpawnPoints)
            {
                if (!DataManager.Instance.SpawnPoints[map.Value.ID].ContainsKey(sp.ID))
                {
                    /*
                    EditorUtility.DisplayDialog("错误",
                        string.Format("地图：{0}中配置的 SpawnPointId：{1}不存在！", map.Value.ID, sp.ID),
                        " 确定");*/
                    DataManager.Instance.SpawnPoints[map.Value.ID][sp.ID] = new SpawnPointDefine();
                }
                SpawnPointDefine def = DataManager.Instance.SpawnPoints[map.Value.ID][sp.ID];
                def.ID = sp.ID;
                def.MapID = map.Value.ID;
                def.Position = GameObjectTool.WorldToLogicN(sp.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(sp.transform.forward);
            }
        }
        DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Scenes/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "刷怪点导出完毕", "确定");
    }
}
