#if UNITY_EDITOR
public class UshortConverter : UnityEditor.UIElements.UxmlAttributeConverter<ushort>
{
    public override ushort FromString(string value) => ushort.Parse(value);
    public override string ToString(ushort value) => value.ToString();
}
#endif