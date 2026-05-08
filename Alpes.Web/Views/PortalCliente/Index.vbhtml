@Code
    ViewData("Title") = "Inicio"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="mhome-page">

    <section class="mhome-banner">
        <div class="mhome-banner-copy">
            <span class="mhome-kicker">Buenas tardes</span>
            <h1>falvarado</h1>
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
            <a href="#">Ver perfil →</a>
        </div>

        <div class="mhome-kpis">
            <article class="mhome-kpi">
                <div class="mhome-kpi-icon brown"><i class="bi bi-bag-check"></i></div>
                <strong>2</strong>
                <span>PEDIDOS<br />TOTALES</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon green"><i class="bi bi-truck"></i></div>
                <strong>0</strong>
                <span>EN<br />CAMINO</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon gold"><i class="bi bi-check-circle"></i></div>
                <strong>0</strong>
                <span>ENTREGADOS</span>
            </article>

            <article class="mhome-kpi">
                <div class="mhome-kpi-icon red"><i class="bi bi-cash"></i></div>
                <strong>Q13.7k</strong>
                <span>TOTAL<br />GASTADO</span>
            </article>
        </div>
    </section>

    <section class="mhome-panels">
        <article class="mhome-panel mhome-orders-panel">
            <div class="mhome-panel-head">
                <div>
                    <span>Compras</span>
                    <h2>Mis pedidos recientes</h2>
                </div>
                <a href="#">Ver todos →</a>
            </div>

            <div class="mhome-orders-list">
                <div class="mhome-order-item">
                    <div>
                        <strong>Pedido #ORD-2026-0001</strong>
                        <span>Mueble Alpes</span>
                    </div>
                    <b>Q9352</b>
                </div>

                <div class="mhome-order-item">
                    <div>
                        <strong>Pedido #ORD-2025-0001</strong>
                        <span>Mueble Alpes</span>
                    </div>
                    <b>Q4312</b>
                </div>
            </div>
        </article>

        <article class="mhome-panel mhome-track-panel">
            <div class="mhome-panel-head">
                <div>
                    <span>Seguimiento</span>
                    <h2>Tracking activo</h2>
                </div>
            </div>

            <div class="mhome-track-card">
                <strong>Pedido #ORD-2026-0001</strong>
                <span>Sofá Alpino — estimado</span>
                <br />
                <span>✅ Pedido confirmado</span>
                <span>🟡 En producción</span>
                <span>○ En camino</span>
                <span>○ Entregado</span>
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

            <a href="#">Ver todo</a>
        </div>

        <div id="homeParaTi" class="mhome-product-row">
        </div>

    </section>


    <section class="mhome-products-large">

        <div class="mhome-section-head">
            <div>
                <span>Productos</span>
            </div>

            <a href="#">Catálogo completo</a>
        </div>

        <div id="homeProductosGrandes" class="mhome-large-grid">
        </div>

        <div id="homeCatalogoCompleto" class="mhome-catalogo-grid">
        </div>

    </section>

</div>

<script>
    document.addEventListener("DOMContentLoaded", function () {

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

                function imagen(p) {
                    return p.ImagenUrl || p.imagenUrl || p.Imagen || p.UrlImagen || p.urlImagen || "";
                }

                function nombre(p) {
                    return p.Nombre || p.nombre || p.NombreProducto || p.nombreProducto || "Producto";
                }

                function precio(p) {
                    return p.PrecioActual || p.precioActual || p.Precio || p.precio || p.PrecioUnitario || p.precioUnitario || 0;
                }

                paraTi.innerHTML = productos.slice(0, 8).map(function (p) {
                    return `
                        <div class="mhome-product-mini">
                            <img src="${imagen(p)}" />
                            <h4>${nombre(p)}</h4>
                            <strong>Q ${precio(p)}</strong>
                            <button>Agregar</button>
                        </div>
                    `;
                }).join("");

                grandes.innerHTML = productos.slice(0, 2).map(function (p) {
                    return `
                        <div class="mhome-large-card">
                            <img src="${imagen(p)}" />
                        </div>
                    `;
                }).join("");

                catalogo.innerHTML = productos.map(function (p) {
                    return `
                        <div class="mhome-catalogo-card">
                            <img src="${imagen(p)}" />
                            <h4>${nombre(p)}</h4>
                            <strong>Q ${precio(p)}</strong>
                            <button>Agregar</button>
                        </div>
                    `;
                }).join("");
            });
    });
</script>