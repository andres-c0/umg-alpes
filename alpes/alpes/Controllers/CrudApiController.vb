Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Web.Mvc
Imports Newtonsoft.Json.Linq

Public Class CrudApiController
    Inherits Controller

    Private Shared ReadOnly _assembly As Assembly = GetType(CrudApiController).Assembly

    <HttpGet>
    Public Function Tablas() As JsonResult
        Dim configs = BuildTableConfigs()
        Return Json(configs, JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    Public Function Listar(ByVal tabla As String) As JsonResult
        Try
            Dim dt = ExecuteDataMethod(tabla, "Listar")
            Return Json(New With {.ok = True, .data = DataTableToList(dt)}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function ObtenerPorId(ByVal tabla As String, ByVal id As Integer) As JsonResult
        Try
            Dim dt = ExecuteDataMethod(tabla, "ObtenerPorId", id)
            Dim row = If(dt.Rows.Count > 0, DataRowToDictionary(dt.Rows(0)), Nothing)
            Return Json(New With {.ok = True, .data = row}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim map = ResolveTableMapping(tabla)
            Dim data = ParsePayload(payload)
            Dim entity = BuildEntityFromPayload(map.EntityType, data)
            Dim result = ExecuteRawDataMethod(map.DataType, "Insertar", entity)
            Return Json(New With {.ok = True, .id = If(result Is Nothing, Nothing, result)})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim map = ResolveTableMapping(tabla)
            Dim data = ParsePayload(payload)
            Dim entity = BuildEntityFromPayload(map.EntityType, data)
            ExecuteRawDataMethod(map.DataType, "Actualizar", entity)
            Return Json(New With {.ok = True})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    <HttpPost>
    Public Function Eliminar(ByVal tabla As String, ByVal id As Integer) As JsonResult
        Try
            ExecuteDataMethod(tabla, "Eliminar", id)
            Return Json(New With {.ok = True})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    Private Shared Function BuildTableConfigs() As IEnumerable(Of Object)
        Return GetTableMappings() _
            .Where(Function(m) HasMethod(m.DataType, "Listar")) _
            .Select(Function(m) New With {
                .key = m.TableKey,
                .label = m.EntityType.Name,
                .idField = GetIdFieldName(m.EntityType),
                .fields = GetEditableFields(m.EntityType)
            }) _
            .OrderBy(Function(x) x.label) _
            .ToList()
    End Function

    Private Shared Function GetEditableFields(ByVal entityType As Type) As IEnumerable(Of String)
        Return entityType.GetProperties(BindingFlags.Public Or BindingFlags.Instance) _
            .Where(Function(p) p.CanWrite) _
            .Select(Function(p) p.Name) _
            .Where(Function(name) Not IsAuditField(name) AndAlso Not name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)) _
            .ToArray()
    End Function

    Private Shared Function IsAuditField(ByVal name As String) As Boolean
        Return name.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) OrElse
               name.Equals("UpdatedAt", StringComparison.OrdinalIgnoreCase) OrElse
               name.Equals("Estado", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Shared Function GetIdFieldName(ByVal entityType As Type) As String
        Dim idProp = entityType.GetProperties().FirstOrDefault(Function(p) p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
        Return If(idProp Is Nothing, String.Empty, idProp.Name)
    End Function

    Private Shared Function ExecuteDataMethod(ByVal tableKey As String, ByVal methodName As String, ParamArray args As Object()) As DataTable
        Dim map = ResolveTableMapping(tableKey)
        Dim result = ExecuteRawDataMethod(map.DataType, methodName, args)

        If result Is Nothing Then
            Return New DataTable()
        End If

        Return CType(result, DataTable)
    End Function

    Private Shared Function ExecuteRawDataMethod(ByVal dataType As Type, ByVal methodName As String, ParamArray args As Object()) As Object
        Dim method = dataType.GetMethod(methodName)
        If method Is Nothing Then
            Throw New InvalidOperationException($"La clase {dataType.Name} no expone {methodName}.")
        End If

        Dim instance = Activator.CreateInstance(dataType)
        Return method.Invoke(instance, args)
    End Function

    Private Shared Function ResolveTableMapping(ByVal tableKey As String) As TableMapping
        If String.IsNullOrWhiteSpace(tableKey) Then
            Throw New ArgumentException("Debe enviar la tabla.")
        End If

        Dim map = GetTableMappings().FirstOrDefault(Function(m) m.TableKey.Equals(tableKey, StringComparison.OrdinalIgnoreCase))
        If map Is Nothing Then
            Throw New ArgumentException("Tabla no soportada: " & tableKey)
        End If

        Return map
    End Function

    Private Shared Function GetTableMappings() As IEnumerable(Of TableMapping)
        Dim dataTypes = _assembly.GetTypes() _
            .Where(Function(t) t.IsClass AndAlso t.Name.EndsWith("Datos", StringComparison.Ordinal) AndAlso Not t.Name.Equals("ConexionOracle", StringComparison.OrdinalIgnoreCase))

        Dim mappings As New List(Of TableMapping)()

        For Each dataType In dataTypes
            Dim baseName = dataType.Name.Substring(0, dataType.Name.Length - 5)
            Dim entityType = _assembly.GetTypes().FirstOrDefault(Function(t) t.IsClass AndAlso t.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase))
            If entityType Is Nothing Then
                Continue For
            End If

            mappings.Add(New TableMapping With {
                .TableKey = ToSnakeCase(baseName),
                .DataType = dataType,
                .EntityType = entityType
            })
        Next

        Return mappings
    End Function

    Private Shared Function ToSnakeCase(ByVal value As String) As String
        Dim sb As New StringBuilder()

        For i = 0 To value.Length - 1
            Dim ch = value(i)
            If Char.IsUpper(ch) AndAlso i > 0 AndAlso value(i - 1) <> "_"c Then
                sb.Append("_"c)
            End If
            sb.Append(Char.ToLowerInvariant(ch))
        Next

        Return sb.ToString().Replace("__", "_")
    End Function

    Private Shared Function BuildEntityFromPayload(ByVal entityType As Type, ByVal data As IDictionary(Of String, Object)) As Object
        Dim entity = Activator.CreateInstance(entityType)

        For Each prop In entityType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
            If Not prop.CanWrite OrElse Not data.ContainsKey(prop.Name) Then
                Continue For
            End If

            Dim raw = data(prop.Name)
            If raw Is Nothing Then
                prop.SetValue(entity, Nothing)
                Continue For
            End If

            Dim targetType = Nullable.GetUnderlyingType(prop.PropertyType)
            If targetType Is Nothing Then
                targetType = prop.PropertyType
            End If

            prop.SetValue(entity, ConvertToType(raw, targetType))
        Next

        Return entity
    End Function

    Private Shared Function ConvertToType(ByVal value As Object, ByVal targetType As Type) As Object
        If targetType Is GetType(DateTime) Then
            Return DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture)
        End If

        If targetType Is GetType(Boolean) Then
            Dim text = value.ToString().Trim().ToUpperInvariant()
            Return (text = "1" OrElse text = "TRUE" OrElse text = "S" OrElse text = "SI" OrElse text = "Y")
        End If

        Return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)
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

    Private Shared Function HasMethod(ByVal type As Type, ByVal methodName As String) As Boolean
        Return type.GetMethod(methodName) IsNot Nothing
    End Function

    Private Class TableMapping
        Public Property TableKey As String
        Public Property DataType As Type
        Public Property EntityType As Type
    End Class

End Class
