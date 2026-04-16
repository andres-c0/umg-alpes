@Code
    Layout = Nothing
    ViewBag.Title = "Registrarse"
End Code
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Crear cuenta — Muebles de los Alpes</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
</head>
<body>
<div class="auth-bg">
    <div class="auth-box">
        <div class="auth-logo">
            <div class="auth-logo__icon"><i class="bi bi-house-heart-fill"></i></div>
            <span class="auth-logo__brand">MUEBLES DE LOS ALPES</span>
            <span class="auth-logo__tag">Artesanía · Calidad · Elegancia</span>
        </div>
        <div class="auth-card">
            <div class="auth-card__hdr">
                <div class="auth-card__accent"></div>
                <div>
                    <div class="auth-card__title">Crear cuenta</div>
                    <div class="auth-card__sub">Únete a Muebles de los Alpes</div>
                </div>
            </div>
            <div id="reg-error" class="a-alert error" style="display:none"></div>
            <div id="reg-ok" class="a-alert success" style="display:none"></div>

            <div class="a-form-group">
                <label>Nombre de usuario</label>
                <div class="a-input-wrap">
                    <i class="bi bi-person a-input-icon"></i>
                    <input type="text" id="r-username" class="a-input" placeholder="Mínimo 4 caracteres" />
                </div>
                <div class="a-input-err" id="e-username">Mínimo 4 caracteres</div>
            </div>
            <div class="a-form-group">
                <label>Correo electrónico</label>
                <div class="a-input-wrap">
                    <i class="bi bi-envelope a-input-icon"></i>
                    <input type="email" id="r-email" class="a-input" placeholder="correo@ejemplo.com" />
                </div>
                <div class="a-input-err" id="e-email">Correo inválido</div>
            </div>
            <div class="a-form-group">
                <label>Contraseña</label>
                <div class="a-input-wrap">
                    <i class="bi bi-lock a-input-icon"></i>
                    <input type="password" id="r-password" class="a-input" placeholder="Mínimo 6 caracteres" />
                    <button type="button" class="a-input-icon-r" id="r-toggle">
                        <i class="bi bi-eye" id="r-eye"></i>
                    </button>
                </div>
                <div class="a-input-err" id="e-password">Mínimo 6 caracteres</div>
            </div>
            <div class="a-form-group">
                <label>Teléfono (opcional)</label>
                <div class="a-input-wrap">
                    <i class="bi bi-phone a-input-icon"></i>
                    <input type="tel" id="r-telefono" class="a-input" placeholder="12345678" />
                </div>
            </div>
            <div style="height:8px"></div>
            <button class="btn-a btn-a-primary btn-a-full" id="btn-registro" style="height:50px;border-radius:12px">
                <span id="reg-txt">CREAR CUENTA</span>
                <span class="a-spin" id="reg-spin" style="display:none"></span>
            </button>
            <div class="auth-divider">o</div>
            <div style="text-align:center;font-size:13px;color:var(--nogal-medio)">
                ¿Ya tienes cuenta?
                <a href="/Home/Login" style="color:var(--cafe-oscuro);font-weight:700;margin-left:4px">Inicia sesión</a>
            </div>
        </div>
        <div class="auth-footer">&copy; 2025 Muebles de los Alpes</div>
    </div>
</div>
@Scripts.Render("~/bundles/jquery")
@Scripts.Render("~/Scripts/alpes.js")
<script>
$(function () {
    $('#r-toggle').on('click', function () {
        var inp = $('#r-password');
        inp.attr('type', inp.attr('type') === 'password' ? 'text' : 'password');
        $('#r-eye').toggleClass('bi-eye bi-eye-slash');
    });

    $('#btn-registro').on('click', function () {
        var user = $('#r-username').val().trim();
        var email = $('#r-email').val().trim();
        var pass = $('#r-password').val();
        var tel = $('#r-telefono').val().trim();
        var ok = true;

        $('.a-input-err').removeClass('show'); $('.a-input').removeClass('is-invalid');
        $('#reg-error').hide();

        if (user.length < 4) { $('#e-username').addClass('show'); $('#r-username').addClass('is-invalid'); ok = false; }
        if (!email || !email.includes('@')) { $('#e-email').addClass('show'); $('#r-email').addClass('is-invalid'); ok = false; }
        if (pass.length < 6) { $('#e-password').addClass('show'); $('#r-password').addClass('is-invalid'); ok = false; }
        if (!ok) return;

        $('#btn-registro').prop('disabled', true);
        $('#reg-txt').text('Creando cuenta...');
        $('#reg-spin').show();

        AlpesAPI.post('/usuarios', {
            username: user, email: email,
            password_hash: pass, telefono: tel,
            rol_id: 3, estado: 'ACTIVO'
        })
        .done(function (res) {
            if (res.ok) {
                $('#reg-ok').text('Cuenta creada. Redirigiendo...').show();
                setTimeout(function () { window.location.href = '/Home/Login'; }, 1500);
            } else {
                $('#reg-error').text(res.mensaje || 'Error al registrar').show();
            }
        })
        .fail(function () { $('#reg-error').text('Error de conexión').show(); })
        .always(function () {
            $('#btn-registro').prop('disabled', false);
            $('#reg-txt').text('CREAR CUENTA');
            $('#reg-spin').hide();
        });
    });
});
</script>
</body>
</html>
