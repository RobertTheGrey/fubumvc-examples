using System;
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
        private readonly ConditionalOutput _outputs;

        public RenderVariableOutput(IFubuRequest fubuRequest, IOutputWriter outputWriter, ConditionalOutput outputs) : base(PartialBehavior.Executes)
        {
            _fubuRequest = fubuRequest;
            _outputWriter = outputWriter;
            _outputs = outputs;
        }

        protected override DoNext performInvoke()
        {
            var detector = _fubuRequest.Get<OutputFormatDetector>();

            var behavior = _outputs.GetOutputBehavior(detector);
            if (behavior != null)
            {
                behavior.Invoke();
            }
            else
            {
                var output = detector.Accept + "\n";
                output += "behavior: " + string.Format("{0}", behavior);
                _outputWriter.Write("text/plain", output);
            }
            return DoNext.Continue;
        }
    }

    public class OutputFormatDetector
    {
        public string Accept { get; set; }
        public string RenderFormat { get; set; }

        public bool AcceptsFormat(string format)
        {
            var rawFormats = Accept.Split(',').Select(f => f.Split(';')[0]);
            return rawFormats.Contains(format);
        }
    }

    public class ConditionalOutput
    {
        public ConditionalOutput(Func<OutputFormatDetector, bool> condition, IActionBehavior behavior)
        {
            Condition = condition;
            Behavior = behavior;
        }

        public ConditionalOutput Inner { get; set; }
        public Func<OutputFormatDetector, bool> Condition { get; set; }
        public IActionBehavior Behavior { get; set; }

        public IActionBehavior GetOutputBehavior(OutputFormatDetector detector)
        {
            if (Condition(detector))
            {
                return Behavior;
            }
            return Inner == null ? null : Inner.GetOutputBehavior(detector);
        }
    }
}