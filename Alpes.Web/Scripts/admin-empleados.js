(function () {

    var empleados = [];
    var empleadoSeleccionado = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarEmpleados();
    });

    function enlazarEventos() {

        $('#btnRecargarEmpleado').on('click', cargarEmpleados);

        $('#txtBuscarEmpleado').on('input', renderEmpleados);

        $('#btnNuevoEmpleado').on('click', function () {
            empleadoSeleccionado = null;
            limpiarFormulario();
            $('#modalEmpleadoTitulo').text('Nuevo empleado');
            abrirModal();
        });

        $('#btnGuardarEmpleado').on('click', guardarEmpleado);

        $('#btnCerrarModalEmpleado, #btnCerrarModalEmpleadoX, #modalEmpleado .modal-a__backdrop')
            .on('click', cerrarModal);
    }

    function cargarEmpleados() {

        $('#empleadosListado').html('<div class="table-empty">Cargando empleados...</div>');

        $.getJSON('/Empleado/Index')
            .done(function (res) {
                empleados = normalizar(res);
                renderEmpleados();
            })
            .fail(function () {
                $('#empleadosListado').html('<div class="table-empty">Error al cargar empleados</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderEmpleados() {

        var filtro = ($('#txtBuscarEmpleado').val() || '').toLowerCase();

        var lista = empleados.filter(function (e) {

            var texto = (
                (e.Nombres || '') + ' ' +
                (e.Apellidos || '') + ' ' +
                (e.Email || '') + ' ' +
                (e.Puesto || '')
            ).toLowerCase();

            return texto.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#empleadosListado').html('<div class="table-empty">No hay empleados</div>');
            return;
        }

        var html = '';

        lista.forEach(function (e) {

            var nombre = (e.Nombres || '') + ' ' + (e.Apellidos || '');
            var inicial = nombre.charAt(0).toUpperCase();

            html += `
            <div class="cliente-card">
                <div class="cliente-card__avatar">${inicial}</div>

                <div class="cliente-card__body">
                    <div class="cliente-card__name">${nombre}</div>

                    <div class="cliente-card__meta">
                        <i class="bi bi-envelope"></i> ${e.Email || ''}
                    </div>

                    <div class="cliente-card__meta">
                        <i class="bi bi-briefcase"></i> ${e.Puesto || ''}
                    </div>
                </div>

                <div class="cliente-card__actions">
                    <button class="btn-icon" onclick="AdminEmpleados.editar(${e.EmpId})">
                        <i class="bi bi-pencil"></i>
                    </button>

                    <button class="btn-icon btn-icon-danger" onclick="AdminEmpleados.eliminar(${e.EmpId})">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>`;
        });

        $('#empleadosListado').html(html);
    }

    function editar(id) {

        $.getJSON('/Empleado/Obtener', { id: id })
            .done(function (e) {

                empleadoSeleccionado = e;

                $('#modalEmpleadoTitulo').text('Editar empleado');

                $('#txtNombresEmp').val(e.Nombres);
                $('#txtApellidosEmp').val(e.Apellidos);
                $('#txtEmailEmp').val(e.Email);
                $('#txtTelefonoEmp').val(e.Telefono);
                $('#txtPuestoEmp').val(e.Puesto);
                $('#selEstadoEmp').val(e.Estado);

                abrirModal();
            });
    }

    function guardarEmpleado() {

        var data = {
            EmpId: empleadoSeleccionado ? empleadoSeleccionado.EmpId : 0,
            Nombres: $('#txtNombresEmp').val(),
            Apellidos: $('#txtApellidosEmp').val(),
            Email: $('#txtEmailEmp').val(),
            Telefono: $('#txtTelefonoEmp').val(),
            Puesto: $('#txtPuestoEmp').val(),
            Estado: $('#selEstadoEmp').val()
        };

        if (!data.Nombres || !data.Apellidos) {
            return alert('Nombre y apellido son obligatorios');
        }

        var url = empleadoSeleccionado
            ? '/Empleado/Actualizar'
            : '/Empleado/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        })
            .done(function () {
                cerrarModal();
                cargarEmpleados();
            });
    }

    function eliminarEmpleado(id) {

        if (!confirm('¿Eliminar empleado?')) return;

        $.ajax({
            url: '/Empleado/Eliminar',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ EmpId: id })
        })
            .done(function () {
                cargarEmpleados();
            });
    }

    function abrirModal() {
        $('#modalEmpleado').show();
    }

    function cerrarModal() {
        $('#modalEmpleado').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#modalEmpleado input').val('');
        empleadoSeleccionado = null;
    }

    window.AdminEmpleados = {
        editar: editar,
        eliminar: eliminarEmpleado
    };

})();