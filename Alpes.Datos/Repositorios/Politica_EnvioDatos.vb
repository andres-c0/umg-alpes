Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Envios

Namespace Repositorios
    Public Class Politica_EnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Politica_Envio) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_INSERTAR_POLITICA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = entidad.Titulo
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion

                    If entidad.VigenciaInicio.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_POLITICA_ENVIO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Politica_Envio)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_ACTUALIZAR_POLITICA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = entidad.PoliticaEnvioId
                    cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = entidad.Titulo
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion

                    If entidad.VigenciaInicio.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_ELIMINAR_POLITICA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Politica_Envio
            Dim entidad As Politica_Envio = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_OBTENER_POLITICA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearPoliticaEnvio(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Politica_Envio)
            Dim lista As New List(Of Politica_Envio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_LISTAR_POLITICAS_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPoliticaEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Politica_Envio)
            Dim lista As New List(Of Politica_Envio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_BUSCAR_POLITICAS_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPoliticaEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearPoliticaEnvio(ByVal dr As OracleDataReader) As Politica_Envio
            Dim entidad As New Politica_Envio()

            entidad.PoliticaEnvioId = Convert.ToInt32(dr("POLITICA_ENVIO_ID"))
            entidad.Titulo = dr("TITULO").ToString()
            entidad.Descripcion = dr("DESCRIPCION").ToString()
            entidad.VigenciaInicio = If(IsDBNull(dr("VIGENCIA_INICIO")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("VIGENCIA_INICIO")))
            entidad.VigenciaFin = If(IsDBNull(dr("VIGENCIA_FIN")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("VIGENCIA_FIN")))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace