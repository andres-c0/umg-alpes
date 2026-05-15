Option Strict On
Option Explicit On

Imports System
Imports System.Configuration
Imports System.Net
Imports System.Net.Mail
Imports System.Text

Namespace Servicios

    Public Class CorreoServicio

        Public Sub EnviarPasswordTemporal(ByVal destinatario As String,
                                           ByVal nombreUsuario As String,
                                           ByVal passwordTemporal As String)
            If String.IsNullOrWhiteSpace(destinatario) Then
                Throw New ArgumentException("Debe indicar el correo destino.", NameOf(destinatario))
            End If

            Dim subject As String = "Recuperación de contraseña - Muebles de los Alpes"
            Dim body As String = ConstruirBodyPasswordTemporal(nombreUsuario, passwordTemporal)

            EnviarCorreo(destinatario.Trim(), subject, body)
        End Sub

        Private Sub EnviarCorreo(ByVal destinatario As String, ByVal asunto As String, ByVal cuerpoHtml As String)
            Dim host As String = ObtenerConfig("SMTP_HOST", "Smtp:Host")
            Dim portTexto As String = ObtenerConfig("SMTP_PORT", "Smtp:Port")
            Dim enableSslTexto As String = ObtenerConfig("SMTP_ENABLE_SSL", "Smtp:EnableSsl")
            Dim user As String = ObtenerConfig("SMTP_USER", "Smtp:User")
            Dim password As String = ObtenerConfig("SMTP_PASSWORD", "Smtp:Password")
            Dim fromEmail As String = ObtenerConfig("SMTP_FROM_EMAIL", "Smtp:FromEmail")
            Dim fromName As String = ObtenerConfig("SMTP_FROM_NAME", "Smtp:FromName")

            If String.IsNullOrWhiteSpace(host) Then
                Throw New InvalidOperationException("No se ha configurado SMTP_HOST o Smtp:Host.")
            End If

            If String.IsNullOrWhiteSpace(fromEmail) Then
                fromEmail = user
            End If

            If String.IsNullOrWhiteSpace(fromEmail) Then
                Throw New InvalidOperationException("No se ha configurado SMTP_FROM_EMAIL o Smtp:FromEmail.")
            End If

            Dim port As Integer = 587
            Integer.TryParse(portTexto, port)

            Dim enableSsl As Boolean = True
            If Not String.IsNullOrWhiteSpace(enableSslTexto) Then
                Boolean.TryParse(enableSslTexto, enableSsl)
            End If

            Using message As New MailMessage()
                message.From = New MailAddress(fromEmail, If(String.IsNullOrWhiteSpace(fromName), "Muebles de los Alpes", fromName))
                message.To.Add(New MailAddress(destinatario))
                message.Subject = asunto
                message.Body = cuerpoHtml
                message.IsBodyHtml = True
                message.BodyEncoding = Encoding.UTF8
                message.SubjectEncoding = Encoding.UTF8

                Using client As New SmtpClient(host, port)
                    client.EnableSsl = enableSsl

                    If Not String.IsNullOrWhiteSpace(user) Then
                        client.Credentials = New NetworkCredential(user, password)
                    End If

                    client.Send(message)
                End Using
            End Using
        End Sub

        Private Shared Function ConstruirBodyPasswordTemporal(ByVal nombreUsuario As String, ByVal passwordTemporal As String) As String
            Dim usuario As String = If(String.IsNullOrWhiteSpace(nombreUsuario), "tu usuario", nombreUsuario.Trim())

            Dim passwordSeguro As String = WebUtility.HtmlEncode(passwordTemporal)
            Dim usuarioSeguro As String = WebUtility.HtmlEncode(usuario)

            Return "<!DOCTYPE html>" &
                   "<html><body style='font-family:Arial,sans-serif;background:#f6f2ea;padding:24px;color:#2f2418;'>" &
                   "<div style='max-width:560px;margin:auto;background:#ffffff;border-radius:18px;padding:26px;border:1px solid #eadcc5;'>" &
                   "<h2 style='margin-top:0;color:#5a3518;'>Recuperación de contraseña</h2>" &
                   "<p>Hola <strong>" & usuarioSeguro & "</strong>, recibimos una solicitud para recuperar tu acceso.</p>" &
                   "<p>Tu nueva contraseña temporal es:</p>" &
                   "<div style='font-size:20px;font-weight:700;letter-spacing:1px;background:#f2eadb;border-radius:12px;padding:14px;text-align:center;color:#3b260f;'>" & passwordSeguro & "</div>" &
                   "<p style='margin-top:18px;'>Ingresa con esta contraseña y luego cámbiala desde <strong>Mi perfil</strong>.</p>" &
                   "<p style='font-size:13px;color:#6f6252;'>Si no solicitaste este cambio, ingresa al sistema y cambia tu contraseña de inmediato.</p>" &
                   "</div></body></html>"
        End Function

        Private Shared Function ObtenerConfig(ByVal envKey As String, ByVal appKey As String) As String
            Dim value As String = Environment.GetEnvironmentVariable(envKey)

            If Not String.IsNullOrWhiteSpace(value) Then
                Return value.Trim()
            End If

            value = System.Configuration.ConfigurationManager.AppSettings(appKey)

            If value Is Nothing Then
                Return String.Empty
            End If

            Return value.Trim()
        End Function

    End Class

End Namespace