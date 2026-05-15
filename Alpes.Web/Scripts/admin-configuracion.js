(function () {
    var usuarios = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarConfiguracion();
    });

    function enlazarEventos() {
        $('#btnNuevoUsuarioConfig').on('click', function () {
            limpiarModalUsuario();
            $('#modalUsuarioConfigTitulo').text('Nuevo usuario');
            abrirModalUsuario();
        });

        $('#btnCerrarModalUsuarioConfigX, #btnCancelarModalUsuarioConfig, #modalUsuarioConfig .modal-a__backdrop').on('click', function () {
            cerrarModalUsuario();
        });

        $('#btnGuardarUsuarioConfig').on('click', function () {
            guardarUsuario();
        });
    }

    function cargarConfiguracion() {
        $('#configUsuariosListado').html('<div class="table-empty">Cargando usuarios...</div>');

        $.getJSON('/Admin/ConfiguracionData')
            .done(function (res) {
                if (res && res.success === false) {
                    $('#configUsuariosListado').html('<div class="table-empty">' + escapeHtml(res.message || 'No se pudo cargar configuración.') + '</div>');
                    return;
                }

                usuarios = normalizarUsuarios(res.usuarios || []);
                cargarPerfil(res.perfil);
                renderUsuarios();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#configUsuariosListado').html('<div class="table-empty">Error al cargar la configuración.</div>');
            });
    }

    function cargarPerfil(perfil) {
        var username = perfil && (perfil.username || perfil.Username) ? (perfil.username || perfil.Username) : 'admin';
        var email = perfil && (perfil.email || perfil.Email) ? (perfil.email || perfil.Email) : '';
        var rol = perfil && (perfil.rol || perfil.Rol) ? (perfil.rol || perfil.Rol) : 'Administrador';

        $('#configProfileName').text(username);
        $('#configProfileEmail').text(email);
        $('#configProfileRole').text(rol);
        $('#configProfileAvatar').text(obtenerInicial(username));
    }

    function normalizarUsuarios(lista) {
        return lista.map(function (u) {
            return {
                UsuarioId: u.UsuarioId || u.usuarioId || u.USU_ID || u.UsuId || u.usuId || u.Id || u.id || 0,
                Username: u.Username || u.username || u.USERNAME || u.Usuario || u.usuario || '',
                Email: u.Email || u.email || u.EMAIL || '',
                Rol: u.Rol || u.rol || u.ROL || u.RolNombre || u.rolNombre || u.ROL_NOMBRE || 'Usuario'
            };
        });
    }

    function renderUsuarios() {
        if (!usuarios.length) {
            $('#configUsuariosListado').html('<div class="table-empty">No hay usuarios registrados.</div>');
            return;
        }

        var html = '';

        usuarios.forEach(function (u, index) {
            var esUltimo = index === usuarios.length - 1;

            html += '<div class="config-user-row ' + (esUltimo ? 'config-user-row--last' : '') + '">';
            html += '   <div class="config-user-row__left">';
            html += '       <div class="config-user-row__avatar">' + obtenerInicial(u.Username) + '</div>';
            html += '       <div class="config-user-row__body">';
            html += '           <div class="config-user-row__name">' + escapeHtml(valor(u.Username)) + '</div>';
            html += '           <div class="config-user-row__email">' + escapeHtml(valor(u.Email)) + '</div>';
            html += '       </div>';
            html += '   </div>';
            html += '   <div class="config-user-row__actions">';
            html += '       <span class="config-profile__role">' + escapeHtml(valor(u.Rol || 'Usuario')) + '</span>';
            html += '       <button type="button" class="btn-icon" onclick="AdminConfiguracion.editarUsuario(' + entero(u.UsuarioId) + ')"><i class="bi bi-pencil"></i></button>';
            html += '       <button type="button" class="btn-icon btn-icon-danger" onclick="AdminConfiguracion.eliminarUsuario(' + entero(u.UsuarioId) + ')"><i class="bi bi-trash"></i></button>';
            html += '   </div>';
            html += '</div>';
        });

        $('#configUsuariosListado').html(html);
    }

    function editarUsuario(id) {
        var u = usuarios.find(function (x) {
            return entero(x.UsuarioId) === entero(id);
        });

        if (!u) {
            alert('No se encontró el usuario.');
            return;
        }

        $('#modalUsuarioConfigTitulo').text('Editar usuario');
        $('#hidUsuarioConfigId').val(entero(u.UsuarioId));
        $('#txtUsuarioConfigUsername').val(valor(u.Username));
        $('#txtUsuarioConfigEmail').val(valor(u.Email));
        $('#selUsuarioConfigRol').val(valor(u.Rol));

        abrirModalUsuario();
    }

    function guardarUsuario() {
        var id = entero($('#hidUsuarioConfigId').val());
        var username = ($('#txtUsuarioConfigUsername').val() || '').trim();
        var email = ($('#txtUsuarioConfigEmail').val() || '').trim();
        var rol = $('#selUsuarioConfigRol').val();

        if (!username) {
            alert('El username es obligatorio.');
            return;
        }

        if (!email) {
            alert('El email es obligatorio.');
            return;
        }

        var payload = {
            UsuarioId: id,
            Username: username,
            Email: email,
            Rol: rol
        };

        $.ajax({
            url: '/Admin/GuardarUsuarioConfig',
            method: 'POST',
            data: payload
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No se pudo guardar el usuario.');
                    return;
                }

                cerrarModalUsuario();
                cargarConfiguracion();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert('Error al guardar el usuario.');
            });
    }

    function eliminarUsuario(id) {
        if (!confirm('¿Deseas eliminar este usuario?')) {
            return;
        }

        $.ajax({
            url: '/Admin/EliminarUsuarioConfig',
            method: 'POST',
            data: { id: id }
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No se pudo eliminar el usuario.');
                    return;
                }

                cargarConfiguracion();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert('Error al eliminar el usuario.');
            });
    }

    function abrirModalUsuario() {
        $('#modalUsuarioConfig').show();
    }

    function cerrarModalUsuario() {
        $('#modalUsuarioConfig').hide();
        limpiarModalUsuario();
    }

    function limpiarModalUsuario() {
        $('#hidUsuarioConfigId').val(0);
        $('#txtUsuarioConfigUsername').val('');
        $('#txtUsuarioConfigEmail').val('');
        $('#selUsuarioConfigRol').val('Usuario');
    }

    function obtenerInicial(texto) {
        var limpio = valor(texto).trim();
        return limpio ? limpio.charAt(0).toUpperCase() : 'U';
    }

    function entero(v) {
        var n = parseInt(v, 10);
        return isNaN(n) ? 0 : n;
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }

    function escapeHtml(texto) {
        return valor(texto)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    window.AdminConfiguracion = {
        editarUsuario: editarUsuario,
        eliminarUsuario: eliminarUsuario
    };
})();