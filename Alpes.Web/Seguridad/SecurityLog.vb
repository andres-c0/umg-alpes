Option Strict On
Option Explicit On

Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Hosting

Namespace Seguridad

    Public NotInheritable Class SecurityLog

        Private Sub New()
        End Sub

        Private Shared ReadOnly Bloqueo As New Object()

        Public Shared Sub Warn(ByVal categoria As String, ByVal mensaje As String)
            Escribir("WARN", categoria, mensaje)
        End Sub

        Public Shared Sub Info(ByVal categoria As String, ByVal mensaje As String)
            Escribir("INFO", categoria, mensaje)
        End Sub

        Private Shared Sub Escribir(ByVal nivel As String, ByVal categoria As String, ByVal mensaje As String)
            Try
                Dim rutaBase As String = HostingEnvironment.MapPath("~/App_Data")

                If String.IsNullOrWhiteSpace(rutaBase) Then
                    Return
                End If

                If Not Directory.Exists(rutaBase) Then
                    Directory.CreateDirectory(rutaBase)
                End If

                Dim rutaArchivo As String = Path.Combine(rutaBase, "security.log")
                Dim linea As String = String.Format("{0:u} [{1}] [{2}] {3}{4}", DateTime.UtcNow, Limpiar(nivel), Limpiar(categoria), Limpiar(mensaje), Environment.NewLine)

                SyncLock Bloqueo
                    File.AppendAllText(rutaArchivo, linea)
                End SyncLock
            Catch
            End Try
        End Sub

        Private Shared Function Limpiar(ByVal valor As String) As String
            If valor Is Nothing Then
                Return String.Empty
            End If

            Return valor.Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        End Function

    End Class

End Namespace
