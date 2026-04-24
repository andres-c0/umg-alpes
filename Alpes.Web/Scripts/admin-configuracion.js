(function () {
    var usuarios = [
        { UsuarioId: 1, Username: 'cliente1', Email: 'cliente@alpes.com', Rol: 'Cliente' },
        { UsuarioId: 2, Username: 'admin1', Email: 'admin@alpes.com', Rol: 'Administrador' }
    ];

    $(document).ready(function () {
        cargarPerfil();
        renderUsuarios();
        enlazarEventos();
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

        $('#cfgVentas, #cfgStock, #cfgPedidos').on('change', function () {
            // aquí luego conectas persistencia real si quieres
        });
    }

    function cargarPerfil() {
        var username = 'admin1';
        var email = 'admin@alpes.com';
        var rol = 'Administrador';

        $('#configProfileName').text(username);
        $('#configProfileEmail').text(email);
        $('#configProfileRole').text(rol);
        $('#configProfileAvatar').text((username || 'A').charAt(0).toUpperCase());
    }

    function renderUsuarios() {
        if (!usuarios.length) {
            $('#configUsuariosListado').html('<div class="table-empty">No hay usuarios registrados.</div>');
            return;
        }

        var html = '';

        usuarios.forEach(function (u, index) {
            html += '<div class="config-user-row ' + (index === usuarios.length - 1 ? 'config-user-row--last' : '') + '">';
            html += '   <div class="config-user-row__left">';
            html += '       <div class="config-user-row__avatar">' + obtenerInicial(u.Username) + '</div>';
            html += '       <div class="config-user-row__body">';
            html += '           <div class="config-user-row__name">' + escapeHtml(valor(u.Username)) + '</div>';
            html += '           <div class="config-user-row__email">' + escapeHtml(valor(u.Email)) + '</div>';
            html += '       </div>';
            html += '   </div>';
            html += '   <div class="config-user-row__actions">';
            html += '       <button type="button" class="btn-icon" onclick="AdminConfiguracion.editarUsuario(' + entero(u.UsuarioId) + ')"><i class="bi bi-pencil"></i></button>';
            html += '       <button type="button" class="btn-icon btn-icon-danger" onclick="AdminConfiguracion.eliminarUsuario(' + entero(u.UsuarioId) + ')"><i class="bi bi-trash"></i></button>';
            html += '   </div>';
            html += '</div>';
        });

        $('#configUsuariosListado').html(html);
    }

    function editarUsuario(id) {
        var u = usuarios.find(function (x) { return entero(x.UsuarioId) === entero(id); });

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

        if (id > 0) {
            var existente = usuarios.find(function (x) { return entero(x.UsuarioId) === id; });
            if (existente) {
                existente.Username = username;
                existente.Email = email;
                existente.Rol = rol;
            }
        } else {
            var nuevoId = usuarios.length ? Math.max.apply(null, usuarios.map(function (x) { return entero(x.UsuarioId); })) + 1 : 1;
            usuarios.push({
                UsuarioId: nuevoId,
                Username: username,
                Email: email,
                Rol: rol
            });
        }

        cerrarModalUsuario();
        renderUsuarios();
    }

    function eliminarUsuario(id) {
        if (!confirm('¿Deseas eliminar este usuario?')) {
            return;
        }

        usuarios = usuarios.filter(function (x) {
            return entero(x.UsuarioId) !== entero(id);
        });

        renderUsuarios();
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
        $('#selUsuarioConfigRol').val('Administrador');
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