<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewBag.Title — Muebles de los Alpes</title>

    <%-- Bootstrap Icons --%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <%-- Alpes Design System --%>
    @Styles.Render("~/Content/alpes.css")

    @RenderSection("styles", required:=False)
</head>
<body>

<div class="a-overlay" id="sidebar-overlay"></div>
<div id="a-toast-wrap"></div>

<div class="a-wrap">

    <%-- ══════════════ SIDEBAR ══════════════ --%>
    <aside class="a-sidebar" id="sidebar">

        <%-- Logo --%>
        <div class="a-sidebar__top">
            <div style="display:flex;align-items:center;gap:10px">
                <div style="width:32px;height:32px;background:var(--oro);border-radius:8px;display:flex;align-items:center;justify-content:center;font-size:16px;color:var(--cafe-oscuro);flex-shrink:0">
                    <i class="bi bi-house-heart-fill"></i>
                </div>
                <div>
                    <div class="a-sidebar__brand">Muebles de los Alpes</div>
                    <div class="a-sidebar__sub" id="sidebar-portal-label">Portal del Cliente</div>
                </div>
            </div>
        </div>

        <%-- Nav Cliente (se muestra si rol = cliente) --%>
        <div id="nav-cliente">
            <div class="a-nav-section">Mi cuenta</div>

            <a class="a-nav-item" href="/Cliente/Index">
                <i class="bi bi-grid-fill"></i>
                <span>Inicio</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Perfil">
                <i class="bi bi-person"></i>
                <span>Mi perfil</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Tarjetas">
                <i class="bi bi-credit-card"></i>
                <span>Mis tarjetas</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Ordenes">
                <i class="bi bi-bag"></i>
                <span>Mis pedidos</span>
                <span class="a-nav-badge" id="badge-pedidos" style="display:none">0</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Seguimiento">
                <i class="bi bi-geo-alt"></i>
                <span>Tracking</span>
            </a>

            <div class="a-nav-section">Tienda</div>

            <a class="a-nav-item" href="/Cliente/Catalogo">
                <i class="bi bi-shop"></i>
                <span>Catálogo</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Favoritos">
                <i class="bi bi-heart"></i>
                <span>Favoritos</span>
            </a>
            <a class="a-nav-item" href="/Cliente/Busqueda">
                <i class="bi bi-search"></i>
                <span>Buscar</span>
            </a>
        </div>

        <%-- Nav Admin (se muestra si rol = admin) --%>
        <div id="nav-admin" style="display:none">
            <div class="a-nav-section">Comercial</div>

            <div class="a-nav-item" data-children="productos">
                <i class="bi bi-box-seam"></i>
                <span>Productos</span>
                <i class="bi bi-chevron-right a-nav-arrow"></i>
            </div>
            <div class="a-nav-children" id="nav-ch-productos">
                <a class="a-nav-item" href="/Admin/Productos">
                    <i class="bi bi-list-ul"></i><span>Lista de productos</span>
                </a>
                <a class="a-nav-item" href="/Admin/Inventario">
                    <i class="bi bi-building"></i><span>Inventario</span>
                </a>
            </div>

            <a class="a-nav-item" href="/Admin/Ordenes">
                <i class="bi bi-receipt"></i>
                <span>Órdenes de venta</span>
                <span class="a-nav-badge" id="badge-ordenes-admin" style="display:none">0</span>
            </a>
            <a class="a-nav-item" href="/Admin/Clientes">
                <i class="bi bi-people"></i><span>Clientes</span>
            </a>
            <a class="a-nav-item" href="/Admin/Marketing">
                <i class="bi bi-megaphone"></i><span>Marketing</span>
            </a>
            <a class="a-nav-item" href="/Admin/Reportes">
                <i class="bi bi-bar-chart"></i><span>Reportes</span>
            </a>

            <div class="a-nav-section">Operativa</div>

            <a class="a-nav-item" href="/Admin/Empleados">
                <i class="bi bi-person-badge"></i><span>Empleados</span>
            </a>
            <a class="a-nav-item" href="/Admin/Nomina">
                <i class="bi bi-cash-coin"></i><span>Nómina</span>
            </a>

            <div class="a-nav-item" data-children="proveedores">
                <i class="bi bi-truck"></i>
                <span>Proveedores</span>
                <i class="bi bi-chevron-right a-nav-arrow"></i>
            </div>
            <div class="a-nav-children" id="nav-ch-proveedores">
                <a class="a-nav-item" href="/Admin/Proveedores">
                    <i class="bi bi-list-ul"></i><span>Lista proveedores</span>
                </a>
                <a class="a-nav-item" href="/Admin/Compras">
                    <i class="bi bi-bag-check"></i><span>Órdenes de compra</span>
                </a>
            </div>

            <a class="a-nav-item" href="/Admin/Produccion">
                <i class="bi bi-gear"></i><span>Producción</span>
            </a>
            <a class="a-nav-item" href="/Admin/Configuracion">
                <i class="bi bi-sliders"></i><span>Configuración</span>
            </a>
        </div>

        <%-- Footer ─ usuario --%>
        <div class="a-sidebar__footer">
            <div class="a-sidebar__user">
                <div class="a-sidebar__avatar" id="sidebar-avatar">U</div>
                <div style="flex:1;min-width:0">
                    <span class="a-sidebar__user-name" id="sidebar-name">—</span>
                    <span class="a-sidebar__user-email" id="sidebar-email">—</span>
                </div>
            </div>
            <button class="a-sidebar__logout">
                <i class="bi bi-box-arrow-left"></i> Cerrar sesión
            </button>
        </div>

    </aside>
    <%-- /sidebar --%>

    <%-- ══════════════ MAIN ══════════════ --%>
    <div class="a-main">

        <%-- Topbar --%>
        <header class="a-topbar">
            <button class="a-topbar__menu-btn a-icon-btn" id="menu-btn">
                <i class="bi bi-list"></i>
            </button>

            <span class="a-topbar__title">@If ViewBag.PageTitle IsNot Nothing Then
                @ViewBag.PageTitle
            Else
                @:Muebles de los Alpes
            End If</span>

            <div class="a-topbar__actions">
                <a class="a-icon-btn" href="/Cliente/Busqueda" id="btn-busqueda">
                    <i class="bi bi-search"></i>
                </a>
                <a class="a-icon-btn" href="/Cliente/Carrito" id="btn-carrito">
                    <i class="bi bi-bag"></i>
                    <span class="a-cart-count" style="display:none">0</span>
                </a>
                <div class="a-icon-btn" style="position:relative">
                    <i class="bi bi-bell"></i>
                    <span class="dot"></span>
                </div>
            </div>
        </header>

        <%-- Page body --%>
        <main class="a-content">
            @RenderBody()
        </main>

    </div>
    <%-- /main --%>

