using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace SimpleWebsite.Behaviors
{
    public class RenderXmlOutput<T> : BasicBehavior where T : class
    {
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;

        public RenderXmlOutput(IOutputWriter writer, IFubuRequest request)
            : base(PartialBehavior.Executes)
        {
            _writer = writer;
            _request = request;
        }

        protected override DoNext performInvoke()
        {
            var output = _request.Get<T>();
            var serializer = new CustomXmlSerializer();
            var builder = serializer.WriteText(output);
            _writer.Write(MediaTypeNames.Text.Xml, builder.ToString());

            return DoNext.Continue;
        }

    }
}