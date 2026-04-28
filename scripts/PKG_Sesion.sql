Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class UsuarioDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(usuario As Usuario) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_USUARIO.SP_INSERTAR_USUARIO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = usuario.Nombre
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = usuario.Email
                    cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = usuario.Password
                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = usuario.RolId

                    Dim pOut As New OracleParameter("P_USUARIO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using

            End Using

        End Function



        Public Function Login(email As String, password As String) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_USUARIO.SP_LOGIN", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = email
                    cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = password
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)

                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt

                    End Using

                End Using

            End Using

        End Function

    End Class

End Namespace

CREATE OR REPLACE PACKAGE BODY PKG_SESION AS


  PROCEDURE SP_CREAR_SESION(
    P_USUARIO_ID IN SESION.USUARIO_ID%TYPE,
    P_IP         IN VARCHAR2,
    P_SESION_ID  OUT SESION.SESION_ID%TYPE
  ) AS
  BEGIN

    INSERT INTO SESION(
        USUARIO_ID,
        FECHA_INICIO,
        IP
    )
    VALUES(
        P_USUARIO_ID,
        SYSTIMESTAMP,
        P_IP
    )
    RETURNING SESION_ID INTO P_SESION_ID;

  END SP_CREAR_SESION;



  PROCEDURE SP_CERRAR_SESION(
    P_SESION_ID IN SESION.SESION_ID%TYPE
  ) AS
  BEGIN

    UPDATE SESION
       SET FECHA_FIN = SYSTIMESTAMP
     WHERE SESION_ID = P_SESION_ID;

  END SP_CERRAR_SESION;



  PROCEDURE SP_SESIONES_POR_USUARIO(
    P_USUARIO_ID IN NUMBER,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN

    OPEN P_CURSOR FOR
        SELECT
            SESION_ID,
            USUARIO_ID,
            FECHA_INICIO,
            FECHA_FIN,
            IP
        FROM SESION
        WHERE USUARIO_ID = P_USUARIO_ID
        ORDER BY FECHA_INICIO DESC;

  END SP_SESIONES_POR_USUARIO;



  PROCEDURE SP_OBTENER_SESION(
    P_SESION_ID IN NUMBER,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN

    OPEN P_CURSOR FOR
        SELECT
            SESION_ID,
            USUARIO_ID,
            FECHA_INICIO,
            FECHA_FIN,
            IP
        FROM SESION
        WHERE SESION_ID = P_SESION_ID;

  END SP_OBTENER_SESION;


END PKG_SESION;
/