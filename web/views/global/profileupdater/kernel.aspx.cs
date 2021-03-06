﻿using System;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_profileupdater_kernel : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            PopulateGrid();
        }
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvProfiles);
    }

    protected void PopulateGrid()
    {
        ddlKernel.DataSource = Helpers.Utility.GetKernels();
        ddlKernel.DataBind();
        ddlKernel.SelectedValue = Settings.DefaultKernel64;
        gvProfiles.DataSource = BLL.ImageProfile.GetAllProfiles();
        gvProfiles.DataBind();
    }

    protected void btnUpdateKernel_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var updateCount = 0;
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvProfiles.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(dataKey.Value));
            imageProfile.Kernel = ddlKernel.Text;
            if (BLL.ImageProfile.UpdateProfile(imageProfile).IsValid)
            {
                updateCount++;
            }
        }
        EndUserMessage = "Updated " + updateCount + " Image Profile(s)";
        PopulateGrid();
    }
}