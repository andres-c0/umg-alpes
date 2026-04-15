Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class DepartamentoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Departamento) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_INSERTAR_DEPARTAMENTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_DEPTO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Departamento)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_ACTUALIZAR_DEPARTAMENTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Value = entidad.DeptoId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_ELIMINAR_DEPARTAMENTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Departamento
            Dim entidad As Departamento = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_OBTENER_DEPARTAMENTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearDepartamento(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Departamento)
            Dim lista As New List(Of Departamento)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_LISTAR_DEPARTAMENTOS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearDepartamento(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearDepartamento(ByVal dr As OracleDataReader) As Departamento
            Dim entidad As New Departamento()

            entidad.DeptoId = Convert.ToInt32(dr("DEPTO_ID"))
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.Descripcion = If(IsDBNull(dr("DESCRIPCION")), Nothing, dr("DESCRIPCION").ToString())
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace