Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client

Namespace Datos

    Public Class ConexionOracle

        Private ReadOnly cadena As String

        Public Sub New()
            cadena = ConfigurationManager.ConnectionStrings("OracleDB").ConnectionString
        End Sub

        Public Function ObtenerConexion() As OracleConnection
            Return New OracleConnection(cadena)
        End Function

    End Class

End Namespace