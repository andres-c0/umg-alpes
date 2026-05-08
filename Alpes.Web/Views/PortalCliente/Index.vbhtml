@Code
    ViewData("Title") = "Inicio"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
    Dim username As String = System.Convert.ToString(ViewData("Username"))
    If String.IsNullOrWhiteSpace(username) Then username = "Cliente"
    Dim primerNombre As String = username
    If Not String.IsNullOrWhiteSpace(username) AndAlso username.Contains(" ") Then primerNombre = username.Split(" "c)(0)
    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code

<div class="mhome-page" data-cli-id="@cliIdTexto">
    <section class="mhome-banner">
        <div class="mhome-banner-copy">
            <span class="mhome-kicker" id="mhomeSaludoHora">Bienvenido</span>
            <h1>@primerNombre</h1>
            <div class="mhome-status"><span></span> Cliente activo</div>
        </div>
        <a href="@Url.Action("Busqueda", "PortalCliente")" class="mhome-banner-btn"><i class="bi bi-grid"></i> Ver catálogo</a>
        <div class="mhome-deco"></div>
    </section>

    <section class="mhome-search-wrap">
        <div class="mhome-search">
            <i class="bi bi-search"></i>
            <input type="text" id="ciBuscarProducto" placeholder="Buscar muebles, salas, comedores…" autocomplete="off" />
            <button type="button" id="ciBtnBuscar" aria-label="Buscar"><i class="bi bi-arrow-right"></i></button>
        </div>
        <div id="mhomeSearchResults" class="mhome-search-results"></div>
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
            <article class="mhome-kpi"><div class="mhome-kpi-icon brown"><i class="bi bi-bag-check"></i></div><strong id="ciPedidosTotal">0</strong><span>PEDIDOS<br />TOTALES</span></article>
            <article class="mhome-kpi"><div class="mhome-kpi-icon green"><i class="bi bi-truck"></i></div><strong id="ciPedidosActivos">0</strong><span>EN<br />CAMINO</span></article>
            <article class="mhome-kpi"><div class="mhome-kpi-icon gold"><i class="bi bi-check-circle"></i></div><strong id="ciPedidosEntregados">0</strong><span>ENTREGADOS</span></article>
            <article class="mhome-kpi"><div class="mhome-kpi-icon red"><i class="bi bi-cash"></i></div><strong id="ciTotalComprado">Q0</strong><span>TOTAL<br />GASTADO</span></article>
        </div>
    </section>

    <section class="mhome-panels">
        <article class="mhome-panel mhome-orders-panel">
            <div class="mhome-panel-head">
                <div><span>Compras</span><h2>Mis pedidos recientes</h2></div>
                <a href="@Url.Action("MisOrdenes", "PortalCliente")">Ver todos →</a>
            </div>
            <div id="mhomeOrdenesRecientes" class="mhome-orders-list">
                <div class="mhome-mini-empty">Cargando pedidos...</div>
            </div>
        </article>

        <article class="mhome-panel mhome-track-panel">
            <div class="mhome-panel-head">
                <div><span>Seguimiento</span><h2>Último envío</h2></div>
            </div>
            <div id="mhomeTrackingActual" class="mhome-track-card">
                <div class="mhome-mini-empty">Consultando tracking...</div>
            </div>
        </article>
    </section>

    <section class="mhome-quick-section">
        <div class="mhome-section-head">
            <div><span>Accesos</span><h2>Todo a la mano</h2></div>
        </div>
        <div class="mhome-quick-row">
            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="mhome-quick"><i class="bi bi-heart-fill"></i><span>Favoritos</span></a>
            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="mhome-quick"><i class="bi bi-receipt"></i><span>Pedidos</span></a>
            <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="mhome-quick"><i class="bi bi-credit-card"></i><span>Tarjetas</span></a>
            <a href="@Url.Action("Soporte", "PortalCliente")" class="mhome-quick"><i class="bi bi-headset"></i><span>Soporte</span></a>
        </div>
    </section>

    <section class="mhome-products-section">
        <div class="mhome-section-head">
            <div><span>Para ti</span><h2>Recomendados</h2></div>
            <a href="@Url.Action("Busqueda", "PortalCliente")">Ver catálogo →</a>
        </div>
        <div id="piRecomendadosContainer" class="mhome-product-strip">
            <div class="mhome-mini-empty">Cargando recomendaciones...</div>
        </div>
    </section>

    <section class="mhome-products-section">
        <div class="mhome-section-head">
            <div><span>Tienda</span><h2>Todos los productos</h2></div>
            <a href="@Url.Action("Busqueda", "PortalCliente")">Ver completo →</a>
        </div>
        <div class="mhome-filter-pill-row" aria-label="Filtros rápidos">
            <select id="ciFiltroCategoria"><option value="">Todas</option></select>
            <select id="ciFiltroTipo"><option value="">Todos los tipos</option></select>
            <select id="ciFiltroColor"><option value="">Colores</option></select>
            <select id="ciFiltroMaterial"><option value="">Materiales</option></select>
            <button type="button" id="ciBtnLimpiar">Limpiar</button>
        </div>
        <div id="ciCatalogoContainer" class="mhome-product-grid">
            <div class="mhome-mini-empty">Cargando catálogo...</div>
        </div>
    </section>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-inicio.js?v=18")"></script>
End Section