</div>
<%-- /wrap --%>

<%-- Scripts --%>
@Scripts.Render("~/bundles/jquery")
<script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
@Scripts.Render("~/Scripts/alpes.js")

<script>
    $(function () {
        // Determinar rol y mostrar nav correcto
        var user = AlpesSession.get();
        if (user) {
            var rolId = user.ROL_ID || user.rol_id;
            if (rolId && rolId !== 3) {
                // Admin
                $('#nav-cliente').hide();
                $('#nav-admin').show();
                $('#sidebar-portal-label').text('Panel Administrativo');
                $('#btn-carrito, #btn-busqueda').hide();
            }
        }

        // Marcar item activo del nav
        var path = window.location.pathname.toLowerCase();
        $('.a-nav-item[href]').each(function () {
            var href = $(this).attr('href').toLowerCase();
            if (path === href || (href !== '/' && path.startsWith(href))) {
                $(this).addClass('active');
                // Abrir padre si existe
                var parent = $(this).closest('.a-nav-children');
                if (parent.length) {
                    parent.addClass('open');
                    parent.prev('.a-nav-item[data-children]').addClass('open');
                }
            }
        });

        // Mobile menu
        $('#menu-btn').on('click', function () {
            $('#sidebar').addClass('open');
            $('#sidebar-overlay').addClass('show');
        });
        $('#sidebar-overlay').on('click', function () {
            $('#sidebar').removeClass('open');
            $(this).removeClass('show');
        });
    });
</script>

@RenderSection("scripts", required:=False)

</body>
</html>
