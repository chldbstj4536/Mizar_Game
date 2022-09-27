using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;

public class TestScript : MonoBehaviour
{
    private float timer;
    private CustomTMPEffect te;

    private void Start()
    {
        te = GetComponent<CustomTMPEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (te.IsDoneTyping)
        {
            timer += Time.deltaTime;
            if (timer >= 3.0f)
            {
                timer = 0.0f;
                te.SetText("�׽�Ʈ<link=\"ts=1\">��Ʈ��</link>�׽�Ʈ<link=shake>��ƶ�׽�Ʈ��Ʈ��</link>�׽�Ʈ��ƶ�׽�Ʈ��Ʈ���׽�Ʈ��ƶ");
            }
        }
    }
}
