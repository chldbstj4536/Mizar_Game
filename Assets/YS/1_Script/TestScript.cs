using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;

public class TestScript : MonoBehaviour
{
    private float timer;
    private CustomTMPEffect te;

    public Texture2D[] curs;

    private void Start()
    {
        te = GetComponent<CustomTMPEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Cursor.SetCursor(curs[0], Vector2.zero, CursorMode.Auto);
        if (Input.GetKeyDown(KeyCode.S))
            Cursor.SetCursor(curs[1], Vector2.zero, CursorMode.Auto);
        if (Input.GetKeyDown(KeyCode.D))
            Cursor.SetCursor(curs[2], Vector2.zero, CursorMode.Auto);
        if (Input.GetKeyDown(KeyCode.F))
            Cursor.SetCursor(curs[3], Vector2.zero, CursorMode.Auto);

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
