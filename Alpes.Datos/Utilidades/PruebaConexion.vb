Option Strict On
Option Explicit On

Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion

Namespace Utilidades
    Public Class PruebaConexion

        Public Function Probar() As String
            Try
                Dim conexionOracle As New ConexionOracle()

                Using cn As OracleConnection = conexionOracle.ObtenerConexion()
                    Return "Conexión exitosa a Oracle."
                End Using

            Catch ex As Exception
                Return "Error al conectar: " & ex.Message
            End Try
        End Function

    End Class
End Namespace