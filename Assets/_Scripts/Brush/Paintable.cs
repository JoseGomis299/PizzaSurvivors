using System;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    [SerializeField] private Brush brush;

    [SerializeField] private Texture2D baseTexture;
    [SerializeField] private Texture2D maskTexture;
    [SerializeField] private Texture2D[] paintTextures;
    private int _paintTextureIndex;
    
    [SerializeField] private bool useMaskAlpha = true;
    [SerializeField] private bool usePaintAlpha;
    
    private SpriteMapper _spriteMapper;
    private Material _material;
    private Texture2D _maskTexture;
    private Texture2D _mainTexture;
    private Texture2D _paintTexture;

    private void OnEnable()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _material = spriteRenderer.material;
     
        _maskTexture = Instantiate(maskTexture ? maskTexture : baseTexture);
        _mainTexture = Instantiate(baseTexture);
        _paintTexture = Instantiate(paintTextures[_paintTextureIndex]);
        
        _material.SetTexture("_MainTex", _mainTexture);
        _material.SetTexture("_MaskTex", _maskTexture);
        _material.SetTexture("_MaskedTex", _paintTexture);
        _material.SetFloat("_UseMaskAplha", useMaskAlpha ? 1 : 0);
        _material.SetFloat("_UsePaintAlpha", usePaintAlpha ? 1 : 0);
        
        spriteRenderer.sprite = Sprite.Create(_mainTexture, new Rect(Vector2.zero,  new Vector2(baseTexture.width, baseTexture.height)), Vector2.one/2f);
        _spriteMapper = new SpriteMapper(spriteRenderer.sprite, transform);

        ResetTexture(_maskTexture, Color.black);
    }
    
    private void ResetTexture(Texture2D texture, Color color)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if(!useMaskAlpha || texture.GetPixel(x,y).a > 0)
                    texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButton(0)) Paint(pos);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangePaint(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangePaint(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangePaint(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangePaint(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangePaint(4);
        }
    }

    private void Paint(Vector3 pos)
    {
        Vector2 coords = _spriteMapper.TextureSpaceUV(pos);
        if(coords.x < 0 || coords.x > 1 || coords.y < 0 || coords.y > 1) return;
        
        int coordX = (int)(coords.x*_maskTexture.width);
        int coordY = (int)(coords.y*_maskTexture.height);
        
        for (int y = 0; y < brush.size; y++)
        {
            for (int x = 0; x < brush.size; x++)
            {
                int texX = coordX + x - brush.size/2;
                int texY = coordY + y - brush.size/2;

                if (texX >= 0 && texX < _maskTexture.width && texY >= 0 && texY < _maskTexture.height)
                { 
                    if(!useMaskAlpha || maskTexture.GetPixel(texX, texY).a > 0)
                        _maskTexture.SetPixel(texX, texY, _maskTexture.GetPixel(texX, texY)+brush.GetPixel(x, y));
                }
            }
        }

        _maskTexture.Apply();
    }
    
    public void ChangePaint(int index)
    {
        CommitChanges();
        if (index < paintTextures.Length && index >= 0)
        {
            _paintTextureIndex = index;
            _paintTexture = Instantiate(paintTextures[_paintTextureIndex]);
            _material.SetTexture("_MaskedTex", _paintTexture);
        }
    }

    private void CommitChanges()
    {
        int widthFactor = _paintTexture.width / _mainTexture.width;
        int heightFactor = _paintTexture.height / _mainTexture.height;

        for (int y = 0; y < _mainTexture.height; y++)
        {
            for (int x = 0; x < _mainTexture.width; x++)
            {
                if (_maskTexture.GetPixel(x, y).r > 0)
                {
                    Color color = Color.Lerp(_mainTexture.GetPixel(x, y), _paintTexture.GetPixel(x * widthFactor, y * heightFactor), _maskTexture.GetPixel(x, y).r);
                    color.a = usePaintAlpha ? color.a : 1;
                    _mainTexture.SetPixel(x, y, color);
                }
            }
        }

        _mainTexture.Apply();

        ResetTexture(_maskTexture, Color.black);
    }
}