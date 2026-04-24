(function () {
    var clientes = [];
    var clienteSeleccionado = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarClientes();
    });

    function enlazarEventos() {
        $('#btnRecargarCliente').on('click', function () {
            cargarClientes();
        });

        $('#txtBuscarCliente').on('input', function () {
            renderClientes();
        });

        $('#btnNuevoCliente').on('click', function () {
            clienteSeleccionado = null;
            limpiarFormulario();
            $('#modalClienteTitulo').text('Nuevo cliente');
            abrirModal();
        });

        $('#btnGuardarCliente').on('click', function () {
            guardarCliente();
        });

        $('#btnCerrarModal, #btnCerrarModalX, #modalCliente .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });
    }

    function cargarClientes() {
        $('#clientesListado').html('<div class="table-empty">Cargando clientes...</div>');

        $.getJSON('/Cliente/ListarJson')
            .done(function (res) {
                clientes = normalizar(res);
                renderClientes();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#clientesListado').html('<div class="table-empty">Error al cargar clientes.</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderClientes() {
        var filtro = ($('#txtBuscarCliente').val() || '').toLowerCase().trim();

        var lista = clientes.filter(function (c) {
            var texto = (
                (c.NombreCompleto || '') + ' ' +
                (c.Email || '') + ' ' +
                (c.Ciudad || '') + ' ' +
                (c.Pais || '')
            ).toLowerCase();

            return !filtro || texto.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#clientesListado').html('<div class="table-empty">No hay clientes.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (c) {
            var inicial = (c.NombreCompleto || 'C').charAt(0).toUpperCase();

            html += '<div class="cliente-card">';
            html += '  <div class="cliente-card__avatar">' + inicial + '</div>';
            html += '  <div class="cliente-card__body">';
            html += '      <div class="cliente-card__name">' + valor(c.NombreCompleto) + '</div>';
            html += '      <div class="cliente-card__meta"><i class="bi bi-envelope"></i> ' + valor(c.Email) + '</div>';
            html += '      <div class="cliente-card__meta"><i class="bi bi-geo-alt"></i> ' + valor(c.Ciudad) + ' • ' + valor(c.Pais) + '</div>';
            html += '  </div>';
            html += '  <div class="cliente-card__actions">';
            html += '      <button type="button" class="btn-icon" onclick="AdminClientes.editar(' + c.CliId + ')"><i class="bi bi-pencil"></i></button>';
            html += '      <button type="button" class="btn-icon btn-icon-danger" onclick="AdminClientes.eliminar(' + c.CliId + ')"><i class="bi bi-trash"></i></button>';
            html += '  </div>';
            html += '</div>';
        });

        $('#clientesListado').html(html);
    }

    function editar(id) {
        var c = clientes.find(function (x) { return x.CliId == id; });

        if (!c) {
            alert('No se encontró el cliente.');
            return;
        }

        clienteSeleccionado = c;

        $('#modalClienteTitulo').text('Editar cliente');

        $('#txtTipoDocumento').val(valor(c.TipoDocumento));
        $('#txtNumDocumento').val(valor(c.NumDocumento));
        $('#txtNombres').val(valor(c.Nombres));
        $('#txtApellidos').val(valor(c.Apellidos));
        $('#txtEmail').val(valor(c.Email));
        $('#txtDireccion').val(valor(c.Direccion));
        $('#txtCiudad').val(valor(c.Ciudad));
        $('#txtDepartamento').val(valor(c.Departamento));
        $('#txtPais').val(valor(c.Pais));

        abrirModal();
    }

    function guardarCliente() {
        var data = {
            CliId: clienteSeleccionado ? clienteSeleccionado.CliId : 0,
            TipoDocumento: $('#txtTipoDocumento').val(),
            NumDocumento: $('#txtNumDocumento').val(),
            Nombres: $('#txtNombres').val(),
            Apellidos: $('#txtApellidos').val(),
            Email: $('#txtEmail').val(),
            Direccion: $('#txtDireccion').val(),
            Ciudad: $('#txtCiudad').val(),
            Departamento: $('#txtDepartamento').val(),
            Pais: $('#txtPais').val()
        };

        if (!data.Nombres || !data.Apellidos) {
            alert('Nombres y apellidos son obligatorios.');
            return;
        }

        if (!data.Email) {
            alert('El email es obligatorio.');
            return;
        }

        var url = clienteSeleccionado
            ? '/Cliente/Actualizar'
            : '/Cliente/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            data: data
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible guardar el cliente.');
                    return;
                }

                cerrarModal();
                cargarClientes();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al guardar el cliente.'));
            });
    }

    function eliminarCliente(id) {
        if (!confirm('¿Deseas eliminar este cliente?')) {
            return;
        }

        $.ajax({
            url: '/Cliente/Eliminar',
            type: 'POST',
            data: { CliId: id }
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarClientes();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al eliminar el cliente.'));
            });
    }

    function abrirModal() {
        $('#modalCliente').show();
    }

    function cerrarModal() {
        $('#modalCliente').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#txtTipoDocumento').val('');
        $('#txtNumDocumento').val('');
        $('#txtNombres').val('');
        $('#txtApellidos').val('');
        $('#txtEmail').val('');
        $('#txtDireccion').val('');
        $('#txtCiudad').val('');
        $('#txtDepartamento').val('');
        $('#txtPais').val('');
        clienteSeleccionado = null;
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }

    function obtenerMensaje(xhr, mensajeDefault) {
        try {
            if (xhr.responseJSON && xhr.responseJSON.message) {
                return xhr.responseJSON.message;
            }
            return mensajeDefault;
        } catch (e) {
            return mensajeDefault;
        }
    }

    window.AdminClientes = {
        editar: editar,
        eliminar: eliminarCliente
    };
})();