using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphing;
using UnityEditor.Importers;
using Utf8Json;

namespace UnityEditor.ShaderGraph
{
    [Serializable]
    // [JsonFormatter(typeof(UnityObjectFormatter<MaterialGraph>))]
    [JsonVersioned(typeof(MaterialGraph1))]
    public class MaterialGraph : AbstractMaterialGraph, IShaderGraph
    {
        public IMasterNode masterNode
        {
            get { return GetNodes<INode>().OfType<IMasterNode>().FirstOrDefault(); }
        }

        public string GetShader(string name, GenerationMode mode, out List<PropertyCollector.TextureInfo> configuredTextures, List<string> sourceAssetDependencyPaths = null)
        {
            return masterNode.GetShader(mode, name, out configuredTextures, sourceAssetDependencyPaths);
        }

        public void LoadedFromDisk()
        {
            OnEnable();
            ValidateGraph();
        }
    }

    [JsonVersioned(typeof(MaterialGraph0))]
    public class MaterialGraph1 : IUpgradableTo<MaterialGraph>
    {
        public MaterialGraph Upgrade()
        {
            throw new NotImplementedException();
        }
    }

    public class MaterialGraph0 : IUpgradableTo<MaterialGraph1>
    {
        public MaterialGraph1 Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
