using System.Collections;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.View;

namespace SimpleWebsite.Behaviors
{
    public class VariableOutputConvention : IConfigurationAction
    {

        public void Configure(BehaviorGraph graph)
        {
            graph.Actions().Where(x => x.HasOutputBehavior() && x.OutputType().CanBeCastTo<IEnumerable>() && getRenderViewNode(x) != null)
                .Each(x =>
                {
                    var modelType = x.OutputType();
                    var view = getRenderViewNode(x);
                    var json = new RenderJsonNode(modelType);
                    var xml = new RenderXmlNode(modelType);

                    var variableOut = new VariableOutputNode();
                    view.ReplaceWith(variableOut);

                    variableOut.AddOutput(a => a.RenderFormat == "json", json);
                    variableOut.AddOutput(a => a.RenderFormat == "xml", xml);
                    variableOut.AddOutput(a => a.AcceptsFormat("text/html"), view);
                    variableOut.AddOutput(a => a.AcceptsFormat("application/xml"), xml);
                    variableOut.AddOutput(a => a.AcceptsFormat("application/json"), json);

                    graph.Observer.RecordCallStatus(x, "Adding variable output behavior");
                });
        }

        private static OutputNode getRenderViewNode(ActionCall x)
        {
            return x.OfType<OutputNode>().FirstOrDefault(y => TypeExtensions.CanBeCastTo<RenderFubuViewBehavior>(y.BehaviorType));
        }
    }
}