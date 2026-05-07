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

    function inicializarMenuMovil() {
        const boton = document.getElementById("publicMenuButton");
        const menu = document.getElementById("publicMobileMenu");

        if (!boton || !menu) {
            return;
        }

        boton.addEventListener("click", function () {
            menu.classList.toggle("open");
        });
    }

    function inicializarCarruselHome() {
        const slider = document.getElementById("homeHeroSlider");

        if (!slider) {
            return;
        }

        const slides = Array.from(slider.querySelectorAll(".home-hero-slide"));
        const indicadores = Array.from(slider.querySelectorAll("[data-target-slide]"));

        if (slides.length === 0) {
            return;
        }

        let indiceActual = 0;
        let intervalo = null;

        function mostrarSlide(indice) {
            if (indice < 0) {
                indice = slides.length - 1;
            }

            if (indice >= slides.length) {
                indice = 0;
            }

            slides.forEach(function (slide, index) {
                slide.classList.toggle("active", index === indice);
            });

            indicadores.forEach(function (indicador, index) {
                indicador.classList.toggle("active", index === indice);
            });

            indiceActual = indice;
        }

        function iniciarAutoPlay() {
            detenerAutoPlay();

            intervalo = window.setInterval(function () {
                mostrarSlide(indiceActual + 1);
            }, 6500);
        }

        function detenerAutoPlay() {
            if (intervalo) {
                window.clearInterval(intervalo);
                intervalo = null;
            }
        }

        indicadores.forEach(function (indicador) {
            indicador.addEventListener("click", function () {
                const indice = Number(indicador.getAttribute("data-target-slide") || "0");
                mostrarSlide(indice);
                iniciarAutoPlay();
            });
        });

        slider.addEventListener("mouseenter", detenerAutoPlay);
        slider.addEventListener("mouseleave", iniciarAutoPlay);

        mostrarSlide(0);
        iniciarAutoPlay();
    }

    function inicializarModalAcceso() {
        const modal = document.getElementById("homeAccessModal");

        if (!modal) {
            return;
        }

        const botonesCerrar = modal.querySelectorAll("[data-close-access-modal]");

        botonesCerrar.forEach(function (boton) {
            boton.addEventListener("click", function () {
                modal.classList.remove("open");
                modal.setAttribute("aria-hidden", "true");
            });
        });
    }

    function obtenerValorProducto(producto, nombreCampo, valorDefault) {
        if (!producto) {
            return valorDefault;
        }

        if (producto[nombreCampo] !== undefined && producto[nombreCampo] !== null) {
            return producto[nombreCampo];
        }

        const nombreCamel = nombreCampo.charAt(0).toLowerCase() + nombreCampo.slice(1);

        if (producto[nombreCamel] !== undefined && producto[nombreCamel] !== null) {
            return producto[nombreCamel];
        }

        return valorDefault;
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

    function construirCardProducto(producto) {
        const productoId = Number(obtenerValorProducto(producto, "ProductoId", 0));
        const nombre = obtenerValorProducto(producto, "Nombre", "Producto sin nombre");
        const descripcion = obtenerValorProducto(producto, "Descripcion", "");
        const tipo = obtenerValorProducto(producto, "Tipo", "PRODUCTO");
        const material = obtenerValorProducto(producto, "Material", "");
        const imagenUrl = normalizarImagenUrl(obtenerValorProducto(producto, "ImagenUrl", ""));
        const precioActual = Number(obtenerValorProducto(producto, "PrecioActual", 0));
        const precioFormateado = obtenerValorProducto(producto, "PrecioFormateado", formatearMoneda(precioActual));
        const cuota12Formateada = obtenerValorProducto(producto, "Cuota12Formateada", formatearMoneda(precioActual / 12));
        const icono = obtenerIconoPorTipo(tipo);

        const subtitulo = material && String(material).trim().length > 0
            ? material
            : descripcion;

        const imagenHtml = imagenUrl
            ? `<img src="${escaparHtml(imagenUrl)}" alt="${escaparHtml(nombre)}" loading="lazy" />`
            : `<i class="bi ${icono}"></i>`;

        return `
            <article class="home-product-card" data-producto-id="${productoId}">
                <a href="/Home/DetalleProducto/${productoId}" class="home-product-image home-product-image-link" aria-label="Ver detalle de ${escaparHtml(nombre)}">
                    ${imagenHtml}
                </a>

                <div class="home-product-info">
                    <span>${escaparHtml(tipo || "PRODUCTO")}</span>

                    <h3>${escaparHtml(nombre)}</h3>

                    <p class="home-product-subtitle">
                        ${escaparHtml(subtitulo || "Mueble artesanal de Muebles de los Alpes.")}
                    </p>

                    <div class="home-product-price-row">
                        <strong>${escaparHtml(precioFormateado)}</strong>

                        <button type="button"
                                class="home-product-cart-btn"
                                data-add-producto="true"
                                data-producto-id="${productoId}"
                                data-nombre="${escaparHtml(nombre)}"
                                data-precio="${precioActual}"
                                data-imagen="${escaparHtml(imagenUrl)}"
                                data-tipo="${escaparHtml(tipo)}"
                                aria-label="Agregar ${escaparHtml(nombre)} al carrito">
                            <i class="bi bi-cart3"></i>
                        </button>
                    </div>

                    <small>12 cuotas de ${escaparHtml(cuota12Formateada)}</small>
                </div>
            </article>
        `;
    }

    async function cargarProductosDestacados() {
        const contenedor = document.getElementById("homeProductosDestacados");
        const loading = document.getElementById("homeProductosLoading");
        const empty = document.getElementById("homeProductosEmpty");

        if (!contenedor) {
            return;
        }

        const endpointBase = contenedor.getAttribute("data-productos-url") || "/Home/ObtenerProductosPublicosData";
        const separador = endpointBase.indexOf("?") >= 0 ? "&" : "?";
        const endpoint = endpointBase + separador + "limite=6";

        try {
            if (loading) {
                loading.style.display = "flex";
            }

            if (empty) {
                empty.style.display = "none";
            }

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

            if (productos.length === 0) {
                contenedor.innerHTML = "";

                if (empty) {
                    empty.style.display = "block";
                }

                return;
            }

            contenedor.innerHTML = productos.map(construirCardProducto).join("");
            inicializarBotonesAgregarProducto();

        } catch (error) {
            console.error("Error al cargar productos públicos:", error);

            contenedor.innerHTML = "";

            if (empty) {
                empty.style.display = "block";
                const titulo = empty.querySelector("h3");
                const texto = empty.querySelector("p");

                if (titulo) {
                    titulo.textContent = "No se pudieron cargar los productos";
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

    function inicializarBotonesAgregarProducto() {
        const botones = document.querySelectorAll("[data-add-producto='true']");

        botones.forEach(function (boton) {
            boton.addEventListener("click", function () {
                const productoId = Number(boton.getAttribute("data-producto-id") || "0");

                if (productoId <= 0) {
                    return;
                }

                const nombre = boton.getAttribute("data-nombre") || "Producto";
                const precio = Number(boton.getAttribute("data-precio") || "0");
                const imagenUrl = boton.getAttribute("data-imagen") || "";
                const tipo = boton.getAttribute("data-tipo") || "";

                agregarProductoAlCarritoInvitado({
                    productoId: productoId,
                    nombre: nombre,
                    precio: precio,
                    imagenUrl: imagenUrl,
                    tipo: tipo,
                    cantidad: 1
                });

                boton.classList.add("agregado");

                const icono = boton.querySelector("i");

                if (icono) {
                    icono.className = "bi bi-check2";
                }

                window.setTimeout(function () {
                    boton.classList.remove("agregado");

                    if (icono) {
                        icono.className = "bi bi-cart3";
                    }
                }, 700);
            });
        });
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

    document.addEventListener("DOMContentLoaded", function () {
        actualizarContadorCarrito();
        inicializarMenuMovil();
        inicializarCarruselHome();
        inicializarModalAcceso();
        cargarProductosDestacados();
    });
})();