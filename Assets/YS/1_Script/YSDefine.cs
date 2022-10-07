using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class ImageReference
    {
        public const string Mizar_Normal = "Characters/Mizare/Right/mizare_origin.png";
    }
    public class ResourcePath
    {
        // Path
        public static string ItemDataPath => "Data/ItemData";
        public static string BackgroundDataPath => "Data/BackgroundData";
        public static string PuzzleDataPath => "Data/PuzzleData";
        public static string ScriptDataPath => "Data/ScriptData";
        // Prefab
        public static string BGItemPrefabPath => "Prefab/BGItemPrefab";
        public static string PuzzlePiecePrefabPath => "Prefab/PuzzlePiecePrefab";
        // Image
        public static string TouchToStartBG => "Image/TouchToStartBG";
        // Video
        public static string TouchToStartVideoPath => "dummy/opening01video";
    }
    [System.Serializable, DisableContextMenu]
    public struct VariableData
    {
        [LabelText("변수명"), DisableIf("@true"), DisableContextMenu]
        public string name;
        [SerializeReference, LabelText("변수 타입")]
        public CustomVariable value;
    }
    [System.Serializable, DisableContextMenu]
    public struct BackgroundData
    {
        [LabelText("배경 이름"), DisableContextMenu]
        public string name;
        [LabelText("배경 이미지")]
        public Sprite img;
        [LabelText("아이템들")]
        public List<BackgroundItemData> items;
    }
    [System.Serializable]
    public struct BackgroundItemData
    {
        public ITEM_INDEX index;
        public Vector3 pos;

        public BackgroundItemData(Item item)
        {
            index = item.index;
            pos = item.transform.localPosition;
        }
    }
    public class ResourceManager
    {
        static Dictionary<string, Object> resourceMap = new Dictionary<string, Object>();

        public static T GetResource<T>(string path) where T : Object
        {
            if (resourceMap.ContainsKey(path))
                return (T)resourceMap[path];

            T obj = Resources.Load<T>(path);
            resourceMap.Add(path, obj);
            return obj;
        }

        public static void Remove(string path)
        {
            Resources.UnloadAsset(resourceMap[path]);
            resourceMap.Remove(path);
        }

        public static void Clear()
        {
            foreach (Object o in resourceMap.Values)
                Resources.UnloadAsset(o);
            resourceMap.Clear();
        }
    }
    public struct CachedWaitForSeconds
    {
        private static Dictionary<float, WaitForSeconds> cache = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds Get(float time)
        {
            if (!cache.ContainsKey(time))
                cache.Add(time, new WaitForSeconds(time));

            return cache[time];
        }
    }
    [System.Serializable]
    public struct Bezier
    {
        public Vector3[] bezierPos;

        public Vector3 GetBezierPosition(float t)
        {
            Vector3[] result = (Vector3[])bezierPos.Clone();

            for (int i = result.Length - 1; i > 0; --i)
                for (int j = 0; j < i; ++j)
                    result[j] = Vector3.Lerp(result[j], result[j + 1], t);

            return result[0];
        }
    }
    public class Path
    {
        public static string AppDataFolder => System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/ROAR/";
        public static string ConfigData => AppDataFolder + "config";
        public static string SaveData => AppDataFolder + "save";
    }
}