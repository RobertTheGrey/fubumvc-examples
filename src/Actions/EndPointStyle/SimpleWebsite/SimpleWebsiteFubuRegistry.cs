using System;
using System.Collections.Generic;
using FubuMVC.Core;
using SimpleWebsite.Core;
using SimpleWebsite.EndPoints;

namespace SimpleWebsite
{
    public class SimpleWebsiteFubuRegistry : FubuRegistry
    {
        public SimpleWebsiteFubuRegistry()
        {
            IncludeDiagnostics(true);

            Applies.ToThisAssembly();

            var httpVerbs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
                {"GET", "POST", "PUT", "HEAD"};

            Actions
                .IncludeTypes(t => t.Namespace.StartsWith(typeof(EndPointUrlPolicy).Namespace) && t.Name.EndsWith("Endpoint"))
                .IncludeMethods(action => httpVerbs.Contains(action.Method.Name));

            httpVerbs.Each(verb => Routes.ConstrainToHttpMethod(action => action.Method.Name.Equals(verb, StringComparison.InvariantCultureIgnoreCase), verb));

            Routes.UrlPolicy<EndPointUrlPolicy>();

            Output.ToJson.WhenCallMatches(action => action.Returns<AjaxResponse>());

            Views.TryToAttach(findViews => findViews.by_ViewModel_and_Namespace());

            // Note: Outside of a sample application, you would only configure services that Fubu requires within your FubuRegistry
            // Non-Fubu services should be configured through your container in the usual way (StructureMap Registry, etc)
            Services(s => s.AddService<IRepository, FakeRepository>());
        }
    }
}