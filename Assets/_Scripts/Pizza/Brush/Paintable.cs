using System;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class Paintable : MonoBehaviour
{
    [SerializeField] private Brush brush;

    [SerializeField] private Texture2D baseTexture;
    [SerializeField] private Texture2D maskTexture;

    [Serializable]
    class Paint
    {
        public Texture2D texture;
        public int quantity;
        public float targetQuantityPercentage;
    }
    [SerializeField] private Paint[] paints;
    private int _paintIndex;
    
    [SerializeField] private bool useMaskAlpha = true;
    [SerializeField] private bool usePaintAlpha;
    
    private int[] _pixelParent;
    
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    
    private Material _material;
    private Texture2D _maskTexture;
    private Texture2D _mainTexture;
    private Texture2D _paintTexture;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer != null) SpriteMapper.SetRenderer(_spriteRenderer);
        else
        {
            _image = GetComponent<Image>();
            if(!_image.enabled) _image.enabled = true;
        }
        
        Initialize();
    }

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer != null) SpriteMapper.SetRenderer(_spriteRenderer);
        else
        {
            _image = GetComponent<Image>();
        }
    }

    public void Initialize()
    {
        _material = _spriteRenderer == null ? Instantiate(_image.material) : Instantiate(_spriteRenderer.material);

        _maskTexture = Instantiate(maskTexture ? maskTexture : baseTexture);
        _mainTexture = Instantiate(baseTexture);
        _paintTexture = Instantiate(paints[_paintIndex].texture);
        
        _material.SetTexture("_MainTex", _mainTexture);
        _material.SetTexture("_MaskTex", _maskTexture);
        _material.SetTexture("_MaskedTex", _paintTexture);
        _material.SetFloat("_UseMaskAplha", useMaskAlpha ? 1 : 0);
        _material.SetFloat("_UsePaintAlpha", usePaintAlpha ? 1 : 0);

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = Sprite.Create(_mainTexture, new Rect(Vector2.zero, new Vector2(baseTexture.width, baseTexture.height)), Vector2.one / 2f);
            transform.localScale /= baseTexture.width / _spriteRenderer.sprite.pixelsPerUnit;
        }
        else
        {
            _image.sprite = Sprite.Create(_mainTexture, new Rect(Vector2.zero, new Vector2(baseTexture.width, baseTexture.height)), Vector2.one / 2f);
            //transform.localScale /= baseTexture.width / _image.sprite.pixelsPerUnit;
        }

        _pixelParent = new int[_maskTexture.width*_maskTexture.height];
        
        ResetTexture(_maskTexture, Color.black);
        
        if(_spriteRenderer != null) _spriteRenderer.material = _material;
        else _image.material = _material;
        
        
        for (int i = 0; i < paints.Length; i++)
        {
            paints[i].quantity = (int)((paints[i].targetQuantityPercentage) * _maskTexture.width*_maskTexture.height);
        }
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

    public void Draw(Vector3 pos)
    {
        Vector2 coords = Vector2.zero;
        if (_spriteRenderer != null)
        {
            if (!SpriteMapper.TryGetTextureSpaceUV(_spriteRenderer, pos, out  coords)) return;
        }
        else if(!ImageMapper.TryGetTextureSpaceUV(_image, pos, out  coords)) return;
        
        int coordX = (int)(coords.x*_maskTexture.width);
        int coordY = (int)(coords.y*_maskTexture.height);
        
        float percentage = 0;
        
        for (int y = 0; y < brush.size; y++)
        {
            for (int x = 0; x < brush.size; x++)
            {
                int texX = coordX + x - brush.size/2;
                int texY = coordY + y - brush.size/2;

                if (texX >= 0 && texX < _maskTexture.width && texY >= 0 && texY < _maskTexture.height && brush.GetPixel(x, y).a > 0)
                {
                    Color maskColor = _maskTexture.GetPixel(texX, texY);
                    if(maskColor == Color.white) continue;
                    
                    if (!useMaskAlpha || maskColor.a > 0)
                    {
                        if (paints[_paintIndex].quantity-- < 0)
                        {
                            _maskTexture.Apply();
                            GetPercentages();
                            return;
                        }
                        _maskTexture.SetPixel(texX, texY, maskColor+brush.GetPixel(x, y));
                        _pixelParent[texX + texY * _maskTexture.width] = _paintIndex+1;
                    }
                }
            }
        }

        _maskTexture.Apply();
    }
    
    public void ChangePaint(int index)
    {
        CommitChanges();
        if (index < paints.Length && index >= 0)
        {
            _paintIndex = index;
            _paintTexture = Instantiate(paints[_paintIndex].texture);
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

    public float[] GetPercentages()
    {
        float[] percentages = new float[paints.Length+1];
        int[] pixelCount = new int[paints.Length+1];
        int total = _pixelParent.Length;
        
        int height = useMaskAlpha ? _maskTexture.height : _mainTexture.height;
        int width = useMaskAlpha ? _maskTexture.width : _mainTexture.width;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (useMaskAlpha && _maskTexture.GetPixel(x,  y).a == 0) total--;
                else pixelCount[_pixelParent[x + y * width]]++;
            }
        }
        
        for (int i = 0; i < percentages.Length; i++)
        {
            percentages[i] = (float)pixelCount[i]/total;
        }

        for (int i = 0; i < paints.Length; i++)
        {
            if(paints[i].targetQuantityPercentage > percentages[i+1])
            {
                paints[i].quantity = (int)((paints[i].targetQuantityPercentage - percentages[i+1]) * total);
            }
        }
        
        return percentages;
    }
    
    public Sprite GetSprite()
    {
        return Sprite.Create(paints[_paintIndex].texture, new Rect(Vector2.zero, new Vector2(baseTexture.width, baseTexture.height)), Vector2.one / 2f);
    }

    public bool IsPainted(Vector3 mousePos)
    {
        Vector2 coords = Vector2.zero;
        if (_spriteRenderer != null)
        {
            if (!SpriteMapper.TryGetTextureSpaceUV(_spriteRenderer, mousePos, out  coords)) return false;
        }
        else if(!ImageMapper.TryGetTextureSpaceUV(_image, mousePos, out  coords)) return false;
        
        int coordX = (int)(coords.x*_maskTexture.width);
        int coordY = (int)(coords.y*_maskTexture.height);
        
        int coordXMain = (int)(coords.x*_mainTexture.width);
        int coordYMain = (int)(coords.y*_mainTexture.height);
        
        return _maskTexture.GetPixel(coordX, coordY).r > 0 || _mainTexture.GetPixel(coordXMain, coordYMain).a > 0;
    }
}

