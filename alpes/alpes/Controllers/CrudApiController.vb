Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Web.Mvc
Imports Newtonsoft.Json.Linq

Public Class CrudApiController
    Inherits Controller

    <HttpGet>
    Public Function Tablas() As JsonResult
        Return Json(GetTableConfigs(), JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    Public Function Listar(ByVal tabla As String) As JsonResult
        Try
            Dim dt = ExecuteList(tabla)
            Return Json(New With {
                .ok = True,
                .data = DataTableToList(dt)
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function ObtenerPorId(ByVal tabla As String, ByVal id As Integer) As JsonResult
        Try
            Dim dt = ExecuteGet(tabla, id)
            Dim row = If(dt.Rows.Count > 0, DataRowToDictionary(dt.Rows(0)), Nothing)
            Return Json(New With {.ok = True, .data = row}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim data = ParsePayload(payload)
            Dim id = ExecuteInsert(tabla, data)
            Return Json(New With {.ok = True, .id = id})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim data = ParsePayload(payload)
            ExecuteUpdate(tabla, data)
            Return Json(New With {.ok = True})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    <HttpPost>
    Public Function Eliminar(ByVal tabla As String, ByVal id As Integer) As JsonResult
        Try
            ExecuteDelete(tabla, id)
            Return Json(New With {.ok = True})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    Private Shared Function ParsePayload(ByVal payload As String) As IDictionary(Of String, Object)
        If String.IsNullOrWhiteSpace(payload) Then
            Return New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
        End If

        Dim jObject = JObject.Parse(payload)
        Return jObject.Properties().ToDictionary(
            Function(p) p.Name,
            Function(p) CType(If(p.Value.Type = JTokenType.Null, Nothing, CType(p.Value, JValue).Value), Object),
            StringComparer.OrdinalIgnoreCase)
    End Function

    Private Shared Function DataTableToList(ByVal dt As DataTable) As IList(Of Dictionary(Of String, Object))
        Dim list As New List(Of Dictionary(Of String, Object))()

        For Each row As DataRow In dt.Rows
            list.Add(DataRowToDictionary(row))
        Next

        Return list
    End Function

    Private Shared Function DataRowToDictionary(ByVal row As DataRow) As Dictionary(Of String, Object)
        Dim dict As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

        For Each col As DataColumn In row.Table.Columns
            dict(col.ColumnName) = If(IsDBNull(row(col)), Nothing, row(col))
        Next

        Return dict
    End Function

    Private Shared Function GetTableConfigs() As Object
        Return New Object() {
            BuildConfig("cliente", "Clientes", "CliId", New String() {"TipoDocumento", "NumDocumento", "Nit", "Nombres", "Apellidos", "Email", "TelResidencia", "TelCelular", "Direccion", "Ciudad", "Departamento", "Pais", "Profesion"}),
            BuildConfig("cupon", "Cupones", "CuponId", New String() {"Codigo", "Descripcion", "VigenciaInicio", "VigenciaFin", "LimiteUsoTotal", "LimiteUsoPorCliente", "UsosActuales"}),
            BuildConfig("estado_envio", "Estados de envío", "EstadoEnvioId", New String() {"Codigo", "Descripcion"}),
            BuildConfig("promocion", "Promociones", "PromocionId", New String() {"TipoPromocionId", "Nombre", "Descripcion", "VigenciaInicio", "VigenciaFin", "Prioridad"}),
            BuildConfig("tipo_promocion", "Tipos de promoción", "TipoPromocionId", New String() {"Nombre", "Descripcion"}),
            BuildConfig("tipo_entrega", "Tipos de entrega", "TipoEntregaId", New String() {"Nombre", "Descripcion"}),
            BuildConfig("zona_envio", "Zonas de envío", "ZonaEnvioId", New String() {"Nombre", "Pais", "Departamento", "Ciudad"}),
            BuildConfig("tarifa_envio", "Tarifas de envío", "TarifaEnvioId", New String() {"ZonaEnvioId", "TipoEntregaId", "PesoDesdeKg", "PesoHastaKg", "Costo"}),
            BuildConfig("politica_envio", "Políticas de envío", "PoliticaEnvioId", New String() {"Titulo", "Descripcion", "VigenciaInicio", "VigenciaFin"}),
            BuildConfig("regla_envio_gratis", "Reglas de envío gratis", "ReglaEnvioGratisId", New String() {"ZonaEnvioId", "MontoMinimo", "PesoMaxKg", "VigenciaInicio", "VigenciaFin"}),
            BuildConfig("regla_promocion", "Reglas de promoción", "ReglaPromocionId", New String() {"PromocionId", "MinCompra", "MinItems", "AplicaTipoProducto", "TopeDescuento"}),
            BuildConfig("promocion_producto", "Promociones por producto", "PromocionProductoId", New String() {"PromocionId", "ProductoId", "LimiteUnidades"}),
            BuildConfig("campana_marketing", "Campañas de marketing", "CampanaMarketingId", New String() {"Nombre", "Canal", "Presupuesto", "Inicio", "Fin"})
        }
    End Function

    Private Shared Function BuildConfig(ByVal key As String, ByVal label As String, ByVal idField As String, ByVal fields As IEnumerable(Of String)) As Object
        Return New With {
            .key = key,
            .label = label,
            .idField = idField,
            .fields = fields
        }
    End Function

    Private Shared Sub FillEntity(Of T As New)(ByVal target As T, ByVal data As IDictionary(Of String, Object))
        Dim props = GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)

        For Each prop In props
            If Not prop.CanWrite OrElse Not data.ContainsKey(prop.Name) Then
                Continue For
            End If

            Dim raw = data(prop.Name)
            If raw Is Nothing Then
                prop.SetValue(target, Nothing)
                Continue For
            End If

            Dim targetType = Nullable.GetUnderlyingType(prop.PropertyType)
            If targetType Is Nothing Then
                targetType = prop.PropertyType
            End If

            Dim safeValue = ConvertToType(raw, targetType)
            prop.SetValue(target, safeValue)
        Next
    End Sub

    Private Shared Function ConvertToType(ByVal value As Object, ByVal targetType As Type) As Object
        If targetType Is GetType(DateTime) Then
            If TypeOf value Is DateTime Then
                Return value
            End If
            Return DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture)
        End If

        If targetType Is GetType(Boolean) Then
            If TypeOf value Is Boolean Then
                Return value
            End If

            Dim text = value.ToString().Trim().ToUpperInvariant()
            Return (text = "1" OrElse text = "TRUE" OrElse text = "S" OrElse text = "SI" OrElse text = "Y")
        End If

        If targetType.IsEnum Then
            Return [Enum].Parse(targetType, value.ToString())
        End If

        Return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)
    End Function

    Private Shared Function ExecuteList(ByVal tabla As String) As DataTable
        Select Case tabla.ToLowerInvariant()
            Case "cliente" : Return New Servicios.ClienteServicio().Listar()
            Case "cupon" : Return New CuponServicio().Listar()
            Case "estado_envio" : Return New EstadoEnvioServicio().Listar()
            Case "promocion" : Return New PromocionServicio().Listar()
            Case "tipo_promocion" : Return New TipoPromocionServicio().Listar()
            Case "tipo_entrega" : Return New TipoEntregaServicio().Listar()
            Case "zona_envio" : Return New ZonaEnvioServicio().Listar()
            Case "tarifa_envio" : Return New TarifaEnvioServicio().Listar()
            Case "politica_envio" : Return New PoliticaEnvioServicio().Listar()
            Case "regla_envio_gratis" : Return New ReglaEnvioGratisServicio().Listar()
            Case "regla_promocion" : Return New ReglaPromocionServicio().Listar()
            Case "promocion_producto" : Return New PromocionProductoServicio().Listar()
            Case "campana_marketing" : Return New CampanaMarketingServicio().Listar()
            Case Else : Throw New ArgumentException("Tabla no soportada: " & tabla)
        End Select
    End Function

    Private Shared Function ExecuteGet(ByVal tabla As String, ByVal id As Integer) As DataTable
        Select Case tabla.ToLowerInvariant()
            Case "cliente" : Return New Servicios.ClienteServicio().ObtenerPorId(id)
            Case "cupon" : Return New CuponServicio().ObtenerPorId(id)
            Case "estado_envio" : Return New EstadoEnvioServicio().ObtenerPorId(id)
            Case "promocion" : Return New PromocionServicio().ObtenerPorId(id)
            Case "tipo_promocion" : Return New TipoPromocionServicio().ObtenerPorId(id)
            Case "tipo_entrega" : Return New TipoEntregaServicio().ObtenerPorId(id)
            Case "zona_envio" : Return New ZonaEnvioServicio().ObtenerPorId(id)
            Case "tarifa_envio" : Return New TarifaEnvioServicio().ObtenerPorId(id)
            Case "politica_envio" : Return New PoliticaEnvioServicio().ObtenerPorId(id)
            Case "regla_envio_gratis" : Return New ReglaEnvioGratisServicio().ObtenerPorId(id)
            Case "regla_promocion" : Return New ReglaPromocionServicio().ObtenerPorId(id)
            Case "promocion_producto" : Return New PromocionProductoServicio().ObtenerPorId(id)
            Case "campana_marketing" : Return New CampanaMarketingServicio().ObtenerPorId(id)
            Case Else : Throw New ArgumentException("La tabla no expone ObtenerPorId o no está soportada: " & tabla)
        End Select
    End Function

    Private Shared Function ExecuteInsert(ByVal tabla As String, ByVal data As IDictionary(Of String, Object)) As Integer
        Select Case tabla.ToLowerInvariant()
            Case "cliente"
                Dim entity As New Entidades.Cliente()
                FillEntity(entity, data)
                Return New Servicios.ClienteServicio().Insertar(entity)
            Case "cupon"
                Dim entity As New Cupon()
                FillEntity(entity, data)
                Return New CuponServicio().Insertar(entity)
            Case "estado_envio"
                Dim entity As New EstadoEnvio()
                FillEntity(entity, data)
                Return New EstadoEnvioServicio().Insertar(entity)
            Case "promocion"
                Dim entity As New Promocion()
                FillEntity(entity, data)
                Return New PromocionServicio().Insertar(entity)
            Case "tipo_promocion"
                Dim entity As New TipoPromocion()
                FillEntity(entity, data)
                Return New TipoPromocionServicio().Insertar(entity)
            Case "tipo_entrega"
                Dim entity As New TipoEntrega()
                FillEntity(entity, data)
                Return New TipoEntregaServicio().Insertar(entity)
            Case "zona_envio"
                Dim entity As New ZonaEnvio()
                FillEntity(entity, data)
                Return New ZonaEnvioServicio().Insertar(entity)
            Case "tarifa_envio"
                Dim entity As New TarifaEnvio()
                FillEntity(entity, data)
                Return New TarifaEnvioServicio().Insertar(entity)
            Case "politica_envio"
                Dim entity As New PoliticaEnvio()
                FillEntity(entity, data)
                Return New PoliticaEnvioServicio().Insertar(entity)
            Case "regla_envio_gratis"
                Dim entity As New ReglaEnvioGratis()
                FillEntity(entity, data)
                Return New ReglaEnvioGratisServicio().Insertar(entity)
            Case "regla_promocion"
                Dim entity As New ReglaPromocion()
                FillEntity(entity, data)
                Return New ReglaPromocionServicio().Insertar(entity)
            Case "promocion_producto"
                Dim entity As New PromocionProducto()
                FillEntity(entity, data)
                Return New PromocionProductoServicio().Insertar(entity)
            Case "campana_marketing"
                Dim entity As New CampanaMarketing()
                FillEntity(entity, data)
                Return New CampanaMarketingServicio().Insertar(entity)
            Case Else
                Throw New ArgumentException("Tabla no soportada: " & tabla)
        End Select
    End Function

    Private Shared Sub ExecuteUpdate(ByVal tabla As String, ByVal data As IDictionary(Of String, Object))
        Select Case tabla.ToLowerInvariant()
            Case "cliente"
                Dim entity As New Entidades.Cliente()
                FillEntity(entity, data)
                New Servicios.ClienteServicio().Actualizar(entity)
            Case "cupon"
                Dim entity As New Cupon()
                FillEntity(entity, data)
                New CuponServicio().Actualizar(entity)
            Case "estado_envio"
                Dim entity As New EstadoEnvio()
                FillEntity(entity, data)
                New EstadoEnvioServicio().Actualizar(entity)
            Case "promocion"
                Dim entity As New Promocion()
                FillEntity(entity, data)
                New PromocionServicio().Actualizar(entity)
            Case "tipo_promocion"
                Dim entity As New TipoPromocion()
                FillEntity(entity, data)
                New TipoPromocionServicio().Actualizar(entity)
            Case "tipo_entrega"
                Dim entity As New TipoEntrega()
                FillEntity(entity, data)
                New TipoEntregaServicio().Actualizar(entity)
            Case "zona_envio"
                Dim entity As New ZonaEnvio()
                FillEntity(entity, data)
                New ZonaEnvioServicio().Actualizar(entity)
            Case "tarifa_envio"
                Dim entity As New TarifaEnvio()
                FillEntity(entity, data)
                New TarifaEnvioServicio().Actualizar(entity)
            Case "politica_envio"
                Dim entity As New PoliticaEnvio()
                FillEntity(entity, data)
                New PoliticaEnvioServicio().Actualizar(entity)
            Case "regla_envio_gratis"
                Dim entity As New ReglaEnvioGratis()
                FillEntity(entity, data)
                New ReglaEnvioGratisServicio().Actualizar(entity)
            Case "regla_promocion"
                Dim entity As New ReglaPromocion()
                FillEntity(entity, data)
                New ReglaPromocionServicio().Actualizar(entity)
            Case "promocion_producto"
                Dim entity As New PromocionProducto()
                FillEntity(entity, data)
                New PromocionProductoServicio().Actualizar(entity)
            Case "campana_marketing"
                Dim entity As New CampanaMarketing()
                FillEntity(entity, data)
                New CampanaMarketingServicio().Actualizar(entity)
            Case Else
                Throw New ArgumentException("La tabla no soporta actualización: " & tabla)
        End Select
    End Sub

    Private Shared Sub ExecuteDelete(ByVal tabla As String, ByVal id As Integer)
        Select Case tabla.ToLowerInvariant()
            Case "cliente" : New Servicios.ClienteServicio().Eliminar(id)
            Case "cupon" : New CuponServicio().Eliminar(id)
            Case "estado_envio" : New EstadoEnvioServicio().Eliminar(id)
            Case "promocion" : New PromocionServicio().Eliminar(id)
            Case "tipo_promocion" : New TipoPromocionServicio().Eliminar(id)
            Case "tipo_entrega" : New TipoEntregaServicio().Eliminar(id)
            Case "zona_envio" : New ZonaEnvioServicio().Eliminar(id)
            Case "tarifa_envio" : New TarifaEnvioServicio().Eliminar(id)
            Case "politica_envio" : New PoliticaEnvioServicio().Eliminar(id)
            Case "regla_envio_gratis" : New ReglaEnvioGratisServicio().Eliminar(id)
            Case "regla_promocion" : New ReglaPromocionServicio().Eliminar(id)
            Case "promocion_producto" : New PromocionProductoServicio().Eliminar(id)
            Case "campana_marketing" : New CampanaMarketingServicio().Eliminar(id)
            Case Else : Throw New ArgumentException("La tabla no soporta eliminación: " & tabla)
        End Select
    End Sub

End Class
