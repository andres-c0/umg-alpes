Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Marketing

Namespace Repositorios
    Public Class Campana_MarketingDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Campana_Marketing) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_INSERTAR_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Canal) Then
                        cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = entidad.Canal
                    End If

                    If entidad.Presupuesto.HasValue Then
                        cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = entidad.Presupuesto.Value
                    Else
                        cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_INICIO", OracleDbType.Date).Value = entidad.Inicio
                    cmd.Parameters.Add("P_FIN", OracleDbType.Date).Value = entidad.Fin
                    cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_CAMPANA_MARKETING_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Campana_Marketing)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_ACTUALIZAR_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = entidad.CampanaMarketingId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Canal) Then
                        cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = entidad.Canal
                    End If

                    If entidad.Presupuesto.HasValue Then
                        cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = entidad.Presupuesto.Value
                    Else
                        cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_INICIO", OracleDbType.Date).Value = entidad.Inicio
                    cmd.Parameters.Add("P_FIN", OracleDbType.Date).Value = entidad.Fin

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_ELIMINAR_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Campana_Marketing
            Dim entidad As Campana_Marketing = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_OBTENER_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearCampanaMarketing(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Campana_Marketing)
            Dim lista As New List(Of Campana_Marketing)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_LISTAR_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCampanaMarketing(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Campana_Marketing)
            Dim lista As New List(Of Campana_Marketing)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_BUSCAR_CAMPANA_MARKETING", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCampanaMarketing(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearCampanaMarketing(ByVal dr As OracleDataReader) As Campana_Marketing
            Dim entidad As New Campana_Marketing()

            entidad.CampanaMarketingId = Convert.ToInt32(dr("CAMPANA_MARKETING_ID"))
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.Canal = If(IsDBNull(dr("CANAL")), Nothing, dr("CANAL").ToString())
            entidad.Presupuesto = If(IsDBNull(dr("PRESUPUESTO")), CType(Nothing, Decimal?), Convert.ToDecimal(dr("PRESUPUESTO")))
            entidad.Inicio = Convert.ToDateTime(dr("INICIO"))
            entidad.Fin = Convert.ToDateTime(dr("FIN"))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace