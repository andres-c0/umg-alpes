Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Envios

Namespace Repositorios
    Public Class Zona_EnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Zona_Envio) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_INSERTAR_ZONA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Pais) Then
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Departamento) Then
                        cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = entidad.Departamento
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ciudad) Then
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    End If

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ZONA_ENVIO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Zona_Envio)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_ACTUALIZAR_ZONA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Pais) Then
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Departamento) Then
                        cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = entidad.Departamento
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ciudad) Then
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_ELIMINAR_ZONA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Zona_Envio
            Dim entidad As Zona_Envio = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_OBTENER_ZONA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearZonaEnvio(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Zona_Envio)
            Dim lista As New List(Of Zona_Envio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_LISTAR_ZONAS_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearZonaEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Zona_Envio)
            Dim lista As New List(Of Zona_Envio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_BUSCAR_ZONAS_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearZonaEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearZonaEnvio(ByVal dr As OracleDataReader) As Zona_Envio
            Dim entidad As New Zona_Envio()

            entidad.ZonaEnvioId = Convert.ToInt32(dr("ZONA_ENVIO_ID"))
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.Pais = If(IsDBNull(dr("PAIS")), Nothing, dr("PAIS").ToString())
            entidad.Departamento = If(IsDBNull(dr("DEPARTAMENTO")), Nothing, dr("DEPARTAMENTO").ToString())
            entidad.Ciudad = If(IsDBNull(dr("CIUDAD")), Nothing, dr("CIUDAD").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace