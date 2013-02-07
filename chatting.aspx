<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <script src="http://code.jquery.com/jquery-1.7.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-1.0.0-rc2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            var connection = $.connection('/MyConnection');

            connection.received(function(data) {
                $('#messages').append('<li>' + data + '</li>');
            });

            connection.start().done(function() {
                $('#broadcast').click(function() {
                    connection.send($('#msg').val());
                });
            });
        });
    </script>
</head>
<body>
    <div>
        <input type="text" id="msg"/>
        <input type="button" id="broadcast" value="broadcast"/>
        
        <ul id="messages">
        </ul>
    </div>
</body>
</html>
