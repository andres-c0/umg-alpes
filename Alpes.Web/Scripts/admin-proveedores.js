(function () {
    var proveedores = [];
    var proveedorSeleccionado = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarProveedores();
    });

    function enlazarEventos() {
        $('#btnRecargarProveedor').on('click', function () {
            cargarProveedores();
        });

        $('#txtBuscarProveedor').on('input', function () {
            renderProveedores();
        });

        $('#btnNuevoProveedor').on('click', function () {
            proveedorSeleccionado = null;
            limpiarFormulario();
            $('#modalProveedorTitulo').text('Nuevo proveedor');
            abrirModal();
        });

        $('#btnGuardarProveedor').on('click', function () {
            guardarProveedor();
        });

        $('#btnCerrarModalProveedor, #btnCerrarModalProveedorX, #modalProveedor .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });
    }

    function cargarProveedores() {
        $('#proveedoresListado').html('<div class="table-empty">Cargando proveedores...</div>');

        $.getJSON('/Proveedor/Index')
            .done(function (res) {
                proveedores = normalizar(res);
                renderProveedores();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#proveedoresListado').html('<div class="table-empty">Error al cargar proveedores.</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderProveedores() {
        var filtro = ($('#txtBuscarProveedor').val() || '').toLowerCase().trim();

        var lista = proveedores.filter(function (p) {
            var texto = (
                valor(p.RazonSocial) + ' ' +
                valor(p.Nit) + ' ' +
                valor(p.Email) + ' ' +
                valor(p.Telefono) + ' ' +
                valor(p.Ciudad) + ' ' +
                valor(p.Pais)
            ).toLowerCase();

            return !filtro || texto.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#proveedoresListado').html('<div class="table-empty">No hay proveedores.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (p) {
            var inicial = valor(p.RazonSocial).charAt(0).toUpperCase() || 'P';

            html += '<div class="cliente-card">';
            html += '  <div class="cliente-card__avatar">' + inicial + '</div>';
            html += '  <div class="cliente-card__body">';
            html += '      <div class="cliente-card__name">' + valor(p.RazonSocial) + '</div>';
            html += '      <div class="cliente-card__meta"><i class="bi bi-credit-card-2-front"></i> ' + valor(p.Nit) + '</div>';
            html += '      <div class="cliente-card__meta"><i class="bi bi-envelope"></i> ' + valor(p.Email) + '</div>';
            html += '      <div class="cliente-card__meta"><i class="bi bi-telephone"></i> ' + valor(p.Telefono) + ' &nbsp;&nbsp; <i class="bi bi-geo-alt"></i> ' + valor(p.Ciudad) + ' • ' + valor(p.Pais) + '</div>';
            html += '  </div>';
            html += '  <div class="cliente-card__actions">';
            html += '      <button type="button" class="btn-icon" onclick="AdminProveedores.editar(' + p.ProvId + ')"><i class="bi bi-pencil"></i></button>';
            html += '      <button type="button" class="btn-icon btn-icon-danger" onclick="AdminProveedores.eliminar(' + p.ProvId + ')"><i class="bi bi-trash"></i></button>';
            html += '  </div>';
            html += '</div>';
        });

        $('#proveedoresListado').html(html);
    }

    function editar(id) {
        $.getJSON('/Proveedor/Obtener', { id: id })
            .done(function (res) {
                if (!res || !res.ProvId) {
                    alert('No se encontró el proveedor.');
                    return;
                }

                proveedorSeleccionado = res;

                $('#modalProveedorTitulo').text('Editar proveedor');
                $('#txtRazonSocial').val(valor(res.RazonSocial));
                $('#txtNit').val(valor(res.Nit));
                $('#txtEmailProveedor').val(valor(res.Email));
                $('#txtTelefonoProveedor').val(valor(res.Telefono));
                $('#txtDireccionProveedor').val(valor(res.Direccion));
                $('#txtCiudadProveedor').val(valor(res.Ciudad));
                $('#txtPaisProveedor').val(valor(res.Pais));

                abrirModal();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al obtener el proveedor.'));
            });
    }

    function guardarProveedor() {
        var data = {
            ProvId: proveedorSeleccionado ? proveedorSeleccionado.ProvId : 0,
            RazonSocial: $('#txtRazonSocial').val(),
            Nit: $('#txtNit').val(),
            Email: $('#txtEmailProveedor').val(),
            Telefono: $('#txtTelefonoProveedor').val(),
            Direccion: $('#txtDireccionProveedor').val(),
            Ciudad: $('#txtCiudadProveedor').val(),
            Pais: $('#txtPaisProveedor').val()
        };

        if (!data.RazonSocial) {
            alert('La razón social es obligatoria.');
            return;
        }

        if (!data.Nit) {
            alert('El NIT es obligatorio.');
            return;
        }

        var url = proveedorSeleccionado
            ? '/Proveedor/Actualizar'
            : '/Proveedor/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible guardar el proveedor.');
                    return;
                }

                cerrarModal();
                cargarProveedores();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al guardar el proveedor.'));
            });
    }

    function eliminarProveedor(id) {
        if (!confirm('¿Deseas eliminar este proveedor?')) {
            return;
        }

        $.ajax({
            url: '/Proveedor/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ ProvId: id })
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarProveedores();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al eliminar el proveedor.'));
            });
    }

    function abrirModal() {
        $('#modalProveedor').show();
    }

    function cerrarModal() {
        $('#modalProveedor').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#txtRazonSocial').val('');
        $('#txtNit').val('');
        $('#txtEmailProveedor').val('');
        $('#txtTelefonoProveedor').val('');
        $('#txtDireccionProveedor').val('');
        $('#txtCiudadProveedor').val('');
        $('#txtPaisProveedor').val('');
        proveedorSeleccionado = null;
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

    window.AdminProveedores = {
        editar: editar,
        eliminar: eliminarProveedor
    };
})();