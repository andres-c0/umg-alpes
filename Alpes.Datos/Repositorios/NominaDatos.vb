Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class NominaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Nomina) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.PeriodoInicio.HasValue Then
                        cmd.Parameters.Add("p_periodo_inicio", OracleDbType.Date).Value = entidad.PeriodoInicio.Value
                    Else
                        cmd.Parameters.Add("p_periodo_inicio", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.PeriodoFin.HasValue Then
                        cmd.Parameters.Add("p_periodo_fin", OracleDbType.Date).Value = entidad.PeriodoFin.Value
                    Else
                        cmd.Parameters.Add("p_periodo_fin", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.MontoBruto.HasValue Then
                        cmd.Parameters.Add("p_monto_bruto", OracleDbType.Decimal).Value = entidad.MontoBruto.Value
                    Else
                        cmd.Parameters.Add("p_monto_bruto", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.MontoNeto.HasValue Then
                        cmd.Parameters.Add("p_monto_neto", OracleDbType.Decimal).Value = entidad.MontoNeto.Value
                    Else
                        cmd.Parameters.Add("p_monto_neto", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.FechaPago.HasValue Then
                        cmd.Parameters.Add("p_fecha_pago", OracleDbType.Date).Value = entidad.FechaPago.Value
                    Else
                        cmd.Parameters.Add("p_fecha_pago", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("p_nomina_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("p_nomina_id").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Nomina)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", OracleDbType.Int32).Value = entidad.NominaId

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.PeriodoInicio.HasValue Then
                        cmd.Parameters.Add("p_periodo_inicio", OracleDbType.Date).Value = entidad.PeriodoInicio.Value
                    Else
                        cmd.Parameters.Add("p_periodo_inicio", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.PeriodoFin.HasValue Then
                        cmd.Parameters.Add("p_periodo_fin", OracleDbType.Date).Value = entidad.PeriodoFin.Value
                    Else
                        cmd.Parameters.Add("p_periodo_fin", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.MontoBruto.HasValue Then
                        cmd.Parameters.Add("p_monto_bruto", OracleDbType.Decimal).Value = entidad.MontoBruto.Value
                    Else
                        cmd.Parameters.Add("p_monto_bruto", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.MontoNeto.HasValue Then
                        cmd.Parameters.Add("p_monto_neto", OracleDbType.Decimal).Value = entidad.MontoNeto.Value
                    Else
                        cmd.Parameters.Add("p_monto_neto", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.FechaPago.HasValue Then
                        cmd.Parameters.Add("p_fecha_pago", OracleDbType.Date).Value = entidad.FechaPago.Value
                    Else
                        cmd.Parameters.Add("p_fecha_pago", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Nomina
            Dim entidad As Nomina = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearNomina(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Nomina)
            Dim lista As New List(Of Nomina)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearNomina(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearNomina(ByVal dr As OracleDataReader) As Nomina
            Dim entidad As New Nomina()

            entidad.NominaId = LeerEntero(dr, "NOMINA_ID")
            entidad.EmpId = LeerEnteroNullable(dr, "EMP_ID")
            entidad.PeriodoInicio = LeerFechaNullable(dr, "PERIODO_INICIO")
            entidad.PeriodoFin = LeerFechaNullable(dr, "PERIODO_FIN")
            entidad.MontoBruto = LeerDecimalNullable(dr, "MONTO_BRUTO")
            entidad.MontoNeto = LeerDecimalNullable(dr, "MONTO_NETO")
            entidad.FechaPago = LeerFechaNullable(dr, "FECHA_PAGO")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")
            entidad.Estado = LeerTexto(dr, "ESTADO")

            Return entidad
        End Function

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return dr(columna).ToString()
            End If

            Return String.Empty
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return 0
        End Function

        Private Function LeerEnteroNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Integer?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerDecimalNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Decimal?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDecimal(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).ToUpper() = nombreColumna.ToUpper() Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace