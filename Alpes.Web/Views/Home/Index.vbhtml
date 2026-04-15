@Code
    ViewBag.Title = "Inicio"
End Code

<section class="home-hero">
    <div class="container">
        <div class="hero-grid">
            <div class="hero-copy">
                <div class="hero-badge">Colección artesanal</div>
                <h1 class="hero-title">Muebles que convierten espacios en experiencias</h1>
                <p class="hero-text">
                    En Muebles de los Alpes diseñamos piezas para interior y exterior con acabados elegantes,
                    materiales de calidad y una presencia cálida que transforma cualquier ambiente.
                </p>

                <div class="hero-actions">
                    <a href="#" class="btn btn-gold">Ver catálogo</a>
                    <a href="#" class="btn btn-outlined">Explorar colecciones</a>
                </div>

                <div class="hero-highlights">
                    <div class="highlight-item">
                        <span class="highlight-number">+120</span>
                        <span class="highlight-label">Modelos disponibles</span>
                    </div>
                    <div class="highlight-item">
                        <span class="highlight-number">Interior</span>
                        <span class="highlight-label">Líneas elegantes</span>
                    </div>
                    <div class="highlight-item">
                        <span class="highlight-number">Exterior</span>
                        <span class="highlight-label">Resistencia y estilo</span>
                    </div>
                </div>
            </div>

            <div class="hero-visual">
                <div class="hero-card hero-card-main">
                    <div class="hero-card-label">Diseño destacado</div>
                    <div class="hero-card-title">Sala Colonial Alpes</div>
                    <div class="hero-card-text">
                        Madera fina, textura cálida y una composición pensada para espacios sofisticados.
                    </div>
                </div>

                <div class="hero-floating-card">
                    <div class="floating-tag">Nuevo</div>
                    <div class="floating-title">Colección Terraza</div>
                    <div class="floating-text">Ideal para espacios exteriores elegantes.</div>
                </div>
            </div>
        </div>
    </div>
</section>

<section class="home-section">
    <div class="container">
        <div class="section-heading">
            <h2 class="section-title">Nuestra propuesta</h2>
            <p class="section-subtitle">
                El portal está pensado para que el cliente pueda consultar muebles, agregarlos a su carrito
                y realizar compras de forma cómoda y segura, mientras la administración controla productos,
                clientes, precios y reportes. :contentReference[oaicite:2]{index=2}
            </p>
        </div>

        <div class="feature-grid">
            <div class="feature-card card-base">
                <div class="feature-icon">01</div>
                <h3 class="feature-title">Catálogo elegante</h3>
                <p class="feature-text">
                    Una experiencia visual refinada para descubrir muebles por estilo, material y tipo.
                </p>
            </div>

            <div class="feature-card card-base">
                <div class="feature-icon">02</div>
                <h3 class="feature-title">Compra segura</h3>
                <p class="feature-text">
                    Flujo claro desde la selección del producto hasta el proceso de pago y confirmación.
                </p>
            </div>

            <div class="feature-card card-base">
                <div class="feature-icon">03</div>
                <h3 class="feature-title">Gestión administrativa</h3>
                <p class="feature-text">
                    Soporte para administración de clientes, productos, precios, inventario y reportes.
                </p>
            </div>
        </div>
    </div>
</section>

<section class="home-section alt-section">
    <div class="container">
        <div class="section-heading">
            <h2 class="section-title">Colecciones destacadas</h2>
            <p class="section-subtitle">
                Ejemplos visuales para la página principal. Más adelante los conectaremos a los datos reales.
            </p>
        </div>

        <div class="collection-grid">
            <article class="collection-card card-base">
                <div class="collection-image image-wood"></div>
                <div class="collection-content">
                    <div class="badge-pill badge-warning">Interior</div>
                    <h3 class="collection-title">Línea Nogal Clásico</h3>
                    <p class="collection-text">
                        Piezas sobrias y elegantes para sala, comedor y dormitorio.
                    </p>
                    <a href="#" class="collection-link">Ver detalles</a>
                </div>
            </article>

            <article class="collection-card card-base">
                <div class="collection-image image-outdoor"></div>
                <div class="collection-content">
                    <div class="badge-pill badge-info">Exterior</div>
                    <h3 class="collection-title">Terraza Selva</h3>
                    <p class="collection-text">
                        Diseños resistentes para jardines, terrazas y espacios abiertos.
                    </p>
                    <a href="#" class="collection-link">Ver detalles</a>
                </div>
            </article>

            <article class="collection-card card-base">
                <div class="collection-image image-luxury"></div>
                <div class="collection-content">
                    <div class="badge-pill badge-success">Premium</div>
                    <h3 class="collection-title">Edición Artesanal</h3>
                    <p class="collection-text">
                        Acabados finos, detalles exclusivos y presencia de alto nivel.
                    </p>
                    <a href="#" class="collection-link">Ver detalles</a>
                </div>
            </article>
        </div>
    </div>
