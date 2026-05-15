Imports System.IO

Namespace Seguridad

    Public NotInheritable Class EnvLoader

        Private Sub New()
        End Sub

        Public Shared Sub Load(envPath As String, Optional overrideExisting As Boolean = False)
            If String.IsNullOrWhiteSpace(envPath) OrElse Not File.Exists(envPath) Then
                Return
            End If

            For Each rawLine In File.ReadAllLines(envPath)
                Dim line = rawLine.Trim()

                If String.IsNullOrWhiteSpace(line) OrElse line.StartsWith("#") Then
                    Continue For
                End If

                Dim separatorIndex = line.IndexOf("="c)

                If separatorIndex <= 0 Then
                    Continue For
                End If

                Dim key = line.Substring(0, separatorIndex).Trim()
                Dim value = line.Substring(separatorIndex + 1).Trim()

                If String.IsNullOrWhiteSpace(key) Then
                    Continue For
                End If

                If value.StartsWith("""") AndAlso value.EndsWith("""") AndAlso value.Length >= 2 Then
                    value = value.Substring(1, value.Length - 2)
                End If

                If value.StartsWith("'") AndAlso value.EndsWith("'") AndAlso value.Length >= 2 Then
                    value = value.Substring(1, value.Length - 2)
                End If

                Dim currentValue = Environment.GetEnvironmentVariable(key)

                If overrideExisting OrElse String.IsNullOrEmpty(currentValue) Then
                    Environment.SetEnvironmentVariable(key, value)
                End If
            Next
        End Sub

    End Class

End Namespace