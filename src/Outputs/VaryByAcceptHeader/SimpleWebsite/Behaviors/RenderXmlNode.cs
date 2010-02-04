using System;
using FubuMVC.Core.Registration.Nodes;

namespace SimpleWebsite.Behaviors
{
    public class RenderXmlNode : OutputNode
    {
        private readonly Type _modelType;

        public RenderXmlNode(Type modelType) : base(typeof (RenderXmlOutput<>).MakeGenericType(modelType))
        {
            _modelType = modelType;
        }

        public Type ModelType { get { return _modelType; } }
        public override string Description { get { return "Xml"; } }

    }
}