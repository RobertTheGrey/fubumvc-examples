using System.Collections;
using FubuMVC.Core;
using FubuMVC.Core.Registration.Nodes;
using SimpleWebsite.Behaviors;
using SimpleWebsite.Controllers;
using SimpleWebsite.Core;

namespace SimpleWebsite
{
    public class SimpleWebsiteFubuRegistry : FubuRegistry
    {
        public SimpleWebsiteFubuRegistry()
        {
            IncludeDiagnostics(true);

            Applies.ToThisAssembly();

            Actions.IncludeTypesNamed(x => x.EndsWith("Controller"));

            Routes.IgnoreControllerNamespaceEntirely();

            Output.ToJson.WhenCallMatches(action => action.Returns<AjaxResponse>());
            Output.To(call =>
            {
                var node = new VariableOutputNode();
                node.AddOutput(a => true, new RenderHtmlDocumentNode());
                node.AddOutput(a => false, new RenderHtmlTagNode());
                return node;
            }).WhenTheOutputModelIs<IEnumerable>();
            //Views.TryToAttach(findViews => findViews.by_ViewModel_and_Namespace_and_MethodName());

            // Note: Outside of a sample application, you would only configure services that Fubu requires within your FubuRegistry
            // Non-Fubu services should be configured through your container in the usual way (StructureMap Registry, etc)
            Services(s => s.AddService<IRepository, FakeRepository>());
        }
    }
}