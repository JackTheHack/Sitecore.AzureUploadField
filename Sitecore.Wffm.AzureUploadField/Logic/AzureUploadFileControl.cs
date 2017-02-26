using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using Sitecore.Form.Web.UI.Controls;
using Sitecore.WFFM.Abstractions.Actions;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Wffm.AzureUploadField.Logic
{
    [Adapter(typeof(AzureFileUploadAdapter))]
    public class AzureUploadFileControl : ValidateControl, IHasTitle
    {
        private static readonly string baseCssClassName = "scfFileUploadBorder";

        protected Panel generalPanel = new Panel();

        protected System.Web.UI.WebControls.Label title = new System.Web.UI.WebControls.Label();

        protected FileUpload upload = new FileUpload();

        public override string ID
        {
            get
            {
                return this.upload.ID;
            }
            set
            {
                this.title.ID = value + "text";
                this.upload.ID = value;
                base.ID = value + "scope";
            }
        }

        

        public override ControlResult Result
        {
            get
            {
                if (this.upload.HasFile)
                {
                    return new ControlResult(this.ControlName, new PostedFile(this.upload.FileBytes, this.upload.FileName, string.Empty), "medialink")
                    {
                        AdaptForAnalyticsTag = false
                    };
                }
                return new ControlResult(this.ControlName, null, string.Empty);
            }
            set
            {
            }
        }

        public string Title
        {
            get
            {
                return this.title.Text;
            }
            set
            {
                this.title.Text = value;
            }
        }

        [VisualProperty("Azure Container:", 700),VisualCategory("Azure Upload"),  DefaultValue("default")]
        public string AzureContainer { get; set; }

        [VisualFieldType(typeof(BooleanField)), VisualCategory("Azure Upload"), VisualProperty("Resize Media:", 700), DefaultValue(false)]
        public string ResizeMedia { get; set; }

        [VisualProperty("Resize Width:", 700), VisualCategory("Azure Upload"), DefaultValue("1000")]
        public int MaxWidth { get; set; }

        [VisualProperty("Allowed File Size Limit:", 700), VisualCategory("Azure Upload"), DefaultValue("10000")]
        public string FileSizeLimit { get; set; }

        
        [VisualProperty("Allowed File Extensions:", 700), VisualCategory("Azure Upload"), DefaultValue(".jpg,.jpeg,.png,.bmp")]
        public string FileExtensions{ get; set; }

        protected override Control ValidatorContainer
        {
            get
            {
                return this;
            }
        }

        protected override Control InnerValidatorContainer
        {
            get
            {
                return this.generalPanel;
            }
        }

        public AzureUploadFileControl(HtmlTextWriterTag tag) : base(tag)
        {
            this.CssClass = AzureUploadFileControl.baseCssClassName;
        }

        public AzureUploadFileControl() : this(HtmlTextWriterTag.Div)
        {
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            this.DoRender(writer);
        }

        protected virtual void DoRender(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
        }

        protected override void OnInit(EventArgs e)
        {
            this.upload.CssClass = "scfFileUpload";
            this.help.CssClass = "scfFileUploadUsefulInfo";
            this.title.CssClass = "scfFileUploadLabel";
            this.title.AssociatedControlID = this.upload.ID;
            this.generalPanel.CssClass = "scfFileUploadGeneralPanel";
            this.Controls.AddAt(0, this.generalPanel);
            this.Controls.AddAt(0, this.title);
            this.generalPanel.Controls.AddAt(0, this.help);
            this.generalPanel.Controls.AddAt(0, this.upload);
        }
    }
}
