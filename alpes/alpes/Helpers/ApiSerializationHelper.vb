Option Strict On
Option Explicit On

Imports System.Data
Imports System.Globalization

Public Module ApiSerializationHelper

    Public Function ToCollection(ByVal table As DataTable) As List(Of Dictionary(Of String, Object))
        Dim items As New List(Of Dictionary(Of String, Object))()

        If table Is Nothing Then
            Return items
        End If

        For Each row As DataRow In table.Rows
            items.Add(ToDictionary(row))
        Next

        Return items
    End Function

    Public Function ToItem(ByVal table As DataTable) As Dictionary(Of String, Object)
        If table Is Nothing OrElse table.Rows.Count = 0 Then
            Return Nothing
        End If

        Return ToDictionary(table.Rows(0))
    End Function

    Public Function ToDictionary(ByVal row As DataRow) As Dictionary(Of String, Object)
        Dim item As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

        For Each column As DataColumn In row.Table.Columns
            item(column.ColumnName) = NormalizeValue(row(column))
        Next

        Return item
    End Function

    Private Function NormalizeValue(ByVal value As Object) As Object
        If value Is Nothing OrElse Convert.IsDBNull(value) Then
            Return Nothing
        End If

        If TypeOf value Is DateTime Then
            Return DirectCast(value, DateTime).ToString("o", CultureInfo.InvariantCulture)
        End If

        Return value
    End Function

End Module
