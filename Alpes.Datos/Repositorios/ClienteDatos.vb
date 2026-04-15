Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Clientes

Namespace Repositorios
    Public Class ClienteDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Cliente) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_INSERTAR_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TIPO_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.TipoDocumento
                    cmd.Parameters.Add("P_NUM_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.NumDocumento

                    If String.IsNullOrWhiteSpace(entidad.Nit) Then
                        cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = entidad.Nit
                    End If

                    cmd.Parameters.Add("P_NOMBRES", OracleDbType.Varchar2).Value = entidad.Nombres
                    cmd.Parameters.Add("P_APELLIDOS", OracleDbType.Varchar2).Value = entidad.Apellidos
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email

                    If String.IsNullOrWhiteSpace(entidad.TelResidencia) Then
                        cmd.Parameters.Add("P_TEL_RESIDENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TEL_RESIDENCIA", OracleDbType.Varchar2).Value = entidad.TelResidencia
                    End If

                    If String.IsNullOrWhiteSpace(entidad.TelCelular) Then
                        cmd.Parameters.Add("P_TEL_CELULAR", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TEL_CELULAR", OracleDbType.Varchar2).Value = entidad.TelCelular
                    End If

                    cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = entidad.Direccion
                    cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = entidad.Departamento
                    cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais

                    If String.IsNullOrWhiteSpace(entidad.Profesion) Then
                        cmd.Parameters.Add("P_PROFESION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PROFESION", OracleDbType.Varchar2).Value = entidad.Profesion
                    End If

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_CLI_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Cliente)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_ACTUALIZAR_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_TIPO_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.TipoDocumento
                    cmd.Parameters.Add("P_NUM_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.NumDocumento

                    If String.IsNullOrWhiteSpace(entidad.Nit) Then
                        cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = entidad.Nit
                    End If

                    cmd.Parameters.Add("P_NOMBRES", OracleDbType.Varchar2).Value = entidad.Nombres
                    cmd.Parameters.Add("P_APELLIDOS", OracleDbType.Varchar2).Value = entidad.Apellidos
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email

                    If String.IsNullOrWhiteSpace(entidad.TelResidencia) Then
                        cmd.Parameters.Add("P_TEL_RESIDENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TEL_RESIDENCIA", OracleDbType.Varchar2).Value = entidad.TelResidencia
                    End If

                    If String.IsNullOrWhiteSpace(entidad.TelCelular) Then
                        cmd.Parameters.Add("P_TEL_CELULAR", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TEL_CELULAR", OracleDbType.Varchar2).Value = entidad.TelCelular
                    End If

                    cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = entidad.Direccion
                    cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = entidad.Departamento
                    cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais

                    If String.IsNullOrWhiteSpace(entidad.Profesion) Then
                        cmd.Parameters.Add("P_PROFESION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PROFESION", OracleDbType.Varchar2).Value = entidad.Profesion
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_ELIMINAR_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cliente
            Dim entidad As Cliente = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_OBTENER_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearClienteCompleto(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Cliente)
            Dim lista As New List(Of Cliente)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_LISTAR_CLIENTES", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearClienteCompleto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Cliente)
            Dim lista As New List(Of Cliente)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_BUSCAR_CLIENTES", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearClienteCompleto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearClienteCompleto(ByVal dr As OracleDataReader) As Cliente
            Dim entidad As New Cliente()

            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.TipoDocumento = If(IsDBNull(dr("TIPO_DOCUMENTO")), Nothing, dr("TIPO_DOCUMENTO").ToString())
            entidad.NumDocumento = If(IsDBNull(dr("NUM_DOCUMENTO")), Nothing, dr("NUM_DOCUMENTO").ToString())
            entidad.Nit = If(IsDBNull(dr("NIT")), Nothing, dr("NIT").ToString())
            entidad.Nombres = If(IsDBNull(dr("NOMBRES")), Nothing, dr("NOMBRES").ToString())
            entidad.Apellidos = If(IsDBNull(dr("APELLIDOS")), Nothing, dr("APELLIDOS").ToString())
            entidad.NombreCompleto = If(IsDBNull(dr("NOMBRE_COMPLETO")), (entidad.Nombres & " " & entidad.Apellidos).Trim(), dr("NOMBRE_COMPLETO").ToString())
            entidad.Email = If(IsDBNull(dr("EMAIL")), Nothing, dr("EMAIL").ToString())
            entidad.TelResidencia = If(IsDBNull(dr("TEL_RESIDENCIA")), Nothing, dr("TEL_RESIDENCIA").ToString())
            entidad.TelCelular = If(IsDBNull(dr("TEL_CELULAR")), Nothing, dr("TEL_CELULAR").ToString())
            entidad.Direccion = If(IsDBNull(dr("DIRECCION")), Nothing, dr("DIRECCION").ToString())
            entidad.Ciudad = If(IsDBNull(dr("CIUDAD")), Nothing, dr("CIUDAD").ToString())
            entidad.Departamento = If(IsDBNull(dr("DEPARTAMENTO")), Nothing, dr("DEPARTAMENTO").ToString())
            entidad.Pais = If(IsDBNull(dr("PAIS")), Nothing, dr("PAIS").ToString())
            entidad.Profesion = If(IsDBNull(dr("PROFESION")), Nothing, dr("PROFESION").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

        Private Function MapearClienteListado(ByVal dr As OracleDataReader) As Cliente
            Dim entidad As New Cliente()

            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.NumDocumento = dr("NUM_DOCUMENTO").ToString()
            entidad.NombreCompleto = dr("NOMBRE_COMPLETO").ToString()
            entidad.Email = dr("EMAIL").ToString()
            entidad.Ciudad = dr("CIUDAD").ToString()
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace