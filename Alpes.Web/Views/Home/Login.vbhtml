@ModelType Alpes.Web.Models.LoginViewModel

@Code
    Layout = Nothing
    ViewData("Title") = "Iniciar sesión"
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar sesión - Muebles de los Alpes</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
</head>
<body>

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
                        <div class="auth-card__title">Iniciar sesión</div>
                        <div class="auth-card__sub">Ingresa tus credenciales</div>
                    </div>
                </div>

                @If ViewData("Error") IsNot Nothing Then
                    @<div class="a-alert error" style="display:block">
                        <i class="bi bi-exclamation-circle"></i>
                        <span>@ViewData("Error")</span>
                    </div>
                End If

                @Using Html.BeginForm("Login", "Home", FormMethod.Post)
                    @Html.AntiForgeryToken()

                    @<div class="a-form-group">
                        <label for="Username">Usuario</label>
                        <div class="a-input-wrap">
                            <i class="bi bi-person a-input-icon"></i>
                            @Html.TextBoxFor(Function(m) m.Username, New With {
                                .class = "a-input",
                                .placeholder = "Tu nombre de usuario",
                                .autocomplete = "username"
                            })
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.Username, "", New With {.style = "color:#b3261e;font-size:12px;"})
                    </div>

                    @<div class="a-form-group">
                        <label for="Password">Contraseña</label>
                        <div class="a-input-wrap">
                            <i class="bi bi-lock a-input-icon"></i>
                            @Html.PasswordFor(Function(m) m.Password, New With {
                                .class = "a-input",
                                .placeholder = "Tu contraseña",
                                .autocomplete = "current-password",
                                .id = "password"
                            })
                            <button type="button" class="a-input-icon-r" id="toggle-pass">
                                <i class="bi bi-eye" id="eye-icon"></i>
                            </button>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.Password, "", New With {.style = "color:#b3261e;font-size:12px;"})
                    </div>

                    @<div style="height:14px"></div>

                    @<button type="submit" class="btn-a btn-a-primary btn-a-full"
                             style="height:50px;border-radius:12px;font-size:14px">
                        INGRESAR
                    </button>
                End Using

                <div class="auth-divider">o</div>

                <div style="text-align:center;font-size:13px;color:var(--nogal-medio)">
                    ¿No tienes cuenta?
                    <a href="/Home/Registro"
                       style="color:var(--cafe-oscuro);font-weight:700;margin-left:4px">
                        Regístrate
                    </a>
                </div>
            </div>

            <div class="auth-footer">
                &copy; 2025 Muebles de los Alpes &mdash; Todos los derechos reservados
            </div>

        </div>
    </div>

    @Scripts.Render("~/bundles/jquery")

    <script>
        $(function () {
            $('#toggle-pass').on('click', function () {
                var inp = $('#password');
                var icon = $('#eye-icon');

                if (inp.attr('type') === 'password') {
                    inp.attr('type', 'text');
                    icon.removeClass('bi-eye').addClass('bi-eye-slash');
                } else {
                    inp.attr('type', 'password');
                    icon.removeClass('bi-eye-slash').addClass('bi-eye');
                }
            });
        });
    </script>

</body>
</html>