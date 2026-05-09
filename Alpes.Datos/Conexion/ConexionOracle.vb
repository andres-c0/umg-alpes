Option Strict On
Option Explicit On

Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace Conexion

    Public Class ConexionOracle

        Private ReadOnly _connectionString As String
        Private ReadOnly _replicaConnectionString As String

        Public Sub New()

            ' BASE PRINCIPAL
            _connectionString =
                ConfigurationManager.ConnectionStrings("OracleDb").ConnectionString

            ' RÉPLICA
            _replicaConnectionString =
                ConfigurationManager.ConnectionStrings("OracleDbReplica").ConnectionString

        End Sub

        ' =========================
        ' CONEXIÓN PRINCIPAL
        ' =========================
        Public Function ObtenerConexion() As OracleConnection

            Dim conexion As New OracleConnection(_connectionString)

            conexion.Open()

            Return conexion

        End Function

        ' =========================
        ' CONEXIÓN RÉPLICA
        ' =========================
        Public Function ObtenerConexionReplica() As OracleConnection

            Dim conexion As New OracleConnection(_replicaConnectionString)

            conexion.Open()

            Return conexion

        End Function

    End Class

End Namespace