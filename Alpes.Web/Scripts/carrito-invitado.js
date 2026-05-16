(function () {
    const CLAVE_CARRITO_INVITADO = "alpes_carrito_invitado";

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

    function calcularTotales(carrito) {
        let cantidadTotal = 0;
        let subtotal = 0;

        carrito.items.forEach(function (item) {
            const cantidad = Number(item.cantidad || 0);
            const precio = Number(item.precio || 0);

            cantidadTotal += cantidad;
            subtotal += cantidad * precio;
        });

        return {
            cantidadTotal: cantidadTotal,
            subtotal: subtotal,
            total: subtotal
        };
    }

    function construirItemHtml(item) {
        const productoId = Number(item.productoId || 0);
        const nombre = item.nombre || "Producto";
        const tipo = item.tipo || "PRODUCTO";
        const precio = Number(item.precio || 0);
        const cantidad = Number(item.cantidad || 1);
        const imagenUrl = normalizarImagenUrl(item.imagenUrl || "");
        const subtotal = precio * cantidad;

        const imagenHtml = imagenUrl
            ? `<img src="${escaparHtml(imagenUrl)}" alt="${escaparHtml(nombre)}" loading="lazy" />`
            : `<i class="bi ${obtenerIconoPorTipo(tipo)}"></i>`;

        return `
            <article class="carrito-item" data-producto-id="${productoId}">
                <a href="/Home/DetalleProducto/${productoId}" class="carrito-item-imagen">
                    ${imagenHtml}
                </a>

                <div class="carrito-item-info">
                    <span>${escaparHtml(tipo)}</span>
                    <h3>${escaparHtml(nombre)}</h3>

                    <div class="carrito-item-precio-mobile">
                        ${formatearMoneda(precio)}
                    </div>

                    <div class="carrito-item-controls">
                        <button type="button" data-carrito-restar="${productoId}" aria-label="Restar cantidad">
                            <i class="bi bi-dash"></i>
                        </button>

                        <strong>${cantidad}</strong>

                        <button type="button" data-carrito-sumar="${productoId}" aria-label="Sumar cantidad">
                            <i class="bi bi-plus"></i>
                        </button>
                    </div>
                </div>

                <div class="carrito-item-precio">
                    <span>Precio</span>
                    <strong>${formatearMoneda(precio)}</strong>
                </div>

                <div class="carrito-item-subtotal">
                    <span>Subtotal</span>
                    <strong>${formatearMoneda(subtotal)}</strong>
                </div>

                <button type="button" class="carrito-item-eliminar" data-carrito-eliminar="${productoId}" aria-label="Eliminar producto">
                    <i class="bi bi-trash3"></i>
                </button>
            </article>
        `;
    }

    function renderizarCarrito() {
        const carrito = obtenerCarritoInvitado();
        const contenido = document.getElementById("carritoInvitadoContenido");
        const empty = document.getElementById("carritoInvitadoEmpty");
        const itemsContainer = document.getElementById("carritoInvitadoItems");
        const cantidadTexto = document.getElementById("carritoInvitadoCantidadTexto");
        const subtotalTexto = document.getElementById("carritoSubtotal");
        const totalTexto = document.getElementById("carritoTotal");

        actualizarContadorCarrito();

        if (!itemsContainer || !contenido || !empty) {
            return;
        }

        if (!carrito.items || carrito.items.length === 0) {
            contenido.style.display = "none";
            empty.style.display = "block";
            return;
        }

        contenido.style.display = "grid";
        empty.style.display = "none";

        itemsContainer.innerHTML = carrito.items.map(construirItemHtml).join("");

        const totales = calcularTotales(carrito);

        if (cantidadTexto) {
            cantidadTexto.textContent = totales.cantidadTotal === 1
                ? "1 producto"
                : totales.cantidadTotal + " productos";
        }

        if (subtotalTexto) {
            subtotalTexto.textContent = formatearMoneda(totales.subtotal);
        }

        if (totalTexto) {
            totalTexto.textContent = formatearMoneda(totales.total);
        }

        inicializarEventosItems();
    }

    function actualizarCantidad(productoId, cambio) {
        const carrito = obtenerCarritoInvitado();

        const item = carrito.items.find(function (producto) {
            return Number(producto.productoId) === Number(productoId);
        });

        if (!item) {
            return;
        }

        item.cantidad = Number(item.cantidad || 0) + cambio;

        if (item.cantidad <= 0) {
            carrito.items = carrito.items.filter(function (producto) {
                return Number(producto.productoId) !== Number(productoId);
            });
        }

        guardarCarritoInvitado(carrito);
        renderizarCarrito();
    }

    function eliminarProducto(productoId) {
        const carrito = obtenerCarritoInvitado();

        carrito.items = carrito.items.filter(function (producto) {
            return Number(producto.productoId) !== Number(productoId);
        });

        guardarCarritoInvitado(carrito);
        renderizarCarrito();
    }

    function vaciarCarrito() {
        guardarCarritoInvitado({
            items: []
        });

        renderizarCarrito();
    }

    function inicializarEventosItems() {
        document.querySelectorAll("[data-carrito-sumar]").forEach(function (boton) {
            boton.addEventListener("click", function () {
                actualizarCantidad(Number(boton.getAttribute("data-carrito-sumar")), 1);
            });
        });

        document.querySelectorAll("[data-carrito-restar]").forEach(function (boton) {
            boton.addEventListener("click", function () {
                actualizarCantidad(Number(boton.getAttribute("data-carrito-restar")), -1);
            });
        });

        document.querySelectorAll("[data-carrito-eliminar]").forEach(function (boton) {
            boton.addEventListener("click", function () {
                eliminarProducto(Number(boton.getAttribute("data-carrito-eliminar")));
            });
        });
    }

    function inicializarAccionesGenerales() {
        const btnVaciar = document.getElementById("btnVaciarCarritoInvitado");
        const btnCheckout = document.getElementById("btnFinalizarCompraInvitado");

        if (btnVaciar) {
            btnVaciar.addEventListener("click", function () {
                const carrito = obtenerCarritoInvitado();

                if (!carrito.items || carrito.items.length === 0) {
                    return;
                }

                const confirmar = window.confirm("¿Deseas vaciar el carrito?");

                if (confirmar) {
                    vaciarCarrito();
                }
            });
        }

        if (btnCheckout) {
            btnCheckout.addEventListener("click", function () {
                const carrito = obtenerCarritoInvitado();

                if (!carrito.items || carrito.items.length === 0) {
                    renderizarCarrito();
                    return;
                }

                window.location.href = "/Home/Login?returnUrl=/PortalCliente/Checkout";
            });
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        renderizarCarrito();
        inicializarAccionesGenerales();
    });
})();