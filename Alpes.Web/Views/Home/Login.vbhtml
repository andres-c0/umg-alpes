@Code
    ViewBag.Title = "Login"
End Code

<section class="login-page">
    <div class="login-decoration login-decoration-top"></div>
    <div class="login-decoration login-decoration-bottom"></div>

    <div class="login-wrapper">
        <div class="login-brand-panel">
            <div class="login-brand-tag">Portal oficial</div>
            <h1 class="login-brand-title">MUEBLES DE LOS ALPES</h1>
            <p class="login-brand-subtitle">ARTESANÍA · CALIDAD · ELEGANCIA</p>

            <div class="login-brand-copy">
                Accede al portal para consultar productos, gestionar compras y administrar la operación
                del sistema con una experiencia visual elegante y profesional.
            </div>
        </div>

        <div class="login-card">
            <div class="login-card-logo">A</div>

            <div class="login-card-header">
                <div class="login-accent-bar"></div>
                <div>
                    <h2 class="login-card-title">Iniciar sesión</h2>
                    <p class="login-card-text">Ingresa tus credenciales para continuar.</p>
                </div>
            </div>

            <form class="login-form" method="post" action="#">
                <div class="form-group">
                    <label for="username">Usuario o correo</label>
                    <div class="input-icon-wrapper">
                        <span class="input-icon">@@</span>
                        <input type="text" id="username" name="username" class="login-input" placeholder="Ingresa tu usuario" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="password">Contraseña</label>
                    <div class="input-icon-wrapper">
                        <span class="input-icon">••</span>
                        <input type="password" id="password" name="password" class="login-input" placeholder="Ingresa tu contraseña" />
                    </div>
                </div>

                <div class="login-actions">
                    <button type="submit" class="btn btn-primary login-btn">INGRESAR</button>
                </div>
            </form>

            <div class="login-footer-note">
                Acceso para clientes y administradores.
            </div>
        </div>
    </div>

    <div class="login-page-footer">
        © 2026 Muebles de los Alpes
    </div>
</section>

<style>
    .login-page {
        min-height: calc(100vh - 72px - 92px);
        position: relative;
        overflow: hidden;
        background: linear-gradient(180deg, #2C1810 0%, #3D2416 55%, #1E0E08 100%);
        display: flex;
        flex-direction: column;
        justify-content: center;
        padding: 48px 16px;
    }

    .login-decoration {
        position: absolute;
        border-radius: 50%;
        background: radial-gradient(circle, rgba(212,168,83,0.18) 0%, rgba(212,168,83,0.05) 45%, transparent 70%);
        pointer-events: none;
    }

    .login-decoration-top {
        width: 360px;
        height: 360px;
        top: -120px;
        right: -80px;
    }

    .login-decoration-bottom {
        width: 420px;
        height: 420px;
        left: -120px;
        bottom: -180px;
    }

    .login-wrapper {
        width: 100%;
        max-width: 1180px;
        margin: 0 auto;
        display: grid;
        grid-template-columns: 1fr 420px;
        gap: 48px;
        align-items: center;
        position: relative;
        z-index: 2;
        animation: loginFadeUp 0.6s ease-out;
    }

    .login-brand-panel {
        color: var(--blanco);
        padding-right: 24px;
    }

    .login-brand-tag {
        display: inline-flex;
        padding: 6px 12px;
        border-radius: 999px;
        background: rgba(212,168,83,0.16);
        color: #F1D8A1;
        font-size: 12px;
        font-weight: 700;
        margin-bottom: 18px;
    }

    .login-brand-title {
        color: var(--blanco);
        font-size: 52px;
        line-height: 1.05;
        letter-spacing: 2.5px;
        margin-bottom: 12px;
    }

    .login-brand-subtitle {
        color: var(--arena-calida);
        font-size: 14px;
        letter-spacing: 2px;
        margin-bottom: 22px;
    }

    .login-brand-copy {
        max-width: 560px;
        color: rgba(255,255,255,0.80);
        font-size: 16px;
        line-height: 1.7;
    }

    .login-card {
        background: var(--blanco);
        border-radius: 20px;
        box-shadow: 0 16px 40px rgba(0,0,0,0.30);
        padding: 30px;
        border: 1px solid rgba(232,224,213,0.65);
    }

    .login-card-logo {
        width: 72px;
        height: 72px;
        border-radius: 18px;
        background: var(--oro-guatemalteco);
        color: var(--cafe-oscuro);
        font-family: var(--font-display);
        font-size: 34px;
        font-weight: 700;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: 24px;
        box-shadow: var(--shadow-gold);
    }

    .login-card-header {
        display: flex;
        align-items: flex-start;
        gap: 14px;
        margin-bottom: 22px;
    }

    .login-accent-bar {
        width: 5px;
        min-width: 5px;
        height: 52px;
        border-radius: 999px;
        background: var(--oro-guatemalteco);
        margin-top: 2px;
    }

    .login-card-title {
        font-size: 30px;
        margin-bottom: 6px;
    }

    .login-card-text {
        margin: 0;
        color: var(--nogal-medio);
        font-size: 14px;
    }

    .login-form .form-group {
        margin-bottom: 18px;
    }

    .login-form label {
        color: var(--arena-calida);
        font-size: 13px;
        margin-bottom: 8px;
    }

    .input-icon-wrapper {
        position: relative;
    }

    .input-icon {
        position: absolute;
        left: 16px;
        top: 50%;
        transform: translateY(-50%);
        color: var(--nogal-medio);
        font-size: 18px;
        font-weight: 700;
        pointer-events: none;
    }

    .login-input {
        background: var(--crema-fondo) !important;
        border: 1px solid var(--pergamino) !important;
        border-radius: 12px !important;
        padding: 14px 16px 14px 44px !important;
        font-size: 14px !important;
        color: var(--cafe-oscuro) !important;
        font-weight: 500;
    }

        .login-input:focus {
            border: 1.5px solid var(--cafe-oscuro) !important;
            outline: none;
            box-shadow: 0 0 0 3px rgba(44,24,16,0.08);
        }

        .login-input::placeholder {
            color: var(--arena-calida);
        }

    .login-actions {
        margin-top: 8px;
    }

    .login-btn {
        width: 100%;
        min-height: 50px;
        border-radius: 12px;
    }

    .login-footer-note {
        margin-top: 18px;
        text-align: center;
        color: var(--nogal-medio);
        font-size: 13px;
    }

    .login-page-footer {
        position: relative;
        z-index: 2;
        margin-top: 26px;
        text-align: center;
        color: var(--arena-calida);
        font-size: 13px;
    }

    @@keyframes loginFadeUp {
        from {
            opacity: 0;
            transform: translateY(24px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    @@media (max-width: 992px) {
        .login-wrapper {
            grid-template-columns: 1fr;
            gap: 28px;
            max-width: 720px;
        }

        .login-brand-panel {
            padding-right: 0;
            text-align: center;
        }

        .login-brand-copy {
            margin: 0 auto;
        }

        .login-brand-title {
            font-size: 40px;
        }
    }

    @@media (max-width: 576px) {
        .login-page {
            padding: 28px 12px;
        }

        .login-card {
            padding: 22px;
        }

        .login-brand-title {
            font-size: 30px;
        }

        .login-card-title {
            font-size: 24px;
        }

        .login-accent-bar {
            height: 42px;
        }
    }
</style>