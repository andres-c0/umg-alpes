@ModelType System.Data.DataTable

@Code
    ViewData("Title") = "Clientes"
End Code

<h2>Listado de Clientes</h2>

<p>
    @Html.ActionLink("Nuevo Cliente", "Create")
</p>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>ID</th>
            <th>Documento</th>
            <th>Nombre</th>
            <th>Email</th>
            <th>Ciudad</th>
            <th>Estado</th>
        </tr>
    </thead>
    <tbody>
        @For Each row As Data.DataRow In Model.Rows
            @<tr>
                <td>@row("CLI_ID")</td>
                <td>@row("NUM_DOCUMENTO")</td>
                <td>@row("NOMBRE_COMPLETO")</td>
                <td>@row("EMAIL")</td>
                <td>@row("CIUDAD")</td>
                <td>@row("ESTADO")</td>
            </tr>
        Next
    </tbody>
</table>