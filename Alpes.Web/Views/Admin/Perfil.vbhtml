@Code
    ViewData("Title") = "Mi perfil"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"

    Dim username As String = If(Session("Username") IsNot Nothing, Session("Username").ToString(), "admin")
    Dim email As String = If(Session("Email") IsNot Nothing, Session("Email").ToString(), "roberto.lopez@alpes.com.gt")
    Dim avatar As String = username.Substring(0, 1).ToUpper()
End Code

<div class="perfil-page">

    <div class="perfil-card">
        <div class="perfil-avatar">@avatar</div>

        <div>
            <h2>Mi perfil</h2>
            <div class="perfil-user-badge">
                <i class="bi bi-lock"></i>
                @username
            </div>
            <p>El usuario no puede modificarse</p>
        </div>
    </div>

    <div class="perfil-section-title">
        Datos personales
    </div>

    <div class="perfil-form">
        <div class="perfil-row">
            <div class="perfil-input">
                <i class="bi bi-person"></i>
                <input type="text" placeholder="Nombre" />
            </div>

            <div class="perfil-input">
                <i class="bi bi-person"></i>
                <input type="text" placeholder="Apellido" />
            </div>
        </div>

        <div class="perfil-input">
            <i class="bi bi-envelope"></i>
            <input type="email" value="@email" />
        </div>

        <div class="perfil-password-title" id="btnTogglePassword">
            <i class="bi bi-arrow-clockwise"></i>
            Cambiar contraseña
            <i class="bi bi-chevron-down" id="iconPasswordToggle"></i>
        </div>

        <div id="passwordFields" style="display:none;">
            <div class="perfil-input">
                <i class="bi bi-lock"></i>
                <input type="password" placeholder="Contraseña actual" />
                <i class="bi bi-eye"></i>
            </div>

            <div class="perfil-input">
                <i class="bi bi-lock"></i>
                <input type="password" placeholder="Nueva contraseña" />
                <i class="bi bi-eye"></i>
            </div>

            <div class="perfil-input">
                <i class="bi bi-lock"></i>
                <input type="password" placeholder="Confirmar nueva contraseña" />
                <i class="bi bi-eye"></i>
            </div>
        </div>


        <button type="button" class="perfil-btn">
            GUARDAR CAMBIOS
        </button>
    </div>

</div>

@section scripts
    <script>
        $(function () {
            $('#btnTogglePassword').on('click', function () {
                $('#passwordFields').slideToggle(180);
                $('#iconPasswordToggle').toggleClass('bi-chevron-down bi-chevron-up');
            });
        });
    </script>
End Section