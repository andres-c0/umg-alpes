@ModelType Alpes.Web.Models.RegistroViewModel

@Code
    Layout = Nothing
    ViewData("Title") = "Crear cuenta"
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Crear cuenta - Muebles de los Alpes</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
</head>
<body>

    <div class="auth-bg">
        <div class="auth-box auth-box--wide">

            <div class="auth-logo">
                <div class="auth-logo__icon">
                    <i class="bi bi-house-heart-fill"></i>
                </div>
                <span class="auth-logo__brand">MUEBLES DE LOS ALPES</span>
                <span class="auth-logo__tag">Crea tu cuenta para comenzar a comprar</span>
            </div>

            <div class="auth-card auth-card--register">
                <div class="auth-card__hdr">
                    <div class="auth-card__accent"></div>
                    <div>
                        <div class="auth-card__title">Crear cuenta</div>
                        <div class="auth-card__sub">Únete a Muebles de los Alpes</div>
                    </div>
                </div>

                @If ViewData("Error") IsNot Nothing Then
                    @<div class="a-alert error" style="display:block">
                        <i class="bi bi-exclamation-circle"></i>
                        <span>@ViewData("Error")</span>
                    </div>
                End If

                @Using Html.BeginForm("Registro", "Home", FormMethod.Post)
                    @Html.AntiForgeryToken()

                    @<div class="form-grid auth-register-grid">
                        <div class="a-form-group">
                            <label for="Username">Nombre de usuario</label>
                            <div class="a-input-wrap">
                                <i class="bi bi-person a-input-icon"></i>
                                @Html.TextBoxFor(Function(m) m.Username, New With {
                         .class = "a-input",
                         .placeholder = "Mínimo 4 caracteres",
                         .autocomplete = "username"
                     })
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.Username, "", New With {.style = "color:#b3261e;font-size:12px;"})
                        </div>

                        <div class="a-form-group">
                            <label for="Email">Correo electrónico</label>
                            <div class="a-input-wrap">
                                <i class="bi bi-envelope a-input-icon"></i>
                                @Html.TextBoxFor(Function(m) m.Email, New With {
                         .class = "a-input",
                         .placeholder = "correo@ejemplo.com",
                         .autocomplete = "email",
                         .type = "email"
                     })
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.style = "color:#b3261e;font-size:12px;"})
                        </div>

                        <div class="a-form-group">
                            <label for="Password">Contraseña</label>
                            <div class="a-input-wrap">
                                <i class="bi bi-lock a-input-icon"></i>
                                @Html.PasswordFor(Function(m) m.Password, New With {
                         .class = "a-input",
                         .placeholder = "Mínimo 6 caracteres",
                         .autocomplete = "new-password",
                         .id = "reg-password"
                     })
                                <button type="button" class="a-input-icon-r" id="toggle-reg-pass">
                                    <i class="bi bi-eye" id="reg-eye-icon"></i>
                                </button>
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.Password, "", New With {.style = "color:#b3261e;font-size:12px;"})
                        </div>

                        <div class="a-form-group">
                            <label for="ConfirmPassword">Confirmar contraseña</label>
                            <div class="a-input-wrap">
                                <i class="bi bi-shield-lock a-input-icon"></i>
                                @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {
                         .class = "a-input",
                         .placeholder = "Repite tu contraseña",
                         .autocomplete = "new-password",
                         .id = "reg-confirm-password"
                     })
                                <button type="button" class="a-input-icon-r" id="toggle-reg-pass-confirm">
                                    <i class="bi bi-eye" id="reg-eye-icon-confirm"></i>
                                </button>
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.ConfirmPassword, "", New With {.style = "color:#b3261e;font-size:12px;"})
                        </div>

                        <div class="a-form-group form-grid__full">
                            <label for="Telefono">Teléfono (opcional)</label>
                            <div class="a-input-wrap">
                                <i class="bi bi-telephone a-input-icon"></i>
                                @Html.TextBoxFor(Function(m) m.Telefono, New With {
                         .class = "a-input",
                         .placeholder = "Ej. 12345678",
                         .autocomplete = "tel"
                     })
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.Telefono, "", New With {.style = "color:#b3261e;font-size:12px;"})
                        </div>
                    </div>

                    @<div style="height:14px"></div>

                    @<button type="submit" class="btn-a btn-a-primary btn-a-full"
                             style="height:52px;border-radius:14px;font-size:15px">
                        CREAR CUENTA
                    </button>
                End Using

                <div class="auth-divider">o</div>

                <div style="text-align:center;font-size:14px;color:var(--nogal-medio)">
                    ¿Ya tienes cuenta?
                    <a href="@Url.Action("Login", "Home")"
                       style="color:var(--cafe-oscuro);font-weight:700;margin-left:4px">
                        Inicia sesión
                    </a>
                </div>
            </div>

            <div class="auth-footer">
                &copy; 2026 Muebles de los Alpes &mdash; Todos los derechos reservados
            </div>

        </div>
    </div>

    @Scripts.Render("~/bundles/jquery")

    <script>
        $(function () {
            $('#toggle-reg-pass').on('click', function () {
                var inp = $('#reg-password');
                var icon = $('#reg-eye-icon');

                if (inp.attr('type') === 'password') {
                    inp.attr('type', 'text');
                    icon.removeClass('bi-eye').addClass('bi-eye-slash');
                } else {
                    inp.attr('type', 'password');
                    icon.removeClass('bi-eye-slash').addClass('bi-eye');
                }
            });

            $('#toggle-reg-pass-confirm').on('click', function () {
                var inp = $('#reg-confirm-password');
                var icon = $('#reg-eye-icon-confirm');

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