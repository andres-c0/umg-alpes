@Code
    ViewData("Title") = "Inicio"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="mhome-page">

    <section class="mhome-banner">
        <div class="mhome-banner-copy">
            <span class="mhome-kicker">Buenas tardes</span>
            <h1>@(If(Session("NombreCliente") IsNot Nothing, Session("NombreCliente").ToString().Split(" "c)(0), "Cliente"))</h1>
            <div class="mhome-status">
                <span></span> Cliente activo
            </div>
        </div>

        <a href="@Url.Action("Busqueda", "PortalCliente")" class="mhome-banner-btn">
            <i class="bi bi-grid"></i>
            Ver catálogo
        </a>

        <div class="mhome-deco"></div>
    </section>

    <section class="mhome-search-wrap">
        <form class="mhome-search" method="get" action="@Url.Action("Busqueda", "PortalCliente")">
            <i class="bi bi-search"></i>

            <input type="text"
                   name="q"
                   placeholder="Buscar muebles, salas, comedores..." />

            <button type="submit">
                <i class="bi bi-arrow-right"></i>
            </button>
        </form>
    </section>

    <section class="mhome-stats-block">
        <div class="mhome-section-head">
            <div>
                <span>Resumen de mi cuenta</span>
                <h2>Actividad reciente</h2>
            </div>
            <a href="@Url.Action("MiPerfil", "PortalCliente")">Ver perfil →</a>
        </div>

        <div class="mhome-kpis">
            <article class="mhome-kpi">
                <div class="mhome-kpi-icon brown"><i class="bi bi-bag-check"></i></div>
                <strong id="ciPedidosTotal">0</strong>
                <span>ORDENES<br />TOTALES</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon green"><i class="bi bi-truck"></i></div>
                <strong id="ciPedidosActivos">0</strong>
                <span>EN<br />CAMINO</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon gold"><i class="bi bi-check-circle"></i></div>
                <strong id="ciPedidosEntregados">0</strong>
                <span>ENTREGADOS</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon red"><i class="bi bi-cash"></i></div>
                <strong id="ciTotalComprado">Q0.00</strong>
                <span>TOTAL<br />GASTADO</span>
            </article>
        </div>
    </section>

    <section class="mhome-panels">
        <article class="mhome-panel mhome-orders-panel">
            <div class="mhome-panel-head">
                <div>
                    <span>Compras</span>
                    <h2>Mis órdenes recientes</h2>
                </div>
                <a href="@Url.Action("MisOrdenes", "PortalCliente")">Ver todos →</a>
            </div>

            <div id="mhomeOrdenesRecientes" class="mhome-orders-list">
                <div class="mhome-mini-empty">Cargando órdenes...</div>
            </div>
        </article>

        <article class="mhome-panel mhome-track-panel">
            <div class="mhome-panel-head">
                <div>
                    <span>Seguimiento</span>
                    <h2>Tracking activo</h2>
                </div>
            </div>

            <div class="mhome-track-card" id="mhomeTrackingActual">
                <div class="mhome-mini-empty">Cargando seguimiento...</div>
            </div>
        </article>
    </section>

    <section class="mhome-quick-section">
        <div class="mhome-section-head">
            <div>
                <span>Accesos rápidos</span>
                <h2>Accesos rápidos</h2>
            </div>
        </div>


        <div class="mhome-quick-row">
            <a href="@Url.Action("Busqueda", "PortalCliente")" class="mhome-quick">
                <i class="bi bi-grid"></i>
                <span>Catálogo</span>
            </a>

            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="mhome-quick">
                <i class="bi bi-receipt"></i>
                <span>Mis órdenes</span>
            </a>

            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="mhome-quick">
                <i class="bi bi-heart-fill"></i>
                <span>Favoritos</span>
            </a>

            <a href="@Url.Action("MisResenas", "PortalCliente")" class="mhome-quick">
                <i class="bi bi-star-fill"></i>
                <span>Mis reseñas</span>
            </a>
        </div>
    </section>
    <section class="mhome-products-preview">

        <div class="mhome-section-head">
            <div>
                <span>Para ti</span>
            </div>

            <a href="@Url.Action("Busqueda", "PortalCliente")">Ver todo</a>
        </div>

        <div id="homeParaTi" class="mhome-product-row">
        </div>

    </section>


    <section class="mhome-products-large">

        <div class="mhome-section-head">
            <div>
                <span>Productos</span>
            </div>

            <a href="@Url.Action("Busqueda", "PortalCliente")">Catálogo completo</a>
        </div>

        <div id="homeProductosGrandes" class="mhome-large-grid">
        </div>

        <div id="homeCatalogoCompleto" class="mhome-catalogo-grid">
        </div>

    </section>

