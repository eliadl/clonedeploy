﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Helpers;
using Models;

namespace views.hosts
{
    public partial class Searchhosts : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Settings.DefaultHostView == "all")
                PopulateGrid();
        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvHosts.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvHosts.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                BllComputer.DeleteComputer(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            List<Models.Computer> listHosts = (List<Models.Computer>) gvHosts.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Name).ToList()
                        : listHosts.OrderByDescending(h => h.Name).ToList();
                    break;
                case "Mac":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Mac).ToList()
                        : listHosts.OrderByDescending(h => h.Mac).ToList();
                    break;
                case "Image":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Image).ToList()
                        : listHosts.OrderByDescending(h => h.Image).ToList();
                    break;
            }


            gvHosts.DataSource = listHosts;
            gvHosts.DataBind();

        }

        protected void PopulateGrid()
        {
            gvHosts.DataSource = BllComputer.SearchComputers(txtSearch.Text);
            gvHosts.DataBind();

            lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + BllComputer.TotalCount() + " Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvHosts);
        }
    }
}