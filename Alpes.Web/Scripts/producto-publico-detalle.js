(function () {
    const CLAVE_CARRITO_INVITADO = "alpes_carrito_invitado";
    let productoActual = null;

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

    function agregarProductoAlCarritoInvitado(producto) {
        const carrito = obtenerCarritoInvitado();

        const existente = carrito.items.find(function (item) {
            return Number(item.productoId) === Number(producto.productoId);
        });

        if (existente) {
            existente.cantidad = Number(existente.cantidad || 0) + 1;
        } else {
            carrito.items.push({
                productoId: Number(producto.productoId),
                nombre: producto.nombre || "Producto",
                precio: Number(producto.precio || 0),
                imagenUrl: producto.imagenUrl || "",
                tipo: producto.tipo || "",
                cantidad: 1
            });
        }

        guardarCarritoInvitado(carrito);
        actualizarContadorCarrito();
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

    function mostrarError(mensaje) {
        const loading = document.getElementById("productoDetalleLoading");
        const contenido = document.getElementById("productoDetalleContenido");
        const error = document.getElementById("productoDetalleError");
        const errorTexto = document.getElementById("productoDetalleErrorTexto");

        if (loading) {
            loading.style.display = "none";
        }

        if (contenido) {
            contenido.style.display = "none";
        }

        if (error) {
            error.style.display = "block";
        }

        if (errorTexto) {
            errorTexto.textContent = mensaje || "Intenta nuevamente más tarde.";
        }
    }

    function pintarProducto(producto) {
        productoActual = producto;

        const productoId = Number(obtenerValor(producto, "ProductoId", 0));
        const referencia = obtenerValor(producto, "Referencia", "");
        const nombre = obtenerValor(producto, "Nombre", "Producto");
        const descripcion = obtenerValor(producto, "Descripcion", "");
        const tipo = obtenerValor(producto, "Tipo", "PRODUCTO");
        const material = obtenerValor(producto, "Material", "");
        const color = obtenerValor(producto, "Color", "");
        const alto = Number(obtenerValor(producto, "AltoCm", 0));
        const ancho = Number(obtenerValor(producto, "AnchoCm", 0));
        const profundidad = Number(obtenerValor(producto, "ProfundidadCm", 0));
        const peso = Number(obtenerValor(producto, "PesoGramos", 0));
        const imagenUrl = normalizarImagenUrl(obtenerValor(producto, "ImagenUrl", ""));
        const precioActual = Number(obtenerValor(producto, "PrecioActual", 0));
        const precioFormateado = obtenerValor(producto, "PrecioFormateado", "Q 0.00");
        const cuota12Formateada = obtenerValor(producto, "Cuota12Formateada", "Q 0.00");

        const loading = document.getElementById("productoDetalleLoading");
        const contenido = document.getElementById("productoDetalleContenido");
        const imagen = document.getElementById("productoDetalleImagen");

        if (loading) {
            loading.style.display = "none";
        }

        if (contenido) {
            contenido.style.display = "grid";
        }

        setText("productoDetalleTipo", tipo || "PRODUCTO");
        setText("productoDetalleReferencia", referencia || "Sin referencia");
        setText("productoDetalleNombre", nombre);
        setText("productoDetalleDescripcion", descripcion || "Mueble artesanal de Muebles de los Alpes, diseñado para aportar estilo, comodidad y presencia a tu hogar.");
        setText("productoDetallePrecio", precioFormateado);
        setText("productoDetalleCuota", "12 cuotas de " + cuota12Formateada);
        setText("productoDetalleMaterial", material || "No especificado");
        setText("productoDetalleColor", color || "No especificado");

        if (alto > 0 || ancho > 0 || profundidad > 0) {
            setText("productoDetalleDimensiones", alto + " cm x " + ancho + " cm x " + profundidad + " cm");
        } else {
            setText("productoDetalleDimensiones", "No especificado");
        }

        if (peso > 0) {
            setText("productoDetallePeso", peso + " g");
        } else {
            setText("productoDetallePeso", "No especificado");
        }

        if (imagen) {
            if (imagenUrl) {
                imagen.innerHTML = `<img src="${imagenUrl}" alt="${nombre}" />`;
            } else {
                imagen.innerHTML = `<i class="bi ${obtenerIconoPorTipo(tipo)}"></i>`;
            }
        }

        const botonAgregar = document.getElementById("btnAgregarDetalleCarrito");

        if (botonAgregar) {
            botonAgregar.onclick = function () {
                agregarProductoAlCarritoInvitado({
                    productoId: productoId,
                    nombre: nombre,
                    precio: precioActual,
                    imagenUrl: imagenUrl,
                    tipo: tipo
                });

                const contenidoOriginal = botonAgregar.innerHTML;
                botonAgregar.innerHTML = `<i class="bi bi-check2"></i> Agregado al carrito`;
                botonAgregar.classList.add("producto-agregado");

                window.setTimeout(function () {
                    botonAgregar.innerHTML = contenidoOriginal;
                    botonAgregar.classList.remove("producto-agregado");
                }, 900);
            };
        }
    }

    function setText(id, texto) {
        const elemento = document.getElementById(id);

        if (elemento) {
            elemento.textContent = texto;
        }
    }

    async function cargarDetalleProducto() {
        const page = document.getElementById("productoPublicoPage");

        if (!page) {
            return;
        }

        const productoId = Number(page.getAttribute("data-producto-id") || "0");
        const endpointBase = page.getAttribute("data-producto-url") || "/Home/ObtenerProductoPublicoData";

        if (productoId <= 0) {
            mostrarError("Producto inválido.");
            return;
        }

        try {
            const endpoint = endpointBase + "?id=" + encodeURIComponent(productoId);

            const respuesta = await fetch(endpoint, {
                method: "GET",
                headers: {
                    "Accept": "application/json"
                }
            });

            const data = await respuesta.json();

            if (!respuesta.ok || !data || data.ok === false) {
                throw new Error(data && data.message ? data.message : "No se pudo cargar el producto.");
            }

            pintarProducto(data.data);

        } catch (error) {
            console.error("Error al cargar detalle de producto:", error);
            mostrarError(error.message || "No se pudo cargar el producto.");
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        cargarDetalleProducto();
    });
})();