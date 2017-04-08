﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_buildings_search : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindGrid();
        else
        {
            if (gvBuildings.Rows[0].Cells[0].Text == "No Buildings Have Been Created")
            {
                gvBuildings.Rows[0].Cells.Clear();
                gvBuildings.Rows[0].Cells.Add(new TableCell());
                gvBuildings.Rows[0].Cells[0].Text = "No Buildings Have Been Created";
            }
        }
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void BindGrid()
    {
        gvBuildings.DataSource = Call.BuildingApi.GetAll(Int32.MaxValue, txtSearch.Text);
        gvBuildings.DataBind();

        if (gvBuildings.Rows.Count == 0)
        {
            var obj = new List<BuildingEntity>();
            obj.Add(new BuildingEntity());
            gvBuildings.DataSource = obj;
            gvBuildings.DataBind();

            gvBuildings.Rows[0].Cells.Clear();
            gvBuildings.Rows[0].Cells.Add(new TableCell());

            gvBuildings.Rows[0].Cells[0].Text = "No Buildings Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var building = new BuildingEntity()
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDpAdd")).SelectedValue)
        };

        Call.BuildingApi.Post(building);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvBuildings.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        GridViewRow gvRow = gvBuildings.Rows[e.RowIndex];
        var building = new BuildingEntity()
        {
            Id = Convert.ToInt32(gvBuildings.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDp")).SelectedValue)

        };
        Call.BuildingApi.Put(building.Id,building);

        gvBuildings.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvBuildings.EditIndex = -1;
        BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteGlobal);
        Call.BuildingApi.Delete(Convert.ToInt32(gvBuildings.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }

    protected void gvBuildings_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlDps = null;

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ddlDps = e.Row.FindControl("ddlDpAdd") as DropDownList;
            PopulateClusterGroupsDdl(ddlDps);

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ddlDps = e.Row.FindControl("ddlDp") as DropDownList;
            if (ddlDps != null)
            {
                PopulateClusterGroupsDdl(ddlDps);
                ddlDps.SelectedValue = ((BuildingEntity)(e.Row.DataItem)).DistributionPointId.ToString();
            }
        } 
    }
}