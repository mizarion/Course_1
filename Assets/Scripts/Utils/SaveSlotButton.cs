using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(TextMesh))]
public class SaveSlotButton : MonoBehaviour
{
    public Text Name_Text;

    [SerializeField]
    string _name = "default save";

    public string DateTime
    {
        get => _name;
        set { Name_Text.text = value; _name = value; }
    }

    public string path;

    //public int date { get; set; }

    public void UpdateText()
    {
        Name_Text.text = DateTime;
    }
}
