using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    /// <summary>
    /// �̹����鿡 ���� ��� Set
    /// </summary>
    public class ImageReference
    {
        public const string Mizar_Normal = "Characters/Mizare/Right/mizare_origin.png";
    }
    /// <summary>
    /// ���ҽ��鿡 ���� ��� ����
    /// </summary>
    public class ResourcePath
    {
        // Path
        public static string ItemDatah => "Data/ItemData";
        public static string BackgroundData => "Data/BackgroundData";
        public static string ClueData => "Data/ClueData";
        public static string PuzzleData => "Data/PuzzleData";
        public static string ScriptData => "Data/ScriptData";
        // Prefab
        public static string BGItemPrefab => "Prefab/BGItemPrefab";
        public static string PuzzlePiecePrefab => "Prefab/PuzzlePiecePrefab";
        public static string FingerprintPrefab => "Prefab/FingerprintPrefab";
        // Image
        public static string LoadPreviewDefaultImg => "Image/LoadPreviewDefaultImg";
        // Material
        public static string DefaultMtrl => "Material/DefaultMtrl";
        public static string MonoMtrl => "Material/MonoMtrl";
        public static string BGFXMtrl => "Material/BGFXMtrl";
    }
    /// <summary>
    /// ����Ƽ �����Ϳ��� ���� ������ �ٷ�� �ϱ� ���� ���� Ŭ����
    /// </summary>
    [System.Serializable, DisableContextMenu]
    public struct VariableData
    {
        [LabelText("������"), DisableIf("@true"), DisableContextMenu]
        public string name;
        [SerializeReference, LabelText("���� Ÿ��")]
        public CustomVariable value;
    }
    /// <summary>
    /// ��濡 ���� ������ ��� ���� ������ ����
    /// </summary>
    [System.Serializable, DisableContextMenu]
    public struct BackgroundData
    {
        [LabelText("��� �̸�"), DisableContextMenu]
        public string name;
        [LabelText("��� �̹���")]
        public Sprite img;
        [LabelText("�����۵�")]
        public List<BackgroundItemData> items;
    }
    [System.Serializable, DisableContextMenu]
    public struct BGMData
    {
        public AudioClip bgm;
        public float vol;
        public bool bLoop;
        public float playTime;
        public float dampingTime;
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
    [System.Serializable]
    public struct FingerprintData
    {
        [LabelText("���� �̹���")]
        public Sprite img;
        [LabelText("���� ��ġ")]
        public Vector3 pos;

        public FingerprintData(Image fp)
        {
            img = fp.sprite;
            pos = fp.transform.localPosition;
        }
    }
    [System.Serializable]
    public struct ClueData
    {
        [LabelText("�ܼ� �̸�"), DisableContextMenu]
        public string name;
        [LabelText("�ܼ� �̹���")]
        public Sprite img;
        [LabelText("�����۵�")]
        public List<FingerprintData> fds;
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