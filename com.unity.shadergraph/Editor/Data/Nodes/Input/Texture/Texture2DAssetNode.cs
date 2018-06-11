using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEngine;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Title("Input", "Texture", "Texture 2D Asset")]
    public class Texture2DAssetNode : AbstractMaterialNode, IPropertyFromNode, IGeneratesBodyCode
    {
        public const int OutputSlotId = 0;
        public const int TileOutSlotId = 1;
        public const int OffsetOutSlotId = 2;
        public const int WidthOutSlotId = 3;
        public const int HeightOutSlotId = 4;

        const string kOutputSlotName = "Out";
        const string kTileOutSlotName = "Tiling";
        const string kOffsetOutSlotName = "Offset";
        const string kWidthOutSlotName = "Width";
        const string kHeightOutSlotName = "Height";

        public Texture2DAssetNode()
        {
            name = "Texture 2D Asset";
            UpdateNodeAfterDeserialization();
        }

        public override string documentationURL
        {
            get { return "https://github.com/Unity-Technologies/ShaderGraph/wiki/Texture-2D-Asset-Node"; }
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            AddSlot(new Texture2DMaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output));
            AddSlot(new Vector2MaterialSlot(TileOutSlotId, kTileOutSlotName, kTileOutSlotName, SlotType.Output, Vector2.zero));
            AddSlot(new Vector2MaterialSlot(OffsetOutSlotId, kOffsetOutSlotName, kOffsetOutSlotName, SlotType.Output, Vector2.zero));
            AddSlot(new Vector1MaterialSlot(WidthOutSlotId, kWidthOutSlotName, kWidthOutSlotName, SlotType.Output, 0));
            AddSlot(new Vector1MaterialSlot(HeightOutSlotId, kHeightOutSlotName, kHeightOutSlotName, SlotType.Output, 0));
            RemoveSlotsNameNotMatching(new[] { OutputSlotId, TileOutSlotId, OffsetOutSlotId, WidthOutSlotId, HeightOutSlotId });
        }

        [SerializeField]
        private SerializableTexture m_Texture = new SerializableTexture();

        [TextureControl("")]
        public Texture texture
        {
            get { return m_Texture.texture; }
            set
            {
                if (m_Texture.texture == value)
                    return;
                m_Texture.texture = value;
                Dirty(ModificationScope.Node);
            }
        }

        public override void CollectShaderProperties(PropertyCollector properties, GenerationMode generationMode)
        {
            properties.AddShaderProperty(new TextureShaderProperty()
            {
                overrideReferenceName = GetVariableNameForSlot(OutputSlotId),
                generatePropertyBlock = true,
                value = m_Texture,
                modifiable = false
            });
        }

        public override void CollectPreviewMaterialProperties(List<PreviewProperty> properties)
        {
            properties.Add(new PreviewProperty(PropertyType.Texture2D)
            {
                name = GetVariableNameForSlot(OutputSlotId),
                textureValue = texture
            });
        }

        public IShaderProperty AsShaderProperty()
        {
            var prop = new TextureShaderProperty { value = m_Texture };
            if (texture != null)
                prop.displayName = texture.name;
            return prop;
        }

        public void GenerateNodeCode(ShaderGenerator visitor, GenerationMode generationMode)
        {
            var tiling = string.Format("{0}2 {1} = {2}_ST.xy;"
                        , precision
                        , GetVariableNameForSlot(TileOutSlotId)
                        , GetVariableNameForSlot(OutputSlotId));
            var offset = string.Format("{0}2 {1} = {2}_ST.zw;"
                        , precision
                        , GetVariableNameForSlot(OffsetOutSlotId)
                        , GetVariableNameForSlot(OutputSlotId));
            var width = string.Format("{0} {1} = {2}_TexelSize.z;"
                        , precision
                        , GetVariableNameForSlot(WidthOutSlotId)
                        , GetVariableNameForSlot(OutputSlotId));
            var height = string.Format("{0} {1} = {2}_TexelSize.w;"
                        , precision
                        , GetVariableNameForSlot(HeightOutSlotId)
                        , GetVariableNameForSlot(OutputSlotId));
            visitor.AddShaderChunk(tiling, true);
            visitor.AddShaderChunk(offset, true);
            visitor.AddShaderChunk(width, true);
            visitor.AddShaderChunk(height, true);
        }

        public override string GetVariableNameForSlot(int slotId)
        {
            if (slotId != 0)
                return base.GetVariableNameForSlot(slotId);

            return GetVariableNameForNode();
        }

        public int[] outputSlotIds { get { return new int[]{OutputSlotId, TileOutSlotId, OffsetOutSlotId, WidthOutSlotId, HeightOutSlotId}; } }
    }
}
