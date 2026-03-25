@Code
    ViewData("Title") = "CRUD General"
End Code

<h2>CRUD general por tabla</h2>

<div class="row g-3 mb-3">
    <div class="col-md-4">
        <label class="form-label">Tabla</label>
        <select id="tablaSelect" class="form-select"></select>
    </div>
    <div class="col-md-2">
        <label class="form-label">ID</label>
        <input id="idInput" class="form-control" type="number" />
    </div>
    <div class="col-md-6 d-flex align-items-end gap-2">
        <button id="btnCargar" class="btn btn-primary">Listar</button>
        <button id="btnObtener" class="btn btn-outline-primary">Obtener por ID</button>
        <button id="btnEliminar" class="btn btn-outline-danger">Eliminar por ID</button>
    </div>
</div>

<div class="card mb-3">
    <div class="card-header">Formulario dinámico (JSON)</div>
    <div class="card-body">
        <p class="text-muted">Completa los campos según la tabla y usa Insertar o Actualizar.</p>
        <div id="dynamicFields" class="row g-2"></div>
        <div class="mt-3 d-flex gap-2">
            <button id="btnInsertar" class="btn btn-success">Insertar</button>
            <button id="btnActualizar" class="btn btn-warning">Actualizar</button>
        </div>
    </div>
</div>

<div class="card">
    <div class="card-header">Resultados</div>
    <div class="card-body">
        <pre id="mensajes" class="small"></pre>
        <div class="table-responsive">
            <table class="table table-bordered table-sm" id="tablaDatos"></table>
        </div>
    </div>
</div>

@section scripts
    <script>
        let tableConfigs = [];
        let currentTable = null;

        function log(msg) {
            document.getElementById('mensajes').textContent = typeof msg === 'string' ? msg : JSON.stringify(msg, null, 2);
        }

        async function loadTables() {
            const res = await fetch('@Url.Action("Tablas", "CrudApi")');
            tableConfigs = await res.json();

            const select = document.getElementById('tablaSelect');
            select.innerHTML = '';
            tableConfigs.forEach(cfg => {
                const opt = document.createElement('option');
                opt.value = cfg.key;
                opt.textContent = cfg.label;
                select.appendChild(opt);
            });

            currentTable = tableConfigs[0];
            buildDynamicFields();
            await listar();
        }

        function buildDynamicFields() {
            const container = document.getElementById('dynamicFields');
            container.innerHTML = '';
            currentTable.fields.forEach(f => {
                const wrapper = document.createElement('div');
                wrapper.className = 'col-md-3';
                wrapper.innerHTML = `<label class="form-label">${f}</label><input class="form-control" data-field="${f}" />`;
                container.appendChild(wrapper);
            });
        }

        function collectPayload(includeId) {
            const payload = {};
            if (includeId) {
                payload[currentTable.idField] = Number(document.getElementById('idInput').value || 0);
            }

            document.querySelectorAll('#dynamicFields [data-field]').forEach(input => {
                const val = input.value;
                payload[input.dataset.field] = val === '' ? null : val;
            });
            return payload;
        }

        function renderTable(rows) {
            const table = document.getElementById('tablaDatos');
            table.innerHTML = '';
            if (!rows || rows.length === 0) {
                table.innerHTML = '<tr><td>No hay datos.</td></tr>';
                return;
            }

            const cols = Object.keys(rows[0]);
            const thead = document.createElement('thead');
            thead.innerHTML = `<tr>${cols.map(c => `<th>${c}</th>`).join('')}</tr>`;
            table.appendChild(thead);

            const tbody = document.createElement('tbody');
            rows.forEach(r => {
                const tr = document.createElement('tr');
                tr.innerHTML = cols.map(c => `<td>${r[c] ?? ''}</td>`).join('');
                tbody.appendChild(tr);
            });
            table.appendChild(tbody);
        }

        async function listar() {
            const url = `@Url.Action("Listar", "CrudApi")?tabla=${encodeURIComponent(currentTable.key)}`;
            const res = await fetch(url);
            const data = await res.json();
            log(data);
            if (data.ok) renderTable(data.data);
        }

        async function obtenerPorId() {
            const id = Number(document.getElementById('idInput').value || 0);
            const url = `@Url.Action("ObtenerPorId", "CrudApi")?tabla=${encodeURIComponent(currentTable.key)}&id=${id}`;
            const res = await fetch(url);
            const data = await res.json();
            log(data);
            renderTable(data.data ? [data.data] : []);
        }

        async function eliminar() {
            const id = Number(document.getElementById('idInput').value || 0);
            const body = new URLSearchParams({ tabla: currentTable.key, id: id });
            const res = await fetch('@Url.Action("Eliminar", "CrudApi")', { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body });
            const data = await res.json();
            log(data);
            if (data.ok) await listar();
        }

        async function insertar() {
            const payload = collectPayload(false);
            const body = new URLSearchParams({ tabla: currentTable.key, payload: JSON.stringify(payload) });
            const res = await fetch('@Url.Action("Insertar", "CrudApi")', { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body });
            const data = await res.json();
            log(data);
            if (data.ok) await listar();
        }

        async function actualizar() {
            const payload = collectPayload(true);
            const body = new URLSearchParams({ tabla: currentTable.key, payload: JSON.stringify(payload) });
            const res = await fetch('@Url.Action("Actualizar", "CrudApi")', { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body });
            const data = await res.json();
            log(data);
            if (data.ok) await listar();
        }

        document.getElementById('tablaSelect').addEventListener('change', async (e) => {
            currentTable = tableConfigs.find(t => t.key === e.target.value);
            buildDynamicFields();
            await listar();
        });
        document.getElementById('btnCargar').addEventListener('click', listar);
        document.getElementById('btnObtener').addEventListener('click', obtenerPorId);
        document.getElementById('btnEliminar').addEventListener('click', eliminar);
        document.getElementById('btnInsertar').addEventListener('click', insertar);
        document.getElementById('btnActualizar').addEventListener('click', actualizar);

        loadTables();
    </script>
End Section
