namespace UnityEditor.ShaderGraph
{
    interface IPropertyFromNode
    {
        IShaderProperty AsShaderProperty();
        int[] outputSlotIds { get; }
    }
}
