using UnityEngine;
using System;

[Serializable]
public class Brush 
{
    [SerializeField] private Texture2D texture;
    public int size;
    private Color _transparent = new Color(0,0,0,0);
    
    public Color GetPixel(int x, int y)
    {
        return texture.GetPixel(x*texture.width/size, y*texture.height/size) != Color.black ? Color.white : _transparent;
    }
    
    public void SetTexture(Texture2D texture)
    {
        this.texture = texture;
    }
}