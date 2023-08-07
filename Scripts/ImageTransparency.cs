using UnityEngine;
using UnityEngine.UI;

public class ImageTransparency : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    void Start () 
    {
        image = GetComponent<Image>();
        
        Color c = image.color;
        c.a = 0.9f;
        image.color = c;
    }

}
