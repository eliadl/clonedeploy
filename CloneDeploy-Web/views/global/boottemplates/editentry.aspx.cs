﻿using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_boottemplates_editentry : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtName.Text = base.BootEntry.Name;
        txtDescription.Text = base.BootEntry.Description;
        txtContents.Text = base.BootEntry.Content;
        ddlType.Text = BootEntry.Type;
        txtOrder.Text = BootEntry.Order;
        if (BootEntry.Active == 1) chkActive.Checked = true;
        if (BootEntry.Default == 1) chkDefault.Checked = true;
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var bootEntry = new BootEntryEntity
        {
            Id = BootEntry.Id,
            Name = txtName.Text,
            Description = txtDescription.Text,
            Content = txtContents.Text,
            Type = ddlType.Text,
            Order = txtOrder.Text,
            Active = chkActive.Checked ? 1 : 0,
            Default = chkDefault.Checked ? 1 : 0
        };

        var result = Call.BootEntryApi.Put(bootEntry.Id,bootEntry);
        EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Boot Menu Entry";

    }
}