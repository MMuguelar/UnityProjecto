//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class image_item_code : MonoBehaviour
//{

//    public bool is_special = false;
//    public string hash;
//    public GameObject button;
//    public GameObject image_normal;
//    public GameObject image_add;
//    public TMP_Text bt_text;
//    public Color TransparentC;
//    void Start()
//    {
     

//    }

//    public void set_data(string hashp, bool special=false)
//    {
//        hash = hashp;
//        is_special = special;

//        if (is_special)
//        {
//            bt_text.text = "ADD";
//            button.GetComponent<Image>().color = TransparentC;
//            image_normal.SetActive(false);
//            image_add.SetActive(true);
//        }
//        else
//        {
//            bt_text.text = "VIEW";
//            image_normal.SetActive(true);
//            image_add.SetActive(false);

//        }

//    }

//    public void ver_imagen()
//    {
//        if (is_special)
//            GameObject.Find("CODE").GetComponent<Main_ipfs>().browse_archivo();
//        else
//        GameObject.Find("CODE").GetComponent<Main_ipfs>().load_image_from_hash(hash);
//    }

//    void Update()
//    {
        
//    }
//}
