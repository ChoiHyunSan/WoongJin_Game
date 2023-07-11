using UnityEngine;

[CreateAssetMenu(fileName = "Charcter Default Info", menuName = "Scriptable Object/Charcter Default Info", order = int.MaxValue)]
public class CharacterDefaultInfo : ScriptableObject
{
    [Header("�⺻ ���� ����")]
    [SerializeField]
    private float[] _moveColor = new float[4];
    [SerializeField]
    private float[] _areaColor = new float[4];
    [SerializeField]
    private float[] _borderColor = new float[4];

    [Header("��������Ʈ")]
    [SerializeField]
    private string _characterSprite;

    // Get Function

    public string GetCharcterSpriteName()
    {
        return _characterSprite;
    }

    public float[] GetMoveColor()
    {
        return _moveColor;   
    }

    public float[] GetAreaColor()
    {
        return _areaColor;
    }

    public float[] GetBorderColor()
    {
        return _borderColor;
    }

    public void SetCharacterSpriteName(string spriteName)
    {
        _characterSprite = spriteName;
    }
}
