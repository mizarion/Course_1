using UnityEngine;

/// <summary>
/// �������� �� ������� �������� ������ �� �������
/// </summary>
public class SmoothFollowTarget : Singleton<SmoothFollowTarget>
{
    public GameObject target;           // ������ �� �������� �����
    public Vector3 Offset;              // ���������� �� ����

    /// <summary>
    /// ���������� �����, ������� ���������� � ����� ������� �����. 
    /// ������������ ��� ����������� ������ �� ������.  
    /// </summary>
    public void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) // ���� ������� else � if(target == null) ���������� ��������� �����
            {
                if (GameManager.Instance.CurrentScene == Scenes.MainGame)
                {
                    transform.position = new Vector3(1.5f, 12, -7) + target.transform.position;
                    Offset = transform.position - target.transform.position;
                }
                if (GameManager.Instance.CurrentScene == Scenes.Dungeon)
                {
                    transform.position = new Vector3(6, 12, 6) + target.transform.position;
                    Offset = transform.position - target.transform.position;
                }
            }
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target.transform.position + Offset, Time.deltaTime * 5);
    }
}

