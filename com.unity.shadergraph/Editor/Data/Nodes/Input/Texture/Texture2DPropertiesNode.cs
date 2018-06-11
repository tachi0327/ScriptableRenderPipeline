using System.Linq;
using UnityEngine;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph.Drawing.Controls;

namespace UnityEditor.ShaderGraph
{
    
    [Title("Input", "Texture", "Texture 2D Properties Node")]
    public class Texture2DPropertiesNode : AbstractMaterialNode, IGeneratesBodyCode, IMayRequireMeshUV
    {
        public const int OutputSlotTId = 0;
        public const int OutputSlotOId = 2;
        public const int OutputSlotWId = 3;
        public const int OutputSlotHId = 4;
        public const int TextureInputId = 1;

        const string kOutputSlotTName = "Tiling";
        const string kOutputSlotOName = "Offset";
        const string kOutputSlotWName = "Width";
        const string kOutputSlotHName = "Height";
        const string kTextureInputName = "Texture";

        public override bool hasPreview { get { return false; } }

        public Texture2DPropertiesNode()
        {
            name = "Texture 2D Properties";
            UpdateNodeAfterDeserialization();
        }

        public override string documentationURL
        {
            get { return "https://github.com/Unity-Technologies/ShaderGraph/wiki/Texture-2D-Properties-Node"; }
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            AddSlot(new Vector2MaterialSlot(OutputSlotTId, kOutputSlotTName, kOutputSlotTName, SlotType.Output, Vector2.zero, ShaderStageCapability.Fragment));
            AddSlot(new Vector2MaterialSlot(OutputSlotOId, kOutputSlotOName, kOutputSlotOName, SlotType.Output, Vector2.zero, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(OutputSlotWId, kOutputSlotWName, kOutputSlotWName, SlotType.Output, 0, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(OutputSlotHId, kOutputSlotHName, kOutputSlotHName, SlotType.Output, 0, ShaderStageCapability.Fragment));
            AddSlot(new Texture2DInputMaterialSlot(TextureInputId, kTextureInputName, kTextureInputName));
            RemoveSlotsNameNotMatching(new[] { OutputSlotTId, OutputSlotOId, OutputSlotWId, OutputSlotHId, TextureInputId });
        }

        // Node generations
        public virtual void GenerateNodeCode(ShaderGenerator visitor, GenerationMode generationMode)
        {
            visitor.AddShaderChunk(string.Format("{0}2 {1} = {2}_ST.xy;", precision, GetVariableNameForSlot(OutputSlotTId), GetSlotValue(TextureInputId, generationMode)), true);
			visitor.AddShaderChunk(string.Format("{0}2 {1} = {2}_ST.zw;", precision, GetVariableNameForSlot(OutputSlotOId), GetSlotValue(TextureInputId, generationMode)), true);
			visitor.AddShaderChunk(string.Format("{0}2 {1} = {2}_TexelSize.z;", precision, GetVariableNameForSlot(OutputSlotWId), GetSlotValue(TextureInputId, generationMode)), true);
			visitor.AddShaderChunk(string.Format("{0}2 {1} = {2}_TexelSize.w;", precision, GetVariableNameForSlot(OutputSlotHId), GetSlotValue(TextureInputId, generationMode)), true);
        }

        public bool RequiresMeshUV(UVChannel channel, ShaderStageCapability stageCapability)
        {
            return true;
        }
    }
}
