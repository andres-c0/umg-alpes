@Code
    ViewData("Title") = "CRUD General"
End Code

<h2>CRUD General Dinámico</h2>

<div style="margin-bottom: 15px;">
    <label for="tablaSelect">Tabla:</label>
    <select id="tablaSelect"></select>

    <button type="button" onclick="cargarDatos()">Cargar</button>
</div>

<hr />

<div id="formularioCrud" style="margin-bottom: 20px;">
    <h3>Formulario</h3>
    <input type="hidden" id="modoEdicion" value="insertar" />
    <div id="contenedorFormulario"></div>

    <div style="margin-top: 15px;">
        <button type="button" onclick="guardarRegistro()">Guardar</button>
        <button type="button" onclick="limpiarFormulario()">Limpiar</button>
    </div>
</div>

<hr />

<h3>Datos</h3>
<table border="1" id="tablaDatos" cellpadding="6" cellspacing="0">
    <thead></thead>
    <tbody></tbody>
</table>

<script>
    let tablaActualConfig = null;

    async function inicializarTablas() {
        const response = await fetch("/Metadata/ListarTablas");
        const tablas = await response.json();
        const select = document.getElementById("tablaSelect");

        select.innerHTML = "";

        tablas.forEach(tabla => {
            select.innerHTML += `<option value="${tabla.Value}">${tabla.Text}</option>`;
        });

        select.addEventListener("change", async function () {
            await cambiarTabla();
        });
    }

    async function obtenerMetadataTabla(nombreTabla) {
        const response = await fetch(`/Metadata/ObtenerTabla?nombre=${encodeURIComponent(nombreTabla)}`);
        const data = await response.json();

        if (data.success === false) {
            throw new Error(data.message);
        }

        return data;
    }

    async function cambiarTabla() {
        const tabla = document.getElementById("tablaSelect").value;
        tablaActualConfig = await obtenerMetadataTabla(tabla);
        await generarFormulario();
        limpiarFormulario();
        cargarDatos();
    }

    async function generarFormulario() {
        const config = tablaActualConfig;
        const contenedor = document.getElementById("contenedorFormulario");
        contenedor.innerHTML = "";

        for (const campo of config.Campos) {
            if (campo.Type === "hidden") {
                contenedor.innerHTML += `<input type="hidden" id="${campo.Name}" />`;
                continue;
            }

            let controlHtml = "";

            if (campo.Type === "select") {
                let options = `<option value="">Seleccione...</option>`;

                try {
                    const response = await fetch(campo.DataSource);
                    const data = await response.json();

                    data.forEach(item => {
                        options += `<option value="${item[campo.ValueField]}">${item[campo.TextField]}</option>`;
                    });
                } catch {
                    options += `<option value="">Error cargando opciones</option>`;
                }

                controlHtml = `<select id="${campo.Name}" style="width: 300px;">${options}</select>`;
            } else {
                controlHtml = `<input type="${campo.Type}" id="${campo.Name}" style="width: 300px;" />`;
            }

            const requerido = campo.Required ? ' <span style="color:red;">*</span>' : '';

            contenedor.innerHTML += `
                <div style="margin-bottom: 8px;">
                    <label>${campo.Label}:${requerido}</label><br />
                    ${controlHtml}
                </div>
            `;
        }
    }

    function cargarDatos() {
        const config = tablaActualConfig;

        fetch(`/${config.Endpoint}/Index`)
            .then(res => res.json())
            .then(data => {
                renderTabla(data, config);
            })
            .catch(error => {
                console.error("Error al cargar datos:", error);
                alert("Ocurrió un error al cargar los datos.");
            });
    }

    function renderTabla(data, config) {
        const thead = document.querySelector("#tablaDatos thead");
        const tbody = document.querySelector("#tablaDatos tbody");

        thead.innerHTML = "";
        tbody.innerHTML = "";

        if (!data || data.length === 0) {
            thead.innerHTML = "<tr><th>Sin datos</th></tr>";
            return;
        }

        const columnas = Object.keys(data[0]);

        let headerRow = "<tr>";
        columnas.forEach(col => {
            headerRow += `<th>${col}</th>`;
        });
        headerRow += "<th>Acciones</th></tr>";

        thead.innerHTML = headerRow;

        data.forEach(item => {
            let row = "<tr>";

            columnas.forEach(col => {
                row += `<td>${item[col] ?? ""}</td>`;
            });

            const id = item[config.Pk];

            row += `
                <td>
                    <button type="button" onclick='editarRegistro(${JSON.stringify(item).replace(/'/g, "&apos;")})'>Editar</button>
                    <button type="button" onclick='eliminarRegistro(${id})'>Eliminar</button>
                </td>
            `;

            row += "</tr>";
            tbody.innerHTML += row;
        });
    }

    function editarRegistro(item) {
        const config = tablaActualConfig;
        document.getElementById("modoEdicion").value = "actualizar";

        config.Campos.forEach(campo => {
            const control = document.getElementById(campo.Name);
            if (!control) return;

            const valor = item[campo.Name];

            if (campo.Type === "datetime-local") {
                control.value = formatearFechaInput(valor);
            } else {
                control.value = valor ?? "";
            }
        });
    }

    function validarFormulario() {
        const config = tablaActualConfig;

        for (const campo of config.Campos) {
            if (campo.Type === "hidden") continue;

            const control = document.getElementById(campo.Name);
            if (!control) continue;

            const valor = control.value ? control.value.toString().trim() : "";

            if (campo.Required && valor === "") {
                alert(`El campo "${campo.Label}" es obligatorio.`);
                control.focus();
                return false;
            }
        }

        return true;
    }

    function guardarRegistro() {
        if (!validarFormulario()) {
            return;
        }

        const config = tablaActualConfig;
        const modo = document.getElementById("modoEdicion").value;
        const entidad = {};

        config.Campos.forEach(campo => {
            const control = document.getElementById(campo.Name);
            if (!control) return;

            let valor = control.value;

            if (campo.Type === "number") {
                valor = campo.Nullable ? parseNullableInt(valor) : parseIntSeguro(valor);
            } else if (campo.Type === "datetime-local") {
                valor = parseNullableDate(valor);
            } else if (campo.Type === "select") {
                valor = campo.Nullable ? parseNullableInt(valor) : parseIntSeguro(valor);
            } else if (campo.Type === "hidden") {
                valor = parseIntSeguro(valor);
            }

            entidad[campo.Name] = valor;
        });

        const url = modo === "actualizar"
            ? `/${config.Endpoint}/Actualizar`
            : `/${config.Endpoint}/Insertar`;

        fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(entidad)
        })
            .then(res => res.json())
            .then(data => {
                alert(data.message);
                if (data.success) {
                    limpiarFormulario();
                    cargarDatos();
                }
            })
            .catch(error => {
                console.error("Error al guardar:", error);
                alert("Ocurrió un error al guardar.");
            });
    }

    function eliminarRegistro(id) {
        const config = tablaActualConfig;

        if (!confirm("¿Deseas eliminar este registro?")) {
            return;
        }

        fetch(`/${config.Endpoint}/Eliminar`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ id: id })
        })
            .then(res => res.json())
            .then(data => {
                alert(data.message);
                if (data.success) {
                    cargarDatos();
                }
            })
            .catch(error => {
                console.error("Error al eliminar:", error);
                alert("Ocurrió un error al eliminar.");
            });
    }

    function limpiarFormulario() {
        const config = tablaActualConfig;
        document.getElementById("modoEdicion").value = "insertar";

        config.Campos.forEach(campo => {
            const control = document.getElementById(campo.Name);
            if (!control) return;

            if (campo.DefaultValue !== null && campo.DefaultValue !== undefined) {
                control.value = campo.DefaultValue;
            } else {
                control.value = "";
            }
        });
    }

    function parseIntSeguro(valor) {
        if (!valor || valor.toString().trim() === "") return 0;
        return parseInt(valor);
    }

    function parseNullableInt(valor) {
        if (!valor || valor.toString().trim() === "") return null;
        return parseInt(valor);
    }

    function parseNullableDate(valor) {
        if (!valor || valor.toString().trim() === "") return null;
        return valor;
    }

    function formatearFechaInput(valor) {
        if (!valor) return "";

        try {
            const fecha = new Date(valor);
            if (isNaN(fecha.getTime())) return "";

            const year = fecha.getFullYear();
            const month = String(fecha.getMonth() + 1).padStart(2, "0");
            const day = String(fecha.getDate()).padStart(2, "0");
            const hours = String(fecha.getHours()).padStart(2, "0");
            const minutes = String(fecha.getMinutes()).padStart(2, "0");

            return `${year}-${month}-${day}T${hours}:${minutes}`;
        } catch {
            return "";
        }
    }

    window.onload = async function () {
        await inicializarTablas();
        await cambiarTabla();
    };
</script>