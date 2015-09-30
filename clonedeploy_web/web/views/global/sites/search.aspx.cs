﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class views_global_sites_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindGrid();
        else
        {
            if (gvSites.Rows[0].Cells[0].Text == "No Sites Have Been Created")
            {
                gvSites.Rows[0].Cells.Clear();
                gvSites.Rows[0].Cells.Add(new TableCell());
                gvSites.Rows[0].Cells[0].Text = "No Sites Have Been Created";
            }
        }
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void BindGrid()
    {
        gvSites.DataSource = BllSite.SearchSites(txtSearch.Text);
        gvSites.DataBind();

        if (gvSites.Rows.Count == 0)
        {
            var obj = new List<Models.Site>();
            obj.Add(new Models.Site());
            gvSites.DataSource = obj;
            gvSites.DataBind();

            gvSites.Rows[0].Cells.Clear();
            gvSites.Rows[0].Cells.Add(new TableCell()); 

            gvSites.Rows[0].Cells[0].Text = "No Sites Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var site = new Models.Site
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text
        };

        BllSite.AddSite(site);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSites.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvRow = gvSites.Rows[e.RowIndex];
        var site = new Models.Site
        {
            Id = Convert.ToInt32(gvSites.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text

        };
        BllSite.UpdateSite(site);

        gvSites.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvSites.EditIndex = -1;
        BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BllSite.DeleteSite(Convert.ToInt32(gvSites.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }

   
}