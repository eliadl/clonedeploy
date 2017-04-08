﻿using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.masters
{
    public partial class ImageMaster : MasterBaseMaster
    {
        private Images imagesBasePage { get; set; }
        public ImageEntity Image { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {
            imagesBasePage = (Page as Images);
            if (imagesBasePage != null) Image = imagesBasePage.Image;
            if (Image == null)
            {
                Level2.Visible = false;
                btnDelete.Visible = false;
                return;
            }

            Level1.Visible = false;
            if (Settings.RequireImageApproval.ToLower() == "true" && Image.Approved != 1)
                btnApproveImage.Visible = true;

            if (Request.QueryString["cat"] == "profiles")
            {
                btnDelete.Visible = false;
            }
  
           
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(Authorizations.DeleteImage, Image.Id);
            lblTitle.Text = "Delete This Image?";
            DisplayConfirm();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(Authorizations.ApproveImage,Image.Id);
            Image.Approved = 1;
            PageBaseMaster.EndUserMessage = imagesBasePage.Call.ImageApi.Put(Image.Id,Image).Success
                ? "Successfully Approved Image"
                : "Could Not Approve Image";
            imagesBasePage.Call.ImageApi.SendImageApprovedMail(Image.Id);
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            var result = imagesBasePage.Call.ImageApi.Delete(Image.Id);
            if (result.Success)
            {
                PageBaseMaster.EndUserMessage = "Successfully Deleted Image";
                Response.Redirect("~/views/images/search.aspx");
            }
            else
            {
                PageBaseMaster.EndUserMessage = result.ErrorMessage;
            }
        }
    }
}