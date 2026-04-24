document.addEventListener('DOMContentLoaded', function () {
    var btnAbrir = document.getElementById('btnAbrirEditarPerfil');
    var btnCerrar = document.getElementById('btnCerrarEditarPerfil');
    var overlay = document.getElementById('epOverlay');
    var form = document.getElementById('epForm');

    var inputNombre = document.getElementById('epNombre');
    var inputApellido = document.getElementById('epApellido');
    var inputEmail = document.getElementById('epEmail');

    var headerName = document.querySelector('.np-name');
    var headerEmail = document.querySelector('.np-email');
    var headerAvatar = document.getElementById('npAvatar');

    var endpointObtener = '/PortalCliente/ObtenerPerfilActual';
    var endpointActualizar = '/PortalCliente/ActualizarPerfil';

    if (!btnAbrir || !overlay || !form) {
        return;
    }

    function abrirModal() {
        overlay.classList.add('show');
    }

    function cerrarModal() {
        overlay.classList.remove('show');
    }

    function setLoading(loading) {
        var submitBtn = form.querySelector('button[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = loading;
            submitBtn.textContent = loading ? 'GUARDANDO...' : 'GUARDAR';
        }
    }

    function obtenerValor(obj, posiblesNombres) {
        var i;
        for (i = 0; i < posiblesNombres.length; i += 1) {
            if (obj && obj[posiblesNombres[i]] !== undefined && obj[posiblesNombres[i]] !== null) {
                return obj[posiblesNombres[i]];
            }
        }
        return '';
    }

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: null, message: 'Respuesta vacía del servidor.' };
        }

        if (payload.ok !== undefined) {
            return {
                ok: payload.ok === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        if (payload.success !== undefined) {
            return {
                ok: payload.success === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        return {
            ok: true,
            data: payload,
            message: ''
        };
    }

    function actualizarCabecera(perfil) {
        var nombres = String(obtenerValor(perfil, ['Nombres', 'nombres', 'Nombre', 'nombre'])).trim();
        var apellidos = String(obtenerValor(perfil, ['Apellidos', 'apellidos', 'Apellido', 'apellido'])).trim();
        var email = String(obtenerValor(perfil, ['Email', 'email'])).trim();

        var nombreCompleto = (nombres + ' ' + apellidos).trim();
        var inicial = nombreCompleto !== '' ? nombreCompleto.charAt(0).toUpperCase() : 'C';

        if (headerName) {
            headerName.textContent = nombreCompleto !== '' ? nombreCompleto : 'Cliente';
        }

        if (headerEmail) {
            headerEmail.textContent = email !== '' ? email : 'No disponible';
        }

        if (headerAvatar) {
            headerAvatar.textContent = inicial;
        }
    }

    function cargarPerfil() {
        fetch(endpointObtener, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo obtener el perfil del cliente.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok || !respuesta.data) {
                    throw new Error(respuesta.message || 'No se pudo cargar el perfil.');
                }

                var perfil = respuesta.data;

                if (inputNombre) {
                    inputNombre.value = obtenerValor(perfil, ['Nombres', 'nombres', 'Nombre', 'nombre']);
                }

                if (inputApellido) {
                    inputApellido.value = obtenerValor(perfil, ['Apellidos', 'apellidos', 'Apellido', 'apellido']);
                }

                if (inputEmail) {
                    inputEmail.value = obtenerValor(perfil, ['Email', 'email']);
                }

                actualizarCabecera(perfil);
            })
            .catch(function (error) {
                console.error(error);

                if (headerName) {
                    headerName.textContent = 'No se pudo cargar el perfil';
                }

                if (headerEmail) {
                    headerEmail.textContent = 'Intenta recargar la página';
                }
            });
    }

    btnAbrir.addEventListener('click', function () {
        abrirModal();
    });

    if (btnCerrar) {
        btnCerrar.addEventListener('click', function () {
            cerrarModal();
        });
    }

    overlay.addEventListener('click', function (e) {
        if (e.target === overlay) {
            cerrarModal();
        }
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            cerrarModal();
        }
    });

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        var payload = {
            nombres: inputNombre ? inputNombre.value.trim() : '',
            apellidos: inputApellido ? inputApellido.value.trim() : '',
            email: inputEmail ? inputEmail.value.trim() : ''
        };

        if (payload.nombres === '' || payload.apellidos === '' || payload.email === '') {
            alert('Completa los campos obligatorios.');
            return;
        }

        setLoading(true);

        fetch(endpointActualizar, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo actualizar el perfil.');
                }
                return response.json();
            })
            .then(function (payloadRespuesta) {
                var respuesta = normalizarRespuesta(payloadRespuesta);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo actualizar el perfil.');
                }

                actualizarCabecera(respuesta.data || {
                    Nombres: payload.nombres,
                    Apellidos: payload.apellidos,
                    Email: payload.email
                });

                cerrarModal();
                alert(respuesta.message || 'Perfil actualizado correctamente.');
            })
            .catch(function (error) {
                alert(error.message || 'Ocurrió un error al actualizar el perfil.');
            })
            .finally(function () {
                setLoading(false);
            });
    });

    cargarPerfil();
});