</div>

<script>
    function actualizarBadgeCarritoInicio() {
        fetch("/PortalCliente/ObtenerCarritoData", {
            method: "GET",
            credentials: "same-origin",
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
            .then(function (r) { return r.json(); })
            .then(function (res) {
                var data = res.data || res.Data || res || {};
                var items = data.Items || data.items || [];
                var total = 0;

                items.forEach(function (item) {
                    total += Number(item.Cantidad || item.cantidad || 0);
                });

                var badges = document.querySelectorAll("#pcTopbarCartBadge, #pcBadgeCart");

                badges.forEach(function (badge) {
                    badge.textContent = total;
                    badge.style.display = total > 0 ? "" : "none";
                });
            });
    }
    document.addEventListener("DOMContentLoaded", function () {
        function money(valor) {
            var n = Number(valor || 0);
            return 'Q' + n.toLocaleString('es-GT', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
        }

        function mostrarToast(mensaje, tipo) {
            var toast = document.createElement("div");
            toast.style.position = "fixed";
            toast.style.top = "24px";
            toast.style.right = "24px";
            toast.style.zIndex = "99999";
            toast.style.padding = "16px 20px";
            toast.style.borderRadius = "16px";
            toast.style.fontWeight = "700";
            toast.style.background = tipo === "error" ? "#c0392b" : "#1f8f4d";
            toast.style.color = "#fff";
            toast.style.boxShadow = "0 12px 30px rgba(0,0,0,.18)";
            toast.innerHTML = (tipo === "error" ? "⚠ " : "✓ ") + mensaje;
            document.body.appendChild(toast);

            setTimeout(function () {
                toast.remove();
            }, 2500);
        }

        function imagen(p) {
            return p.ImagenUrl || p.imagenUrl || p.Imagen || p.UrlImagen || p.urlImagen || "";
        }

        function nombre(p) {
            return p.Nombre || p.nombre || p.NombreProducto || p.nombreProducto || "Producto";
        }

        function productoId(p) {
            return p.ProductoId || p.productoId || p.PRODUCTO_ID || 0;
        }

        function precio(p) {
            return p.PrecioActual || p.precioActual || p.Precio || p.precio || p.PrecioUnitario || p.precioUnitario || 0;
        }

        fetch("/PortalCliente/ObtenerMisOrdenesData", {
            method: "GET",
            credentials: "same-origin",
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
            .then(function (r) { return r.json(); })
            .then(function (res) {
                var ordenes = res.data || res.Data || [];
                var cont = document.getElementById("mhomeOrdenesRecientes");
                var tracking = document.getElementById("mhomeTrackingActual");
                var totalGastado = 0;
                var activas = 0;
                var entregadas = 0;

                ordenes.forEach(function (o) {
                    var estado = String(o.EstadoUi || o.Estado || "").toUpperCase();
                    totalGastado += Number(o.Total || o.TotalOrden || 0);

                    if (estado.indexOf("ENTREG") >= 0) {
                        entregadas++;
                    } else if (estado.indexOf("CANCEL") < 0) {
                        activas++;
                    }
                });

                var totalPedidos = document.getElementById("ciPedidosTotal");
                var pedidosActivos = document.getElementById("ciPedidosActivos");
                var pedidosEntregados = document.getElementById("ciPedidosEntregados");
                var totalComprado = document.getElementById("ciTotalComprado");

                if (totalPedidos) totalPedidos.textContent = ordenes.length;
                if (pedidosActivos) pedidosActivos.textContent = activas;
                if (pedidosEntregados) pedidosEntregados.textContent = entregadas;
                if (totalComprado) totalComprado.textContent = money(totalGastado);

                if (cont) {
                    cont.innerHTML = ordenes.length
                        ? ordenes.slice(0, 2).map(function (o) {
                            return `
                            <a class="mhome-order-item" href="/PortalCliente/DetalleOrden/${o.OrdenVentaId || ""}">
                                <div>
                                   <strong>${window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "Order" : "Orden"} ${o.NumOrden || o.NumeroOrden || o.Codigo || ""}</strong>
                                    <span>${o.FechaOrdenTexto || o.FechaOrden || o.Fecha || ""}</span>
                                </div>
                                <b>${money(o.Total || o.TotalOrden || 0)}</b>
                            </a>`;
                        }).join("")
                        : `<div class="mhome-mini-empty">${window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "No recent orders." : "Sin órdenes recientes."}</div>`;
                }

                if (tracking) {
                    var activa = ordenes.filter(function (o) {
                        var estado = String(o.EstadoUi || o.Estado || "").toUpperCase();
                        return estado.indexOf("ENTREG") < 0 && estado.indexOf("CANCEL") < 0;
                    })[0];

                    tracking.innerHTML = activa
                        ? `<strong>Orden ${activa.NumOrden || activa.NumeroOrden || ""}</strong>
                           <span>${activa.Resumen || "Pedido en proceso"}</span>
                           <br />
                           <span>✅ Orden confirmada</span>
                           <span>${String(activa.EstadoUi || "").toUpperCase().indexOf("CAMINO") >= 0 ? "✅" : "🟡"} ${activa.EstadoUi || "En proceso"}</span>
                           <a href="/PortalCliente/Tracking?ordenVentaId=${activa.OrdenVentaId || ""}">Ver tracking →</a>`
                        : `<div class="mhome-mini-empty">No tienes envíos activos.</div>`;
                }

                if (window.PortalClienteActualizarBadges) {
                    window.PortalClienteActualizarBadges();
                }
            });

        fetch("/PortalCliente/ObtenerCatalogoData")
            .then(function (r) { return r.json(); })
            .then(function (res) {
                var data = res.data || res.Data || res || [];
                var productos = data.map(function (x) {
                    return x.Producto || x.producto || x;
                });

                var paraTi = document.getElementById("homeParaTi");
                var grandes = document.getElementById("homeProductosGrandes");
                var catalogo = document.getElementById("homeCatalogoCompleto");

                if (paraTi) {
                    paraTi.innerHTML = productos.slice(0, 8).map(function (p) {
                        return `
                        <div class="mhome-product-mini">
                            <img src="${imagen(p)}" />
                            <h4>${nombre(p)}</h4>
                            <strong>${money(precio(p))}</strong>
                            <button type="button" data-add-cart="${productoId(p)}">${window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "Add" : "Agregar"}</button>
                        </div>`;
                    }).join("");
                }

                if (grandes) {
                    grandes.innerHTML = productos.slice(0, 2).map(function (p) {
                        return `
                        <div class="mhome-large-card">
                            <img src="${imagen(p)}" />
                        </div>`;
                    }).join("");
                }

                if (catalogo) {
                    catalogo.innerHTML = productos.map(function (p) {
                        return `
                        <div class="mhome-catalogo-card">
                            <img src="${imagen(p)}" />
                            <h4>${nombre(p)}</h4>
                            <strong>${money(precio(p))}</strong>
                            <button type="button" data-add-cart="${productoId(p)}">${window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "Add" : "Agregar"}</button>
                        </div>`;
                    }).join("");
                }
            });

        document.addEventListener("click", function (e) {
            var btn = e.target.closest("[data-add-cart]");
            if (!btn) return;

            var id = Number(btn.getAttribute("data-add-cart") || 0);
            if (!id) return;

            btn.disabled = true;

            fetch("/PortalCliente/AgregarAlCarritoData", {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify({
                    productoId: id,
                    cantidad: 1
                })
            })
                .then(function (r) { return r.json(); })
                .then(function () {
                    mostrarToast(window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "Product added to cart." : "Producto agregado al carrito.", "success");
                    actualizarBadgeCarritoInicio();

                    if (window.PortalClienteActualizarBadges) {
                        window.PortalClienteActualizarBadges();
                    }
                })
                .catch(function () {
                    mostrarToast(window.PortalIdioma && window.PortalIdioma.get && window.PortalIdioma.get() === "en" ? "Could not add to cart." : "No se pudo agregar al carrito.", "error");
                })
                .finally(function () {
                    btn.disabled = false;
                });
        });
        actualizarBadgeCarritoInicio();
    });
</script>