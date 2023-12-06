using UnityEngine;

public class SpriteMapper
{
    private static SpriteRenderer _spriteRenderer;
    public static bool SetRenderer(SpriteRenderer spriteRenderer)
    {
        if (_spriteRenderer == null || spriteRenderer.sortingOrder > _spriteRenderer.sortingOrder ||
            !_spriteRenderer.gameObject.activeInHierarchy || !_spriteRenderer.TryGetComponent(out Paintable paintable) || !paintable.enabled)
        {
            _spriteRenderer = spriteRenderer;
        }

        return spriteRenderer == _spriteRenderer;
    }
    
    public static bool TryGetTextureSpaceCoord(SpriteRenderer spriteRenderer, Vector3 worldPos, out Vector2 coord) {
        coord = Vector2.zero;
        if(!SetRenderer(spriteRenderer)) return false;
        
        var sprite = spriteRenderer.sprite;
        var transform = spriteRenderer.transform;
        
        float ppu = sprite.pixelsPerUnit;
        
        // Local position on the sprite in pixels.
        Vector2 localPos = transform.InverseTransformPoint(worldPos) * ppu;
        
        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;
        coord = texSpacePivot + localPos;

        return true;
    }
    
    public static bool TryGetTextureSpaceUV(SpriteRenderer spriteRenderer, Vector3 worldPos, out Vector2 uvs) 
    {
        uvs = Vector2.zero;
        if(!TryGetTextureSpaceCoord(spriteRenderer, worldPos, out var texSpaceCoord)) return false;
        Texture2D tex = _spriteRenderer.sprite.texture;

        // Pixels to UV(0-1) conversion.
        uvs = texSpaceCoord;
        uvs.x /= tex.width;
        uvs.y /= tex.height;

        return !(uvs.x < 0) && !(uvs.x > 1) && !(uvs.y < 0) && !(uvs.y > 1);
    }
}