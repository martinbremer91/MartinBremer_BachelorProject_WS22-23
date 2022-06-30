using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class Square : MonoBehaviour
{
    public Vector2Int coords;
    
    private SquareType _type;
    private SquareType type
    {
        set
        {
            SetLineSprite(value);
            _type = value;
        }
        get => _type;
    }

    private SquareOrientation _orientation;
    private SquareOrientation orientation
    {
        set
        {
            _orientation = value;
            transform.DORotate(GetTileRotation(value), rotateDuration);
        }
        get => _orientation;
    }

    private float _rotateDuration;
    private float rotateDuration
    {
        get
        {
            if (_rotateDuration == 0)
                _rotateDuration = ReferenceCollection.Instance.configs.sqRotateDuration;
            return _rotateDuration;
        }
    }

    [SerializeField] private SpriteRenderer fillSpriteRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private MaterialPropertyBlock fillMpb;
    private MaterialPropertyBlock FillMpb {
        get {
            if (fillMpb == null) {
                fillMpb = new MaterialPropertyBlock();
                fillSpriteRenderer.GetPropertyBlock(fillMpb);
            }
            return fillMpb;
        }
    }
    
    private MaterialPropertyBlock mpb;
    private MaterialPropertyBlock Mpb {
        get {
            if (mpb == null) {
                mpb = new MaterialPropertyBlock();
                spriteRenderer.GetPropertyBlock(mpb);
            }
            return mpb;
        }
    }
    
    private static readonly int arcProp = Shader.PropertyToID("_Arc1");
    private static readonly int angleProp = Shader.PropertyToID("_Angle");
    private static readonly int colorProp = Shader.PropertyToID("_Color");

    public void SetRandomType() => type = (SquareType)Enum.ToObject(typeof(SquareType), Random.Range(0, 3));

    public void SetRandomOrientation()
    {
        int rand = (int)Mathf.Pow(2, Random.Range(0, 4));
        orientation = (SquareOrientation) Enum.ToObject(typeof(SquareOrientation), rand);
    }

    public void SetRandomColors()
    {
        Color[] colors = ReferenceCollection.Instance.configs.colors;
        
        int colorsNumber = 
            Mathf.Min(colors.Length, ReferenceCollection.Instance.configs.activeColorsNumber);
        
        if (colorsNumber < 2)
            throw new Exception("At least two colors required");

        int rngA = Random.Range(0, colorsNumber);
        int rngB = Random.Range(0, colorsNumber);

        while (rngA == rngB)
            rngB = Random.Range(0, colorsNumber);

        Mpb.SetColor(colorProp, colors[rngA]);
        FillMpb.SetColor(colorProp, colors[rngB]);
        
        spriteRenderer.SetPropertyBlock(Mpb);
        fillSpriteRenderer.SetPropertyBlock(FillMpb);
    }

    private void SetLineSprite(SquareType lineType)
    {
        switch (lineType)
        {
            case SquareType.Narrow:
                FillMpb.SetFloat(angleProp, 90);
                FillMpb.SetFloat(arcProp, 180);
                fillSpriteRenderer.SetPropertyBlock(FillMpb);
                break;
            case SquareType.Half:
                FillMpb.SetFloat(angleProp, 45);
                FillMpb.SetFloat(arcProp, 180);
                fillSpriteRenderer.SetPropertyBlock(FillMpb);
                break;
            case SquareType.Wide:
                FillMpb.SetFloat(angleProp, 45);
                FillMpb.SetFloat(arcProp, 225);
                fillSpriteRenderer.SetPropertyBlock(FillMpb);
                break;
            default:
                throw new ArgumentException("Invalid type");
        }
    }

    public void TurnTile(bool clockwise)
    {
        SquareOrientation o = clockwise ?
            (SquareOrientation)Enum.ToObject(typeof(SquareOrientation), (byte)orientation >> 1) :
            (SquareOrientation)Enum.ToObject(typeof(SquareOrientation), (byte)orientation << 1);

        if (clockwise && (byte)o < 1)
            o = (SquareOrientation)Enum.ToObject(typeof(SquareOrientation),
                Enum.GetValues(typeof(SquareOrientation)).Cast<SquareOrientation>().Last());
        if (!clockwise && (byte)o > (int)Enum.GetValues(typeof(SquareOrientation)).Cast<SquareOrientation>().Last())
            o = (SquareOrientation) Enum.ToObject(typeof(SquareOrientation), 1);

        orientation = o;
    }

    private Vector3 GetTileRotation(SquareOrientation o)
    {
        switch (o)
        {
            case SquareOrientation.North:
                return Vector3.zero;
            case SquareOrientation.East:
                return new Vector3(0, 0, 90);
            case SquareOrientation.South:
                return new Vector3(0, 0, 180);
            case SquareOrientation.West:
                return new Vector3(0, 0, 270);
            default:
                throw new ArgumentException("Invalid orientation");
        }
    }
}