</section>

<section class="home-section">
    <div class="container">
        <div class="cta-panel">
            <div class="cta-copy">
                <h2 class="cta-title">Diseño guatemalteco con carácter</h2>
                <p class="cta-text">
                    Explora nuestro catálogo y descubre muebles creados para combinar artesanía,
                    calidad y elegancia en un solo lugar.
                </p>
            </div>

            <div class="cta-actions">
                <a href="#" class="btn btn-primary">Ingresar al portal</a>
                <a href="#" class="btn btn-outlined">Conocer más</a>
            </div>
        </div>
    </div>
</section>

<style>
    .home-hero {
        padding: 56px 0 48px;
        background: radial-gradient(circle at top right, rgba(212,168,83,0.18), transparent 22%), radial-gradient(circle at bottom left, rgba(139,111,71,0.12), transparent 24%), linear-gradient(180deg, #f8f4ef 0%, #f7f3ee 100%);
    }

    .hero-grid {
        display: grid;
        grid-template-columns: 1.1fr 0.9fr;
        gap: 32px;
        align-items: center;
    }

    .hero-copy {
        padding-right: 16px;
    }

    .hero-badge {
        display: inline-flex;
        align-items: center;
        padding: 6px 12px;
        border-radius: 999px;
        background: rgba(212,168,83,0.18);
        color: var(--cafe-oscuro);
        font-size: 12px;
        font-weight: 700;
        letter-spacing: 0.5px;
        margin-bottom: 18px;
    }

    .hero-title {
        font-size: 52px;
        line-height: 1.08;
        margin-bottom: 18px;
        max-width: 700px;
    }

    .hero-text {
        font-size: 16px;
        color: var(--nogal-medio);
        max-width: 620px;
        margin-bottom: 24px;
    }

    .hero-actions {
        display: flex;
        gap: 12px;
        flex-wrap: wrap;
        margin-bottom: 28px;
    }

    .hero-highlights {
        display: flex;
        gap: 14px;
        flex-wrap: wrap;
    }

    .highlight-item {
        min-width: 130px;
        background: var(--blanco);
        border: var(--border-light);
        border-radius: 12px;
        padding: 14px 16px;
        box-shadow: var(--shadow-card);
    }

    .highlight-number {
        display: block;
        font-family: var(--font-display);
        color: var(--cafe-oscuro);
        font-size: 22px;
        font-weight: 700;
        margin-bottom: 4px;
    }

    .highlight-label {
        display: block;
        color: var(--nogal-medio);
        font-size: 12px;
    }

    .hero-visual {
        position: relative;
        min-height: 460px;
        display: flex;
        align-items: stretch;
    }

    .hero-card-main {
        width: 100%;
        min-height: 460px;
        border-radius: 20px;
        padding: 32px;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        background: linear-gradient(180deg, rgba(44,24,16,0.10) 0%, rgba(44,24,16,0.78) 100%), linear-gradient(135deg, #8B6F47 0%, #2C1810 100%);
        color: var(--blanco);
        overflow: hidden;
        position: relative;
    }

        .hero-card-main::before {
            content: "";
            position: absolute;
            inset: 0;
            background: radial-gradient(circle at top left, rgba(255,255,255,0.14), transparent 26%), radial-gradient(circle at center right, rgba(212,168,83,0.18), transparent 24%);
            pointer-events: none;
        }

    .hero-card-label,
    .hero-card-title,
    .hero-card-text {
        position: relative;
        z-index: 2;
    }

    .hero-card-label {
        font-size: 12px;
        letter-spacing: 1px;
        text-transform: uppercase;
        color: #F3D79A;
        margin-bottom: 12px;
    }

    .hero-card-title {
        font-family: var(--font-display);
        font-size: 34px;
        font-weight: 700;
        line-height: 1.1;
        margin-bottom: 10px;
    }

    .hero-card-text {
        font-size: 14px;
        color: rgba(255,255,255,0.85);
        max-width: 320px;
    }

    .hero-floating-card {
        position: absolute;
        right: -18px;
        bottom: 28px;
        width: 220px;
        background: var(--blanco);
        border-radius: 16px;
        padding: 18px;
        box-shadow: var(--shadow-elevated);
        border: var(--border-light);
    }

    .floating-tag {
        display: inline-block;
        background: rgba(212,168,83,0.20);
        color: var(--cafe-oscuro);
        font-size: 11px;
        font-weight: 700;
        border-radius: 999px;
        padding: 4px 10px;
        margin-bottom: 10px;
    }

    .floating-title {
        font-family: var(--font-display);
        font-size: 20px;
        color: var(--cafe-oscuro);
        margin-bottom: 6px;
    }

    .floating-text {
        font-size: 13px;
        color: var(--nogal-medio);
    }

    .home-section {
        padding: 56px 0;
    }

    .alt-section {
        background: rgba(232,224,213,0.32);
        border-top: var(--border-light);
        border-bottom: var(--border-light);
    }

    .section-heading {
        margin-bottom: 26px;
    }

    .feature-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 20px;
    }

    .feature-card {
        padding: 22px;
    }

    .feature-icon {
        width: 42px;
        height: 42px;
        border-radius: 10px;
        background: var(--pergamino);
        color: var(--cafe-oscuro);
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 700;
        margin-bottom: 14px;
    }

    .feature-title {
        font-size: 22px;
        margin-bottom: 10px;
    }

    .feature-text {
        color: var(--nogal-medio);
        margin-bottom: 0;
    }

    .collection-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 22px;
    }

    .collection-card {
        overflow: hidden;
    }

    .collection-image {
        height: 220px;
        background-size: cover;
        background-position: center;
        border-bottom: var(--border-light);
    }

    .image-wood {
        background: linear-gradient(180deg, rgba(44,24,16,0.12), rgba(44,24,16,0.48)), linear-gradient(135deg, #C4A882, #8B6F47);
    }

    .image-outdoor {
        background: linear-gradient(180deg, rgba(26,58,42,0.10), rgba(26,58,42,0.50)), linear-gradient(135deg, #8B6F47, #1A3A2A);
    }

    .image-luxury {
        background: linear-gradient(180deg, rgba(212,168,83,0.10), rgba(44,24,16,0.42)), linear-gradient(135deg, #D4A853, #2C1810);
    }

    .collection-content {
        padding: 20px;
    }

    .collection-title {
        font-size: 24px;
        margin: 12px 0 10px;
    }

    .collection-text {
        color: var(--nogal-medio);
        margin-bottom: 14px;
    }

    .collection-link {
        color: var(--cafe-oscuro);
        font-weight: 700;
    }

        .collection-link:hover {
            color: var(--oro-guatemalteco);
        }

    .cta-panel {
        background: linear-gradient(135deg, #2C1810 0%, #4a2c1f 100%);
        color: var(--blanco);
        border-radius: 20px;
        padding: 32px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 24px;
        box-shadow: var(--shadow-elevated);
    }

    .cta-title {
        color: var(--blanco);
        font-size: 34px;
        margin-bottom: 10px;
    }

    .cta-text {
        color: rgba(255,255,255,0.82);
        margin-bottom: 0;
        max-width: 680px;
    }

    .cta-actions {
        display: flex;
        gap: 12px;
        flex-wrap: wrap;
        justify-content: flex-end;
    }

        .cta-actions .btn-outlined {
            color: var(--blanco);
            border-color: rgba(255,255,255,0.7);
        }

            .cta-actions .btn-outlined:hover {
                background: rgba(255,255,255,0.08);
                color: var(--blanco);
            }

    @@media (max-width: 1100px) {
        .hero-title {
            font-size: 42px;
        }

        .feature-grid,
        .collection-grid {
            grid-template-columns: repeat(2, 1fr);
        }
    }

    @@media (max-width: 900px) {
        .hero-grid {
            grid-template-columns: 1fr;
        }

        .hero-copy {
            padding-right: 0;
        }

        .hero-visual {
            min-height: 360px;
        }

        .hero-card-main {
            min-height: 360px;
        }

        .hero-floating-card {
            right: 16px;
            bottom: 16px;
        }

        .cta-panel {
            flex-direction: column;
            align-items: flex-start;
        }

        .cta-actions {
            justify-content: flex-start;
        }
    }

    @@media (max-width: 640px) {
        .home-hero {
            padding-top: 34px;
        }

        .hero-title {
            font-size: 34px;
        }

        .feature-grid,
        .collection-grid {
            grid-template-columns: 1fr;
        }

        .hero-card-main {
            padding: 24px;
        }

        .hero-card-title {
            font-size: 28px;
        }

        .hero-floating-card {
            position: static;
            width: 100%;
            margin-top: 14px;
        }
    }
</style>