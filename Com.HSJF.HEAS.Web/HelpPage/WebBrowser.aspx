<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebBrowser.aspx.cs" Inherits="Com.HSJF.HEAS.Web.HelpPage.WebBrowser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>浏览</title>
    <style>
    	*{
    		padding:0;
    		margin:0;
    	}
    	html,body,#div,form,img,center{
    		height:100%;
    	}
    </style>
    
</head>
<body>
	<script>
    	window.onload = function(){
    		window.parent.forIframe(document.getElementById("imgdisplay").offsetWidth,document.getElementById("imgdisplay").offsetHeight);
    		document.getElementById("imgdisplay").style.height = document.body.clientHeight;
    	}
    </script>
    <div id="div">
    	<form id="form1" runat="server">
	        <center>
	            <img id="imgdisplay" runat="server"/>
	        </center>
	    </form>
    </div>
</body>
</html>
