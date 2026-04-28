Option Strict On
Option Explicit On

Imports Alpes.Datos.Utilidades

Namespace Servicios
    Public Class PruebaConexionServicio

        Public Function Probar() As String
            Dim prueba As New PruebaConexion()
            Return prueba.Probar()
        End Function

    End Class
End Namespace