@Code
    ViewData("Title") = "Mis órdenes"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<style>
    .ordenes-mobile-page {
        margin: -16px -20px 0 -20px;
        background: #f6f2ec;
        min-height: calc(100vh - 70px);
        padding-bottom: 80px;
    }

    .ordenes-top {
        height: 58px;
        background: #321407;
        color: white;
        display: grid;
        grid-template-columns: 50px 1fr 50px;
        align-items: center;
        padding: 0 12px;
    }

        .ordenes-top h1 {
            margin: 0;
            font-size: 18px;
            font-weight: 900;
            text-align: center;
            color: white;
        }

        .ordenes-top span {
            display: block;
            text-align: center;
            color: #d8b66c;
            font-size: 11px;
        }

    .ordenes-back,
    .ordenes-refresh {
        width: 36px;
        height: 36px;
        border-radius: 10px;
        border: 0;
        background: rgba(255,255,255,.12);
        color: white;
        display: grid;
        place-items: center;
        text-decoration: none;
        font-size: 24px;
    }

    .ordenes-filtros {
        display: flex;
        gap: 10px;
        padding: 12px;
        background: white;
        border-bottom: 1px solid #eee4d8;
    }

        .ordenes-filtros button {
            border: 1px solid #cfc3b7;
            background: white;
            border-radius: 20px;
            padding: 9px 16px;
            color: #321407;
            font-size: 12px;
            font-weight: 800;
        }

            .ordenes-filtros button.active {
                background: #321407;
                color: white;
            }

    .ordenes-lista {
        padding: 12px;
    }

    .orden-card {
        background: white;
        border: 1px solid #eee4d8;
        border-radius: 12px;
        padding: 14px;
        margin-bottom: 14px;
    }

    .orden-card-top {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
    }

    .orden-left {
        display: flex;
        gap: 12px;
    }

    .orden-icon {
        width: 34px;
        height: 34px;
        border-radius: 10px;
        background: #efe5d8;
        display: grid;
        place-items: center;
        color: #9b7a45;
    }

    .orden-left strong,
    .orden-total strong {
        color: #321407;
        font-size: 13px;
        font-weight: 900;
    }

    .orden-left span {
        display: block;
        font-size: 11px;
        color: #7b6a5d;
    }

    .orden-total {
        text-align: right;
    }

        .orden-total span {
            display: inline-block;
            margin-top: 4px;
            background: #f2eadf;
            color: #9b7a45;
            padding: 3px 8px;
            border-radius: 10px;
            font-size: 10px;
            font-weight: 800;
        }

    .orden-progress {
        display: grid;
        grid-template-columns: auto 1fr auto 1fr auto;
        align-items: center;
        margin-top: 16px;
    }

        .orden-progress .line {
            height: 1px;
            background: #ddd2c3;
        }

    .step {
        text-align: center;
    }

        .step span {
            width: 13px;
            height: 13px;
            border-radius: 50%;
            background: #e2d8ca;
            display: inline-block;
        }

        .step.active span {
            background: #321407;
        }

        .step.current span {
            background: #caa46a;
        }

        .step small {
            display: block;
            font-size: 9px;
            color: #9a8b7c;
            margin-top: 4px;
        }

    .orden-detalle {
        display: block;
        text-align: right;
        margin-top: 12px;
        color: #8a6a3d;
        font-size: 12px;
        font-weight: 900;
        text-decoration: none;
    }

    .ordenes-empty {
        min-height: 430px;
        display: grid;
        place-items: center;
        text-align: center;
        color: #7b6a5d;
    }

    .ordenes-bottom-nav {
        position: fixed;
        left: 164px;
        right: 0;
        bottom: 0;
        height: 64px;
        background: white;
        display: grid;
        grid-template-columns: repeat(5, 1fr);
        border-top: 1px solid #eee4d8;
        z-index: 99;
    }

        .ordenes-bottom-nav a {
            display: grid;
            place-items: center;
            align-content: center;
            gap: 3px;
            color: #b39a6b;
            text-decoration: none;
            font-size: 11px;
            font-weight: 700;
        }

            .ordenes-bottom-nav a i {
                font-size: 18px;
            }

            .ordenes-bottom-nav a.active {
                color: #321407;
            }
</style>

