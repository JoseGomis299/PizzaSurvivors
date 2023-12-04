using UnityEngine;

public class SpriteMapper
{
    private readonly Sprite _sprite;
    private readonly Transform _transform;

    public SpriteMapper(Sprite sprite, Transform transform)
    {
        _sprite = sprite;
        _transform = transform;
    }
    
    public Vector2 TextureSpaceCoord(Vector3 worldPos) {
        float ppu = _sprite.pixelsPerUnit;
        
        // Local position on the sprite in pixels.
        Vector2 localPos = _transform.InverseTransformPoint(worldPos) * ppu;
        
        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(_sprite.rect.x, _sprite.rect.y) + _sprite.pivot;
        Vector2 texSpaceCoord = texSpacePivot + localPos;

        return texSpaceCoord;
    }
    
    public Vector2 TextureSpaceUV(Vector3 worldPos) {
        Texture2D tex = _sprite.texture;
        Vector2 texSpaceCoord = TextureSpaceCoord(worldPos);
        
        // Pixels to UV(0-1) conversion.
        Vector2 uvs = texSpaceCoord;
        uvs.x /= tex.width;
        uvs.y /= tex.height;


        return uvs;
    }
}