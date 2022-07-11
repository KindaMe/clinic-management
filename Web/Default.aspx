<%@ Page Title="Strona Główna" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="MainThing">
        <h1>Wybierz Specjalizację:</h1>
        <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" Height="176px" Width="348px" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"></asp:ListBox>

        <h1>Wybierz Lekarza:</h1>
        <asp:ListBox ID="ListBox2" runat="server" AutoPostBack="True" Height="176px" Width="348px" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged"></asp:ListBox>

        <h1>Najbliższa Wola Wizyta:</h1>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="&nbsp;"></asp:Label></h2>

        <p>&nbsp;</p>
        <p>&nbsp;</p>

        <h1>Rejestracja:</h1>
        <p>Dzwoń pod numer</p>
        <p><b>666-777-888</b></p>
        <p>aby zarejestrować się już teraz!</p>
    </div>

</asp:Content>