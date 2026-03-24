@Code
    ViewData("Title") = "Consola CRUD"
    Dim entidades = CType(ViewBag.Entidades, String())
End Code

<main>
    <h2>Consola de CRUDs</h2>
    <p>Selecciona una entidad y ejecuta operaciones usando la API genérica.</p>

    <div class="row g-3">
        <div class="col-md-6">
            <label for="entidadSelect" class="form-label">Entidad</label>
            <select id="entidadSelect" class="form-select">
                @For Each entidad In entidades
                    @<option value="@entidad">@entidad</option>
                Next
            </select>
        </div>
        <div class="col-md-3">
            <label for="idInput" class="form-label">ID</label>
            <input id="idInput" class="form-control" type="number" min="1" />
        </div>
        <div class="col-md-3 d-flex align-items-end">
            <button id="btnListar" class="btn btn-primary w-100">Listar</button>
        </div>
    </div>

    <div class="row mt-3 g-3">
        <div class="col-md-4">
            <button id="btnObtener" class="btn btn-outline-secondary w-100">Obtener por ID</button>
        </div>
        <div class="col-md-4">
            <button id="btnEliminar" class="btn btn-outline-danger w-100">Eliminar</button>
        </div>
        <div class="col-md-4 d-flex gap-2">
            <input id="criterioInput" class="form-control" placeholder="Criterio" />
            <input id="valorInput" class="form-control" placeholder="Valor" />
            <button id="btnBuscar" class="btn btn-outline-info">Buscar</button>
        </div>
    </div>

    <div class="mt-4">
        <label for="payloadInput" class="form-label">Payload JSON (crear/actualizar)</label>
        <textarea id="payloadInput" class="form-control" rows="8" placeholder='{"Campo":"Valor"}'></textarea>
    </div>

    <div class="row mt-3 g-3">
        <div class="col-md-6">
            <button id="btnCrear" class="btn btn-success w-100">Crear</button>
        </div>
        <div class="col-md-6">
            <button id="btnActualizar" class="btn btn-warning w-100">Actualizar</button>
        </div>
    </div>

    <hr />

    <h4>Resultado</h4>
    <pre id="resultado" class="p-3 bg-light border rounded" style="max-height: 300px; overflow:auto;"></pre>

    <h4 class="mt-4">Tabla</h4>
    <div class="table-responsive">
        <table class="table table-striped table-bordered" id="tablaResultado"></table>
    </div>
</main>

@section scripts
    <script>
        const entidadSelect = document.getElementById('entidadSelect');
        const idInput = document.getElementById('idInput');
        const criterioInput = document.getElementById('criterioInput');
        const valorInput = document.getElementById('valorInput');
        const payloadInput = document.getElementById('payloadInput');
        const resultado = document.getElementById('resultado');
        const tabla = document.getElementById('tablaResultado');

        function entidadActual() {
            return encodeURIComponent(entidadSelect.value);
        }

        function mostrarResultado(data) {
            resultado.textContent = JSON.stringify(data, null, 2);
            renderTabla(data);
        }

        function renderTabla(data) {
            tabla.innerHTML = '';
            if (!Array.isArray(data) || data.length === 0) {
                return;
            }

            const cols = Object.keys(data[0]);
            const thead = document.createElement('thead');
            const trHead = document.createElement('tr');
            cols.forEach(c => {
                const th = document.createElement('th');
                th.textContent = c;
                trHead.appendChild(th);
            });
            thead.appendChild(trHead);
            tabla.appendChild(thead);

            const tbody = document.createElement('tbody');
            data.forEach(row => {
                const tr = document.createElement('tr');
                cols.forEach(c => {
                    const td = document.createElement('td');
                    td.textContent = row[c] ?? '';
                    tr.appendChild(td);
                });
                tbody.appendChild(tr);
            });
            tabla.appendChild(tbody);
        }

        async function llamarApi(url, method = 'GET', body = null) {
            const options = { method, headers: {} };
            if (body !== null) {
                options.headers['Content-Type'] = 'application/json';
                options.body = JSON.stringify(body);
            }

            const response = await fetch(url, options);
            const data = await response.json();
            mostrarResultado(data);
        }

        document.getElementById('btnListar').addEventListener('click', () => {
            llamarApi(`/api/entidades/${entidadActual()}/listar`);
        });

        document.getElementById('btnObtener').addEventListener('click', () => {
            if (!idInput.value) return;
            llamarApi(`/api/entidades/${entidadActual()}/obtener/${idInput.value}`);
        });

        document.getElementById('btnBuscar').addEventListener('click', () => {
            const criterio = encodeURIComponent(criterioInput.value || '');
            const valor = encodeURIComponent(valorInput.value || '');
            llamarApi(`/api/entidades/${entidadActual()}/buscar?criterio=${criterio}&valor=${valor}`);
        });

        document.getElementById('btnCrear').addEventListener('click', () => {
            let body = {};
            try { body = JSON.parse(payloadInput.value || '{}'); } catch { return; }
            llamarApi(`/api/entidades/${entidadActual()}/crear`, 'POST', body);
        });

        document.getElementById('btnActualizar').addEventListener('click', () => {
            let body = {};
            try { body = JSON.parse(payloadInput.value || '{}'); } catch { return; }
            llamarApi(`/api/entidades/${entidadActual()}/actualizar`, 'PUT', body);
        });

        document.getElementById('btnEliminar').addEventListener('click', () => {
            if (!idInput.value) return;
            llamarApi(`/api/entidades/${entidadActual()}/eliminar/${idInput.value}`, 'DELETE');
        });

        document.getElementById('btnListar').click();
    </script>
End Section
