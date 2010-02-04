using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using System.Linq;

namespace SimpleWebsite.Behaviors
{
    public class VariableOutputNode : OutputNode
    {
        private readonly IList<Output> _outputs = new List<Output>();

        public VariableOutputNode() : base(typeof(RenderVariableOutput))
        {
        }

        public void AddOutput(Func<AcceptTypeDetector, bool> isMatch, OutputNode output)
        {
            _outputs.Add(new Output(isMatch, output));
        }

        protected override void configureObject(ObjectDef def)
        {
            ObjectDef currentCandidate = null;
            foreach (var pair in _outputs.Reverse())
            {
                var candidate = new ObjectDef(typeof(Candidate));
                candidate.Child(typeof(Func<AcceptTypeDetector, bool>), pair.IsMatch);
                candidate.Dependencies.Add(new ConfiguredDependency { Definition = pair.Output1.ToObjectDef(), DependencyType = typeof(IActionBehavior) });
                if (currentCandidate != null)
                {
                    candidate.Dependencies.Add(new ConfiguredDependency { Definition = currentCandidate, DependencyType = typeof(Candidate) });
                }
                currentCandidate = candidate;
            }

            def.Dependencies.Add(new ConfiguredDependency { Definition = currentCandidate, DependencyType = typeof(Candidate) });
        }

        class Output
        {
            private readonly Func<AcceptTypeDetector, bool> _isMatch;
            private readonly OutputNode _output;
            public Func<AcceptTypeDetector, bool> IsMatch { get { return _isMatch; } }
            public OutputNode Output1 { get { return _output; } }

            public Output(Func<AcceptTypeDetector, bool> isMatch, OutputNode output)
            {
                _isMatch = isMatch;
                _output = output;
            }
        }
    }


}