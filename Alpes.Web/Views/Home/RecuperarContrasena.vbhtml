@ModelType Alpes.Web.Models.RecuperarContrasenaViewModel

@Code
    Layout = Nothing
    ViewData("Title") = "Recuperar contraseña"
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Recuperar contraseña - Muebles de los Alpes</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
</head>
<body>

    <a href="@Url.Action("Login", "Home")"
       class="login-back-btn"
       style="position:fixed;top:24px;left:24px;width:58px;height:58px;border-radius:18px;background:rgba(255,255,255,.10);border:1px solid rgba(255,255,255,.18);display:flex;align-items:center;justify-content:center;color:#d8ab49;font-size:28px;text-decoration:none;z-index:9999;box-shadow:0 8px 24px rgba(0,0,0,.25);">
        <i class="bi bi-chevron-left"></i>
    </a>

    <div class="auth-bg">
        <div class="auth-box">

            <div class="auth-logo">
                <div class="auth-logo__icon">
                    <i class="bi bi-house-heart-fill"></i>
                </div>
                <span class="auth-logo__brand">MUEBLES DE LOS ALPES</span>
                <span class="auth-logo__tag">Artesanía · Calidad · Elegancia</span>
            </div>

            <div class="auth-card">
                <div class="auth-card__hdr">
                    <div class="auth-card__accent"></div>
                    <div>
                        <div class="auth-card__title">Recuperar contraseña</div>
                        <div class="auth-card__sub">Te enviaremos una contraseña temporal a tu correo registrado</div>
                    </div>
                </div>

                @If ViewData("Error") IsNot Nothing Then
                    @<div class="a-alert error" style="display:block">
                        <i class="bi bi-exclamation-circle"></i>
                        <span>@ViewData("Error")</span>
                    </div>
                End If

                @If TempData("Success") IsNot Nothing Then
                    @<div class="a-alert success" style="display:block">
                        <i class="bi bi-check-circle"></i>
                        <span>@TempData("Success")</span>
                    </div>
                End If

                @Using Html.BeginForm("RecuperarContrasena", "Home", FormMethod.Post)
                    @Html.AntiForgeryToken()

                    @<div class="a-form-group">
                        <label for="Email">Correo electrónico</label>
                        <div class="a-input-wrap">
                            <i class="bi bi-envelope a-input-icon"></i>
                            <input type="email"
                                   id="Email"
                                   name="Email"
                                   class="a-input"
                                   placeholder="correo@ejemplo.com"
                                   autocomplete="email"
                                   value="@(If(Model IsNot Nothing, Model.Email, String.Empty))" />
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.style = "color:#b3261e;font-size:12px;"})
                    </div>

                    @<div style="height:14px"></div>

                    @<button type="submit" class="btn-a btn-a-primary btn-a-full" style="height:50px;border-radius:12px;font-size:14px">
                        ENVIAR CONTRASEÑA TEMPORAL
                    </button>
                End Using

                <div class="auth-divider">o</div>

                <div style="text-align:center;font-size:13px;color:var(--nogal-medio)">
                    ¿Ya recordaste tu contraseña?
                    <a href="@Url.Action("Login", "Home")" style="color:var(--cafe-oscuro);font-weight:700;margin-left:4px">
                        Iniciar sesión
                    </a>
                </div>
            </div>

            <div class="auth-footer">
                &copy; 2026 Muebles de los Alpes &mdash; Todos los derechos reservados
            </div>

        </div>
    </div>

    @Scripts.Render("~/bundles/jquery")
</body>
</html>
