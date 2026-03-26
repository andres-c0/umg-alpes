Option Strict On
Option Explicit On
Option Infer On

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
        Dim configs As IEnumerable(Of Object) = BuildTableConfigs()
        Return Json(configs, JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    Public Function Listar(ByVal tabla As String) As JsonResult
        Try
            Dim dt As DataTable = ExecuteDataMethod(tabla, "Listar")
            Return Json(New With {.ok = True, .data = DataTableToList(dt)}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function ObtenerPorId(ByVal tabla As String, ByVal id As Integer) As JsonResult
        Try
            Dim dt As DataTable = ExecuteDataMethod(tabla, "ObtenerPorId", id)
            Dim row As Dictionary(Of String, Object) = If(dt.Rows.Count > 0, DataRowToDictionary(dt.Rows(0)), Nothing)
            Return Json(New With {.ok = True, .data = row}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim map As TableMapping = ResolveTableMapping(tabla)
            Dim data As IDictionary(Of String, Object) = ParsePayload(payload)
            Dim entity As Object = BuildEntityFromPayload(map.EntityType, data)
            Dim result As Object = ExecuteRawDataMethod(map.DataType, "Insertar", entity)
            Return Json(New With {.ok = True, .id = If(result Is Nothing, Nothing, result)})
        Catch ex As Exception
            Return Json(New With {.ok = False, .error = ex.Message})
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar(ByVal tabla As String, ByVal payload As String) As JsonResult
        Try
            Dim map As TableMapping = ResolveTableMapping(tabla)
            Dim data As IDictionary(Of String, Object) = ParsePayload(payload)
            Dim entity As Object = BuildEntityFromPayload(map.EntityType, data)
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
        Dim idProp As PropertyInfo = entityType.GetProperties().FirstOrDefault(Function(p) p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
        Return If(idProp Is Nothing, String.Empty, idProp.Name)
    End Function

    Private Shared Function ExecuteDataMethod(ByVal tableKey As String, ByVal methodName As String, ParamArray args As Object()) As DataTable
        Dim map As TableMapping = ResolveTableMapping(tableKey)
        Dim result As Object = ExecuteRawDataMethod(map.DataType, methodName, args)

        If result Is Nothing Then
            Return New DataTable()
        End If

        Return CType(result, DataTable)
    End Function

    Private Shared Function ExecuteRawDataMethod(ByVal dataType As Type, ByVal methodName As String, ParamArray args As Object()) As Object
        Dim method As MethodInfo = dataType.GetMethod(methodName)
        If method Is Nothing Then
            Throw New InvalidOperationException(String.Format("La clase {0} no expone {1}.", dataType.Name, methodName))
        End If

        Dim instance As Object = Activator.CreateInstance(dataType)
        Return method.Invoke(instance, args)
    End Function

    Private Shared Function ResolveTableMapping(ByVal tableKey As String) As TableMapping
        If String.IsNullOrWhiteSpace(tableKey) Then
            Throw New ArgumentException("Debe enviar la tabla.")
        End If

        Dim map As TableMapping = GetTableMappings().FirstOrDefault(Function(m) m.TableKey.Equals(tableKey, StringComparison.OrdinalIgnoreCase))
        If map Is Nothing Then
            Throw New ArgumentException("Tabla no soportada: " & tableKey)
        End If

        Return map
    End Function

    Private Shared Function GetTableMappings() As IEnumerable(Of TableMapping)
        Dim dataTypes As IEnumerable(Of Type) = _assembly.GetTypes() _
            .Where(Function(t) t.IsClass AndAlso t.Name.EndsWith("Datos", StringComparison.Ordinal) AndAlso Not t.Name.Equals("ConexionOracle", StringComparison.OrdinalIgnoreCase))

        Dim mappings As New List(Of TableMapping)()

        For Each dataType As Type In dataTypes
            Dim baseName As String = dataType.Name.Substring(0, dataType.Name.Length - 5)
            Dim entityType As Type = _assembly.GetTypes().FirstOrDefault(Function(t) t.IsClass AndAlso t.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase))

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

        For i As Integer = 0 To value.Length - 1
            Dim ch As Char = value(i)
            If Char.IsUpper(ch) AndAlso i > 0 AndAlso value(i - 1) <> "_"c Then
                sb.Append("_"c)
            End If
            sb.Append(Char.ToLowerInvariant(ch))
        Next

        Return sb.ToString().Replace("__", "_")
    End Function

    Private Shared Function BuildEntityFromPayload(ByVal entityType As Type, ByVal data As IDictionary(Of String, Object)) As Object
        Dim entity As Object = Activator.CreateInstance(entityType)

        For Each prop As PropertyInfo In entityType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
            If Not prop.CanWrite OrElse Not data.ContainsKey(prop.Name) Then
                Continue For
            End If

            Dim raw As Object = data(prop.Name)

            If raw Is Nothing Then
                prop.SetValue(entity, Nothing)
                Continue For
            End If

            Dim targetType As Type = Nullable.GetUnderlyingType(prop.PropertyType)
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
            Dim text As String = value.ToString().Trim().ToUpperInvariant()
            Return (text = "1" OrElse text = "TRUE" OrElse text = "S" OrElse text = "SI" OrElse text = "Y")
        End If

        Return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)
    End Function

    Private Shared Function ParsePayload(ByVal payload As String) As IDictionary(Of String, Object)
        If String.IsNullOrWhiteSpace(payload) Then
            Return New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
        End If

        Dim jsonObj As JObject = JObject.Parse(payload)
        Dim result As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

        For Each prop As JProperty In jsonObj.Properties()
            If prop.Value Is Nothing OrElse prop.Value.Type = JTokenType.Null Then
                result(prop.Name) = Nothing
            Else
                Dim jVal As JValue = TryCast(prop.Value, JValue)
                If jVal IsNot Nothing Then
                    result(prop.Name) = jVal.Value
                Else
                    result(prop.Name) = prop.Value.ToString()
                End If
            End If
        Next

        Return result
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