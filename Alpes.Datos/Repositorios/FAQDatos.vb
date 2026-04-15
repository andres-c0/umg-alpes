Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Clientes

Namespace Repositorios
    Public Class FAQDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As FAQ) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_INSERTAR_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PREGUNTA", OracleDbType.Varchar2).Value = entidad.Pregunta
                    cmd.Parameters.Add("P_RESPUESTA", OracleDbType.Varchar2).Value = entidad.Respuesta

                    If entidad.Orden.HasValue Then
                        cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = entidad.Orden.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_FAQ_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As FAQ)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_ACTUALIZAR_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Value = entidad.FaqId
                    cmd.Parameters.Add("P_PREGUNTA", OracleDbType.Varchar2).Value = entidad.Pregunta
                    cmd.Parameters.Add("P_RESPUESTA", OracleDbType.Varchar2).Value = entidad.Respuesta

                    If entidad.Orden.HasValue Then
                        cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = entidad.Orden.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_ELIMINAR_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As FAQ
            Dim entidad As FAQ = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_OBTENER_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearFAQ(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of FAQ)
            Dim lista As New List(Of FAQ)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_LISTAR_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFAQ(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of FAQ)
            Dim lista As New List(Of FAQ)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_BUSCAR_FAQ", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFAQ(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearFAQ(ByVal dr As OracleDataReader) As FAQ
            Dim entidad As New FAQ()

            entidad.FaqId = Convert.ToInt32(dr("FAQ_ID"))
            entidad.Pregunta = dr("PREGUNTA").ToString()
            entidad.Respuesta = dr("RESPUESTA").ToString()
            entidad.Orden = If(IsDBNull(dr("ORDEN")), CType(Nothing, Integer?), Convert.ToInt32(dr("ORDEN")))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace