﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="views_images_profiles_upload" %>
<%@ MasterType VirtualPath="~/views/images/profiles/profiles.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Master.Image.Id %>&profileid=<%= Master.ImageProfile.Id %>&cat=profiles"><%= Master.ImageProfile.Name %></a></li>
    <li>Upload Options</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#upload').addClass("nav-current");
        });
    </script>
    
      <div class="size-9 column">
        Remove GPT Structures
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRemoveGpt" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Don't Shrink Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrink" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Don't Shrink LVM Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrinkLVM" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Calculate Size Debug
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpDebugResize" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateUpload" runat="server" OnClick="btnUpdateUpload_OnClick" Text="Update Upload Options" CssClass="submits"/>
    </div>
</asp:Content>

