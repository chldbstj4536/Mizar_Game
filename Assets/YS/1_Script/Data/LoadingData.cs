using UnityEngine;

namespace YS
{
    public class LoadingData : MonoBehaviour
    {
        public LOADING_SCENE loadingScene;
    }

    public enum LOADING_SCENE
    {
        TITLE,
        GAME
    }
}