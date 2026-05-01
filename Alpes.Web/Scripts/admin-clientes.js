(function () {
    var clientes = [];
    var clienteSeleccionado = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarClientes();
    });

    function enlazarEventos() {
        $('#txtBuscarCliente').on('input', renderClientes);

        $('#btnNuevoCliente').on('click', function () {
            clienteSeleccionado = null;
            limpiarFormulario();
            $('#modalClienteTitulo').text('Nuevo cliente');
            abrirModal();
        });

        $('#btnGuardarCliente').on('click', guardarCliente);

        $('#btnCerrarModal, #btnCerrarModalX, #modalCliente .modal-a__backdrop').on('click', cerrarModal);
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
                valor(c.NombreCompleto) + ' ' +
                valor(c.Nombres) + ' ' +
                valor(c.Apellidos) + ' ' +
                valor(c.Email) + ' ' +
                telefonoCliente(c) + ' ' +
                valor(c.Pais) + ' ' +
                valor(c.Departamento) + ' ' +
                valor(c.Ciudad) + ' ' +
                valor(c.Direccion)
            ).toLowerCase();

            return !filtro || texto.indexOf(filtro) >= 0;
        });

        $('#countClientes').text(clientes.length);
        $('#clientesTotalTexto').text(lista.length + ' cliente' + (lista.length === 1 ? '' : 's'));

        renderAlertaIncompletos(clientes);

        if (!lista.length) {
            $('#clientesListado').html('<div class="table-empty">No hay clientes.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (c, index) {
            var inicial = (valor(c.NombreCompleto) || valor(c.Nombres) || 'C').charAt(0).toUpperCase();
            var nombre = valor(c.NombreCompleto) || (valor(c.Nombres) + ' ' + valor(c.Apellidos)).trim();
            var estado = valor(c.Estado || 'ACTIVO').toUpperCase();
            var extraId = 'cliente-extra-' + c.CliId;
            var abierto = index === 0 ? ' style="display:block;"' : '';

            html += ''
                + '<div class="cliente-card-mobile">'
                + '  <div class="cliente-card-mobile__main">'
                + '    <div class="cliente-card-mobile__avatar">' + inicial + '</div>'
                + '    <div class="cliente-card-mobile__body">'
                + '      <div class="cliente-card-mobile__name">' + nombre + '</div>'
                + '      <div class="cliente-card-mobile__meta"><i class="bi bi-envelope"></i> ' + valor(c.Email || 'Sin email') + '</div>'
                + '      <div class="cliente-card-mobile__meta"><i class="bi bi-telephone"></i> ' + valor(telefonoCliente(c) || 'Sin teléfono') + '</div>'
                + '      <div class="cliente-card-mobile__meta"><i class="bi bi-geo-alt"></i> ' + valor(c.Ciudad || 'Sin ciudad') + ', ' + valor(c.Pais || 'Sin país') + '</div>'
                + '    </div>'
                + '    <div class="cliente-card-mobile__right">'
                + '      <span class="cliente-status ' + (estado === 'ACTIVO' ? 'cliente-status--activo' : 'cliente-status--inactivo') + '">● ' + capitalizar(estado) + '</span>'
                + '      <button type="button" class="cliente-icon-btn" onclick="AdminClientes.editar(' + c.CliId + ')"><i class="bi bi-pencil"></i></button>'
                + '      <button type="button" class="cliente-icon-btn cliente-icon-btn--danger" onclick="AdminClientes.eliminar(' + c.CliId + ')"><i class="bi bi-trash"></i></button>'
                + '      <button type="button" class="cliente-icon-btn" onclick="AdminClientes.toggle(' + c.CliId + ')"><i class="bi bi-chevron-down"></i></button>'
                + '    </div>'
                + '  </div>'
                + '  <div class="cliente-card-mobile__extra" id="' + extraId + '"' + abierto + '>'
                + '    <div class="cliente-extra-title">Información adicional</div>'
                + '    <div class="cliente-extra-row"><span>Documento</span><strong>' + valor(c.TipoDocumento) + ': ' + valor(c.NumDocumento) + '</strong></div>'
                + '    <div class="cliente-extra-row"><span>País</span><strong>' + valor(c.Pais) + '</strong></div>'
                + '    <div class="cliente-extra-row"><span>Departamento</span><strong>' + valor(c.Departamento) + '</strong></div>'
                + '    <div class="cliente-extra-row"><span>Ciudad</span><strong>' + valor(c.Ciudad) + '</strong></div>'
                + '    <div class="cliente-extra-row"><span>Dirección</span><strong>' + valor(c.Direccion) + '</strong></div>'
                + '  </div>'
                + '</div>';
        });

        $('#clientesListado').html(html);
    }

    function renderAlertaIncompletos(lista) {
        var incompletos = lista.filter(function (c) {
            return !telefonoCliente(c) || !valor(c.Direccion);
        });

        if (!incompletos.length) {
            $('#alertClientes').hide();
            return;
        }

        var c = incompletos[0];
        var nombre = valor(c.NombreCompleto) || (valor(c.Nombres) + ' ' + valor(c.Apellidos)).trim();
        var falta = !telefonoCliente(c) ? 'teléfono' : 'dirección';
        var inicial = (nombre || 'C').charAt(0).toUpperCase();

        $('#alertClientes').show().html(
            '<div class="clientes-alert__title"><i class="bi bi-exclamation-triangle"></i> Clientes pendientes de completar información (' + incompletos.length + ')</div>' +
            '<div class="clientes-alert__item">' +
            '  <div class="clientes-alert__avatar">' + inicial + '</div>' +
            '  <div><strong>' + nombre + '</strong><span>Falta: ' + falta + '</span></div>' +
            '</div>'
        );
    }

    function toggleCliente(id) {
        $('#cliente-extra-' + id).slideToggle(180);
    }

    function editar(id) {
    var c = clientes.find(function (x) { return x.CliId == id; });

    if (!c) {
        alert('No se encontró el cliente.');
        return;
    }

    console.log('CLIENTE EDITAR:', c);

    clienteSeleccionado = c;

    $('#modalClienteTitulo').text('Editar cliente');

    $('#txtTipoDocumento').val(valor(c.TipoDocumento));
    $('#txtNumDocumento').val(valor(c.NumDocumento));
    $('#txtNombres').val(valor(c.Nombres));
    $('#txtApellidos').val(valor(c.Apellidos));
    $('#txtEmail').val(valor(c.Email));

    $('#txtTelResidencia').val(valor(c.TelResidencia || c.TEL_RESIDENCIA));
    $('#txtTelCelular').val(valor(c.TelCelular || c.TEL_CELULAR));

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
            TelResidencia: $('#txtTelResidencia').val(),
            TelCelular: $('#txtTelCelular').val(),
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

        var url = clienteSeleccionado ? '/Cliente/Actualizar' : '/Cliente/Insertar';

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
        if (!confirm('¿Deseas eliminar este cliente?')) return;

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
        $('#txtTelResidencia').val('');
        $('#txtTelCelular').val('');
        $('#txtDireccion').val('');
        $('#txtCiudad').val('');
        $('#txtDepartamento').val('');
        $('#txtPais').val('');
        clienteSeleccionado = null;
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }
    function telefonoCliente(c) {
    return valor(
        c.Telefono ||
        c.TELEFONO ||
        c.TelCelular ||
        c.TEL_CELULAR ||
        c.TelResidencia ||
        c.TEL_RESIDENCIA ||
        ''
    );
}
    function capitalizar(v) {
        v = valor(v).toLowerCase();
        return v.charAt(0).toUpperCase() + v.slice(1);
    }

    function obtenerMensaje(xhr, mensajeDefault) {
        try {
            if (xhr.responseJSON && xhr.responseJSON.message) return xhr.responseJSON.message;
            return mensajeDefault;
        } catch (e) {
            return mensajeDefault;
        }
    }

    window.AdminClientes = {
        editar: editar,
        eliminar: eliminarCliente,
        toggle: toggleCliente
    };
})();