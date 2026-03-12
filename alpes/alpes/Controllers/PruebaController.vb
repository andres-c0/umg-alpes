Imports System.Web.Mvc
Imports Oracle.ManagedDataAccess.Client
Imports alpes.Datos

Public Class PruebaController
    Inherits Controller

    Function Conexion() As ActionResult
        Dim mensaje As String

        Try
            Dim conexionBD As New ConexionOracle()

            Using conn As OracleConnection = conexionBD.ObtenerConexion()
                conn.Open()
                mensaje = "Conexión exitosa a Oracle."
            End Using

        Catch ex As Exception
            mensaje = "Error al conectar con Oracle: " & ex.Message
        End Try

        ViewBag.Mensaje = mensaje
        Return View()
    End Function

End Class