using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using System.Linq;

namespace SimpleWebsite.Behaviors
{
    public class RenderVariableOutput : BasicBehavior
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly IOutputWriter _outputWriter;
        private readonly Candidate _outputs;

        public RenderVariableOutput(IFubuRequest fubuRequest, IOutputWriter outputWriter, Candidate outputs) : base(PartialBehavior.Executes)
        {
            _fubuRequest = fubuRequest;
            _outputWriter = outputWriter;
            _outputs = outputs;
        }

        protected override DoNext performInvoke()
        {
            var detector = _fubuRequest.Get<AcceptTypeDetector>();

            var output = detector.Accept + "\n";
            var behavior = _outputs.GetBehavior(detector);
            output += "behavior: "  + string.Format("{0}", behavior);
            _outputWriter.Write("text/plain", output);
            return DoNext.Continue;
        }
    }

    public class AcceptTypeDetector
    {
        public string Accept { get; set; }
    }

    public class Candidate
    {
        public Candidate(Func<AcceptTypeDetector, bool> match, IActionBehavior behavior)
        {
            Match = match;
            Behavior = behavior;
        }

        public Candidate Inner { get; set; }
        public Func<AcceptTypeDetector, bool> Match { get; set; }
        public IActionBehavior Behavior { get; set; }

        public IActionBehavior GetBehavior(AcceptTypeDetector detector)
        {
            if (Match(detector))
            {
                return Behavior;
            }
            return Inner == null ? null : Inner.GetBehavior(detector);
        }
    }
}