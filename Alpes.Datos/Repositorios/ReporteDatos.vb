Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Datos.Modelos
Imports System.Linq

Namespace Repositorios

    Public Class ReporteDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function ObtenerReporte(periodo As String,
                                       anio As Integer,
                                       mesInicio As Integer,
                                       mesFin As Integer,
                                       trimestre As Integer) As ReporteDashboard

            AjustarRango(periodo, mesInicio, mesFin, trimestre)

            Dim reporte As New ReporteDashboard()

            reporte.VentasMensuales = ObtenerVentasMensuales(anio, mesInicio, mesFin)
            reporte.UltimasOrdenes = ObtenerUltimasOrdenes(anio, mesInicio, mesFin)
            reporte.ProductosTop = ObtenerProductosTop(anio, mesInicio, mesFin)
            reporte.VentasAgrupadas = ObtenerVentasAgrupadas(anio, mesInicio, mesFin)

            reporte.ProductoMasVendido = ObtenerProductoMasVendido(anio, mesInicio, mesFin)
            reporte.CierreCaja = ObtenerCierreCaja(anio, mesInicio, mesFin)
            reporte.DetalleMensualProductos = ObtenerDetalleMensualProductos(anio, mesInicio, mesFin)
            reporte.ItemsVendidos = ObtenerItemsVendidos(anio, mesInicio, mesFin)

            reporte.Cohortes = ObtenerCohortes(anio, mesInicio, mesFin)
            reporte.ClientesCohorte = reporte.Cohortes.Sum(Function(c) c.Clientes)
            reporte.VentaCohorte = reporte.Cohortes.Sum(Function(c) c.Ventas)

            If reporte.ClientesCohorte > 0 Then
                reporte.RetencionM1 = Math.Round(Convert.ToDecimal(reporte.Cohortes.Sum(Function(c) c.M1)) / reporte.ClientesCohorte * 100D, 1)
                reporte.RetencionM2 = Math.Round(Convert.ToDecimal(reporte.Cohortes.Sum(Function(c) c.M2)) / reporte.ClientesCohorte * 100D, 1)
            End If

            reporte.VentasTotales = ObtenerDecimal("
                SELECT NVL(SUM(TOTAL),0)
                FROM ORDEN_VENTA
                WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
                anio, mesInicio, mesFin)

            reporte.Ordenes = ObtenerEntero("
                SELECT COUNT(*)
                FROM ORDEN_VENTA
                WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
                anio, mesInicio, mesFin)

            reporte.Clientes = ObtenerEntero("
                SELECT COUNT(DISTINCT CLI_ID)
                FROM ORDEN_VENTA
                WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
                anio, mesInicio, mesFin)

            reporte.Canceladas = ObtenerEntero("
                SELECT COUNT(*)
                FROM ORDEN_VENTA O
                LEFT JOIN ESTADO_ORDEN EO ON EO.ESTADO_ORDEN_ID = O.ESTADO_ORDEN_ID
                WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
                AND (
                    UPPER(NVL(EO.CODIGO, '')) LIKE '%CANCEL%'
                    OR UPPER(NVL(EO.DESCRIPCION, '')) LIKE '%CANCEL%'
                )",
                anio, mesInicio, mesFin)

            reporte.ProductosStockBajo = 0

            If reporte.Ordenes > 0 Then
                reporte.TicketPromedio = reporte.VentasTotales / reporte.Ordenes
            Else
                reporte.TicketPromedio = 0D
            End If

            reporte.ComprasPorCliente = ObtenerComprasPorCliente(anio, mesInicio, mesFin)
            reporte.IndicadoresMensuales = ObtenerIndicadoresMensuales(anio, mesInicio, mesFin)

            If reporte.Clientes > 0 Then
                reporte.ValorPorCliente = reporte.VentasTotales / reporte.Clientes
            End If

            reporte.PorcentajeRecompra = ObtenerPorcentajeRecompra(anio, mesInicio, mesFin)

            Return reporte
        End Function

        Private Sub AjustarRango(periodo As String,
                                 ByRef mesInicio As Integer,
                                 ByRef mesFin As Integer,
                                 trimestre As Integer)

            If String.Equals(periodo, "anual", StringComparison.OrdinalIgnoreCase) Then
                mesInicio = 1
                mesFin = 12
                Return
            End If

            If String.Equals(periodo, "trimestre", StringComparison.OrdinalIgnoreCase) Then
                Select Case trimestre
                    Case 1
                        mesInicio = 1
                        mesFin = 3
                    Case 2
                        mesInicio = 4
                        mesFin = 6
                    Case 3
                        mesInicio = 7
                        mesFin = 9
                    Case 4
                        mesInicio = 10
                        mesFin = 12
                    Case Else
                        mesInicio = 1
                        mesFin = 3
                End Select
            End If
        End Sub

        Private Function ObtenerVentasMensuales(anio As Integer,
                                                mesInicio As Integer,
                                                mesFin As Integer) As List(Of VentaMensualViewModel)

            Dim lista As New List(Of VentaMensualViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
                    SELECT 
                        EXTRACT(MONTH FROM FECHA_ORDEN) AS MES_NUM,
                        TO_CHAR(FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH') AS MES,
                        NVL(SUM(TOTAL),0) AS TOTAL,
                        COUNT(*) AS ORDENES
                    FROM ORDEN_VENTA
                    WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
                    AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
                    GROUP BY 
                        EXTRACT(MONTH FROM FECHA_ORDEN),
                        TO_CHAR(FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH')
                    ORDER BY MES_NUM", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(New VentaMensualViewModel With {
                                .Mes = dr("MES").ToString().Trim(),
                                .Total = Convert.ToDecimal(dr("TOTAL")),
                                .Ordenes = Convert.ToInt32(dr("ORDENES"))
                            })
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function ObtenerUltimasOrdenes(anio As Integer,
                                               mesInicio As Integer,
                                               mesFin As Integer) As List(Of OrdenReporteViewModel)

            Dim lista As New List(Of OrdenReporteViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
                    SELECT *
                    FROM (
                        SELECT 
                            O.NUM_ORDEN,
                            O.FECHA_ORDEN,
                            O.TOTAL,
                            CASE
                                WHEN C.NOMBRES IS NOT NULL THEN
                                    TRIM(C.NOMBRES || ' ' || NVL(C.APELLIDOS, ''))
                                ELSE
                                    'Cliente #' || O.CLI_ID
                            END AS CLIENTE,
                            NVL(EO.DESCRIPCION, NVL(EO.CODIGO, NVL(O.ESTADO, 'No especificado'))) AS ESTADO
                        FROM ORDEN_VENTA O
                        LEFT JOIN CLIENTE C ON C.CLI_ID = O.CLI_ID
                        LEFT JOIN ESTADO_ORDEN EO ON EO.ESTADO_ORDEN_ID = O.ESTADO_ORDEN_ID
                        WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                        AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
                        ORDER BY O.FECHA_ORDEN DESC
                    )
                    WHERE ROWNUM <= 10", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(New OrdenReporteViewModel With {
                                .Codigo = dr("NUM_ORDEN").ToString(),
                                .Cliente = dr("CLIENTE").ToString(),
                                .FechaOrden = Convert.ToDateTime(dr("FECHA_ORDEN")),
                                .Estado = dr("ESTADO").ToString(),
                                .Total = Convert.ToDecimal(dr("TOTAL"))
                            })
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function ObtenerItemsVendidos(anio As Integer,
                                              mesInicio As Integer,
                                              mesFin As Integer) As Integer

            Return ObtenerEntero("
                SELECT NVL(SUM(D.CANTIDAD),0)
                FROM ORDEN_VENTA_DETALLE D
                INNER JOIN ORDEN_VENTA O ON O.ORDEN_VENTA_ID = D.ORDEN_VENTA_ID
                WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
                anio, mesInicio, mesFin)

        End Function

        Private Function ObtenerProductosTop(anio As Integer,
                                     mesInicio As Integer,
                                     mesFin As Integer) As List(Of ProductoTopViewModel)

            Dim lista As New List(Of ProductoTopViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
            SELECT *
            FROM (
                SELECT 
                    NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID) AS PRODUCTO,
                    NVL(P.TIPO, 'SIN TIPO') AS TIPO,
                    SUM(D.CANTIDAD) AS CANTIDAD,
                    SUM(D.SUBTOTAL_LINEA) AS TOTAL
                FROM ORDEN_VENTA_DETALLE D
                INNER JOIN ORDEN_VENTA O
                    ON O.ORDEN_VENTA_ID = D.ORDEN_VENTA_ID
                LEFT JOIN PRODUCTO P
                    ON P.PRODUCTO_ID = D.PRODUCTO_ID
                WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
                GROUP BY 
                    NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID),
                    NVL(P.TIPO, 'SIN TIPO')
                ORDER BY SUM(D.CANTIDAD) DESC
            )
            WHERE ROWNUM <= 7", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(New ProductoTopViewModel With {
                        .Producto = dr("PRODUCTO").ToString(),
                        .Tipo = dr("TIPO").ToString(),
                        .Cantidad = Convert.ToInt32(dr("CANTIDAD")),
                        .Total = Convert.ToDecimal(dr("TOTAL"))
                    })
                        End While
                    End Using
                End Using
            End Using

            Return lista

        End Function

        Private Function ObtenerProductoMasVendido(anio As Integer,
                                           mesInicio As Integer,
                                           mesFin As Integer) As ProductoTopViewModel

            Dim lista = ObtenerProductosTop(anio, mesInicio, mesFin)

            If lista Is Nothing OrElse lista.Count = 0 Then
                Return Nothing
            End If

            Return lista(0)

        End Function

        Private Function ObtenerCierreCaja(anio As Integer,
                                   mesInicio As Integer,
                                   mesFin As Integer) As List(Of CierreCajaViewModel)

            Dim lista As New List(Of CierreCajaViewModel)()

            Dim totalVentas As Decimal = ObtenerDecimal("
        SELECT NVL(SUM(TOTAL),0)
        FROM ORDEN_VENTA
        WHERE EXTRACT(YEAR FROM FECHA_ORDEN)=:ANIO
        AND EXTRACT(MONTH FROM FECHA_ORDEN)
            BETWEEN :MES_INICIO AND :MES_FIN",
        anio, mesInicio, mesFin)

            Dim ordenes As Integer = ObtenerEntero("
        SELECT COUNT(*)
        FROM ORDEN_VENTA
        WHERE EXTRACT(YEAR FROM FECHA_ORDEN)=:ANIO
        AND EXTRACT(MONTH FROM FECHA_ORDEN)
            BETWEEN :MES_INICIO AND :MES_FIN",
        anio, mesInicio, mesFin)

            lista.Add(New CierreCajaViewModel With {
        .FormaPago = "No especificado",
        .Ordenes = ordenes,
        .Total = totalVentas,
        .Porcentaje = 100D
    })

            Return lista

        End Function

        Private Function ObtenerDecimal(sql As String,
                                        anio As Integer,
                                        mesInicio As Integer,
                                        mesFin As Integer) As Decimal

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand(sql, cn)
                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        Return 0D
                    End If

                    Return Convert.ToDecimal(result)
                End Using
            End Using
        End Function

        Private Function ObtenerEntero(sql As String,
                                       anio As Integer,
                                       mesInicio As Integer,
                                       mesFin As Integer) As Integer

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand(sql, cn)
                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        Return 0
                    End If

                    Return Convert.ToInt32(result)
                End Using
            End Using
        End Function

        Private Function ObtenerVentasAgrupadas(anio As Integer,
                                        mesInicio As Integer,
                                        mesFin As Integer) As List(Of VentaAgrupadaViewModel)

            Dim lista As New List(Of VentaAgrupadaViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()

                Using cmd As New OracleCommand("
                    SELECT
                        NVL(P.TIPO, 'SIN TIPO') AS TIPO,
                        O.FECHA_ORDEN,
                        NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID) AS PRODUCTO,
                        D.CANTIDAD,
                        D.PRECIO_UNITARIO_SNAPSHOT AS COSTO_UNITARIO,
                        D.SUBTOTAL_LINEA AS TOTAL
                    FROM ORDEN_VENTA_DETALLE D
                    INNER JOIN ORDEN_VENTA O
                        ON O.ORDEN_VENTA_ID = D.ORDEN_VENTA_ID
                    LEFT JOIN PRODUCTO P
                        ON P.PRODUCTO_ID = D.PRODUCTO_ID
                    WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                    AND EXTRACT(MONTH FROM O.FECHA_ORDEN)
                        BETWEEN :MES_INICIO AND :MES_FIN
                    ORDER BY NVL(P.TIPO, 'SIN TIPO'), O.FECHA_ORDEN
                ", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()

                        While dr.Read()

                            lista.Add(New VentaAgrupadaViewModel With {
                                .Tipo = dr("TIPO").ToString(),
                                .Fecha = If(IsDBNull(dr("FECHA_ORDEN")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("FECHA_ORDEN"))),
                                .Producto = dr("PRODUCTO").ToString(),
                                .Cantidad = Convert.ToInt32(dr("CANTIDAD")),
                                .CostoUnitario = Convert.ToDecimal(dr("COSTO_UNITARIO")),
                                .Total = Convert.ToDecimal(dr("TOTAL"))
                            })

                        End While

                    End Using

                End Using

            End Using

            Return lista

        End Function

        Private Function ObtenerComprasPorCliente(anio As Integer,
                                          mesInicio As Integer,
                                          mesFin As Integer) As List(Of CompraClienteViewModel)

            Dim lista As New List(Of CompraClienteViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
            SELECT *
            FROM (
                SELECT
                    O.FECHA_ORDEN,
                    TRIM(NVL(C.NOMBRES, '') || ' ' || NVL(C.APELLIDOS, '')) AS CLIENTE,
                    O.TOTAL AS VALOR,
                    'No especificado' AS FORMA_PAGO,
                    LISTAGG(NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID), ', ')
                        WITHIN GROUP (ORDER BY P.NOMBRE) AS MUEBLES
                FROM ORDEN_VENTA O
                LEFT JOIN CLIENTE C
                    ON C.CLI_ID = O.CLI_ID
                LEFT JOIN ORDEN_VENTA_DETALLE D
                    ON D.ORDEN_VENTA_ID = O.ORDEN_VENTA_ID
                LEFT JOIN PRODUCTO P
                    ON P.PRODUCTO_ID = D.PRODUCTO_ID
                WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
                GROUP BY
                    O.ORDEN_VENTA_ID,
                    O.FECHA_ORDEN,
                    TRIM(NVL(C.NOMBRES, '') || ' ' || NVL(C.APELLIDOS, '')),
                    O.TOTAL
                ORDER BY O.FECHA_ORDEN DESC
            )
            WHERE ROWNUM <= 12", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(New CompraClienteViewModel With {
                        .FechaCompra = Convert.ToDateTime(dr("FECHA_ORDEN")),
                        .Cliente = dr("CLIENTE").ToString(),
                        .Valor = Convert.ToDecimal(dr("VALOR")),
                        .FormaPago = dr("FORMA_PAGO").ToString(),
                        .MueblesIncluidos = dr("MUEBLES").ToString()
                    })
                        End While
                    End Using
                End Using
            End Using

            Return lista

        End Function

        Private Function ObtenerIndicadoresMensuales(anio As Integer,
                                                     mesInicio As Integer,
                                                     mesFin As Integer) As List(Of IndicadorMensualViewModel)

            Dim lista As New List(Of IndicadorMensualViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
            SELECT
                EXTRACT(MONTH FROM FECHA_ORDEN) AS MES_NUM,
                TO_CHAR(FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH') AS MES,
                NVL(SUM(TOTAL),0) AS VENTAS,
                COUNT(*) AS ORDENES,
                COUNT(DISTINCT CLI_ID) AS CLIENTES_ACTIVOS
            FROM ORDEN_VENTA
            WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
            AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
            GROUP BY
                EXTRACT(MONTH FROM FECHA_ORDEN),
                TO_CHAR(FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH')
            ORDER BY MES_NUM", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            Dim ventas = Convert.ToDecimal(dr("VENTAS"))
                            Dim ordenes = Convert.ToInt32(dr("ORDENES"))

                            lista.Add(New IndicadorMensualViewModel With {
                                .Mes = dr("MES").ToString().Trim(),
                                .Ventas = ventas,
                                .Ordenes = ordenes,
                                .ClientesActivos = Convert.ToInt32(dr("CLIENTES_ACTIVOS")),
                                .TicketPromedio = If(ordenes > 0, ventas / ordenes, 0D)
                            })
                        End While
                    End Using
                End Using
            End Using

            Return lista

        End Function

        Private Function ObtenerDetalleMensualProductos(anio As Integer,
                                                mesInicio As Integer,
                                                mesFin As Integer) As List(Of DetalleMensualProductoViewModel)

            Dim lista As New List(Of DetalleMensualProductoViewModel)()
            Dim totalPeriodo As Decimal = ObtenerDecimal("
        SELECT NVL(SUM(TOTAL),0)
        FROM ORDEN_VENTA
        WHERE EXTRACT(YEAR FROM FECHA_ORDEN)=:ANIO
        AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
        anio, mesInicio, mesFin)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
            SELECT
                EXTRACT(MONTH FROM O.FECHA_ORDEN) AS MES_NUM,
                TO_CHAR(O.FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH') AS MES,
                NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID) AS PRODUCTO,
                SUM(D.CANTIDAD) AS CANTIDAD,
                SUM(D.SUBTOTAL_LINEA) AS VENTAS
            FROM ORDEN_VENTA_DETALLE D
            INNER JOIN ORDEN_VENTA O ON O.ORDEN_VENTA_ID = D.ORDEN_VENTA_ID
            LEFT JOIN PRODUCTO P ON P.PRODUCTO_ID = D.PRODUCTO_ID
            WHERE EXTRACT(YEAR FROM O.FECHA_ORDEN) = :ANIO
            AND EXTRACT(MONTH FROM O.FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
            GROUP BY
                EXTRACT(MONTH FROM O.FECHA_ORDEN),
                TO_CHAR(O.FECHA_ORDEN, 'Month', 'NLS_DATE_LANGUAGE=SPANISH'),
                NVL(P.NOMBRE, 'Producto #' || D.PRODUCTO_ID)
            ORDER BY MES_NUM, PRODUCTO", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            Dim ventas = Convert.ToDecimal(dr("VENTAS"))
                            Dim porcentaje As Decimal = 0D

                            If totalPeriodo > 0 Then
                                porcentaje = Math.Round((ventas / totalPeriodo) * 100D, 1)
                            End If

                            lista.Add(New DetalleMensualProductoViewModel With {
                        .Mes = dr("MES").ToString().Trim(),
                        .Producto = dr("PRODUCTO").ToString(),
                        .Cantidad = Convert.ToInt32(dr("CANTIDAD")),
                        .Ventas = ventas,
                        .PorcentajeTotal = porcentaje
                    })
                        End While
                    End Using
                End Using
            End Using

            Return lista

        End Function


        Private Function ObtenerCohortes(anio As Integer,
                                 mesInicio As Integer,
                                 mesFin As Integer) As List(Of CohorteViewModel)

            Dim lista As New List(Of CohorteViewModel)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexionReplica()
                Using cmd As New OracleCommand("
            SELECT
                MES_NUM,
                TO_CHAR(TO_DATE(MES_NUM, 'MM'), 'Month', 'NLS_DATE_LANGUAGE=SPANISH') AS MES,
                COUNT(DISTINCT CLI_ID) AS CLIENTES,
                COUNT(*) AS M0,
                0 AS M1,
                0 AS M2,
                0 AS M3,
                NVL(SUM(TOTAL),0) AS VENTAS
            FROM (
                SELECT
                    CLI_ID,
                    EXTRACT(MONTH FROM FECHA_ORDEN) AS MES_NUM,
                    TOTAL
                FROM ORDEN_VENTA
                WHERE EXTRACT(YEAR FROM FECHA_ORDEN) = :ANIO
                AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
            )
            GROUP BY MES_NUM
            ORDER BY MES_NUM", cn)

                    cmd.BindByName = True
                    cmd.Parameters.Add("ANIO", OracleDbType.Int32).Value = anio
                    cmd.Parameters.Add("MES_INICIO", OracleDbType.Int32).Value = mesInicio
                    cmd.Parameters.Add("MES_FIN", OracleDbType.Int32).Value = mesFin

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(New CohorteViewModel With {
                        .Cohorte = dr("MES").ToString().Trim() & " " & anio.ToString(),
                        .Clientes = Convert.ToInt32(dr("CLIENTES")),
                        .M0 = Convert.ToInt32(dr("M0")),
                        .M1 = Convert.ToInt32(dr("M1")),
                        .M2 = Convert.ToInt32(dr("M2")),
                        .M3 = Convert.ToInt32(dr("M3")),
                        .Ventas = Convert.ToDecimal(dr("VENTAS"))
                    })
                        End While
                    End Using
                End Using
            End Using

            Return lista

        End Function

        Private Function ObtenerPorcentajeRecompra(anio As Integer,
                                                   mesInicio As Integer,
                                                   mesFin As Integer) As Decimal

            Dim clientesTotales As Integer = ObtenerEntero("
        SELECT COUNT(DISTINCT CLI_ID)
        FROM ORDEN_VENTA
        WHERE EXTRACT(YEAR FROM FECHA_ORDEN)=:ANIO
        AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN",
                anio, mesInicio, mesFin)

            If clientesTotales = 0 Then
                Return 0D
            End If

            Dim clientesRecompra As Integer = ObtenerEntero("
        SELECT COUNT(*)
        FROM (
            SELECT CLI_ID
            FROM ORDEN_VENTA
            WHERE EXTRACT(YEAR FROM FECHA_ORDEN)=:ANIO
            AND EXTRACT(MONTH FROM FECHA_ORDEN) BETWEEN :MES_INICIO AND :MES_FIN
            GROUP BY CLI_ID
            HAVING COUNT(*) > 1
        )",
                anio, mesInicio, mesFin)

            Return Math.Round(Convert.ToDecimal((clientesRecompra / clientesTotales) * 100.0), 2)

        End Function

    End Class

End Namespace