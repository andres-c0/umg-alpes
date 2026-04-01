Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class MetodoPagoDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub



Public Function Insertar(entidad As MetodoPago) As Integer

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_INSERTAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(entidad.Nombre Is Nothing, DBNull.Value, entidad.Nombre)

            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                If(entidad.Descripcion Is Nothing, DBNull.Value, entidad.Descripcion)

            Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
            pOut.Direction = ParameterDirection.Output
            cmd.Parameters.Add(pOut)

            conn.Open()
            cmd.ExecuteNonQuery()

            Return Convert.ToInt32(pOut.Value.ToString())

        End Using

    End Using

End Function



Public Sub Actualizar(entidad As MetodoPago)

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_ACTUALIZAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(entidad.Nombre Is Nothing, DBNull.Value, entidad.Nombre)

            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                If(entidad.Descripcion Is Nothing, DBNull.Value, entidad.Descripcion)

            conn.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Using

End Sub



Public Sub Eliminar(id As Integer)

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_ELIMINAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

            conn.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Using

End Sub



Public Function Listar() As DataTable

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_LISTAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction =
                ParameterDirection.Output

            Using da As New OracleDataAdapter(cmd)

                Dim dt As New DataTable()
                da.Fill(dt)
                Return dt

            End Using

        End Using

    End Using

End Function



Public Function Buscar(nombre As String) As DataTable

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_BUSCAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(nombre Is Nothing, DBNull.Value, nombre)

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction =
                ParameterDirection.Output

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

CREATE OR REPLACE PACKAGE BODY PKG_ORDEN_VENTA AS


PROCEDURE SP_INSERTAR(
    P_CLIENTE_ID        IN ORDEN_VENTA.CLIENTE_ID%TYPE,
    P_FECHA             IN ORDEN_VENTA.FECHA%TYPE,
    P_TOTAL             IN ORDEN_VENTA.TOTAL%TYPE,
    P_ESTADO_ORDEN_ID   IN ORDEN_VENTA.ESTADO_ORDEN_ID%TYPE,
    P_ID                OUT ORDEN_VENTA.ORDEN_VENTA_ID%TYPE
) AS
BEGIN

    INSERT INTO ORDEN_VENTA
    (
        CLIENTE_ID,
        FECHA,
        TOTAL,
        ESTADO_ORDEN_ID,
        ESTADO
    )
    VALUES
    (
        P_CLIENTE_ID,
        P_FECHA,
        P_TOTAL,
        P_ESTADO_ORDEN_ID,
        'A'
    )
    RETURNING ORDEN_VENTA_ID INTO P_ID;

END SP_INSERTAR;



PROCEDURE SP_ACTUALIZAR(
    P_ID                IN ORDEN_VENTA.ORDEN_VENTA_ID%TYPE,
    P_CLIENTE_ID        IN ORDEN_VENTA.CLIENTE_ID%TYPE,
    P_FECHA             IN ORDEN_VENTA.FECHA%TYPE,
    P_TOTAL             IN ORDEN_VENTA.TOTAL%TYPE,
    P_ESTADO_ORDEN_ID   IN ORDEN_VENTA.ESTADO_ORDEN_ID%TYPE
) AS
BEGIN

    UPDATE ORDEN_VENTA
    SET
        CLIENTE_ID = P_CLIENTE_ID,
        FECHA = P_FECHA,
        TOTAL = P_TOTAL,
        ESTADO_ORDEN_ID = P_ESTADO_ORDEN_ID
    WHERE ORDEN_VENTA_ID = P_ID;

END SP_ACTUALIZAR;



PROCEDURE SP_ELIMINAR(
    P_ID IN ORDEN_VENTA.ORDEN_VENTA_ID%TYPE
) AS
BEGIN

    UPDATE ORDEN_VENTA
    SET ESTADO = 'I'
    WHERE ORDEN_VENTA_ID = P_ID;

END SP_ELIMINAR;



PROCEDURE SP_OBTENER(
    P_ID     IN ORDEN_VENTA.ORDEN_VENTA_ID%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
) AS
BEGIN

    OPEN P_CURSOR FOR
        SELECT
            ORDEN_VENTA_ID,
            CLIENTE_ID,
            FECHA,
            TOTAL,
            ESTADO_ORDEN_ID,
            ESTADO
        FROM ORDEN_VENTA
        WHERE ORDEN_VENTA_ID = P_ID;

END SP_OBTENER;



PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
) AS
BEGIN

    OPEN P_CURSOR FOR
        SELECT
            ORDEN_VENTA_ID,
            CLIENTE_ID,
            FECHA,
            TOTAL,
            ESTADO_ORDEN_ID,
            ESTADO
        FROM ORDEN_VENTA
        WHERE ESTADO = 'A'
        ORDER BY FECHA DESC;

END SP_LISTAR;



PROCEDURE SP_BUSCAR(
    P_CLIENTE_ID IN NUMBER,
    P_CURSOR OUT SYS_REFCURSOR
) AS
BEGIN

    OPEN P_CURSOR FOR
        SELECT
            ORDEN_VENTA_ID,
            CLIENTE_ID,
            FECHA,
            TOTAL,
            ESTADO_ORDEN_ID,
            ESTADO
        FROM ORDEN_VENTA
        WHERE CLIENTE_ID = P_CLIENTE_ID
        AND ESTADO = 'A'
        ORDER BY FECHA DESC;

END SP_BUSCAR;


END PKG_ORDEN_VENTA;
/
