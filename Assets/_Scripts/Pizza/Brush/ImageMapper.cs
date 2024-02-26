using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class ImageMapper
{
    
    private static Image _image;
    
    public static bool TryGetTextureSpaceCoord(Image image, Vector3 screenPoint, out Vector2 coord) {
        coord = Vector2.zero;
        _image = image;
        
        var sprite = image.sprite;
        var transform = image.transform;
        
        float ppu = sprite.pixelsPerUnit;
        
        // Local position on the sprite in pixels.
        Vector2 localPos = transform.InverseTransformPoint(screenPoint) * ppu;
        
        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;
        coord = texSpacePivot + localPos;

        return true;
    }
    
    public static bool TryGetTextureSpaceUV(Image image, Vector3 screenPoint, out Vector2 uvs) 
    {
        uvs = Vector2.zero;
        _image = image;
        
        uvs = _image.rectTransform.position - screenPoint;
        
        uvs.x /= _image.rectTransform.rect.width * _image.rectTransform.localScale.x;
        uvs.y /= _image.rectTransform.rect.height * _image.rectTransform.localScale.y;
        uvs.x -= 0.5f;
        uvs.y -= 0.5f;
        uvs *= -1;
        
        return !(uvs.x < 0) && !(uvs.x > 1) && !(uvs.y < 0) && !(uvs.y > 1);
    }
}