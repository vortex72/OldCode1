<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.N2CMS.ContentPage>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
	Engine Performance Warehouse - <%= Model.Title %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h1><%= Model.Title %></h1>
<%= Model.Text %>
</asp:Content>
