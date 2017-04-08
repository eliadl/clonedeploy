﻿using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.images
{
    public partial class ImageCreate : Images
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            

            chkVisible.Checked = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.CreateImage);
            var image = new ImageEntity()
            {
                Name = txtImageName.Text,
                Os = "",
                Environment = ddlEnvironment.Text,
                Description = txtImageDesc.Text,
                Protected = chkProtected.Checked ? 1 : 0,
                IsVisible = chkVisible.Checked ? 1 : 0,
                Enabled = 1
            };

            image.Type = ddlEnvironment.Text == "macOS" ? "Block" : ddlImageType.Text;
            image.Type = ddlEnvironment.Text == "winpe" ? "File" : ddlImageType.Text;
            image.OsxType = ddlEnvironment.Text == "macOS" ? "thick" : "";
           
           
            var result = Call.ImageApi.Post(image);
            if (result.Success)
            {
                EndUserMessage = "Successfully Added Image";
                Response.Redirect("~/views/images/edit.aspx?imageid=" + result.Id);
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }

        }

        protected void ddlEnvironment_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEnvironment.Text == "macOS" || ddlEnvironment.Text == "winpe")
            {
                imageType.Visible = false;
               
            }
            else
            {
                imageType.Visible = true;
               
            }
        }

        protected void ddlOsxImageType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOsxImageType.Text == "thin")
            {
                thinImage.Visible = true;
                ddlThinOS.DataSource = Call.FilesystemApi.GetThinImages();
                ddlThinOS.DataBind();
                ddlThinRecovery.DataSource = Call.FilesystemApi.GetThinImages();
                ddlThinRecovery.DataBind();
            }
            else
            {
                thinImage.Visible = false;
            }
        }
    }
}