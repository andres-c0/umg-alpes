@Code
    Dim username As String = System.Convert.ToString(ViewData("Username"))

    If String.IsNullOrWhiteSpace(username) AndAlso Session("Username") IsNot Nothing Then
        username = Session("Username").ToString()
    End If

    If String.IsNullOrWhiteSpace(username) Then
        username = "Cliente"
    End If

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
    If String.IsNullOrWhiteSpace(cliIdTexto) AndAlso Session("CliId") IsNot Nothing Then
        cliIdTexto = Session("CliId").ToString()
    End If
End Code
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData("Title") - Muebles de los Alpes</title>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/css")
    <link rel="stylesheet" href="@Url.Content("~/Content/portal-cliente.css")" />
</head>
<body class="pp-body" data-cli-id="@cliIdTexto" data-username="@username">
    @RenderBody()

    @Scripts.Render("~/bundles/jquery")
    @RenderSection("scripts", required:=False)
</body>
</html>