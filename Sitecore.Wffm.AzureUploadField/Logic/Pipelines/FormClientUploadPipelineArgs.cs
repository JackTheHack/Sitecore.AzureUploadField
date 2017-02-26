using Sitecore.Pipelines;
using Sitecore.WFFM.Abstractions.Actions;
using System.Collections.Generic;

namespace Sitecore.Wffm.AzureUploadField.Logic.Pipelines
{
    public class FormClientUploadPipelineArgs : PipelineArgs
    {
        public object ReturnValue { get; set; }

        public Dictionary<string, string> FieldParameters { get; set; }

        public PostedFile PostedFile { get; set; }
        public string AspectRatioValue { get; set; }

        public ControlResult ControlResult { get; set; }

        public bool StopProcessing { get; set; }


        public FormClientUploadPipelineArgs() {

        }
    }
}
