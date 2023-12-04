using UnityEngine;
using System;

[Serializable]
public class Brush 
{
    [SerializeField] private Texture2D texture;
    public int size;
    
    public Color GetPixel(int x, int y)
    {
        return texture.GetPixel(x*texture.width/size, y*texture.height/size) != Color.black ? Color.white : Color.black;
    }
    
    public void SetTexture(Texture2D texture)
    {
        this.texture = texture;
    }
}