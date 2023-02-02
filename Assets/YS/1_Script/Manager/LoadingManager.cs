using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YS
{
    public class LoadingManager : MonoBehaviour
    {
        private CustomTMPEffect te;
        private float timer = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            te = GetComponent<CustomTMPEffect>();

            LoadingData ld = FindObjectOfType<LoadingData>();
            if (ld == null)
                StartCoroutine(LoadData(LOADING_SCENE.TITLE));
            else
            {
                StartCoroutine(LoadData(ld.loadingScene));
                DestroyImmediate(ld.gameObject);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (te.IsDoneTyping)
            {
                timer += Time.deltaTime;
                if (timer >= 1.0f)
                {
                    timer = 0.0f;
                    te.SetText("Loading...");
                }
            }
        }
        private IEnumerator LoadData(LOADING_SCENE ls)
        {
            AsyncOperation oper = null;
            switch (ls)
            {
                case LOADING_SCENE.TITLE:
                    oper = SceneManager.LoadSceneAsync(1);
                    break;
                case LOADING_SCENE.GAME:
                    oper = SceneManager.LoadSceneAsync(2);
                    DontDestroyOnLoad(FindObjectOfType<InGameInitData>().gameObject);
                    break;
            }

            while (!oper.isDone)
                yield return CachedWaitForSeconds.Get(0.1f);
        }
    }
}