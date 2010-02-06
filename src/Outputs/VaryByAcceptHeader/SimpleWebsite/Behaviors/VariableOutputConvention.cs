using System.Collections;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.View;
using TypeExtensions=FubuMVC.Core.TypeExtensions;

namespace SimpleWebsite.Behaviors
{
    public class VariableOutputConvention : IConfigurationAction
    {

        public void Configure(BehaviorGraph graph)
        {
            graph.Actions().Where(x => x.HasOutputBehavior() && x.OutputType().CanBeCastTo<IEnumerable>() && getRenderViewNode(x) != null)
                .Each(x =>
                {
                    var viewNode = getRenderViewNode(x);


                    var modelType = x.OutputType();
                    var node = new VariableOutputNode(modelType);
                    viewNode.InsertDirectlyBefore(node);
                    //need to figure out how to disconnect existing viewnode, and insert into variable node

                    node.AddOutput(a => a.RenderFormat == "json", new RenderJsonNode(modelType));
                    node.AddOutput(a => a.RenderFormat == "xml", new RenderXmlNode(modelType));

                    graph.Observer.RecordCallStatus(x, "Adding variable output behavior");
                });
        }

        private static OutputNode getRenderViewNode(ActionCall x)
        {
            return x.OfType<OutputNode>().FirstOrDefault(y => TypeExtensions.CanBeCastTo<RenderFubuViewBehavior>(y.BehaviorType));
        }
    }
}