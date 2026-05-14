Option Strict On
Option Explicit On

Imports Oracle.ManagedDataAccess.Client
Imports System
Imports System.Configuration

Namespace Conexion

    Public Class ConexionOracle

        Private ReadOnly _connectionString As String
        Private ReadOnly _replicaConnectionString As String

        Public Sub New()
            _connectionString = ObtenerCadenaConexionSegura("OracleDb", "ALPES_ORACLE_MAIN")
            _replicaConnectionString = ObtenerCadenaConexionSegura("OracleDbReplica", "ALPES_ORACLE_REPLICA")
        End Sub

        Public Function ObtenerConexion() As OracleConnection
            Dim conexion As New OracleConnection(_connectionString)
            conexion.Open()
            Return conexion
        End Function

        Public Function ObtenerConexionReplica() As OracleConnection
            Dim conexion As New OracleConnection(_replicaConnectionString)
            conexion.Open()
            Return conexion
        End Function

        Private Shared Function ObtenerCadenaConexionSegura(ByVal nombreConfig As String, ByVal variableEntorno As String) As String
            Dim valorEntorno As String = Environment.GetEnvironmentVariable(variableEntorno)

            If Not String.IsNullOrWhiteSpace(valorEntorno) Then
                Return valorEntorno.Trim()
            End If

            Dim configuracion As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(nombreConfig)

            If configuracion IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(configuracion.ConnectionString) Then
                Return configuracion.ConnectionString.Trim()
            End If

            Throw New ConfigurationErrorsException("No se encontró la cadena de conexión " & nombreConfig & ". Configura la variable de entorno " & variableEntorno & ".")
        End Function

    End Class

End Namespace
