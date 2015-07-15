<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WhiteBoard.aspx.cs"
    Inherits="SuperWebSocketWeb.WhiteBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        
        $(document).ready(function () {
            Connect("<%= Request.Url.Host %>", "<%= WebSocketPort %>", "#drawing-canvus", "#messageBox","#info");
            InitializeWhiteBoard("#messageBox","#messageBoard","#drawing-canvus","#draw","#info","#imgMode","#clear");
            LoadPreviousActivity();
        }); 

        function LoadPreviousActivity()
        {
            <%
            if(SuperWebSocket.ApplicationData.Data != null)
            {
                List<string> history = SuperWebSocket.ApplicationData.Data as List<string>;
                if (history != null)
                {
                    foreach (string activity in history)
                    {
                        %>
                            AppendActivity('<%=activity %>');
                        <% 
                    }
                }
            }
            %>
        }


    </script>
    <div style="float: left;">
        <div id="draw">
        <canvas width="700" height="500" id="drawing-canvus"></canvas>
        </div>
        
        <div style="margin-top:5px;">
            <span>Currently selected : <a id="ink" href="javascript:void(0)">
                <img id="imgMode" src="Images/pencil.jpg" title="Click to select Eraser" /></a></span> <span><a
                    id="clear" href="javascript:void(0)">Clear Board</a></span> <span id="info">
                    </span>
        </div>
    </div>
    <div id="messageArea">
        <div>
            <div id="conversation">Conversation</div>
            <div id="messageBoard"></div>
        </div>
        <div>
            <textarea id="messageBox"></textarea>
        </div>
        <div style="text-align: right">
            <input type="button" id="btnSend" value="Send" onclick="return sendChatMessage('#messageBox')" />
        </div>
    </div>
</asp:Content>
