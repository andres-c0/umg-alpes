@ModelType alpes.Entidades.Cliente

@Code
    ViewData("Title") = "Nuevo Cliente"
End Code

<h2>Nuevo Cliente</h2>

@Using Html.BeginForm()
    @<div class="form-group">
        <label>Tipo Documento</label>
        @Html.TextBoxFor(Function(m) m.TipoDocumento, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Número Documento</label>
        @Html.TextBoxFor(Function(m) m.NumDocumento, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>NIT</label>
        @Html.TextBoxFor(Function(m) m.Nit, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Nombres</label>
        @Html.TextBoxFor(Function(m) m.Nombres, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Apellidos</label>
        @Html.TextBoxFor(Function(m) m.Apellidos, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Email</label>
        @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Teléfono Residencia</label>
        @Html.TextBoxFor(Function(m) m.TelResidencia, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Teléfono Celular</label>
        @Html.TextBoxFor(Function(m) m.TelCelular, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Dirección</label>
        @Html.TextBoxFor(Function(m) m.Direccion, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Ciudad</label>
        @Html.TextBoxFor(Function(m) m.Ciudad, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Departamento</label>
        @Html.TextBoxFor(Function(m) m.Departamento, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>País</label>
        @Html.TextBoxFor(Function(m) m.Pais, New With {.class = "form-control"})
    </div>

    @<div class="form-group">
        <label>Profesión</label>
        @Html.TextBoxFor(Function(m) m.Profesion, New With {.class = "form-control"})
    </div>

    @<button type="submit" class="btn btn-primary">Guardar</button>
End Using