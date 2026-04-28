Option Strict On
Option Explicit On

Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace Conexion
    Public Class ConexionOracle

        Private ReadOnly _connectionString As String

        Public Sub New()
            _connectionString = ConfigurationManager.ConnectionStrings("OracleDb").ConnectionString
        End Sub

        Public Function ObtenerConexion() As OracleConnection
            Dim conexion As New OracleConnection(_connectionString)
            conexion.Open()
            Return conexion
        End Function

    End Class
End Namespace