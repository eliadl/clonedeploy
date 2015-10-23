﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="views.hosts.HostLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?hostid=<%= Computer.Id %>" ><%= Computer.Name %></a></li>
    <li>Logs</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#log').addClass("nav-current");
        });
    </script>
  
    <div id="SearchLogs" runat="server">
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvLogs" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvLogs_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
          
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="LogTime" HeaderText="Time" SortExpression="LogTime" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="SubType" HeaderText="Type" SortExpression="SubType" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
          
              <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnView" runat="server" OnClick="btnView_OnClick" Text="View"/>
                </ItemTemplate>
            </asp:TemplateField>
         <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_OnClick" Text="Export"/>
                </ItemTemplate>
            </asp:TemplateField>
             </Columns>
        <EmptyDataTemplate>
            No Logs Found
        </EmptyDataTemplate>
    </asp:GridView>
        </div>
    
    <div id="ViewLog" runat="server" Visible="False">
       <asp:GridView ID="gvLogView" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>
    </div>

</asp:Content>