<div class="ordenes-mobile-page">

    <div class="ordenes-top">
        <a href="@Url.Action("Index", "PortalCliente")" class="ordenes-back">‹</a>

        <div>
            <h1>Mis Órdenes</h1>
            <span id="ordenesCantidad">0 pedidos</span>
        </div>

        <button type="button" id="ordenesRefresh" class="ordenes-refresh">
            <i class="bi bi-arrow-clockwise"></i>
        </button>
    </div>

    <div class="ordenes-filtros">
        <button class="active" data-filter="TODAS">Todas</button>
        <button data-filter="PENDIENTE">Pendiente</button>
        <button data-filter="EN PROCESO">En proceso</button>
        <button data-filter="ENTREGADA">Entregado</button>
        <button data-filter="CANCELADA">Cancelado</button>
    </div>

    <div id="ordenesLista" class="ordenes-lista"></div>

    <nav class="ordenes-bottom-nav">
        <a href="@Url.Action("Index", "PortalCliente")">
            <i class="bi bi-house"></i>
            <span>Inicio</span>
        </a>

        <a href="@Url.Action("Busqueda", "PortalCliente")">
            <i class="bi bi-grid"></i>
            <span>Catálogo</span>
        </a>

        <a href="@Url.Action("Favoritos", "PortalCliente")">
            <i class="bi bi-heart"></i>
            <span>Favoritos</span>
        </a>

        <a class="active" href="@Url.Action("MisOrdenes", "PortalCliente")">
            <i class="bi bi-receipt"></i>
            <span>Órdenes</span>
        </a>

        <a href="#">
            <i class="bi bi-person"></i>
            <span>Perfil</span>
        </a>
    </nav>

</div>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        var lista = document.getElementById("ordenesLista");
        var cantidad = document.getElementById("ordenesCantidad");
        var refresh = document.getElementById("ordenesRefresh");
        var botones = document.querySelectorAll(".ordenes-filtros button");
        var filtroActual = "TODAS";
        var ordenes = [];

        function moneda(valor) {
            var n = Number(valor || 0);
            return "Q " + n.toFixed(2);
        }

        function normalizarEstado(estado) {
            return String(estado || "Pendiente");
        }

        function coincide(item) {
            if (filtroActual === "TODAS") return true;

            var estado = normalizarEstado(item.EstadoUi || item.Estado || "").toUpperCase();

            if (filtroActual === "PENDIENTE") return estado.includes("PENDIENTE");
            if (filtroActual === "EN PROCESO") return estado.includes("PROCESO") || estado.includes("ACTIVA");
            if (filtroActual === "ENTREGADA") return estado.includes("ENTREGADA");
            if (filtroActual === "CANCELADA") return estado.includes("CANCELADA");

            return true;
        }
        function actualizarBadgeOrdenes(total) {
            var badges = document.querySelectorAll("#pcBadgeOrders");

            badges.forEach(function (badge) {
                badge.textContent = total;
                badge.style.display = total > 0 ? "" : "none";
            });
        }
        function pintar() {
            var visibles = ordenes.filter(coincide);
            cantidad.textContent = ordenes.length + " ordenes";
            actualizarBadgeOrdenes(ordenes.length);
            if (!visibles.length) {
                lista.innerHTML = `
                    <div class="ordenes-empty">
                        <div>
                            <i class="bi bi-receipt" style="font-size:48px;color:#c9ad75;"></i>
                            <h3>Sin órdenes</h3>
                            <p>No tienes órdenes con este estado</p>
                        </div>
                    </div>`;
                return;
            }

            lista.innerHTML = visibles.map(function (o) {
                var num = o.NumOrden || o.NumeroOrden || ("ORD-" + o.OrdenVentaId);
                var fecha = o.FechaOrdenTexto || o.Fecha || "";
                var total = o.Total || o.TotalOrden || 0;
                var id = o.OrdenVentaId || o.Id || "";

                return `
                    <div class="orden-card">
                        <div class="orden-card-top">
                            <div class="orden-left">
                                <div class="orden-icon">
                                    <i class="bi bi-receipt"></i>
                                </div>

                                <div>
                                    <strong>Orden ${num}</strong>
                                    <span>${fecha}</span>
                                </div>
                            </div>

                            <div class="orden-total">
                                <strong>${moneda(total)}</strong>
                                <span>ACTIVO</span>
                            </div>
                        </div>

                        <div class="orden-progress">
                            <div class="step active">
                                <span></span>
                                <small>Pendiente</small>
                            </div>

                            <div class="line"></div>

                            <div class="step current">
                                <span></span>
                                <small>En proceso</small>
                            </div>

                            <div class="line"></div>

                            <div class="step">
                                <span></span>
                                <small>Entregado</small>
                            </div>
                        </div>

                        <a class="orden-detalle" href="@Url.Action("DetalleOrden", "PortalCliente")?id=${id}">
                            Ver detalles →
                        </a>
                    </div>
                `;
            }).join("");
        }

        function cargar() {
            lista.innerHTML = '<div class="ordenes-empty">Cargando órdenes...</div>';

            fetch("@Url.Action("ObtenerMisOrdenesData", "PortalCliente")")
                .then(function (r) { return r.json(); })
                .then(function (res) {
                    ordenes = res.data || res.Data || [];
                    pintar();
                    setTimeout(function () {
                        actualizarBadgeOrdenes(ordenes.length);
                    }, 500);
                });
        }

        botones.forEach(function (btn) {
            btn.addEventListener("click", function () {
                botones.forEach(function (b) { b.classList.remove("active"); });
                btn.classList.add("active");
                filtroActual = btn.getAttribute("data-filter");
                pintar();
            });
        });

        refresh.addEventListener("click", cargar);

        cargar();
    });
</script>