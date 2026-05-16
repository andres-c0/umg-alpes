@Code
    ViewData("Title") = "Mis Favoritos"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code
<style>
    .fav-page .fav-card.catalog-card {
        background: #fff !important;
        border: 1px solid #eadfd3 !important;
        border-radius: 16px !important;
        padding: 14px !important;
        box-shadow: 0 10px 30px rgba(58, 19, 8, .08) !important;
        overflow: hidden !important;
    }

    .fav-page .fav-image {
        height: 175px !important;
        background: #fff !important;
        border: 1px solid #eee2d5 !important;
        position: relative !important;
        display: grid !important;
        place-items: center !important;
        overflow: hidden !important;
    }

        .fav-page .fav-image img {
            width: 100% !important;
            height: 100% !important;
            object-fit: contain !important;
            padding: 12px !important;
        }

    .fav-page .fav-heart {
        position: absolute !important;
        top: 10px !important;
        right: 10px !important;
        width: 34px !important;
        height: 34px !important;
        border: 1px solid #eccaca !important;
        background: #f8e5e5 !important;
        color: #9f3535 !important;
        border-radius: 10px !important;
        display: grid !important;
        place-items: center !important;
        z-index: 10 !important;
    }

        .fav-page .fav-heart i {
            color: #9f3535 !important;
        }

    .fav-page .fav-actions {
        display: grid !important;
        gap: 8px !important;
        margin-top: 14px !important;
    }

    .fav-page .fav-btn {
        width: 100% !important;
        border-radius: 9px !important;
        padding: 11px 14px !important;
        text-align: center !important;
        font-size: 13px !important;
        font-weight: 900 !important;
        text-decoration: none !important;
        display: block !important;
    }

    .fav-page .fav-btn-detail {
        background: #fff !important;
        color: #2f1208 !important;
        border: 1px solid #eadfd3 !important;
    }

    .fav-page .fav-btn-cart {
        background: #deb04f !important;
        color: #2f1208 !important;
        border: 0 !important;
    }
    .fav-toast {
        position: fixed;
        top: 92px;
        right: 28px;
        background: #431406;
        color: #fff7e8;
        border-radius: 16px;
        padding: 14px 18px;
        display: flex;
        align-items: center;
        gap: 12px;
        box-shadow: 0 18px 40px rgba(67, 20, 6, .24);
        z-index: 9999;
        opacity: 0;
        transform: translateY(-12px);
        transition: all .25s ease;
    }

        .fav-toast.show {
            opacity: 1;
            transform: translateY(0);
        }

    .fav-toast-icon {
        width: 34px;
        height: 34px;
        border-radius: 12px;
        background: #deb04f;
        color: #431406;
        display: grid;
        place-items: center;
        font-weight: 900;
    }

    .fav-toast strong {
        display: block;
        font-size: 13px;
        font-weight: 900;
    }

    .fav-toast span {
        display: block;
        color: #ffe4a8;
        font-size: 11px;
        margin-top: 2px;
    }
    .fav-hero-btn {
        background: rgba(255, 255, 255, 0.12) !important;
        border: 1px solid rgba(255, 255, 255, 0.22) !important;
        color: #ffffff !important;
        text-decoration: none !important;
        font-weight: 800 !important;
        padding: 14px 20px !important;
        border-radius: 14px !important;
        display: inline-flex !important;
        align-items: center !important;
        justify-content: center !important;
        gap: 8px !important;
        min-width: 150px !important;
    }

        .fav-hero-btn:hover {
            background: rgba(255, 255, 255, 0.18) !important;
            color: #ffffff !important;
        }

        .fav-hero-btn i {
            color: #ffffff !important;
        }
</style>

<div class="fav-page" data-cli-id="@cliIdTexto">

    <section class="fav-hero">
        <div>
            <span class="fav-pill">Colección personal</span>
            <h1>Mis favoritos</h1>
            <p>Guarda los muebles que más te gustan y vuelve a ellos cuando quieras comprar.</p>
        </div>

        <a href="@Url.Action("Busqueda", "PortalCliente")" class="fav-hero-btn">
            <i class="bi bi-grid"></i>
            Ver catálogo
        </a>
    </section>

    <section class="fav-panel">
        <div class="fav-section-head">
            <div>
                <span>Favoritos guardados</span>
                <h2>Productos guardados</h2>
                <p>Información consultada desde la base de datos.</p>
            </div>
        </div>

        <div class="fav-grid" id="pfFavoritosContainer">
            <div class="fav-empty-card">
                <div class="fav-empty-icon"><i class="bi bi-heart"></i></div>
                <strong>Cargando favoritos...</strong>
                <span>Espera un momento mientras se consulta la base de datos.</span>
            </div>
        </div>
    </section>

    <nav class="fav-bottom-nav">
        <a href="@Url.Action("Index", "PortalCliente")">
            <i class="bi bi-house"></i>
            <span>Inicio</span>
        </a>

        <a href="@Url.Action("Busqueda", "PortalCliente")">
            <i class="bi bi-grid"></i>
            <span>Catálogo</span>
        </a>

        <a class="active" href="@Url.Action("MisFavoritos", "PortalCliente")">
            <i class="bi bi-heart"></i>
            <span>Favoritos</span>
        </a>

        <a href="@Url.Action("MisOrdenes", "PortalCliente")">
            <i class="bi bi-receipt"></i>
            <span>Órdenes</span>
        </a>

        <a href="@Url.Action("MiPerfil", "PortalCliente")">
            <i class="bi bi-person"></i>
            <span>Perfil</span>
        </a>
    </nav>

</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-favoritos.js?v=32")"></script>
End Section