using Sitecore.Diagnostics;
using Sitecore.Form.Core.Attributes;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Pipelines;
using Sitecore.Wffm.AzureUploadField.Logic.Pipelines;
using Sitecore.WFFM.Abstractions.Actions;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace Sitecore.Wffm.AzureUploadField.Logic
{
    [Adapter(typeof(AzureFileUploadAdapter))]
    public class AzureFileUploadField : ValuedFieldViewModel<HttpPostedFileBase>
    {
        [DataType(DataType.Upload)]
        public override HttpPostedFileBase Value
        {
            get;
            set;
        }

        public override string ResultParameters
        {
            get
            {
                return "azuremedia";
            }
        }

        public AzureFileUploadField()
        {
        }

        public override ControlResult GetResult()
        {
            HttpPostedFileBase value = this.Value;
            if (value != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                value.InputStream.CopyTo(memoryStream);

                try
                {
                    var postedFile = new PostedFile(memoryStream.ToArray(), value.FileName, string.Empty);

                    var args = new FormClientUploadPipelineArgs()
                    {
                        PostedFile = postedFile,
                        FieldParameters = this.Parameters,
                        ReturnValue = value
                    };

                    Log.Debug("Azure File Upload: Running the formCLientUpload pipeline...", this);

                    CorePipeline.Run("formClientUpload", args);

                    return new ControlResult(base.FieldItemId, this.Title, args.ReturnValue, this.ResultParameters, false);
                }
                catch (Exception e)
                {
                    Log.Warn("Exception during processing the form upload...", e, this);
                }
            }
            return new ControlResult(base.FieldItemId, this.Title, null, this.ResultParameters, false);
        }

        public override void SetValueFromQuery(string valueFromQuery)
        {
        }
    }
}
