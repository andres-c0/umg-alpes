(function () {
    const CLAVE_CARRITO_INVITADO = "alpes_carrito_invitado";

    let filtroActual = "";
    let busquedaActual = "";
    let timeoutBusqueda = null;

    function obtenerCarritoInvitado() {
        try {
            const carrito = localStorage.getItem(CLAVE_CARRITO_INVITADO);

            if (!carrito) {
                return {
                    items: []
                };
            }

            const data = JSON.parse(carrito);

            if (!data || !Array.isArray(data.items)) {
                return {
                    items: []
                };
            }

            return data;
        } catch (error) {
            return {
                items: []
            };
        }
    }

    function guardarCarritoInvitado(carrito) {
        localStorage.setItem(CLAVE_CARRITO_INVITADO, JSON.stringify(carrito));
    }

    function actualizarContadorCarrito() {
        const carrito = obtenerCarritoInvitado();

        const totalItems = carrito.items.reduce(function (total, item) {
            return total + Number(item.cantidad || 0);
        }, 0);

        const contadorDesktop = document.getElementById("publicCartCount");
        const contadorMobile = document.getElementById("publicMobileCartCount");

        if (contadorDesktop) {
            contadorDesktop.textContent = totalItems;
        }

        if (contadorMobile) {
            contadorMobile.textContent = totalItems;
        }
    }

    function escaparHtml(texto) {
        if (texto === null || texto === undefined) {
            return "";
        }

        return String(texto)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function obtenerValor(producto, campo, valorDefault) {
        if (!producto) {
            return valorDefault;
        }

        if (producto[campo] !== undefined && producto[campo] !== null) {
            return producto[campo];
        }

        const camel = campo.charAt(0).toLowerCase() + campo.slice(1);

        if (producto[camel] !== undefined && producto[camel] !== null) {
            return producto[camel];
        }

        return valorDefault;
    }

    function formatearMoneda(valor) {
        const numero = Number(valor || 0);

        return "Q " + numero.toLocaleString("es-GT", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function normalizarImagenUrl(imagenUrl) {
        if (!imagenUrl) {
            return "";
        }

        const url = String(imagenUrl).trim();

        if (url.length === 0) {
            return "";
        }

        if (url.startsWith("http://") || url.startsWith("https://") || url.startsWith("/")) {
            return url;
        }

        return "/" + url;
    }

    function obtenerIconoPorTipo(tipo) {
        const tipoNormalizado = String(tipo || "").toUpperCase();

        if (tipoNormalizado === "EXTERIOR") {
            return "bi-tree";
        }

        if (tipoNormalizado === "INTERIOR") {
            return "bi-house-door";
        }

        return "bi-armchair";
    }

    function agregarProductoAlCarritoInvitado(producto) {
        const carrito = obtenerCarritoInvitado();

        const existente = carrito.items.find(function (item) {
            return Number(item.productoId) === Number(producto.productoId);
        });

        if (existente) {
            existente.cantidad = Number(existente.cantidad || 0) + Number(producto.cantidad || 1);
        } else {
            carrito.items.push({
                productoId: Number(producto.productoId),
                nombre: producto.nombre || "Producto",
                precio: Number(producto.precio || 0),
                imagenUrl: producto.imagenUrl || "",
                tipo: producto.tipo || "",
                cantidad: Number(producto.cantidad || 1)
            });
        }

        guardarCarritoInvitado(carrito);
        actualizarContadorCarrito();
    }

    function construirCardProducto(producto) {
        const productoId = Number(obtenerValor(producto, "ProductoId", 0));
        const referencia = obtenerValor(producto, "Referencia", "");
        const nombre = obtenerValor(producto, "Nombre", "Producto sin nombre");
        const descripcion = obtenerValor(producto, "Descripcion", "");
        const tipo = obtenerValor(producto, "Tipo", "PRODUCTO");
        const material = obtenerValor(producto, "Material", "");
        const color = obtenerValor(producto, "Color", "");
        const imagenUrl = normalizarImagenUrl(obtenerValor(producto, "ImagenUrl", ""));
        const precioActual = Number(obtenerValor(producto, "PrecioActual", 0));
        const precioFormateado = obtenerValor(producto, "PrecioFormateado", formatearMoneda(precioActual));
        const cuota12Formateada = obtenerValor(producto, "Cuota12Formateada", formatearMoneda(precioActual / 12));
        const icono = obtenerIconoPorTipo(tipo);

        const detalleTexto = material || color || descripcion || "Mueble artesanal de Muebles de los Alpes.";

        const imagenHtml = imagenUrl
            ? `<img src="${escaparHtml(imagenUrl)}" alt="${escaparHtml(nombre)}" loading="lazy" />`
            : `<i class="bi ${icono}"></i>`;

        return `
            <article class="catalogo-card" data-producto-id="${productoId}">
                <a href="/Home/DetalleProducto/${productoId}" class="catalogo-card-image">
                    ${imagenHtml}

                    <span class="catalogo-card-badge">${escaparHtml(tipo || "PRODUCTO")}</span>
                </a>

                <div class="catalogo-card-body">
                    <span class="catalogo-card-ref">${escaparHtml(referencia || "Sin referencia")}</span>

                    <h3>${escaparHtml(nombre)}</h3>

                    <p>${escaparHtml(detalleTexto)}</p>

                    <div class="catalogo-card-price">
                        <strong>${escaparHtml(precioFormateado)}</strong>
                        <small>12 cuotas de ${escaparHtml(cuota12Formateada)}</small>
                    </div>

                    <div class="catalogo-card-actions">
                        <a href="/Home/DetalleProducto/${productoId}" class="catalogo-card-detail">
                            Ver detalle
                        </a>

                        <button type="button"
                                class="catalogo-card-cart"
                                data-add-catalogo="true"
                                data-producto-id="${productoId}"
                                data-nombre="${escaparHtml(nombre)}"
                                data-precio="${precioActual}"
                                data-imagen="${escaparHtml(imagenUrl)}"
                                data-tipo="${escaparHtml(tipo)}"
                                aria-label="Agregar ${escaparHtml(nombre)} al carrito">
                            <i class="bi bi-cart3"></i>
                        </button>
                    </div>
                </div>
            </article>
        `;
    }

    function actualizarBotonesFiltro() {
        const botones = document.querySelectorAll("[data-catalogo-filtro]");

        botones.forEach(function (boton) {
            const valor = boton.getAttribute("data-catalogo-filtro") || "";
            boton.classList.toggle("active", valor === filtroActual);
        });
    }

    function actualizarUrl() {
        const params = new URLSearchParams();

        if (filtroActual) {
            params.set("categoria", filtroActual);
        }

        if (busquedaActual) {
            params.set("q", busquedaActual);
        }

        const nuevaUrl = params.toString()
            ? window.location.pathname + "?" + params.toString()
            : window.location.pathname;

        window.history.replaceState({}, "", nuevaUrl);
    }

    async function cargarCatalogo() {
        const page = document.getElementById("catalogoPublicoPage");
        const grid = document.getElementById("catalogoGrid");
        const loading = document.getElementById("catalogoLoading");
        const empty = document.getElementById("catalogoEmpty");
        const totalTexto = document.getElementById("catalogoTotalProductos");

        if (!page || !grid) {
            return;
        }

        const endpointBase = page.getAttribute("data-productos-url") || "/Home/ObtenerProductosPublicosData";

        const params = new URLSearchParams();

        if (filtroActual) {
            params.set("tipo", filtroActual);
        }

        if (busquedaActual) {
            params.set("q", busquedaActual);
        }

        const endpoint = endpointBase + (params.toString() ? "?" + params.toString() : "");

        try {
            if (loading) {
                loading.style.display = "flex";
            }

            if (empty) {
                empty.style.display = "none";
            }

            grid.innerHTML = "";

            const respuesta = await fetch(endpoint, {
                method: "GET",
                headers: {
                    "Accept": "application/json"
                }
            });

            const data = await respuesta.json();

            if (!respuesta.ok || !data || data.ok === false) {
                throw new Error(data && data.message ? data.message : "No se pudieron cargar los productos.");
            }

            const productos = Array.isArray(data.data) ? data.data : [];

            if (totalTexto) {
                totalTexto.textContent = productos.length;
            }

            if (productos.length === 0) {
                if (empty) {
                    empty.style.display = "block";
                }

                return;
            }

            grid.innerHTML = productos.map(construirCardProducto).join("");
            inicializarBotonesAgregar();

        } catch (error) {
            console.error("Error al cargar catálogo público:", error);

            if (totalTexto) {
                totalTexto.textContent = "0";
            }

            if (empty) {
                empty.style.display = "block";

                const titulo = empty.querySelector("h2");
                const texto = empty.querySelector("p");

                if (titulo) {
                    titulo.textContent = "No se pudo cargar el catálogo";
                }

                if (texto) {
                    texto.textContent = error.message || "Verifica la conexión con el servidor.";
                }
            }
        } finally {
            if (loading) {
                loading.style.display = "none";
            }
        }
    }

    function inicializarBotonesAgregar() {
        document.querySelectorAll("[data-add-catalogo='true']").forEach(function (boton) {
            boton.addEventListener("click", function () {
                const productoId = Number(boton.getAttribute("data-producto-id") || "0");

                if (productoId <= 0) {
                    return;
                }

                agregarProductoAlCarritoInvitado({
                    productoId: productoId,
                    nombre: boton.getAttribute("data-nombre") || "Producto",
                    precio: Number(boton.getAttribute("data-precio") || "0"),
                    imagenUrl: boton.getAttribute("data-imagen") || "",
                    tipo: boton.getAttribute("data-tipo") || "",
                    cantidad: 1
                });

                const icono = boton.querySelector("i");

                boton.classList.add("agregado");

                if (icono) {
                    icono.className = "bi bi-check2";
                }

                window.setTimeout(function () {
                    boton.classList.remove("agregado");

                    if (icono) {
                        icono.className = "bi bi-cart3";
                    }
                }, 750);
            });
        });
    }

    function inicializarCatalogo() {
        const page = document.getElementById("catalogoPublicoPage");

        if (!page) {
            return;
        }

        filtroActual = (page.getAttribute("data-categoria-inicial") || "").toUpperCase();
        busquedaActual = page.getAttribute("data-busqueda-inicial") || "";

        const inputBusqueda = document.getElementById("catalogoBusqueda");
        const limpiarBtn = document.getElementById("catalogoLimpiarFiltros");

        if (inputBusqueda) {
            inputBusqueda.value = busquedaActual;

            inputBusqueda.addEventListener("input", function () {
                window.clearTimeout(timeoutBusqueda);

                timeoutBusqueda = window.setTimeout(function () {
                    busquedaActual = inputBusqueda.value.trim();
                    actualizarUrl();
                    cargarCatalogo();
                }, 350);
            });
        }

        document.querySelectorAll("[data-catalogo-filtro]").forEach(function (boton) {
            boton.addEventListener("click", function () {
                filtroActual = boton.getAttribute("data-catalogo-filtro") || "";
                actualizarBotonesFiltro();
                actualizarUrl();
                cargarCatalogo();
            });
        });

        if (limpiarBtn) {
            limpiarBtn.addEventListener("click", function () {
                filtroActual = "";
                busquedaActual = "";

                if (inputBusqueda) {
                    inputBusqueda.value = "";
                }

                actualizarBotonesFiltro();
                actualizarUrl();
                cargarCatalogo();
            });
        }

        actualizarBotonesFiltro();
        cargarCatalogo();
    }

    document.addEventListener("DOMContentLoaded", function () {
        actualizarContadorCarrito();
        inicializarCatalogo();
    });
})();