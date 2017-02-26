using Sitecore.Diagnostics;
using Sitecore.WFFM.Abstractions.Data;
using Sitecore.WFFM.Abstractions.Shared;
using Sitecore.Form.Core.Client.Submit;
using Newtonsoft.Json;
using System.Linq;
using Sitecore.WFFM.Abstractions.Dependencies;
using System.Collections.Generic;

namespace Sitecore.Wffm.AzureUploadField.Logic
{
    public class AzureFileUploadAdapter : Adapter
    {
        private readonly IResourceManager resourceManager;

        public AzureFileUploadAdapter() : this(DependenciesManager.ResourceManager)
        {
        }

        public AzureFileUploadAdapter(IResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }  

        public override string AdaptResult(IFieldItem field, object value)
        {
            AzurePostedFile postedFile = value as AzurePostedFile;                       

            Log.Info("+++ Adapting the result for Azure file " + value, this);

            if (postedFile == null)
            {
                return string.Empty;
            }

            Log.Info("+++ File url - " + postedFile.Url, this);

            var serializedValue = JsonConvert.SerializeObject(new { Url = postedFile.Url, AspectRatio = postedFile.AspectRatio });
            return serializedValue;

            //return HttpUtility.UrlEncode(postedFile.Url) + "&" + HttpUtility.UrlEncode(postedFile.AspectRatio);
        }

        public override string AdaptToSitecoreStandard(IFieldItem field, string value)
        {
            return string.Empty;            
        }

        public override string AdaptToFriendlyValue(IFieldItem field, string value)
        {
            return string.Empty;            
        }
    }
}
