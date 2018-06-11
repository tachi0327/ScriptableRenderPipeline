using System;
using System.Linq;
using UnityEngine;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Title("Input", "Property")]
    public class PropertyNode : AbstractMaterialNode, IGeneratesBodyCode, IOnAssetEnabled
    {
        private Guid m_PropertyGuid;

        [SerializeField]
        private string m_PropertyGuidSerialized;

        //public const int OutputSlotId = 0;
        //public const int OutputSlotId1 = 1;
        //public const int OutputSlotId2 = 2;
        //public const int OutputSlotId3 = 3;
        //public const int OutputSlotId4 = 4;

        public static readonly int[] OutputSlotIds = {0, 1, 2, 3, 4};

        public PropertyNode()
        {
            name = "Property";
            UpdateNodeAfterDeserialization();
        }

        public override string documentationURL
        {
            get { return "https://github.com/Unity-Technologies/ShaderGraph/wiki/Property-Node"; }
        }

        private void UpdateNode()
        {
            var graph = owner as AbstractMaterialGraph;
            var property = graph.properties.FirstOrDefault(x => x.guid == propertyGuid);
            if (property == null)
                return;

            if (property is Vector1ShaderProperty)
            {
                AddSlot(new Vector1MaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, 0));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is Vector2ShaderProperty)
            {
                AddSlot(new Vector2MaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, Vector4.zero));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is Vector3ShaderProperty)
            {
                AddSlot(new Vector3MaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, Vector4.zero));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is Vector4ShaderProperty)
            {
                AddSlot(new Vector4MaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, Vector4.zero));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is ColorShaderProperty)
            {
                AddSlot(new Vector4MaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, Vector4.zero));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is TextureShaderProperty)
            {
                AddSlot(new Texture2DMaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output));
                AddSlot(new Vector2MaterialSlot(OutputSlotIds[1], "Tiling", "Tiling", SlotType.Output, Vector2.zero));
                AddSlot(new Vector2MaterialSlot(OutputSlotIds[2], "Offset", "Offset", SlotType.Output, Vector2.zero));
                AddSlot(new Vector1MaterialSlot(OutputSlotIds[3], "Width", "Width", SlotType.Output, 0));
                AddSlot(new Vector1MaterialSlot(OutputSlotIds[4], "Height", "Height", SlotType.Output, 0));
                RemoveSlotsNameNotMatching(OutputSlotIds);
            }
            else if (property is Texture2DArrayShaderProperty)
            {
                AddSlot(new Texture2DArrayMaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is Texture3DShaderProperty)
            {
                AddSlot(new Texture3DMaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output));
                RemoveSlotsNameNotMatching(new[] {OutputSlotIds[0]});
            }
            else if (property is CubemapShaderProperty)
            {
                AddSlot(new CubemapMaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output));
                RemoveSlotsNameNotMatching(new[] { OutputSlotIds[0] });
            }
            else if (property is BooleanShaderProperty)
            {
                AddSlot(new BooleanMaterialSlot(OutputSlotIds[0], property.displayName, "Out", SlotType.Output, false));
                RemoveSlotsNameNotMatching(new[] { OutputSlotIds[0] });
            }
        }

        public void GenerateNodeCode(ShaderGenerator visitor, GenerationMode generationMode)
        {
            var graph = owner as AbstractMaterialGraph;
            var property = graph.properties.FirstOrDefault(x => x.guid == propertyGuid);
            if (property == null)
                return;

            if (property is Vector1ShaderProperty)
            {
                var result = string.Format("{0} {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is Vector2ShaderProperty)
            {
                var result = string.Format("{0}2 {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is Vector3ShaderProperty)
            {
                var result = string.Format("{0}3 {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is Vector4ShaderProperty)
            {
                var result = string.Format("{0}4 {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is ColorShaderProperty)
            {
                var result = string.Format("{0}4 {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is BooleanShaderProperty)
            {
                var result = string.Format("{0} {1} = {2};"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[0])
                        , property.referenceName);
                visitor.AddShaderChunk(result, true);
            }
            else if (property is TextureShaderProperty)
            {
                var tiling = string.Format("{0}2 {1} = {2}_ST.xy;"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[1])
                        , property.referenceName);
                var offset = string.Format("{0}2 {1} = {2}_ST.zw;"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[2])
                        , property.referenceName);
                var width = string.Format("{0} {1} = {2}_TexelSize.z;"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[3])
                        , property.referenceName);
                var height = string.Format("{0} {1} = {2}_TexelSize.w;"
                        , precision
                        , GetVariableNameForSlot(OutputSlotIds[4])
                        , property.referenceName);
                visitor.AddShaderChunk(tiling, true);
                visitor.AddShaderChunk(offset, true);
                visitor.AddShaderChunk(width, true);
                visitor.AddShaderChunk(height, true);
            }            
        }

        public Guid propertyGuid
        {
            get { return m_PropertyGuid; }
            set
            {
                if (m_PropertyGuid == value)
                    return;

                var graph = owner as AbstractMaterialGraph;
                var property = graph.properties.FirstOrDefault(x => x.guid == value);
                if (property == null)
                    return;
                m_PropertyGuid = value;

                UpdateNode();

                Dirty(ModificationScope.Topological);
            }
        }

        public override string GetVariableNameForSlot(int slotId)
        {
            var graph = owner as AbstractMaterialGraph;
            var property = graph.properties.FirstOrDefault(x => x.guid == propertyGuid);

            if (slotId != 0 ||
                !(property is TextureShaderProperty) &&
                !(property is Texture2DArrayShaderProperty) &&
                !(property is Texture3DShaderProperty) &&
                !(property is CubemapShaderProperty))
                return base.GetVariableNameForSlot(slotId);

            return property.referenceName;
        }

        protected override bool CalculateNodeHasError()
        {
            var graph = owner as AbstractMaterialGraph;

            if (!graph.properties.Any(x => x.guid == propertyGuid))
                return true;

            return false;
        }

        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            m_PropertyGuidSerialized = m_PropertyGuid.ToString();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            if (!string.IsNullOrEmpty(m_PropertyGuidSerialized))
                m_PropertyGuid = new Guid(m_PropertyGuidSerialized);
        }

        public void OnEnable()
        {
            UpdateNode();
        }
    }
}
